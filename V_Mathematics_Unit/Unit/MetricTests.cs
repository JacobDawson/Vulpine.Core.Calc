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
    public abstract class MetricTests
    {
        /// <summary>
        /// Obtains the additive ininty, or zero vector for the set of
        /// vectors that are being tested.
        /// </summary>
        /// <returns>The zero vector</returns>
        public abstract dynamic GetZero();

        /// <summary>
        /// Method which generates samples to be used in tests.
        /// </summary>
        /// <param name="i">The index of the sample to obtain</param>
        /// <returns>A sample vector sutable for testing</returns>
        public abstract dynamic GetSample(int i);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Dist_SameValue_ReturnsZero(int xi)
        {
            dynamic x = GetSample(xi);

            double dist = x.Dist(x);

            Assert.That(dist, Ist.Zero());
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public void Dist_SymetricValues_SameResult(int xi, int yi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            double d1 = x.Dist(y);
            double d2 = y.Dist(x);

            Assert.That(d1, Ist.WithinTolOf(d2, VMath.ERR));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 3, 4)]
        public void Dist_RandomValues_TriangleHolds(int xi, int yi, int zi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic z = GetSample(zi);

            double d1 = x.Dist(z);
            double d2 = x.Dist(y) + y.Dist(z);

            Assert.That(d1, Is.LessThanOrEqualTo(d2));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Dist_WithZeroVector_EqualsMag(int xi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetZero();

            double d1 = x.Dist(y);
            double d2 = x.Mag();

            Assert.That(d1, Ist.WithinTolOf(d2, VMath.ERR));
        }       
    }
}
