/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// When dealing with numerical methods, particulary itterative methods, values 
    /// are seldom exact. Practaily every value reported has some inherent error 
    /// associated with it, no mater how small. This struct provides a way to report 
    /// the error allong with a given value. It also reports the number of itterations
    /// it took to compute the result when using iterative numerical methods.
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    /// <remarks>Last Update: 2017-01-26</remarks>
    public struct Result<T> : IFormattable, IComparable<Result<T>>
    {
        #region Class Definitions...

        //stores a refrence to the result
        private T result;

        //the amount of error in the result
        private double error;

        //stores the number of itterations
        private int count;

        /// <summary>
        /// Constructs a new result with the given error value and iteration
        /// count. These values should be computed by the Algorithm class. 
        /// </summary>
        /// <param name="val">Value of the result</param>
        /// <param name="err">Amount of error in the result</param>
        /// <param name="count">Number of interations used</param>
        internal Result(T val, double err, int count)
        {
            this.result = val;
            this.error = Math.Abs(err);
            this.count = count;
        }

        internal Result(T val)
        {
            this.result = val;
            this.error = Double.PositiveInfinity;
            this.count = 0;
        }

        /// <summary>
        /// Reports the computed value plus or minius the associated absolute
        /// error, indicating a range of values where the true value is
        /// expected to lie.
        /// </summary>
        /// <returns>The result as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("g10", null);
        }

        /// <summary>
        /// Reports the computed value plus or minius the associated absolute
        /// error, indicating a range of values where the true value is
        /// expected to lie. It passes on the formating information to the
        /// corisponding vlaue if it is able.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The result as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            var temp = result as IFormattable;
            string s1, s2;

            if (temp == null) s1 = result.ToString();
            else s1 = temp.ToString(format, provider);

            s2 = error.ToString(format, provider);

            return String.Format("res:<{0}>, err:{1}", s1, s2);
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
            if (obj is Result<T>)
            {
                var other = (Result<T>)obj;

                if (!result.Equals(other.result)) return false;
                if (!error.Equals(other.error)) return false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a hash code for the curent result, constructed from
        /// the value of the result, and the amound of error in the result.
        /// </summary>
        /// <returns>The hash of the result</returns>
        public override int GetHashCode()
        {
            int a1 = result.GetHashCode();
            int a2 = error.GetHashCode();

            return unchecked((a1 * 907) ^ a2);
        }

        /// <summary>
        /// Compares the amount of error in the current result to another
        /// result. It returns a negative value if the curent result is more 
        /// actuate, a positive value if it is less accurate, and zero if
        /// they are the same.
        /// </summary>
        /// <param name="other">A result to compare</param>
        /// <returns>See description</returns>
        public int CompareTo(Result<T> other)
        {
            if (this.error < other.error) return -1;
            if (this.error > other.error) return 1;

            return 0;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The computed value. 
        /// </summary>
        public T Value
        {
            get { return result; }
        }

        /// <summary>
        /// The amount error between the generated value and the previous value.
        /// Wheather the error indicated is the reletive error or absolute error,
        /// is dependent upon the generating algorythim.
        /// </summary>
        public double Error
        {
            get { return error; }
        }

        /// <summary>
        /// The number of itterations used to compute the given result. What
        /// qualifies as an iteration is determined by the spesific algroythim
        /// that generated the result.
        /// </summary>
        public int NumSteps
        {
            get { return count; }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Allows the value of the result to be automaticly extracted
        /// whenever the result is used as it's internal type.
        /// </summary>
        public static implicit operator T(Result<T> val)
        { 
            return val.result; 
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the CompareTo() function
        public static bool operator >(Result<T> a, Result<T> b)
        { return a.CompareTo(b) > 0; }

        //refferences the CompareTo() function
        public static bool operator <(Result<T> a, Result<T> b)
        { return a.CompareTo(b) < 0; }

        //refferences the CompareTo() function
        public static bool operator >=(Result<T> a, Result<T> b)
        { return a.CompareTo(b) >= 0; }

        //refferences the CompareTo() function
        public static bool operator <=(Result<T> a, Result<T> b)
        { return a.CompareTo(b) <= 0; }

        #endregion /////////////////////////////////////////////////////////////

    }
}
