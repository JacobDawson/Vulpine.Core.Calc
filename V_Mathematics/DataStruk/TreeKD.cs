using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;
using Vulpine.Core.Data.Queues;
using Vulpine.Core.Data.Lists;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Data
{
    /// <summary>
    /// A KD-Tree or K-Dimentional Tree, is a special type of data structur that stores
    /// vectors, and can be used to find the closest vectors to a given probe. It works
    /// by partioning the K-Dimentional space with hyper planes at each node. Everything
    /// to one side of the hyperplane gets stored in the left subtree, while everything
    /// on the other side gets stored in the right subtree. As such, the number of
    /// dimentions must be spesified in advance. It works best when the number of
    /// dimentions is kept small, reletive to the amount of data.
    /// </summary>
    /// <typeparam name="E">Element type of the KD-Tree</typeparam>
    public class TreeKD<E> : TreeVector<E>
    {
        #region Class Deffinitions...

        //stores the dimetion and the number of nodes contained
        private int dim;
        private int tcount;

        //provides a staging area for points as they are added
        private VList<VectorPair<E>> insertions;

        //refrences the root of the internal structor
        private NodeKD<E> root;

        /// <summary>
        /// Constructs a new KD-Tree with the given number of dimentions.
        /// </summary>
        /// <param name="dim">Number of dimentions</param>
        public TreeKD(int dim)
        {
            //sets the number of dimentions
            this.dim = dim;
            this.tcount = 0;

            //computes the expected size
            int exsize = (int)Math.Pow(2.0, dim);

            //sets up the internal data structors
            insertions = new VListArray<VectorPair<E>>(exsize);
            root = null;
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins if it is nessary to (re)build the data structor, in order
        /// to get the maximum efficent search time.
        /// </summary>
        public override bool BuildRequired
        {
            get
            {
                if (root == null) return true;
                if (!insertions.Empty) return true;
                
                return false;
            }
        }

        /// <summary>
        /// Determins the number of points contained in the KD-Tree, including
        /// those that have yet to be added.
        /// </summary>
        public override int Count
        {
            get { return insertions.Count + tcount; }
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Vector Tree Implementation...

        /// <summary>
        /// Preemptively adds a new vector pair to the data structor. Note that
        /// this insertion is not garenteed to take effect untill after the data
        /// structor has been (re)built.
        /// </summary>
        /// <param name="loc">Location of the data as a vector</param>
        /// <param name="data">Data pointed to by the vector</param>
        public override void Add(Vector loc, E data)
        {
            //we do not allow vectors that are smaller than our dimention
            if (loc.Length < dim) throw new InvalidOperationException();

            //we create a new vector pair and add it to the list
            var temp = new VectorPair<E>(data, loc);
            insertions.Add(temp);
        }

        /// <summary>
        /// Builds the internal data structor used to preform fast searches. Certain
        /// methods may not work until this method is called at least once. If any
        /// insertions are made between method calls, it may be nessary to rebuild
        /// the data structor for those insertions to take effect.
        /// </summary>
        public override void Build()
        {
            //creates a working set
            int ncount = insertions.Count + tcount;
            var working = new VListArray<VectorPair<E>>(ncount);
            var ittr = ListPairsInTree();

            //grabes all the points, inserted or otherwise
            foreach (var pair in insertions)
                working.Add(pair);           
            foreach (var pair in ittr)
                working.Add(pair);

            //clears out the insertions
            insertions.Clear();
            tcount = ncount;

            //Disposes of the original structor
            ClearTree();

            //starts incremently building the structor
            root = BuildInc(working, 0);
        }

        /// <summary>
        /// Obtains the vector pair closest to the given probe position.
        /// </summary>
        /// <param name="probe">Vector to probe the data set</param>
        /// <returns>The vector pair closest to the given probe</returns>
        /// <exception cref=" InvalidOperationException">If this method is
        /// called before the internal data strucur is built</exception>
        public override VectorPair<E> GetNearest(Vector probe)
        {
            //we can't search the tree if the structor hasn't been built
            if (root == null) throw new InvalidOperationException();

            var stack = new DequeArray<NodeKD<E>>();
            double mindist = Double.PositiveInfinity;

            NodeKD<E> curr = root;
            NodeKD<E> best = null;

            stack.PushBack(root);

            while (!stack.Empty)
            {
                curr = stack.PopBack();
                double dist = curr.Dist(probe);

                if (curr.IsLeaf)
                {
                    //ignore leaves greater than our minimum distance
                    if (dist > mindist) continue;

                    //updates the best match so far
                    best = curr;
                    mindist = dist;
                }
                else
                {
                    //obtains the direction to probe and it's sibling
                    NodeKD<E> temp = curr.Trace(probe);
                    NodeKD<E> other = curr.GetOther(temp);

                    //pushes the nodes in reverse visiting order
                    if (dist < mindist) stack.PushBack(other);
                    stack.PushBack(temp);
                }
            }

            //if we somehow fail to find a match
            if (best == null) return null;

            //extracts the value and the location
            Vector loc = best.Location;
            E value = best.Value;

            //creates a new vector pair
            var best_pair = new VectorPair<E>(value, loc);

            //any insertions remaning must be searched linearly
            foreach (var pair in insertions)
            {
                Vector test = pair.Location;
                double dist = test.Dist(probe);

                if (dist < mindist)
                {
                    mindist = dist;
                    best_pair = pair;
                }
            }

            return best_pair;
        }

        /// <summary>
        /// Obtains the top N closest vector pairs to the given probe positon,
        /// where N is the number of desired pairs the method should return.
        /// </summary>
        /// <param name="probe">Vector to probe the data set</param>
        /// <param name="count">Number of vector pairs to return</param>
        /// <returns>The top N closest vector pairs</returns>
        /// <exception cref=" InvalidOperationException">If this method is
        /// called before the internal data strucur is built</exception>
        public override IEnumerable<VectorPair<E>> GetNearest(Vector probe, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an enumeration of all the vectors and their assigned values in
        /// the vector tree. If you are intrested in only the vectors or their values
        /// consider using one of the other methods.
        /// </summary>
        /// <returns>An enumeration of vector pairs</returns>
        public override IEnumerator<VectorPair<E>> GetEnumerator()
        {
            //obtains an itterator for the internal structur
            var ittr = ListPairsInTree();

            //grabes all the points, inserted or otherwise
            foreach (var pair in ittr) yield return pair;
            foreach (var pair in insertions) yield return pair;           
        }

        /// <summary>
        /// Clears the tree of all data, returning it to it's original state.
        /// </summary>
        public override void Clear()
        {
            insertions.Clear();
            ClearTree();
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Itterates through the tree, returning all the vector pairs contained
        /// therin. Note that vector pairs are only stored in the leaves.
        /// </summary>
        /// <returns>All the vector pairs in the tree</returns>
        private IEnumerable<VectorPair<E>> ListPairsInTree()
        {
            //if there is no tree, there is nothing to itterate
            if (root == null) yield break;

            var stack = new DequeArray<NodeKD<E>>();
            NodeKD<E> curr = root;

            stack.PushBack(root);

            while (!stack.Empty)
            {
                curr = stack.PopBack();

                if (curr.IsLeaf)
                {
                    //extracts both the location and the data
                    Vector loc = curr.Location;
                    E value = curr.Value;

                    //yields a new vector pair
                    yield return new VectorPair<E>(value, loc);
                }
                else
                {
                    //pushes the nodes in reverse visiting order
                    stack.PushBack(curr.Right);
                    stack.PushBack(curr.Left);
                }
            }
        }

        /// <summary>
        /// Builds the the tree incremently, by building the sub-tree which
        /// contains only the elements in the working set.
        /// </summary>
        /// <param name="working">Points to include in subtree</param>
        /// <param name="level">Level of the subtree</param>
        /// <returns>The root node of the subtree</returns>
        private NodeKD<E> BuildInc(VList<VectorPair<E>> working, int level)
        {
            //we can't build a node if we have no working set
            if (working.Empty) throw new InvalidOperationException();

            //if we contain a single data point, return a leaf
            if (working.Count == 1)
            {
                VectorPair<E> pair = working.GetItem(0);
                return new NodeKD<E>(pair.Location, pair.Value);
            }

            //determins the axis we are sorting on
            int axis = level % dim;
            double mean = 0.0;
            int count = 1;

            //computes the mean of the data set
            foreach (var pair in working)
            {
                double val = pair.Location[axis];
                double delta = (val - mean) / count;

                mean += delta;
                count = count + 1;
            }

            int half_size = (working.Count + 1) / 2;
            var leftset = new VListArray<VectorPair<E>>(half_size);
            var rightset = new VListArray<VectorPair<E>>(half_size);

            //splits the working set in half
            foreach (var pair in working)
            {
                double val = pair.Location[axis];

                if (val > mean) rightset.Add(pair);
                else leftset.Add(pair);
            }

            working.Clear();

            //itterativly builds the left and right subtrees
            NodeKD<E> left = BuildInc(leftset, level + 1);
            NodeKD<E> right = BuildInc(rightset, level + 1);

            //returns the root of the curent subtree
            return new NodeKD<E>(axis, mean, left, right);

        }

        /// <summary>
        /// Clears the tree portion of all data, allowing the root
        /// to be safely disposed.
        /// </summary>
        private void ClearTree()
        {
            //if there is no tree, there is nothing to remove
            if (root == null) return;

            var stack = new DequeArray<NodeKD<E>>();
            NodeKD<E> curr = root;

            stack.PushBack(root);

            while (!stack.Empty)
            {
                curr = stack.PopBack();

                if (!curr.IsLeaf)
                {
                    //pushes the nodes in reverse visiting order
                    stack.PushBack(curr.Right);
                    stack.PushBack(curr.Left);
                }

                curr.Dispose();
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////
    }
}
