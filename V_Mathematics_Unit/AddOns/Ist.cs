using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Vulpine_Core_Calc_Tests.AddOns
{
    public static class Ist
    {
        public static ErrorConstraint WithinTolOf
            (object expected, double tollerence)
        {
            return new ErrorConstraint(expected, tollerence);
        }

        public static Constraint WithinTolOf
            (this ConstraintExpression exp, object expected, double tollerence)
        {
            return exp.Matches(new ErrorConstraint(expected, tollerence));
        }

        public static ZeroConstraint Zero()
        {
            return new ZeroConstraint();
        }

        public static Constraint Zero
            (this ConstraintExpression exp)
        {
            return exp.Matches(new ZeroConstraint());
        }
    }

    
}
