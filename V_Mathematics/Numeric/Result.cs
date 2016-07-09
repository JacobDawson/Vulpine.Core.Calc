using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Numeric
{
    /// <summary>
    /// When dealing with numerical methods, values are seldom exact. Nearly
    /// every value reported has some inherent error associated with it. This
    /// struct provides a way to report a value with it's error, when the
    /// suspected amount of error is known. Tipicaly the reletive error is
    /// used, rather than the exact, as it is a beter indication of the level
    /// of precision in the result.
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    /// <remarks>Last Update: 2013-11-17</remarks>
    public struct Result<T>
    {
        #region Class Definitions...

        //stores a refrence to the result
        private T result;

        //the amount of error in the result
        private double error;

        /// <summary>
        /// Constructs a result-error pair with the given amount of implied
        /// error in the result. The error should be in relitive terms.
        /// </summary>
        /// <param name="val">The value computed</param>
        /// <param name="err">The implied error in the value</param>
        public Result(T val, double err)
        {
            this.result = val;
            this.error = Math.Abs(err);
        }

        /// <summary>
        /// Constructs a result-error pair where the result is known to
        /// be exact. In other words, the result has zero error.
        /// </summary>
        /// <param name="val">The exact value</param>
        public Result(T val)
        {
            this.result = val;
            this.error = 0.0;
        }

        /// <summary>
        /// Reports the computed value, allong with the relitive amount
        /// of error it is reported to contain.
        /// </summary>
        /// <returns>The result-error pair as a string</returns>
        public override string ToString()
        {
            //reports the result, along with it's relitive error
            return String.Format("<{0} err: {1}>", result, error);
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The computed value. Read-Only
        /// </summary>
        public T Value
        {
            get { return result; }
        }

        /// <summary>
        /// The relitive amount of error reported to be comtained in the
        /// value. Relitive errors are related to the size of the result,
        /// and not just absolute diffrence. Read-Only
        /// </summary>
        public double Error
        {
            get { return error; }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Conversions...

        //allows the value to be extracted implicitly
        public static implicit operator T(Result<T> val)
        { return val.result; }

        #endregion /////////////////////////////////////////////////////////////
    }
}
