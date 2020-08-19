/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2020 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// This class allows for multiple results to be returned from a single, iteritive,
    /// algorythim. In short, multiple results are compined with a single error term
    /// which respesnts the precision (not acuracy) of the combined agrigate results.
    /// For full details, please see the "Result" class.
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    public class ResultMulti<T> : IFormattable, IEnumerable<T>, IComparable<ResultMulti<T>> 
    {
        #region Class Definitions...

        //stores a refrence to the result
        private T[] results;

        //the amount of error in the result
        private double error;

        //stores the number of itterations
        private int count;

        /// <summary>
        /// Contsrutcs a new result with the given error value
        /// and iteration count
        /// </summary>
        /// <param name="vals">List of values in the result</param>
        /// <param name="err">Amount of error in the result</param>
        /// <param name="count">Number of interations used</param>
        public ResultMulti(IEnumerable<T> vals, double err, int count)
        {
            this.results = vals.ToArray();
            this.error = Math.Abs(err);
            this.count = Math.Abs(count);
            
        }

        /// <summary>
        /// constructs a new result which is concluded to have infinite
        /// error, and has taken no time to compute.
        /// </summary>
        /// <param name="val">Value of the result</param>
        public ResultMulti(IEnumerable<T> vals)
        {
            this.results = vals.ToArray();
            this.error = Double.PositiveInfinity;
            this.count = 0;          
        }

        /// <summary>
        /// Reports the total error for the agrigate result. Tipicaly the
        /// error is reletive when the value is large, and absolute when the
        /// value is small, although the exact interpritaiton can varry.
        /// </summary>
        /// <returns>The result as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("g10", null);
        }

        /// <summary>
        /// Reports the total error for the agrigate result. Tipicaly the
        /// error is reletive when the value is large, and absolute when the
        /// value is small, although the exact interpritaiton can varry.
        /// It passes on the formating information if it is able.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The result as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            string s = error.ToString(format, provider);
            return String.Format("Error: {0}", s);
        }

        /// <summary>
        /// Determins if this result is equal to another result. Two results 
        /// are considered the same if they have the same value and the same 
        /// amount of error.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            //determins if the other object is a multi result
            var other = obj as ResultMulti<T>;
            if (other == null) return false;

            //determins if both objects have the same number of results
            if (results.Length != other.results.Length) return false;

            //determins if the error terms are equal
            if (!error.Equals(other.error)) return false;

            //determins if all the results are equal
            for (int i = 0; i < results.Length; i++)
            {
                T parA = this.results[i];
                T parB = other.results[i];
                if (!parA.Equals(parB)) return false;
            }

            //the items must be equal
            return true;
        }

        /// <summary>
        /// Generates a hash code for the curent result, constructed from
        /// the values of the result, and the amound of error in the result.
        /// </summary>
        /// <returns>The hash of the result</returns>
        public override int GetHashCode()
        {          
            int hash = error.GetHashCode();
            int temp = 0;

            for (int i = 0; i < results.Length; i++)
            {
                unchecked
                {
                    temp = results[i].GetHashCode();
                    hash ^= (hash << 5) + (hash >> 2) + temp;
                }
            }

            return hash;
        }

        /// <summary>
        /// Compares the amount of error in the current result to another
        /// result. It returns a negative value if the curent result is more 
        /// actuate, a positive value if it is less accurate, and zero if
        /// they are the same.
        /// </summary>
        /// <param name="other">A result to compare</param>
        /// <returns>See description</returns>
        public int CompareTo(ResultMulti<T> other)
        {
            if (this.error < other.error) return -1;
            if (this.error > other.error) return 1;

            return 0;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The amount error between the generated value and the previous value.
        /// Tipicaly the error is reletive when the value is large, and absolute 
        /// when the value is small, although the exact interpritaiton can varry.
        /// </summary>
        public double Error
        {
            get { return error; }
        }

        /// <summary>
        /// The number of itterations indicate how long it took to compute the given 
        /// result. Usualy, the number of itterations is prepottional to the number 
        /// of funciton invocations involved, although the exact interpritation 
        /// can varry.
        /// </summary>
        public int Iterations
        {
            get { return count; }
        }

        /// <summary>
        /// The number of resultant values in this multi-result.
        /// </summary>
        public int NumResults
        {
            get { return results.Length; }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Result Acess...

        /// <summary>
        /// Allows for indivitual access to the resultant values, by index.
        /// </summary>
        /// <param name="index">Index of the resultant value</param>
        /// <returns>The desired resultant</returns>
        public T GetResult(int index)
        {
            if (index < 0 || index >= results.Length)
                throw new ArgumentOutOfRangeException();

            return results[index];
        }

        /// <summary>
        /// Enumerates all the resultant vlaues in this multi-result.
        /// </summary>
        /// <returns>A list of all resultant values</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < results.Length; i++)
            {
                yield return results[i];
            }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the CompareTo() function
        public static bool operator >(ResultMulti<T> a, ResultMulti<T> b)
        { return a.CompareTo(b) > 0; }

        //refferences the CompareTo() function
        public static bool operator <(ResultMulti<T> a, ResultMulti<T> b)
        { return a.CompareTo(b) < 0; }

        //refferences the CompareTo() function
        public static bool operator >=(ResultMulti<T> a, ResultMulti<T> b)
        { return a.CompareTo(b) >= 0; }

        //refferences the CompareTo() function
        public static bool operator <=(ResultMulti<T> a, ResultMulti<T> b)
        { return a.CompareTo(b) <= 0; }

        #endregion /////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}
