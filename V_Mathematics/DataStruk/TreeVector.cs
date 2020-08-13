using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Data
{

    /// <summary>
    /// A Vector Tree is a special type of tree-like data structor, that uses vectors to
    /// reference it's elements. Unlike a traditonal data structor, where keys are used
    /// to retieve their matching elements, a vector tree is used to find the closest
    /// matching element to a given probe. This is usefull when you have some multi-
    /// dimentional data set, and you want to find the closest value to some given input.
    /// </summary>
    /// <typeparam name="E">Element type of the vector tree</typeparam>
    public abstract class TreeVector<E> : IEnumerable<VectorPair<E>>
    {
        #region Class Properties...

        /// <summary>
        /// Determins if it is nessary to (re)build the data structor, in order
        /// to get the maximum efficent search time.
        /// </summary>
        public abstract bool BuildRequired { get; }

        /// <summary>
        /// Determins the number of points or vectors contained within
        /// this data structur.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Determines if the tree is empty or contains items. It is
        /// set to true if empty and false if otherwise.
        /// </summary>
        public virtual bool Empty
        {
            get { return Count <= 0; }
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Matinence Operaitons...

        /// <summary>
        /// Preemptively adds a new vector pair to the data structor. Note that
        /// this insertion is not garenteed to take effect untill after the data
        /// structor has been (re)built.
        /// </summary>
        /// <param name="loc">Location of the data as a vector</param>
        /// <param name="data">Data pointed to by the vector</param>
        public abstract void Add(Vector loc, E data);

        /// <summary>
        /// Builds the internal data structor used to preform fast searches. Certain
        /// methods may not work until this method is called at least once. If any
        /// insertions are made between method calls, it may be nessary to rebuild
        /// the data structor for those insertions to take effect.
        /// </summary>
        public abstract void Build();   

        /// <summary>
        /// Creates an enumeration of all the vectors and their assigned values in
        /// the vector tree. If you are intrested in only the vectors or their values
        /// consider using one of the other methods.
        /// </summary>
        /// <returns>An enumeration of vector pairs</returns>
        public abstract IEnumerator<VectorPair<E>> GetEnumerator();

        /// <summary>
        /// Clears the tree of all data, returning it to it's original state.
        /// </summary>
        public abstract void Clear();

        #endregion /////////////////////////////////////////////////////////////////////

        #region Tree Vector Operations...

        /// <summary>
        /// Obtains the vector pair closest to the given probe position.
        /// </summary>
        /// <param name="probe">Vector to probe the data set</param>
        /// <returns>The vector pair closest to the given probe</returns>
        /// <exception cref=" InvalidOperationException">If this method is
        /// called before the internal data strucur is built</exception>
        public abstract VectorPair<E> GetNearest(Vector probe);

        /// <summary>
        /// Obtains the top N closest vector pairs to the given probe positon,
        /// where N is the number of desired pairs the method should return.
        /// </summary>
        /// <param name="probe">Vector to probe the data set</param>
        /// <param name="count">Number of vector pairs to return</param>
        /// <returns>The top N closest vector pairs</returns>
        /// <exception cref=" InvalidOperationException">If this method is
        /// called before the internal data strucur is built</exception>
        public abstract IEnumerable<VectorPair<E>> GetNearest(Vector probe, int count);

        #endregion /////////////////////////////////////////////////////////////////////

        #region Additional Opperations...

        /// <summary>
        /// Lists all the values contained within the tree.
        /// </summary>
        /// <returns>All the values of the table</returns>
        public virtual IEnumerable<E> ListValues()
        {
            //returns only the values from the default enumerator
            foreach (var temp in this) yield return temp.Value;
        }

        /// <summary>
        /// Lists all the vectors contained within the tree.
        /// </summary>
        /// <returns>All the keys of the table</returns>
        public virtual IEnumerable<Vector> ListVectors()
        {
            //returns only the keys from the default enumerator
            foreach (var temp in this) yield return temp.Location;
        }

        #endregion /////////////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }


}
