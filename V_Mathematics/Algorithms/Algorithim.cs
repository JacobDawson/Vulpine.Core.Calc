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

        //determins how the error value is measured
        protected ErrorMeasure measure;
        
        /// <summary>
        /// Creates a new Algorithim object with default stoping criteria.
        /// </summary>
        public Algorithm()
        {
            this.max = 1024;
            this.tol = VMath.TOL;
            this.measure = ErrorMeasure.Augmented;

            Initialise();
        }

        /// <summary>
        /// Creates a new Algorithim object with the given maximum number of
        /// itterations and the minimal augmented error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Algorithm(int max, double tol)
        {
            this.max = (max > 2) ? max : 2;
            this.tol = Math.Abs(tol);
            this.measure = ErrorMeasure.Augmented;

            Initialise();
        }

        /// <summary>
        /// Creates a new Algorythim object with the given maximum number of
        /// itterations, the minimial amount of error allowed, and the error
        /// measument used in calculations
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="em">Error measurment used in calculations</param>
        public Algorithm(int max, double tol, ErrorMeasure em)
        {
            this.max = (max > 2) ? max : 2;
            this.tol = Math.Abs(tol);
            this.measure = em;

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

            //NOTE: Should include the error measure

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

            //NOTE: Should include the error measure

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

        /// <summary>
        /// Represents how the error value is tipicaly calculated. Certain algorythims
        /// may ignore this procedure and substitute their own error value.
        /// </summary>
        public ErrorMeasure Measure
        {
            get { return measure; }
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
            this.error = Math.Abs(error);

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

            //finds the distance between the curent value and the last
            double dist = Math.Abs(curr - last);

            //computes the error value, based on the current measure
            switch (measure)
            {
                case ErrorMeasure.Absolute: 
                    error = dist; break;
                case ErrorMeasure.Relative:
                    error = dist / Math.Abs(curr); break;
                case ErrorMeasure.Augmented:
                    error = dist / (Math.Abs(curr) + 1.0); break;
            }

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

            //finds the distance between the curent value and the last
            double dist = curr.Dist(last);

            //computes the error value, based on the current measure
            switch (measure)
            {
                case ErrorMeasure.Absolute:
                    error = dist; break;
                case ErrorMeasure.Relative:
                    error = dist / curr.Norm(); break;
                case ErrorMeasure.Augmented:
                    error = dist / (curr.Norm() + 1.0); break;
            }

            //determins if sucessive itterations are nessary
            if (error.IsNaN()) return true;
            if (error <= tol) return true;
            if (count >= max) return true;

            return false;     
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
            //compares error values to see if the result passes
            bool pass = (error <= tol) && error.IsANumber();

            //returns the generated result with the error value
            return new Result<T>(value, error, count, pass);
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
            //returns the generated result
            return new ResultMulti<T>(values, error, count);
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
