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
    public abstract class EuclideanTests : MetricTests
    {
        [TestCase(1, 2, 3)]
        [TestCase(2, 3, 4)]
        public void Add_VariousInputs_IsAssosiative(int xi, int yi, int zi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic z = GetSample(zi);

            dynamic sum1 = x.Add(y.Add(z));
            dynamic sum2 = x.Add(y).Add(z);

            Assert.That(sum1, Ist.WithinTolOf(sum2, VMath.TOL));
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public void Add_VariousInputs_IsComunitive(int xi, int yi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            dynamic sum1 = x.Add(y);
            dynamic sum2 = y.Add(x);

            Assert.That(sum1, Ist.WithinTolOf(sum2, VMath.TOL));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Add_WithIdintity_Unchanged(int xi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetZero();

            dynamic sum = x.Add(y);

            Assert.That(sum, Ist.WithinTolOf(x, VMath.TOL));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Sub_WithSelf_ReturnsIdintity(int xi)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetZero();

            dynamic diff = x.Sub(x);

            Assert.That(diff, Ist.WithinTolOfZero(VMath.TOL));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Mult_WithOne_Unchanged(int xi)
        {
            dynamic x = GetSample(xi);

            dynamic p = x.Mult(1.0);

            Assert.That(p, Ist.WithinTolOf(x, VMath.TOL));
        }

        [TestCase(1, 2, 0.5)]
        [TestCase(2, 3, 2.0)]
        [TestCase(3, 4, -1.0)]
        public void Mult_VariousInputs_IsDistributive(int xi, int yi, double a)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            dynamic p1 = x.Add(y).Mult(a);
            dynamic p2 = x.Mult(a).Add(y.Mult(a));

            Assert.That(p1, Ist.WithinTolOf(p2, VMath.TOL));
        }
    }
}
