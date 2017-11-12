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
    /// be specified at invocation. By default, an iterative process will
    /// terminate whenever the resulting error drops below a set tollerance,
    /// or it exausts the maximum number of itterations. In paticular, no such
    /// method is gaurenteed to find a solution.
    /// </summary>
    /// <remarks>Last Update: 2017-01-26</remarks>
    public abstract class Algorithm
    {
        #region Class Definitions...

        //flag indicating the use of relative error
        private bool rel;

        //represents the ending conditions
        private int max;
        private double tol;

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
            this.rel = true;
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
            this.rel = true;
            this.max = (max > 2) ? max : 2;
            this.tol = Math.Abs(tol);

            Initialise();
        }

        /// <summary>
        /// Creates a new Algorithim object with the given maximum number of
        /// itterations and the minimal error allowed. Set the flag to false
        /// to use exact error instead of relative error.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="rel">Flag to use relitive error</param>
        public Algorithm(int max, double tol, bool rel)
        {
            this.rel = rel;
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
            test &= (this.rel == other.rel);

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
            int a2 = rel.GetHashCode();

            unchecked
            {
                h = (h * 2243) ^ a1;
                h = (h * 5581) ^ a2;
            }

            return h;
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
        /// Represents the amount of error tolerance in the output. All iterative
        /// methods will return a solution once the computed error in the output
        /// drops below this threshold.
        /// </summary>
        public double Tolerance
        {
            get { return tol; }
        }

        /// <summary>
        /// Determins what type of error should be used as a stoping criteria.
        /// Returns true to use releative error, and false for absolute error.
        /// </summary>
        public bool IsRelative
        {
            get { return rel; }
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
            double dist = curr - last;
            if (rel) dist = dist / curr;
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
            if (rel) dist = dist / curr.Norm();
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

        #endregion //////////////////////////////////////////////////////////////
    }
}
