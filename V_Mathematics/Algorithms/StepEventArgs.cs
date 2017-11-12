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
    /// This class contains a collection of event arguments to be passed to some 
    /// event listner handeling the iteration of some numerical process. It contains 
    /// values to indicated the curent state of the numerical process, as well as 
    /// a halting flag to indicate if the proceess should end.
    /// </summary>
    /// <remarks>Last Update: 2017-01-26</remarks>
    public class StepEventArgs : EventArgs
    {
        //contains the info from the event
        private double error;
        private int step;

        //uses a flag to indicate when to halt
        private bool halt;

        /// <summary>
        /// Creates a new collection of numeric event arugments, containing
        /// the given arguments passed in to the constructor.
        /// </summary>
        /// <param name="step">Number of iterations preformed</param>
        /// <param name="error">Current error value</param>
        internal StepEventArgs(int step, double error)
        {
            this.step = step;
            this.error = error;
            this.halt = false;
        }

        /// <summary>
        /// The amount of error calculated between the curent value
        /// generated and the previous value generated.
        /// </summary>
        public double Error
        {
            get { return error; }
        }

        /// <summary>
        /// The number of iterations preformed so far.
        /// </summary>
        public int Step
        {
            get { return step; }
        }

        /// <summary>
        /// A flag indicating if the process should halt. The flag can be
        /// raised by setting it to true, but it cannot be unraised.
        /// </summary>
        public bool Halt
        {
            get { return halt; }
            set { halt |= value; }
        }
    }
}
