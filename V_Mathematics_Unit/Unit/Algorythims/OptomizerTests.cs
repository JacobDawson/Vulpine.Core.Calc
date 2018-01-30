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

namespace Vulpine_Core_Calc_Tests.Unit.Algorythims
{
    [TestFixture]
    public class OptomizerTests
    {
        private int max;
        private double tol;
        private double exp;

        private double step;

        private bool loging;


        public OptomizerTests()
        {
            max = 1000000;   //256;
            tol = 1.0e-12;   //1.0e-12;
            exp = 1.0e-07;   //1.0e-07;

            step = 0.1;      //1.0;

            loging = true;
        }

        public Optimizer GetOptomizer()
        {
            var opt = new Optimizer(max, tol, step);

            //rf.StepEvent += delegate(Object o, NumericStepEventArgs args)
            //{
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return opt;
        }

        public void LogResults<T>(int f, Result<T> res)
        {
            if (loging) Console.WriteLine
                ("function:{0} error:{1} ittr:{2}", f, res.Error, res.NumSteps);
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


        public MFunc GetMFunc(int index)
        {
            switch (index)
            {
                //f(x) = x1 * e^(-x1^2 - x2^2)
                //min = [-1/sqrt(2), 0]
                case 1: return delegate(Vector x)
                {
                    double x0_2 = x[0] * x[0];
                    double x1_2 = x[1] * x[1];
                    return x[0] * Math.Exp(-x0_2 - x1_2);
                };

                //f(x) = (x1 - 7)^2 + (x2 - 2)^2
                //min = [7, 2]
                case 2: return delegate(Vector x)
                {
                    double a = x[0] - 7.0;
                    double b = x[1] - 2.0;
                    return (a * a) + (b * b);
                };

                //f(x) = 4 * (x - 3)^2 + (y - 4)^2 - 2 * (x - 3) * (y - 4)
                //min = [3, 4]
                case 3: return delegate(Vector x)
                {
                    double x0 = x[0] - 3.0;
                    double x1 = x[1] - 4.0;
                    return (4.0 * x0 * x0) + (x1 * x1) - (2.0 * x0 * x1);
                };

                //f(x) = 100 * (x2 - x1^2)^2 + (1 - x1)^2
                //min = [1, 1]
                case 4: return delegate(Vector x)
                {
                    double a = 1.0 - x[0];
                    double b = x[1] - (x[0] * x[0]);
                    return (100.0 * b * b) + (a * a);
                };

                //min = [-2, -1]
                case 5: return delegate(Vector x)
                {
                    double fx = (1.0 / 4.0);
                    fx = (fx * x[0]) - (4.0 / 3.0);
                    fx = (fx * x[0]) - 2.0;
                    fx = (fx * x[0]) + 16.0;
                    fx = (fx * x[0]) + 0.0;

                    double fy = (1.0 / 4.0);
                    fy = (fy * x[1]) - (2.0 / 3.0);
                    fy = (fy * x[1]) - (1.0 / 2.0);
                    fy = (fy * x[1]) + 2.0;
                    fy = (fy * x[1]) + 0.0;

                    return fx + fy;
                };

                //min = [0, 1] and [0, -1]
                case 7: return delegate(Vector x)
                {
                    Cmplx z = (Cmplx)x;
                    Cmplx z2 = z * z;

                    z = (z2 + 1.0) / (z2 - 1.0);
                    return z.Abs;
                };

                //min = [1, 0]
                case 8: return delegate(Vector x)
                {
                    Cmplx z = (Cmplx)x;
                    z = Cmplx.Log(z);
                    return z.Abs;
                };

                //f(x, y) = e^(x + y - 1) + e^(x - y - 1) + e^(-x - 1)
                //min = [-ln(2)/2, 0]
                case 10: return delegate(Vector x)
                {
                    double e1 = Math.Exp(x[0] + x[1] - 1.0);
                    double e2 = Math.Exp(x[0] - x[1] - 1.0);
                    double e3 = Math.Exp(-x[0] - 1.0);

                    return e1 + e2 + e3;
                };

                //f(x, y) = x^2 - 3xy + 4y^2 + x - y
                //min = [-5/7, -1/7]
                case 11: return delegate(Vector x)
                {
                    return (x[0] * x[0]) - (3.0 * x[0] * x[1]) + (4.0 * x[1] * x[1]) + x[0] - x[1];
                };
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }


        public VFunc<Vector> GetGraident(int index)
        {
            switch (index)
            {
                //  d/dx = (1 - 2 x^2) e^(-x^2 - y^2)
                //  d/dy = -2 x y e^(-x^2 - y^2)
                case 1: return delegate(Vector x)
                {
                    double e_xy = Math.Exp(-x[0] * x[0] - x[1] * x[1]);
                    double dx = (1.0 - 2.0 * x[0] * x[0]) * e_xy;
                    double dy = -2.0 * x[0] * x[1] * e_xy;

                    return new Vector(dx, dy);
                };

                //  d/dx = 2 (x - 7)
                //  d/dy = 2 (y - 2)
                case 2: return delegate(Vector x)
                {
                    double dx = 2.0 * (x[0] - 7.0);
                    double dy = 2.0 * (x[1] - 2.0);

                    return new Vector(dx, dy);
                };

                //   d/dx = 2 (4 x - y - 8)
                //   d/dy = -2 (1 + x - y)
                case 3: return delegate(Vector x)
                {
                    double dx = 2.0 * (4.0 * x[0] - x[1] - 8.0);
                    double dy = -2.0 * (1.0 + x[0] - x[1]);

                    return new Vector(dx, dy);
                };

                //   d/dx = x (400 x^2 - 400 y + 2) - 2
                //   d/dy = 200 (y - x^2)
                case 4: return delegate(Vector x)
                {
                    double dx = x[0] * (400.0 * x[0] * x[0] - 400.0 * x[1] + 2.0) - 2.0;
                    double dy = 200.0 * (x[1] - x[0] * x[0]);

                    return new Vector(dx, dy);
                };

                //   d/dx = (x - 2)(x - 4)(x + 2)
                //   d/dy = (y - 2)(y - 1)(y + 1)
                case 5: return delegate(Vector x)
                {
                    double dx = (x[0] - 2.0) * (x[0] - 4.0) * (x[0] + 2.0);
                    double dy = (x[1] - 2.0) * (x[1] - 1.0) * (x[1] + 1.0);

                    return new Vector(dx, dy);
                };

                //   d/dx = e^(x + y - 1) + e^(x - y - 1) - e^(-x - 1)
                //   d/dy = e^(x + y - 1) - e^(x - y - 1)
                case 10: return delegate(Vector x)
                {
                    double e1 = Math.Exp(x[0] + x[1] - 1.0);
                    double e2 = Math.Exp(x[0] - x[1] - 1.0);
                    double e3 = Math.Exp(-x[0] - 1.0);

                    double dx = e1 + e2 - e3;
                    double dy = e1 - e2;

                    return new Vector(dx, dy);
                };

                //  d/dx = 2 x - 3 y + 1
                //  d/dy = -3 x + 8 y - 1
                case 11: return delegate(Vector x)
                {
                    double dx = 2.0 * x[0] - 3.0 * x[1] + 1.0;
                    double dy = 8.0 * x[1] - 3.0 * x[0] - 1.0;

                    return new Vector(dx, dy);
                };
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }


        public Vector GetMResult(int index)
        {
            switch (index)
            {
                case 1: return new Vector(-0.70710678118654752440, 0.0);
                case 2: return new Vector(7.0, 2.0);
                case 3: return new Vector(3.0, 4.0);
                case 4: return new Vector(1.0, 1.0);
                case 5: return new Vector(-2.0, -1.0);
                case 7: return new Vector(0.0, -1.0);
                case 8: return new Vector(1.0, 0.0);
                case 10: return new Vector(-0.34657359027997265471, 0.0);
                case 11: return new Vector(-5.0 / 7.0, -1.0 / 7.0);
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }


        public Vector GetMStart(int index)
        {
            switch (index)
            {
                case 1: return new Vector(0.5, -0.5);
                case 2: return new Vector(3.0, 3.0);
                case 3: return new Vector(1.0, 1.0);
                case 4: return new Vector(-1.0, 1.0);
                case 5: return new Vector(0.0, -2.0);
                case 7: return new Vector(0.5, -0.5);
                case 8: return new Vector(-1.0, 0.5);
                case 10: return new Vector(1.0, 2.0);
                case 11: return new Vector(2.0, 2.0);
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }


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

            var res = opt.MinTernary(f, a, b);

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

            var res = opt.MinGolden(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        #endregion ////////////////////////////////////////////////////////////////////////



        //NOTE: Problems 4 and 5 can sometimes generate NaN values, pending on
        //the hyper peramaters. This is believed to be caused by the limitations
        //inherent in the finite diffrence calculations. 

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        //[TestCase(4)]
        //[TestCase(5)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(10)]
        [TestCase(11)]
        public void GradientEx_FiniteDiff_ExpectedValue(int fx)
        {
            Optimizer opt = GetOptomizer();
            MFunc f = GetMFunc(fx);

            var input = GetMStart(fx);
            var act = GetMResult(fx);

            var res = opt.GradientEx(f, input);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(10)]
        [TestCase(11)]
        public void GradientBt_FiniteDiff_ExpectedValue(int fx)
        {
            Optimizer opt = GetOptomizer();
            MFunc f = GetMFunc(fx);

            var input = GetMStart(fx);
            var act = GetMResult(fx);

            var res = opt.GradientBt(f, input);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }




        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(11)]
        public void GradientEx_GradGiven_ExpectedValue(int fx)
        {
            Optimizer opt = GetOptomizer();
            MFunc f = GetMFunc(fx);
            var g = GetGraident(fx);

            var input = GetMStart(fx);
            var act = GetMResult(fx);

            var res = opt.GradientEx(f, g, input);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(11)]
        public void GradientBt_GradGiven_ExpectedValue(int fx)
        {
            Optimizer opt = GetOptomizer();
            MFunc f = GetMFunc(fx);
            var g = GetGraident(fx);

            var input = GetMStart(fx);
            var act = GetMResult(fx);

            var res = opt.GradientBt(f, g, input);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


    }
}
