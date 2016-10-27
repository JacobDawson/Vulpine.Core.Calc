using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Calc_Tests.Unit
{
    [TestFixture]
    class StatRunnerTests
    {
        //the default tolarance to use when none is provided
        public const Double VTOL = 1e-9;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public StatRunnerTests() { tol = VTOL; }
        public StatRunnerTests(double tol) { this.tol = tol; }

        [Test]
        public void Add_NormalDistribution_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            foreach(double x in DataSets.Normal) sr.Add(x);

            double sum = sr.Sum[0];
            double ex = 1345.7341935713;

            Assert.That(sum, Ist.WithinTolOf(ex, VMath.ESP));
        }

        [Test]
        public void Add_NormalDistribution_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Normal) sr.Add(x);

            double avg = sr.Mean[0];
            double ex = 5.4045549943;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_NormalDistribution_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Normal) sr.Add(x);

            double var = sr.Var;
            double ex = 1.503575407;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_NormalDistribution_ExpectedSkew()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Normal) sr.Add(x);

            double skew = sr.Skew[0];
            double ex = -0.3909578714;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_NormalDistribution_ExpectedMin()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Normal) sr.Add(x);

            double skew = sr.Min[0];
            double ex = 1.9471531576;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_NormalDistribution_ExpectedMax()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Normal) sr.Add(x);

            double skew = sr.Max[0];
            double ex = 8.3323329604;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Add_UniformDistribution_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double sum = sr.Sum[0];
            double ex = 129.975554063;

            Assert.That(sum, Ist.WithinTolOf(ex, VMath.ESP));
        }

        [Test]
        public void Add_UniformDistribution_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double avg = sr.Mean[0];
            double ex = 0.521990177;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_UniformDistribution_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double var = sr.Var;
            double ex = 0.0857784859;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_UniformDistribution_ExpectedSkew()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double skew = sr.Skew[0];
            double ex = -0.0941506838;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_UniformDistribution_ExpectedMin()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double skew = sr.Min[0];
            double ex = 0.0041451644;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_UniformDistribution_ExpectedMax()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Uniform) sr.Add(x);

            double skew = sr.Max[0];
            double ex = 0.9990476144;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Add_LogNormalDistribution_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double sum = sr.Sum[0];
            double ex = 285.8757746353;

            Assert.That(sum, Ist.WithinTolOf(ex, VMath.ESP));
        }

        [Test]
        public void Add_LogNormalDistribution_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double avg = sr.Mean[0];
            double ex = 1.1480954805;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LogNormalDistribution_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double var = sr.Var;
            double ex = 0.3553755736;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LogNormalDistribution_ExpectedSkew()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double skew = sr.Skew[0];
            double ex = 1.5414589208;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LogNormalDistribution_ExpectedMin()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double skew = sr.Min[0];
            double ex = 0.3257033841;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LogNormalDistribution_ExpectedMax()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.LogNorm) sr.Add(x);

            double skew = sr.Max[0];
            double ex = 3.5548327624;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Add_LaplaceDistribution_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double sum = sr.Sum[0];
            double ex = 735.128286724;

            Assert.That(sum, Ist.WithinTolOf(ex, VMath.ESP));
        }

        [Test]
        public void Add_LaplaceDistribution_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double avg = sr.Mean[0];
            double ex = 2.9523224366;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LaplaceDistribution_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double var = sr.Var;
            double ex = 0.3569322639;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LaplaceDistribution_ExpectedSkew()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double skew = sr.Skew[0];
            double ex = -0.1972301984;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LaplaceDistribution_ExpectedMin()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double skew = sr.Min[0];
            double ex = 0.7547431959;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_LaplaceDistribution_ExpectedMax()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Laplace) sr.Add(x);

            double skew = sr.Max[0];
            double ex = 5.2710437217;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Add_ExponentialDistribution_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double sum = sr.Sum[0];
            double ex = 50.7779850895;

            Assert.That(sum, Ist.WithinTolOf(ex, VMath.ESP));
        }

        [Test]
        public void Add_ExponentialDistribution_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double avg = sr.Mean[0];
            double ex = 0.203927651;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_ExponentialDistribution_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double var = sr.Var;
            double ex = 0.0368791526;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_ExponentialDistribution_ExpectedSkew()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double skew = sr.Skew[0];
            double ex = 1.876419791;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_ExponentialDistribution_ExpectedMin()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double skew = sr.Min[0];
            double ex = 0.0008863178;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_ExponentialDistribution_ExpectedMax()
        {
            dynamic sr = new StatRunner(1);
            foreach (double x in DataSets.Exponential) sr.Add(x);

            double skew = sr.Max[0];
            double ex = 1.2888835818;

            Assert.That(skew, Ist.WithinTolOf(ex, tol));
        }

        ////////////////////////////////////////////////////////////////////////////////

        [Test]
        public void Add_WeightedData_ExpectedWeight()
        {
            dynamic sr = new StatRunner(1);
            double[] items = DataSets.Normal.ToArray();
            double[] weights = DataSets.Uniform.ToArray();

            for (int i = 0; i < items.Length; i++)
                sr.Add(items[i], weights[i]);

            double wt = sr.Weight;
            double ex = 129.975554063;

            Assert.That(wt, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_WeightedData_ExpectedSum()
        {
            dynamic sr = new StatRunner(1);
            double[] items = DataSets.Normal.ToArray();
            double[] weights = DataSets.Uniform.ToArray();

            for (int i = 0; i < items.Length; i++)
                sr.Add(items[i], weights[i]);

            double sum = sr.Sum[0];
            double ex = 691.2393460496;

            Assert.That(sum, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_WeightedData_ExpectedMean()
        {
            dynamic sr = new StatRunner(1);
            double[] items = DataSets.Normal.ToArray();
            double[] weights = DataSets.Uniform.ToArray();

            for (int i = 0; i < items.Length; i++)
                sr.Add(items[i], weights[i]);

            double avg = sr.Mean[0];
            double ex = 5.3182258082;

            Assert.That(avg, Ist.WithinTolOf(ex, tol));
        }

        [Test]
        public void Add_WeightedData_ExpectedVar()
        {
            dynamic sr = new StatRunner(1);
            double[] items = DataSets.Normal.ToArray();
            double[] weights = DataSets.Uniform.ToArray();

            for (int i = 0; i < items.Length; i++)
                sr.Add(items[i], weights[i]);

            double var = sr.Var;
            double ex = 1.4598749823;

            Assert.That(var, Ist.WithinTolOf(ex, tol));
        }


    }
}
