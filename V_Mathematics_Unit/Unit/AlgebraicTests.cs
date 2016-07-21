using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;

namespace Vulpine_Core_Calc_Tests.Unit
{
    public abstract class AlgebraicTests : EuclideanTests
    {
        /// <summary>
        /// Obtains the multiplicitive ininty, often refered to as the
        /// identiity element, or the one vector.
        /// </summary>
        /// <returns>The one vector</returns>
        public abstract dynamic GetOne();

        [TestCase(1, 2, 3)]
        [TestCase(2, 3, 4)]
        public void Mult_WithOther_IsAssociative(int xi, int yi, int zi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic z = GetSample(zi);

            dynamic prod1 = x.Mult(y.Mult(z));
            dynamic prod2 = x.Mult(y).Mult(z);

            Assert.That(prod1, Ist.WithinTolOf(prod2, VMath.TOL));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 3, 4)]
        public void Mult_WithOther_IsLeftDist(int xi, int yi, int zi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic z = GetSample(zi);

            dynamic prod1 = x.Mult(y.Add(z));            //x * (y + z)
            dynamic prod2 = x.Mult(y).Add(x.Mult(z));    //(x * y) + (x * z)

            Assert.That(prod1, Ist.WithinTolOf(prod2, VMath.TOL));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 3, 4)]
        public void Mult_WithOther_IsRightDist(int xi, int yi, int zi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic z = GetSample(zi);

            dynamic prod1 = x.Add(y).Mult(z);           //(x + y) * z
            dynamic prod2 = x.Mult(z).Add(y.Mult(z));   //(x * z) + (y * z)

            Assert.That(prod1, Ist.WithinTolOf(prod2, VMath.TOL));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Mult_WithIdintity_Unchanged(int xi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetOne();

            dynamic prod = x.Mult(y);

            Assert.That(prod, Ist.WithinTolOf(x, VMath.TOL));
        }

    }
}
