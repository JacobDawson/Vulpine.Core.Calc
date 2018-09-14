/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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
