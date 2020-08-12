using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Data
{
    public class NodeKD<T> : Node<NodeKD<T>>
    {
        ////determins the position and orientation of the hyperplane
        //private int axis;
        //private double split;

        ////determins if the node is a leaf
        //private bool leaf;

        ////contains a link to the left and right children
        //private NodeKD<T> left;
        //private NodeKD<T> right;

        ////stores the location and the data in the leaf
        //private Vector loc;
        //private T data;

        ///////////////////////////////////////////////////////////////////////////

        ////This implementation stores data in all nodes, not just leaves

        ////determins the axis on which to split
        //private int axis;

        ////stores the location and the data it points to
        //private Vector loc;
        //private T data;

        ////contains a link to the left and right
        //private NodeKD<T> left;
        //private NodeKD<T> right;
        
        ///////////////////////////////////////////////////////////////////////////


        ////determins the position and orientation of the hyperplane
        //private int axis;
        //private double split;

        ////determins if the node is a leaf
        //private bool leaf;

        ////contains a link to the left and right children
        ////also used to store the location and data in leaves
        //private Object left;
        //private Object right;


        private short axis;
        private Vector loc;

        private NodeKD<T> left;
        private NodeKD<T> right;

        private T value;

       


        public NodeKD(int axis, double split, NodeKD<T> left, NodeKD<T> right)
        {
            //sets the axis and the split
            this.axis = (short)axis;
            this.loc = new Vector(split);

            //sets the left and right children
            this.left = left;
            this.right = right;

            //intermediat nodes do not store value
            value = default(T);
        }

        public NodeKD(Vector loc, T value)
        {
            //this is a leaf node
            this.axis = -1;

            //stores a copy of the vector
            this.loc = new Vector(loc);
            this.value = value;

            //sets other values to default
            left = null;
            right = null;
        }

        //public NodeKD(VectorPair<T> pair)
        //{
        //    //this is a leaf node
        //    this.leaf = true;

        //    //stores the data in the left pointer
        //    this.left = pair;

        //    //sets other values to default
        //    axis = -1;
        //    split = Double.NaN;
        //    right = null;
        //}

        /// <summary>
        /// Determins if the curent node is a leaf
        /// </summary>
        public bool IsLeaf
        {
            get { return (axis < 0); }
        }

        public int Axis
        {
            get { return axis; }
        }

        public double Split
        {
            get
            {
                if (axis < 0) return 0.0;
                else return loc[0];
            }
        }

        public NodeKD<T> Left
        {
            get { return left; }
        }

        public NodeKD<T> Right
        {
            get { return right; }
        }

        public Vector Location
        {
            get 
            {
                if (axis >= 0) return null;
                else return new Vector(loc); 
            }
        }

        public T Value
        {
            get 
            {
                if (axis < 0) return value;
                else return default(T);
            }
        }

        /// <summary>
        /// Returns a refrence to the current node, nessary for
        /// implementing the Node interface.
        /// </summary>
        public NodeKD<T> Data
        {
            get { return this; }
            set { throw new InvalidOperationException(); }
        }

        //public NodeKD<T> Data
        //{
        //    get
        //    {
        //        //simply returns a refrence to itself
        //        return this;
        //    }
        //    set
        //    {
        //        //we can't change our internal data
        //        throw new InvalidOperationException();
        //    }
        //}

        /// <summary>
        /// Determins which path of the tree should be follwed, based on
        /// a point being probed.
        /// </summary>
        /// <param name="probe">Vector to probe the tree</param>
        /// <returns>The root of the path to be followed</returns>
        public NodeKD<T> Trace(Vector probe)
        {
            //If we are at a leaf, we can't trace further
            if (axis < 0) return null;

            double val = probe.GetExtended(axis);

            if (val > loc[0]) return right;
            else return left;
        }


        public NodeKD<T> GetOther(NodeKD<T> child)
        {          
            if (child == left) return right;
            if (child == right) return left;

            return null;
        }

        public double Dist(Vector probe)
        {
            if (axis < 0)
            {
                //if we are a leaf, just return the distance to the probe
                return loc.Dist(probe);
            }
            else
            {
                //computes the shortest distance to the hyper plane
                double val = probe.GetExtended(axis);
                return Math.Abs(val - loc[0]);
            }
        }


        /// <summary>
        /// Returns the non-null children of this node. Left comes 
        /// before right, if the node has two children.
        /// </summary>
        /// <returns>List of the node's children</returns>
        public IEnumerable<Node<NodeKD<T>>> ListChildren()
        {
            //by definition, leaves do not have children
            if (axis < 0) yield break;

            //returns the left and right children, if they exist
            if (left != null) yield return left;
            if (right != null) yield return right;
        }

        /// <summary>
        /// Removes all linked references from this node in order to avoid
        /// memory leaks. This should only be called when the node structure
        /// is being broken down.
        /// </summary>
        public void Dispose()
        {
            left = null;
            right = null;
        }
    }
}
