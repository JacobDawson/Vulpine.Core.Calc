using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public abstract class TreeVector<E>
    {
        /// <summary>
        /// Determins if it is nessary to (re)build the data structor, in order
        /// to get the maximum efficent search time.
        /// </summary>
        public abstract bool BuildRequired { get; }

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
    }


}
