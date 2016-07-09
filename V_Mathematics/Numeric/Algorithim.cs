using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Numeric
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
    public abstract class Algorithim
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

        //represents the ending conditions
        protected int max;
        protected double tol;

        //tracks the curent state of the algorythim
        private int count;
        private double error;

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the maximum number of iterations. All iterative methods 
        /// will return a solution once the maximum number of iterations are 
        /// exausted, weather or not tollerance has been met. Read-Only
        /// </summary>
        public int MaxIters
        {
            get { return max; }
        }

        /// <summary>
        /// Represents the amount of error tolerance in the output. All iterative
        /// methods will return a solution once the reletive error in the output
        /// drops below this threshold. Read-Only
        /// </summary>
        public double Tolerance
        {
            get { return tol; }
        }

        /// <summary>
        /// Determins the amount of error that was reported in the very last
        /// itteration of algorithim contoler. Read-Only
        /// </summary>
        protected double CurError
        {
            get { return error; }
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
        /// <exception cref="ArgRangeExcp">If the number of steps given
        /// is less than one</exception>
        protected void Increment(int steps)
        {
            ArgRangeExcp.Check("steps", steps, 1, Int16.MaxValue);
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
            dist = dist / last;
            error = Math.Abs(dist);

            //determins if sucessive itterations are nessary
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
            double mag = last.Mag();
            error = Math.Abs(dist / mag);

            //determins if sucessive itterations are nessary
            if (error <= tol) return true;
            if (count >= max) return true;

            return false;     
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
