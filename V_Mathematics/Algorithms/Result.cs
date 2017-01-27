/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
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
    public struct Result<T> : IFormattable
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
    }
}
