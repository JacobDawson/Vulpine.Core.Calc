using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Matrices;

using Vulpine_Core_Calc_Tests.TestCases;

namespace Vulpine_Core_Calc_Tests.Unit.Algorythims
{
    [TestFixture]
    public class OptomizerTests
    {
        private int max;
        private double tol;
        private double exp;
        private double cut;

        private double step;

        private bool loging;


        public OptomizerTests()
        {
            max = 100000;    //256;
            tol = 1.0e-12;   //1.0e-12;
            exp = 1.0e-06;   //1.0e-07;
            cut = 1.0e-06;   //1.0e-06;

            step = 1.0;      //1.0;

            loging = true;
        }

        public Optimizer GetOptomizer()
        {
            var opt = new Optimizer(max, tol, step);

            //opt.StepEvent += delegate(Object o, StepEventArgs args)
            //{
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return opt;
        }

        public void LogResults<T>(int f, Result<T> res)
        {
            if (loging) Console.WriteLine
                ("function:{0} error:{1} ittr:{2}", f, res.Error, res.Iterations);
        }


        public VFunc GetFunc(int index)
        {
            //NOTE: Fix the one where x = 0
            //NOTE: Decide which x = 1/e to keep

            switch (index)
            {

                //////////  Basic Minimization  //////////


                //MIN [-2 .. 8] x^2 - 2x + 6 = 5 @ x = 1
                case 1: return x => (x * x) - (2.0 * x) + 6.0;

                //MIN [-3 .. -1] (1 + x - x^2) / x^2 = -5/4 @ x = -2
                case 2: return x => (1.0 + x - (x * x)) / (x * x);

                //MIN [0 .. 4] -log(sin(x) + 1) = -log(2) @ x = pi/2
                case 3: return x => -Math.Log(Math.Sin(x) + 1);
                
                //MIN [0 .. 30] cos(sqrt(x)) = -1 @ x = pi^2
                case 4: return x => Math.Cos(Math.Sqrt(x));

                //MIN [0 .. 4] x * log(x) = -1/e @ x = 1/e
                case 5: return x => x * Math.Log(x);

                //MIN [-4 .. 2] x * (x + 3) * (x - 5)^2
                //   f(x) = -99.891092428481746762746274837018
                //   x = -1.8155218370325029661078077208966
                case 6: return x => x * (x + 3.0) * (x - 5.0) * (x - 5.0);

                //MIN [0 .. 8] sin(x) / x
                //   f(x) = -0.21723362821122165740827932556247
                //   x = 4.49340945790906417530788093
                case 7: return x => VMath.Sinc(x);

                //MIN [0 .. 3] gamma(x)
                //   f(x) = 0.885603194410888700278815900582589
                //   x = 1.46163214496836234126265954
                case 8: return x => VMath.Gamma(x);
                


            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }



        /// <summary>
        /// This should only be called inside optimisaiton unit tests. It makes
        /// assertions that aim to verify that the result is indeed a minimum
        /// value of the objective funciton.
        /// </summary>
        /// <param name="p">Optimisaiton Problem under test</param>
        /// <param name="res">The output result</param>
        private void TestMinimum(OptimizationProb p, Result<Vector> res)
        {
            //asserts that the gradient is not NaN and is near zero
            Vector grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN, "Gradient Failed");
            Assert.That(grad.Norm(), Is.LessThan(cut), "Gradient Failed");

            //asserts that the solution is near a minimum pont
            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c, "Is Not Minimum");
        }

        //private void TestRoot(SystemEquaitons se, Result<Vector> res)
        //{
        //    //asserts that the funciton is not NaN and is near zero
        //    double root = se.Evaluate(res.Value).Norm();

        //    Assert.That(root, Is.Not.NaN, "Not A Root");
        //    Assert.That(root, Is.LessThan(cut), "Not A Root");


        //    //asserts that the solution is near a solution
        //    Constraint c = Ist.WithinTolOf(se.GetTarget(1), exp);
        //    for (int i = 2; i <= se.TotalSolutions; i++)
        //        c |= Ist.WithinTolOf(se.GetTarget(i), exp);

        //    Assert.That(res.Value, c, "Not A Listed Solution");
        //}

        #region One-Dimentional Optimization

        [TestCase(1, -2.0, 8.0, 1.0)]
        [TestCase(2, -3.0, 1.0, -2.0)]
        [TestCase(3, 0.0, 4.0, 1.5707963267948966192)]
        [TestCase(4, 0.0, 30.0, 9.8696044010893586188)]
        [TestCase(5, 0.0, 4.0, 0.36787944117144232160)]
        [TestCase(6, -4.0, 2.0, -1.8155218370325029661)]
        [TestCase(7, 0.0, 8.0, 4.4934094579090641753)]
        [TestCase(8, 0.0, 3.0, 1.4616321449683623413)]
        public void MinTernary_NormalFunction_ExpectedValue(int fx, double a, double b, double act)
        {
            Optimizer opt = GetOptomizer();
            VFunc f = GetFunc(fx);

            var res = opt.TernaryMin(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(1, -2.0, 8.0, 1.0)]
        [TestCase(2, -3.0, 1.0, -2.0)]
        [TestCase(3, 0.0, 4.0, 1.5707963267948966192)]
        [TestCase(4, 0.0, 30.0, 9.8696044010893586188)]
        [TestCase(5, 0.0, 4.0, 0.36787944117144232160)]
        [TestCase(6, -4.0, 2.0, -1.8155218370325029661)]
        [TestCase(7, 0.0, 8.0, 4.4934094579090641753)]
        [TestCase(8, 0.0, 3.0, 1.4616321449683623413)]
        public void MinGolden_NormalFunction_ExpectedValue(int fx, double a, double b, double act)
        {
            Optimizer opt = GetOptomizer();
            VFunc f = GetFunc(fx);

            var res = opt.GoldenMin(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }

        #endregion ////////////////////////////////////////////////////////////////////////


        [Test, Combinatorial]
        public void GradientMin_FiniteDiff_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.GradientMin(p.Objective, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        [Test, Combinatorial]
        public void GradientMin_GradGiven_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.GradientMin(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        /*********************************************************************************/

        [Test, Combinatorial]
        public void RankOneMin_FiniteDiff_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneMin(p.Objective, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        [Test, Combinatorial]
        public void RankOneMin_GradGiven_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneMin(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        /*********************************************************************************/

        [Test, Combinatorial]
        public void BFGS_Min_FiniteDiff_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.BFGS_Min(p.Objective, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        [Test, Combinatorial]
        public void BFGS_Min_GradGiven_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.BFGS_Min(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        /*********************************************************************************/

        [Test, Combinatorial]
        public void ConjugateMin_FiniteDiff_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.ConjugateMin(p.Objective, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        [Test, Combinatorial]
        public void ConjugateMin_GradGiven_ExpectedValue
            ([Range(1, 12)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            var p = OptimizationProb.GetCase(prob);

            var input = p.GetStart(start);
            var res = opt.ConjugateMin(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            TestMinimum(p, res);
        }

        /*********************************************************************************/




    }
}
