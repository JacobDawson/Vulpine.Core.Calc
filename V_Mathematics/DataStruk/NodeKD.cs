using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Data
{
    public class NodeKD<T> : Node<VectorPair<T>>
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


        //determins the position and orientation of the hyperplane
        private int axis;
        private double split;

        //determins if the node is a leaf
        private bool leaf;

        //contains a link to the left and right children
        //also used to store the location and data in leaves
        private Object left;
        private Object right;


        public NodeKD(int axis, double split, NodeKD<T> left, NodeKD<T> right)
        {
            //this is not a leaf node
            this.leaf = false;

            //sets the axis and the split
            this.axis = axis;
            this.split = split;

            //sets the left and right children
            this.left = left;
            this.right = right;
        }

        public NodeKD(Vector loc, T data)
        {
            //this is a leaf node
            this.leaf = true;

            //stores the data in the left pointer
            this.left = new VectorPair<T>(data, loc);

            //sets other values to default
            axis = -1;
            split = Double.NaN;
            right = null;
        }

        public NodeKD(VectorPair<T> pair)
        {
            //this is a leaf node
            this.leaf = true;

            //stores the data in the left pointer
            this.left = pair;

            //sets other values to default
            axis = -1;
            split = Double.NaN;
            right = null;
        }



        public NodeKD<T> Left
        {
            get
            {
                if (leaf) return null;
                else return left as NodeKD<T>;
            }
        }

        public NodeKD<T> Right
        {
            get
            {
                if (leaf) return null;
                else return right as NodeKD<T>;
            }
        }

        public VectorPair<T> Data
        {
            get
            {
                if (!leaf) return null;
                else return left as VectorPair<T>;
            }
            set
            {
                //we arn't allowed to set the data, especialy if
                //the curent node is not a leaf
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        /// Returns the non-null children of this node. Left comes 
        /// before right, if the node has two children.
        /// </summary>
        /// <returns>List of the node's children</returns>
        public IEnumerable<Node<VectorPair<T>>> ListChildren()
        {
            //leaves, by definition, do not have children
            if (leaf) yield break;

            //converts the references
            NodeKD<T> left = this.left as NodeKD<T>;
            NodeKD<T> right = this.right as NodeKD<T>;

            //returns the non-null children
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
