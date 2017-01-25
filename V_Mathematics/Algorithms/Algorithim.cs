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
    /// <remarks>Last Update: 2013-11-17</remarks>
    public abstract class Algorithm
    {
        #region Constant Values...

        /// <summary>
        /// Represents the default maximum number of itterations, set for
        /// numerical methods when no explicit maximum is given. By default
        /// we allow up to 1000 iterations before terminating.
        /// </summary>
        public const int DMAX = 1000;

        /// <summary>
        /// Represents the default tolarence in error, set for numerical methods
        /// when no explicit tolarence is given. It equates to aproximatly
        /// twelve decimal digits of precision.
        /// </summary>
        public const double DTOL = 1e-12;

        #endregion //////////////////////////////////////////////////////////////

        #region Class Definitions...

        //flag indicating the use of relative error
        protected bool rel;

        //represents the ending conditions
        protected int max;
        protected double tol;

        //tracks the curent state of the algorythim
        private int count;
        private double error;

        //stores the delegate to handel step events
        private EventHandler<NumericStepEventArgs> step_event;

        /// <summary>
        /// Creates a new Algorithim object with default stoping criteria.
        /// </summary>
        public Algorithm()
        {
            this.rel = true;
            this.max = 1024;
            this.tol = VMath.TOL;
        }

        /// <summary>
        /// Creates a new Algorithim object with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial relitave error</param>
        public Algorithm(int max, double tol)
        {
            this.rel = true;
            this.max = (max > 0) ? max : 1;
            this.tol = Math.Abs(tol);
        }

        /// <summary>
        /// Creates a new Algorithim object with the given maximum number of
        /// itterations and the minimal error allowed. Set the flag to false
        /// to use exact error instead of relative error.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial relitave erro</param>
        /// <param name="rel">Flag to use relitive error</param>
        public Algorithm(int max, double tol, bool rel)
        {
            this.rel = rel;
            this.max = (max > 0) ? max : 1;
            this.tol = Math.Abs(tol);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the maximum number of iterations. All iterative methods 
        /// will return a solution once the maximum number of iterations are 
        /// exausted, weather or not tollerance has been met.
        /// </summary>
        public int MaxIters
        {
            get { return max; }
        }

        /// <summary>
        /// Represents the amount of error tolerance in the output. All iterative
        /// methods will return a solution once the reletive error in the output
        /// drops below this threshold.
        /// </summary>
        public double Tolerance
        {
            get { return tol; }
        }

        /// <summary>
        /// Determins what type of error should be used as a stoping criteria.
        /// Returns true to use releative error, and false for exact error.
        /// </summary>
        public bool UseRelative
        {
            get { return rel; }
        }

        /// <summary>
        /// The step event is invoked every time the algorythim updates it's
        /// error values. The event can be used to impart aditional stoping
        /// criteria, by rasing the Halt flag in the event args.
        /// </summary>
        public event EventHandler<NumericStepEventArgs> StepEvent
        {
            add { step_event += value; }
            remove { step_event -= value; }
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
            //Initialises the algorythim for a new run
            error = Double.PositiveInfinity;
            count = 0;
        }

        /// <summary>
        /// Increments the algorithim controler several steps at once, without
        /// updating the error value or checking stoping criteria. This is
        /// usefull for procedures that must run intermediat steps.
        /// </summary>
        /// <param name="steps">Number of steps to advance</param>
        protected void Increment(int steps)
        {
            if (steps < 1) steps = 1;
            count = count + steps;
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
            if (error < tol) return true;
            if (count > max) return true;

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
            if (error < tol) return true;
            if (count > max) return true;

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
            if (step_event == null) return false;

            //creates new event args and invokes the event
            var args = new NumericStepEventArgs(step, error);
            step_event(this, args);  return args.Halt;
        }


        protected Result<Double> Finish(double value)
        {
            //uses the norm of the value
            double abs = Math.Abs(value);

            //computes both the relitive and absolute error
            double err = rel ? error : (error / abs);
            double del = rel ? (error * abs) : error;

            //returns the generated result
            return new Result<Double>(value, err, del);
        }

        protected Result<T> Finish<T>(T value) where T : Metrizable<T>
        {
            //uses the norm of the value
            double abs = value.Norm();

            //computes both the relitive and absolute error
            double err = rel ? error : (error / abs);
            double del = rel ? (error * abs) : error;

            //returns the generated result
            return new Result<T>(value, err, del);
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
