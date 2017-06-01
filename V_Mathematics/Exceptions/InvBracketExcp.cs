﻿/**
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

namespace Vulpine.Core.Calc.Exceptions
{
    /// <summary>
    /// Several numerical methods require that a search bracket be spesified. All
    /// search brackets consist of an upper and lower bound, where the upper bound
    /// is strictly larger than the lower. In adition, threre may be other constraints
    /// placed on the search bracket. For instance, it may be required that the
    /// search bracket contain atleast one solution to a given function. This is the
    /// exception thrown whenever the search bracket proves to be inadiquate.
    /// </summary>
    /// <remarks>Last Update: 2014-01-04</remarks>
    public class InvBracketExcp : MathematicsExcp
    {
        #region Class Definitions...

        /// <summary>
        /// The default message generated by an invalid bracket exception, 
        /// prior to applying any special case formating.
        /// </summary>
        public const string MSG = "The bracket [{0} - {1}] is invalid "
            + "for the current opperaiton.";

        //stores a copy of the search bracket
        private double low;
        private double high;

        /// <summary>
        /// Constructs a new invalid bracket excpetion, passing on the values
        /// for the upper and lower bounds of the bracket.
        /// </summary>
        /// <param name="low">Lower bound of the bracket</param>
        /// <param name="high">Upper bound of the bracket</param>
        protected InvBracketExcp(double low, double high) : base()
        {
            this.low = low;
            this.high = high;
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Generates a custom message, indicating the nature of the exception
        /// as well as the upper and lower bounds of the search bracket.
        /// Read-Only
        /// </summary>
        public override string Message
        {
            get { return String.Format(MSG, low, high); }
        }

        /// <summary>
        /// The reported lower bound of the search bracket. This may not be
        /// strickly lower than the upper bound, pending on the nature of
        /// the exception. Read-Only
        /// </summary>
        public double Lower
        {
            get { return low; }
        }

        /// <summary>
        /// The reported upper bound of the search bracket. This may not be
        /// strickly higher than the lower bound, pending on the nature of
        /// the exception. Read-Only
        /// </summary>
        public double Upper
        {
            get { return high; }
        }

        #endregion /////////////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Creates and throws an invalid bracket exception, passing on the
        /// reported lower and upper bounds of the bracket.
        /// </summary>
        /// <param name="low">Lower bound of the bracket</param>
        /// <param name="high">Upper bound of the bracket</param>
        public static void Throw(double low, double high)
        {
            //throws an exception if the check condtions are not met
            throw new InvBracketExcp(low, high);
        }

        /// <summary>
        /// Checks that the lower bound is indead lower than the upper
        /// bound for the bracket. If this is not the case an invalid
        /// bracket exception is thrown. Otherwise, nothing happens.
        /// </summary>
        /// <param name="low">Lower bound of the bracket</param>
        /// <param name="hi">Upper bound of the bracket</param>
        public static void CheckOrder(double low, double high)
        {
            //throws an exception if the check condtions are not met
            if (low >= high) throw new InvBracketExcp(low, high);
        }

        #endregion /////////////////////////////////////////////////////////////////////
    }
}
