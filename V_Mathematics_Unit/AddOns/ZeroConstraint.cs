using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Vulpine_Core_Calc_Tests.AddOns
{
    public class ZeroConstraint : Constraint
    {
        private double tollerence;

        /// <summary>
        /// Constructs a new zero constraint with the given tollerence.
        /// </summary>
        /// <param name="tollerence">The error tollerence</param>
        public ZeroConstraint(double tollerence)
        {
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
                error = Math.Abs(a);
            }
            else if (actual is float)
            {
                double a = (float)actual;
                error = Math.Abs(a);
            }
            else
            {
                dynamic a = actual;
                double dist = a.Mag();
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
            writer.Write("within {0} error tolerance of zero", tollerence);
        }
    }
}
