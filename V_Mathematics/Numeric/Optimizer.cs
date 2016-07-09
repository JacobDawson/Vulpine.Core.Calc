using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Numeric
{
    public class Optimizer : Algorithim
    {
        public Optimizer(int max = DMAX, double tol = DTOL)
        {
            //sets the tolerance and max itterations
            base.max = (max > 0) ? max : 1;
            base.tol = (tol >= 0.0) ? tol : 0.0;

            //initialises the cotroler
            Initialise();
        }
    }
}
