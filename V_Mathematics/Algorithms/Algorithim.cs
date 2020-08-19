/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
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

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// Several of the numerical methods presented in the mathmatics libarary
    /// use an iterative process to compute their output. This class serves as
    /// a meta-class for all these operations. It provides stoping criteria for
    /// the iterative methods, cuting down on the number of paramaters that must
    /// be specified at invocation. An iterative process will terminate whenever 
    /// the resulting error drops below a set tollerance, or it exausts the 
    /// maximum number of itterations. By defalut, reletive error is used when
    /// the result is large, and absolute error is used when the result is small. 
    /// </summary>
    /// <remarks>Last Update: 2017-01-26</remarks>
    public abstract class Algorithm
    {
        #region Class Definitions...

        //represents the ending conditions
        protected int max;
        protected double tol;

        //tracks the curent state of the algorythim
        protected int count;
        protected double error;

        //stores the delegate to handel step events
        private EventHandler<StepEventArgs> e_step;

        //stores the delegates for the start and finish events
        private EventHandler e_start;
        private EventHandler e_finish;

        /// <summary>
        /// Creates a new Algorithim object with default stoping criteria.
        /// </summary>
        public Algorithm()
        {
            this.max = 1024;
            this.tol = VMath.TOL;

            Initialise();
        }

        /// <summary>
        /// Creates a new Algorithim object with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Algorithm(int max, double tol)
        {
            this.max = (max > 2) ? max : 2;
            this.tol = Math.Abs(tol);

            Initialise();
        }

        /// <summary>
        /// Generates a string representaiton of the curent Algorithim instance,
        /// reporting both it's underlying type and it's meta paramaters, sutch as
        /// the minimum error tollarance and maximum number of itterations.
        /// </summary>
        /// <returns>The Algorithim as a string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type t = base.GetType();

            sb.AppendFormat("{0}: ", t.Name);
            sb.AppendFormat("Max={0}, ", max);
            sb.AppendFormat("Tol={0:g5}", tol);

            return sb.ToString();
        }

        /// <summary>
        /// Determins if two instances of the Algorithim class are equal. Two
        /// instances are considered equal if they are of the same underlying
        /// type and share the same meta-paramaters.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            Algorithm other = obj as Algorithm;
            if (other == null) return false;

            Type t1 = this.GetType();
            Type t2 = other.GetType();

            bool test = t1.Equals(t2);
            test &= (this.max == other.max);
            test &= (this.tol == other.tol);

            return test;
        }

        /// <summary>
        /// Generates a sudo-unique hash code for the curent Alogrithim instance, 
        /// by combining it's meta paramaters: the maximum number of itterations 
        /// with the minimum tollarance value. 
        /// </summary>
        /// <returns>The hash of the current Algorithim</returns>
        public override int GetHashCode()
        {
            int h = max.GetHashCode();
            int a1 = tol.GetHashCode();

            return unchecked((h * 2243) ^ a1);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the maximum number of iterations. All iterative methods 
        /// will return a solution once the maximum number of iterations are 
        /// exausted, weather or not tollerance has been met.
        /// </summary>
        public int MaxSteps
        {
            get { return max; }
        }

        /// <summary>
        /// Represents the amount of error tolerance in the output. By defalut
        /// reletive error is used when the result is large, and absolute error
        /// is used when the result is small. 
        /// </summary>
        public double Tolerance
        {
            get { return tol; }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Events...

        /// <summary>
        /// The step event is invoked every time the algorythim updates it's
        /// error values. The event can be used to impart aditional stoping
        /// criteria, by rasing the Halt flag in the event args.
        /// </summary>
        public event EventHandler<StepEventArgs> StepEvent
        {
            add { e_step += value; }
            remove { e_step -= value; }
        }

        /// <summary>
        /// The start event is envoked as soon as the algorythim starts,
        /// before anything else happens. It's mostly included for sake of
        /// completness, but may be useful in a multi-threaded enviroment.
        /// </summary>
        public event EventHandler StartEvent
        {
            add { e_start += value; }
            remove { e_start -= value; }
        }

        /// <summary>
        /// The finish event is envoked once the algorythim reaches a
        /// terminal condition, allowing the end user to prefrom any 
        /// nessary post processing.
        /// </summary>
        public event EventHandler FinishEvent
        {
            add { e_finish += value; }
            remove { e_finish -= value; }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Protected Methods...

        /// <summary>
        /// Intiialises the algorithim controler to the state it should be
        /// in prior to invoking any iterative procedure. This method should
        /// be called before, and only before, running each new procedure.
        /// </summary>
        protected void Initialise()
        {
            //invokes any starting events that are regesterd
            if (e_start != null) e_start(this, EventArgs.Empty);

            //Initialises the algorythim for a new run
            error = Double.PositiveInfinity;
            count = 0;
        }

        /// <summary>
        /// Increments the algorithim controler several steps at once, without
        /// updating the error value. This is usefull for procedures that must 
        /// run intermediat steps.
        /// </summary>
        /// <param name="steps">Number of steps to advance</param>
        /// <returns>True if the algorythim should stop</returns>
        protected bool Increment(int steps)
        {
            if (steps < 1) steps = 1;
            count = count + steps;

            return (count >= max);
        }

        /// <summary>
        /// Increments the algorythim controler by one step, using the provided
        /// error. This is useful for algorythims which run there own error
        /// calculations, which may be more effecent than the built in calculations. 
        /// </summary>
        /// <param name="error">Calculated error in the current step</param>
        /// <returns>True if the algorythim should stop</returns>
        protected bool Step(double error)
        {
            //updates the count and the error
            this.count = count + 1;
            this.error = error;

            //informs the listeners & checks for halting
            if (OnStep(count, error)) return true;

            //determins if sucessive itterations are nessary
            if (error.IsNaN()) return true;
            if (error <= tol) return true;
            if (count >= max) return true;

            return false;
        }

        /// <summary>
        /// Increments the algorithim controler by one step. It updates
        /// the error value and determins if it is nessary to continue
        /// running the procedure.
        /// </summary>
        /// <param name="last">The last value computed</param>
        /// <param name="curr">The curent value computed</param>
        /// <returns>True if no further iterations are required</returns>
        protected bool Step(double last, double curr)
        {
            //increments the count
            count = count + 1;

            //computes the error value
            double dist = Math.Abs(curr - last);
            error = dist / (Math.Abs(curr) + 1.0);

            //informs the listeners & checks for halting
            if (OnStep(count, error)) return true;

            //determins if sucessive itterations are nessary
            if (error.IsNaN()) return true;
            if (error <= tol) return true;
            if (count >= max) return true;

            return false;
        }

        /// <summary>
        /// Increments the algorithim controler by one step. It updates
        /// the error value and determins if it is nessary to continue
        /// running the procedure.
        /// </summary>
        /// <param name="last">The last value computed</param>
        /// <param name="curr">The curent value computed</param>
        /// <returns>True if no further iterations are required</returns>
        protected bool Step<T>(T last, T curr) where T : Metrizable<T>
        {
            //increments the count
            count = count + 1;

            //computes the error value
            double dist = curr.Dist(last);
            dist = dist / (curr.Norm() + 1.0);
            error = Math.Abs(dist);

            //informs the listeners & checks for halting
            if (OnStep(count, error)) return true;

            //determins if sucessive itterations are nessary
            if (error.IsNaN()) return true;
            if (error <= tol) return true;
            if (count >= max) return true;

            return false;     
        }

        /// <summary>
        /// Raises the step event and informs all listeners. It returns true if
        /// any of the listners indicate that the process should stop.
        /// </summary>
        /// <param name="step">Curent number of iterations preformed</param>
        /// <param name="error">Current calculated error value</param>
        /// <returns>True if the process should stop</returns>
        private bool OnStep(int step, double error)
        {
            //checks that we actualy have someone listening
            if (e_step == null) return false;

            //creates new event args and invokes the event
            var args = new StepEventArgs(step, error);
            e_step(this, args); return args.Halt;
        }

        /// <summary>
        /// Uses the given value to generate a result, reporting the curent
        /// error value and number of iterations along with it. This method
        /// should be called at the end of each procedure, before returning.
        /// </summary>
        /// <param name="value">The final value of computation</param>
        /// <returns>The packaged result</returns>
        protected Result<T> Finish<T>(T value)
        {
            //invokes any finishing events that are regesterd
            if (e_finish != null) e_finish(this, EventArgs.Empty);

            //returns the generated result
            return new Result<T>(value, error, count);
        }

        /// <summary>
        /// Uses multiple values to generate a result, reporting the curent
        /// error value and number of iterations along with them. This method
        /// should be called at the end of each procedure, before returning. 
        /// </summary>
        /// <param name="value">The final value of computation</param>
        /// <returns>The packaged result</returns>
        protected ResultMulti<T> Finish<T>(IEnumerable<T> values)
        {
            //invokes any finishing events that are regesterd
            if (e_finish != null) e_finish(this, EventArgs.Empty);

            //returns the generated result
            return new ResultMulti<T>(values, error, count);
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
