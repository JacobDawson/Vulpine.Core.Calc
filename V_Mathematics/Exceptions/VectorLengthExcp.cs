﻿/**
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

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Exceptions
{
    /// <summary>
    /// Many operations in the mathematics library, paticulary those pertaning
    /// to vectors and matricies, require either a spicific number of arguments or 
    /// a vector of a paticular size. This is the exception thrown when the length 
    /// of the vector given dose not match what was expected.
    /// </summary>
    /// <remarks>Last Update: 2014-01-03</remarks>
    public class VectorLengthExcp : MathematicsExcp
    {
        #region Class Definitions...

        /// <summary>
        /// The default message generated by a vector length exception, prior to 
        /// applying any special case formating.
        /// </summary>
        public const string MSG = "A vector of length {0} was given, when a "
            + "vector of length {1} was expected.";

        //stores the size of the vectors involved
        private int expected;
        private int actual;

        /// <summary>
        /// Generates a new vector size exception, passing on both the expected
        /// size as well as the size that was actualy given.
        /// </summary>
        /// <param name="acc">The actual size</param>
        /// <param name="exp">The excpected size</param>
        protected VectorLengthExcp(int acc, int exp) : base()
        {
            this.expected = exp;
            this.actual = acc;
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Generates a custom message, indicating the nature of the exception
        /// as well as the excpected vector length. Read-Only
        /// </summary>
        public override string Message
        {
            get { return String.Format(MSG, actual, expected); }
        }

        /// <summary>
        /// The expected size of the vector. Read-Only
        /// </summary>
        public int ExpectedLength
        {
            get { return expected; }
        }

        /// <summary>
        /// The actual size of the vector. Read-Only
        /// </summary>
        public int ActualLength
        {
            get { return actual; }
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Determins if the actual length given matches the expected length, if it
        /// dose not it throws a vector length exception. Otherwise, it dose nothing.
        /// </summary>
        /// <param name="actual">Length of the given vector</param>
        /// <param name="expected">The expected length</param>
        public static void Check(int actual, int expected)
        {
            //throws an exception if the check condtions are not met
            if (actual != expected) throw new VectorLengthExcp(actual, expected);
        }

        /// <summary>
        /// Determins if the length of the vector given matches the excpeted length,
        /// if it dose not it throws a vector length exception. Otherwise, it dose
        /// nothing.
        /// </summary>
        /// <param name="actual">The given vector</param>
        /// <param name="expected">The expected length</param>
        public static void Check(Vector actual, int expected)
        {
            //throws an exception if the check condtions are not met
            if (actual.Length != expected)
            throw new VectorLengthExcp(actual.Length, expected);
        }


        /// <summary>
        /// Determins if the actual length given matches one of two expected lengths.
        /// If it dose not match either, it throws a vector length exception, with the
        /// maximum of the two values as the expected value.
        /// </summary>
        /// <param name="actual">Length of the given vector</param>
        /// <param name="e1">One of two acceptable lengths</param>
        /// <param name="e2">Another acceptable length</param>
        public static void Check(int actual, int e1, int e2)
        {
            //throws an exception if the check condtions are not met
            if (actual != e1 && actual != e2)
            throw new VectorLengthExcp(actual, Math.Max(e1, e2));
        }

        /// <summary>
        /// Determins if the length of the vector given matches one of two expected lengths.
        /// If it dose not match either, it throws a vector length exception, with the
        /// maximum of the two values as the expected value.
        /// </summary>
        /// <param name="actual">Length of the given vector</param>
        /// <param name="e1">One of two acceptable lengths</param>
        /// <param name="e2">Another acceptable length</param>
        public static void Check(Vector actual, int e1, int e2)
        {
            //throws an exception if the check condtions are not met
            if (actual.Length != e1 && actual.Length != e2)
            throw new VectorLengthExcp(actual.Length, Math.Max(e1, e2));
        }

        #endregion /////////////////////////////////////////////////////////////////////
    }
}
