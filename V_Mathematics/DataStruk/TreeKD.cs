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
            //NEEDS MORE WORK!!!!!

            //we can't search the tree if the structor hasn't been built
            if (root == null) throw new InvalidOperationException();

            var queue = new DequeArray<NodeKD<E>>();
            double mindist = Double.PositiveInfinity;

            NodeKD<E> curr = root;
            NodeKD<E> prev = null;
            NodeKD<E> other = null;
            NodeKD<E> best = null;
            

            //fined the lowest leaf containing the node
            while (!curr.IsLeaf)
            {
                queue.PushBack(curr);
                curr = curr.Trace(probe);               
            }

            mindist = probe.Dist(curr.Location);
            bool found = false;
            best = curr;

            while (!found)
            {
                prev = curr;
                curr = queue.PopBack();
                other = curr.GetOther(prev);

                double dist = other.Dist(probe);
                if (dist > mindist) continue;

                if (other.IsLeaf)
                {
                    //updates the best match so far
                    best = other;
                    mindist = dist;
                }
                else
                {
                    //starts searching on the alternate branch
                    queue.PushBack(other);
                    curr = other.Trace(probe);
                }
            }

            //LOOK UP DEPTH-FIRST SEARH
            
            

            throw new NotImplementedException();
        }

        public override IEnumerable<VectorPair<E>> GetNearest(Vector probe, int count)
        {
            throw new NotImplementedException();
        }
    }
}
