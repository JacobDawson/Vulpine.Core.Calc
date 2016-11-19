using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Vulpine_Core_Calc_Tests.AddOns
{
    public class ErrorConstraint : Constraint
    {
        private object expected;
        private double tollerence;

        /// <summary>
        /// Creates a new tolerance constraint with the epected value
        /// and the given error tollerence.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="tollerence">The error tollerence</param>
        public ErrorConstraint(object expected, double tollerence)
        {
            this.expected = expected;
            this.tollerence = tollerence;
            this.actual = null;
        }

        /// <summary>
        /// Determins if a given object matches the constraint
        /// </summary>
        /// <param name="actual">Object to match</param>
        /// <returns>True if the object matches the constraint</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            double error;

            if (actual is double)
            {
                double a = (double)actual;
                double e = (double)expected;

                //computes the error value
                double dist = (a - e) / e;
                error = Math.Abs(dist);
            }
            else if (actual is float)
            {
                double a = (float)actual;
                double e = (float)expected;

                //computes the error value
                double dist = (a - e) / e;
                error = Math.Abs(dist);
            }
            else
            {
                dynamic a = actual;
                dynamic e = expected;

                //computes the error value
                double dist = a.Dist(e) / e.Mag();
                error = Math.Abs(dist);
            }

            return error < tollerence;
        }

        /// <summary>
        /// Writes a discription of the constraint
        /// </summary>
        /// <param name="writer">Used to discribe the constraint</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            string s1 = tollerence.ToString();
            string s2 = expected.ToString();
            writer.Write("within {0} error tolerance of {1}", s1, s2);
        }
    }
}
