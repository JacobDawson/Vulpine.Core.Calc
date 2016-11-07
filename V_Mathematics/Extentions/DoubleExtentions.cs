using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Extentions
{
    public static class DoubleExtentions
    {
        public static bool IsNaN(this double x)
        {
            return Double.IsNaN(x);
        }

        public static bool IsInfinity(this double x)
        {
            return Double.IsInfinity(x);
        }

        public static bool IsANumber(this double x)
        {
            return VMath.IsANumber(x);
        }

        public static bool IsZero(this double x)
        {
            return VMath.IsZero(x);
        }
    }
}
