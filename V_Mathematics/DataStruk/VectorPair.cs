using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Data
{
    /// <summary>
    /// A vector pair represents some data, and an n-dimentional vector which points to said data.
    /// For example, cities on a map and the cordinats of those cities. The vector may be a
    /// representation of the data itself, or it may be an element of some function mapping
    /// vectors to some other space. The data the vector points to may in fact be another vector.
    /// In any case, this allows users to represent elements of high-dimentional data sets. 
    /// </summary>
    /// <typeparam name="E">The internal data type</typeparam>
    public class VectorPair<E>
    {
        #region Class Definitions...

        //stores both the data and the vector that points to it
        private E value;
        private Vector vector;

        /// <summary>
        /// Creates a new vector pair, with the given data and the given vector. In
        /// this case, a copy of the vector is made, so as to not be mutable.
        /// </summary>
        /// <param name="value">Value being pointed to</param>
        /// <param name="vector">Vector pointing to the data</param>
        public VectorPair(E value, Vector vector)
        {
            //refrences the data and clones the vector
            this.value = value;
            this.vector = new Vector(vector);
        }

        /// <summary>
        /// Creates a vector pair, with the given data and the elements of the
        /// vector pointing to said data. A new vector is created in this case.
        /// </summary>
        /// <param name="value">Value being pointed to</param>
        /// <param name="vector">Vector pointing to the data</param>
        public VectorPair(E value, params double[] vector)
        {
            //refrences the data and clones the vector
            this.value = value;
            this.vector = new Vector(vector);
        }

        /// <summary>
        /// Generates a string, listing both the vector and the data it points to.
        /// </summary>
        /// <returns>The vector pair as a string</returns>
        public override string ToString()
        {
            string sdata = (value == null) ? "NULL" : value.ToString();
            return String.Format("{0} : {1}", vector, sdata);
        }

        /// <summary>
        /// Determins if two vector pairs are equivlent to each other by comparing
        /// their input vectors. The data pointed to by the vectors is not considered
        /// in this comparison. However, since vectors use floating point representation
        /// it is unlikely that two arbitrary vectors are equal.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            //makes sure the object is a keyed item
            var other = obj as VectorPair<E>;
            if (other == null) return false;

            //compares the keys
            return vector.Equals(other.vector);
        }

        /// <summary>
        /// Computes a hash code using the input vector. For this reason
        /// the internal vector must remain immutable.
        /// </summary>
        /// <returns>The hash of the input vector</returns>
        public override int GetHashCode()
        {
            //uses the hashcode of the 'immutable' vector
            return vector.GetHashCode();
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Obtains the data assigned to the curent vector.
        /// </summary>
        public E Value
        {
            get { return value; }
        }


        /// <summary>
        /// Obtains a copy of the input vector, which points to the data.
        /// Note this is only a copy. Changing it will not change the input vector.
        /// </summary>
        public Vector Location
        {
            get { return new Vector(vector); }
        }

        #endregion /////////////////////////////////////////////////////////////
    }
}
