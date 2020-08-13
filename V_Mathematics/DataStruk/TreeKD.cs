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
    public class TreeKD<E> : TreeVector<E>
    {
        private int dim;

        private VList<VectorPair<E>> pairs;

        private NodeKD<E> root;


        public TreeKD(int dim)
        {
            //sets the number of dimentions
            this.dim = dim;

            pairs = new VListArray<VectorPair<E>>();
            root = null;
        }

        public override bool BuildRequired
        {
            get
            {
                if (root == null) return true;
                
                return false;
            }
        }


        public override void Add(Vector loc, E data)
        {
            //we do not allow vectors that are smaller than our dimention
            if (loc.Length < dim) throw new InvalidOperationException();

            //we create a new vector pair and add it to the list
            var temp = new VectorPair<E>(data, loc);
            pairs.Add(temp);
        }

        public override void Build()
        {
            //int tsize = insertions.Count; //+ existing nodes
            //var working = new VListArray<VectorPair<E>>(tsize);

            //foreach (var pair in insertions)
            //working.Add(pair);

            //creates a working set by copying the main array
            var working = new VListArray<VectorPair<E>>(pairs);

            //NEED TO: Dispose of the original structor

            //starts incrementaly buildign the structor
            root = BuildInc(working, 0);
        }

        private NodeKD<E> BuildInc(VList<VectorPair<E>> working, int level)
        {
            //we can't build a node if we have no working set
            if (working.Empty) return null;

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

            //returns the new vector pair
            return new VectorPair<E>(value, loc);
        }

        public override IEnumerable<VectorPair<E>> GetNearest(Vector probe, int count)
        {
            throw new NotImplementedException();
        }
    }
}
