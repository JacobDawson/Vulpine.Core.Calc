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
            exp = 1.0e-07;   //1.0e-07;
            cut = 1.0e-07;   //1.0e-08;

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
                ("function:{0} error:{1} ittr:{2}", f, res.Error, res.Count);
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


        public OptimizationProb GetOppProb(int index)
        {
            switch (index)
            {
                /**
                 *  Circular Contures around [7, 2]
                 * 
                 *  f(x, y) = (x - 7)^2 + (y - 2)^2
                 *  
                 *  d/dx = 2 (x - 7)
                 *  d/dy = 2 (y - 2)
                 *  
                 *  [5, 3] -> [7, 2]
                 *  [9, 4] -> [7, 2]
                 */
                case 01: return new OptimizationProb(
                    delegate(Vector x) 
                    {
                        double a = x[0] - 7.0;
                        double b = x[1] - 2.0;
                        return (a * a) + (b * b);
                    },
                    delegate(Vector x)
                    {
                        double a = 2 * (x[0] - 7.0);
                        double b = 2 * (x[1] - 2.0);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(7.0, 2.0),
                    },
                    new Vector[]
                    {
                        new Vector(5.0, 3.0),
                        new Vector(9.0, 4.0),
                    });

                /**
                 *  Eleptical Contours around [3, 4]
                 * 
                 *  f(x, y) = y (-2 x + y - 2) + x (4 x - 16) + 28
                 *  
                 *  d/dx = 8 x - 2 y - 16
                 *  d/dy = -2 x + 2 y - 2
                 *  
                 *  [2, 2] -> [3, 4]
                 *  [4, 4] -> [3, 4]
                 */
                case 02: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[1] * (-2.0 * x[0] + x[1] - 2.0);
                        double b = x[0] * (4.0 * x[0] - 16.0);
                        return a + b + 28.0;
                    },
                    delegate(Vector x)
                    {
                        double a = 8.0 * x[0] - 2.0 * x[1] - 16.0;
                        double b = 2.0 * x[1] - 2.0 * x[0] - 2.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(3.0, 4.0),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 2.0),
                        new Vector(4.0, 4.0),
                    });

                /**
                 *  More Eliptical Contours
                 *  
                 *  f(x, y) = x^2 - 3 x y + 4 y^2 + x - y
                 *  
                 *  d/dx =  2 x - 3 y + 1
                 *  d/dy = -3 x + 8 y - 1
                 *  
                 *  [2,  2] -> [-5/7, -1/7]
                 *  [5, -1] -> [-5/7, -1/7]
                 */
                case 03: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = x[0] * x[0];
                        temp = temp - 3.0 * x[0] * x[1];
                        temp = temp + 4.0 * x[1] * x[1];
                        return temp + x[0] - x[1];
                    },
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[0] - 3.0 * x[1] + 1.0;
                        double b = 8.0 * x[1] - 3.0 * x[0] - 1.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-5.0/7.0, -1.0/7.0),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 2.0),
                        new Vector(5.0, -1.0),
                    });

                /**
                 *  A Gentler Version of Rosenbroch
                 *  
                 *  f(x, y) = 25 (y - x^2)^2 + (1 - x)^2
                 *  
                 *  d/dx = x (100 x^2 - 100 y + 2) - 2
                 *  d/dy = 50 (y - x^2)
                 *  
                 *  [-1, 1] -> [1, 1]
                 *  [ 0, 0] -> [1, 1]
                 */
                case 04: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[1] - x[0] * x[0];
                        double b = 1.0 - x[0];
                        return 25.0 * a * a + b * b;
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double a = x[0] * (100.0 * xx - 100.0 * x[1] + 2.0) - 2.0;
                        double b = 50.0 * (x[1] - xx);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 1.0),
                    },
                    new Vector[]
                    {
                        new Vector(-1.0, 1.0),
                        new Vector(0.0, 0.0),
                    });

                /**
                 *  Has one Maximum and one Minimum
                 *  
                 *  f(x, y) = x * e^(-x^2 - y^2)
                 *  
                 *  d/dx = (1 - 2 x^2) e^(-x^2 - y^2)
                 *  d/dy = -2 x y e^(-x^2 - y^2)
                 *  
                 *  [-1.2, 0.4]  -> [-1 / sqrt(2), 0]
                 *  [ 0.4, 0.2]  -> [-1 / sqrt(2), 0]
                 */
                case 05: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = -(x[0] * x[0]) - (x[1] * x[1]);
                        return x[0] * Math.Exp(temp);
                    },
                    delegate(Vector x)
                    {
                        double temp = Math.Exp(-(x[0] * x[0]) - (x[1] * x[1]));
                        double a = (1.0 - 2.0 * x[0] * x[0]) * temp;
                        double b = -2.0 * x[0] * x[1] * temp;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.707106781186548, 0.0),
                    },
                    new Vector[]
                    {
                        new Vector(-1.2, 0.4),
                        new Vector(0.4, 0.2),
                    });

                /**
                 *  Triangle Shaped Valley
                 *  
                 *  f(x, y) = e^(x + y - 1) + e^(x - y - 1) + e^(-x - 1)
                 *  
                 *  d/dx = e^(x + y - 1) + e^(x - y - 1) - e^(-x - 1)
                 *  d/dy = e^(x + y - 1) - e^(x - y - 1)
                 *  
                 *  [ 1, 2] -> [-log(2) / 2, 0]
                 *  [-2, 3] -> [-log(2) / 2, 0]
                 */
                case 06: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[0] + x[1] - 1.0;
                        double b = x[0] - x[1] - 1.0;
                        double c = -x[0] - 1.0;
                        return Math.Exp(a) + Math.Exp(b) + Math.Exp(c);
                    },
                    delegate(Vector x)
                    {
                        double a = Math.Exp(x[0] + x[1] - 1.0);
                        double b = Math.Exp(x[0] - x[1] - 1.0);
                        double c = Math.Exp(-x[0] - 1.0);

                        double g0 = a + b - c;
                        double g1 = a - b;
                        return new Vector(g0, g1);
                    },
                    new Vector[]
                    {
                        new Vector(-0.346573590279973, 0.0),
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 2.0),
                        new Vector(-2.0, 3.0),
                    });

                /**
                 *  Bannana Shaped Curve With Two Minima
                 *  
                 *  f(x, y) = 2 (2 y^2 - x)^2 + (x - 1)^2
                 *  
                 *  d/dx = 6 x - 8 y^2 - 2
                 *  d/dy = y (32 y^2 - 16 x)
                 *  
                 *  [-1, 1] ->  [1,  1 / sqrt(2)]
                 *  [2, -1] ->  [1, -1 / sqrt(2)]
                 */
                case 07: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[1] * x[1] - x[0];
                        double b = x[0] - 1.0;
                        return 2.0 * a * a + b * b;
                    },
                    delegate(Vector x)
                    {
                        double yy = x[1] * x[1];
                        double a = 6.0 * x[0] - 8.0 * yy - 2.0;
                        double b = x[1] * (32.0 * yy - 16.0 * x[0]);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 0.707106781186548),
                        new Vector(1.0, -0.707106781186548),
                    },
                    new Vector[]
                    {
                        new Vector(-1.0, 1.0),
                        new Vector(2.0, -1.0),
                    });

                /**
                 *  Polynomial With Many Local Minima
                 *  
                 *  f(x, y) = ((x^2/3 - 21/10) x^2 + 4) x^2 + y (x + y (4 y^2 - 4))
                 *  
                 *  d/dx = x ((2 x^2 - 42/5) x^2 + 8) + y
                 *  d/dy = x + y (16 y^2 - 8)
                 *  
                 *  x* = +/- 0.08984201310031806242249056062
                 *  y* = +/- 0.7126564030207396333972658142
                 *  
                 *  [0.8,  0.8] -> [x-, y+]
                 *  [0.8, -0.4] -> [x+, y-]
                 */
                case 08: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];
                        double temp = ((xx / 3.0 - 21.0 / 10.0) * xx + 4.0) * xx;
                        return temp + x[1] * (x[0] + x[1] * (4.0 * yy - 4.0));
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double a = x[0] * ((2.0 * xx - 42.0 / 5.0) * xx + 8.0) + x[1];
                        double b = x[0] + x[1] * (16 * x[1] * x[1] - 8.0);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-1.70360671496998, 0.796083568672625),
                        new Vector(-1.60710475292020, -0.568651454884131),
                        new Vector(-0.0898420131003181, 0.712656403020740),
                        new Vector(0.0898420131003181, -0.712656403020740),
                        new Vector(1.60710475292020, 0.568651454884131),
                        new Vector(1.70360671496998, -0.796083568672625),
                    },
                    new Vector[]
                    {
                        new Vector(0.8, 0.8),
                        new Vector(0.8, -0.4),
                    });

                /**
                 *   Slope With A Hole In It
                 *   
                 *   f(x, y) = x^2 + y^2 - ln(x^2 + y^2) - y
                 *   
                 *   d/dx = x (2 - 2/(x^2 + y^2))
                 *   d/dy = y (2 - 2/(x^2 + y^2)) - 1
                 *   
                 *   [1.0,  0.0] -> [0, 1/4 + sqrt(17)/4]
                 *   [0.0, -1.5] -> [0, 1/4 + sqrt(17)/4]
                 */
                case 09: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = x[0] * x[0] + x[1] * x[1];
                        return temp - Math.Log(temp) - x[1];
                    },
                    delegate(Vector x)
                    {
                        double temp = 2.0 - 2.0 / (x[0] * x[0] + x[1] * x[1]);
                        double a = x[0] * temp;
                        double b = x[1] * temp - 1.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(0.0, 1.28077640640442),
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 0.0),
                        new Vector(0.0, -1.5),
                    });

                /**
                 *  Two Humps and Two Valleys
                 *  
                 *  f(x, y) = x * y * e^(-x^2 - y^2)
                 *  
                 *  d/dx = y (2 x^2 - 1) (-e^(-x^2 - y^2))
                 *  d/dy = x (2 y^2 - 1) (-e^(-x^2 - y^2))
                 *  
                 *  [-1.5, 0.5] -> [-1/sqrt(2), 1/sqrt(2)]
                 *  [0.1, -0.1] -> [1/sqrt(2), -1/sqrt(2)]
                 */
                case 10: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = -(x[0] * x[0]) - (x[1] * x[1]);
                        return x[0] * x[1] * Math.Exp(temp);
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];
                        double temp = -Math.Exp(-xx - yy);

                        double a = x[1] * (2.0 * xx - 1.0) * temp;
                        double b = x[0] * (2.0 * yy - 1.0) * temp;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.707106781186548, 0.707106781186548),
                        new Vector(0.707106781186548, -0.707106781186548),
                    },
                    new Vector[]
                    {
                        new Vector(-1.5, 0.5),
                        new Vector(0.1, -0.1),
                    });

                /**
                 *  A Wavy Valley
                 *  
                 *  f(x, y) = sin(x) + sin(2 y) + x^2 + y^2
                 *  
                 *  d/dx = 2 x + cos(x)
                 *  d/dy = 2 y + 2 cos(2 y)
                 *  
                 *  x* = -0.4501836112948735730365386968
                 *  y* = -0.5149332646611294138010592584
                 *
                 *  [-5, 5] -> [x*, y*]
                 *  [ 8, 0] -> [x*, y*]
                 */
                case 11: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = (x[0] * x[0]) + (x[1] * x[1]);
                        return Math.Sin(x[0]) + Math.Sin(2.0 * x[1]) + temp;
                    },
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[0] + Math.Cos(x[0]);
                        double b = 2.0 * x[1] + 2.0 * Math.Cos(2.0 * x[1]);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.450183611294874, -0.514933264661129),
                    },
                    new Vector[]
                    {
                        new Vector(-5.0, 5.0),
                        new Vector(8.0, 0.0),
                    });


                        
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

        [Test, Combinatorial]
        public void GradientEx_FiniteDiff_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientEx(p.Objective, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void GradientEx_FiniteDiff_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientEx(p.Objective, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void GradientBt_FiniteDiff_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientBt(p.Objective, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void GradientBt_FiniteDiff_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientBt(p.Objective, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void GradientEx_GradGiven_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientEx(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void GradientEx_GradGiven_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientEx(p.Objective, p.Gradient, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void GradientBt_GradGiven_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientBt(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void GradientBt_GradGiven_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.GradientBt(p.Objective, p.Gradient, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }


        /*********************************************************************************/

        [Test, Combinatorial]
        public void RankOneEx_FiniteDiff_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneEx(p.Objective, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void RankOneEx_FiniteDiff_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneEx(p.Objective, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void RankOneBt_FiniteDiff_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneBt(p.Objective, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void RankOneBt_FiniteDiff_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneBt(p.Objective, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void RankOneEx_GradGiven_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneEx(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void RankOneEx_GradGiven_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneEx(p.Objective, p.Gradient, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }

        [Test, Combinatorial]
        public void RankOneBt_GradGiven_ExpectedValue
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneBt(p.Objective, p.Gradient, input);
            LogResults(prob, res);

            Constraint c = Ist.WithinTolOf(p.GetTarget(1), exp);
            for (int i = 2; i <= p.TotalMinima; i++)
                c |= Ist.WithinTolOf(p.GetTarget(i), exp);

            Assert.That(res.Value, c);
        }

        [Test, Combinatorial]
        public void RankOneBt_GradGiven_GradZero
            ([Range(1, 11)] int prob, [Values(1, 2)] int start)
        {
            Optimizer opt = GetOptomizer();
            OptimizationProb p = GetOppProb(prob);

            var input = p.GetStart(start);
            var res = opt.RankOneBt(p.Objective, p.Gradient, input);
            var grad = p.Gradient(res.Value);

            Assert.That(grad.Norm(), Is.Not.NaN);
            Assert.That(grad.Norm(), Is.LessThan(cut));
        }


        /*********************************************************************************/

        //[Test]
        //public void MinTernary_MinZero_ExpectedValue()
        //{
        //    Optimizer opt = GetOptomizer();
        //    //VFunc f = GetFunc(fx);

        //    var res = opt.MinGolden(x => x * x, -1.0, 2.0);

        //    //LogResults(fx, res);
        //    Assert.That(res.Value, Ist.Zero());
        //}



        //[Test]
        //public void GradientEx_GradGiven_ZeroValue()
        //{
        //    Optimizer opt = GetOptomizer();
        //    //MFunc f = GetMFunc(fx);
        //    //var g = GetGraident(fx);

        //    //var input = GetMStart(fx);
        //    //var act = GetMResult(fx);

        //    MFunc f = delegate(Vector x)
        //    {
        //        return (4.0 * x[0] * x[0]) + (x[1] * x[1]) - (2.0 * x[0] * x[1]);
        //    };

        //    VFunc<Vector> g = delegate(Vector x)
        //    {
        //        double dx = 8.0 * x[0] - 2.0 * x[1];
        //        double dy = 2.0 * x[1] - 2.0 * x[0];

        //        return new Vector(dx, dy);
        //    };

        //    Vector input = new Vector(-1.0, -2.0);

        //    var res = opt.RankOneEx(f, g, input);

        //    //LogResults(fx, res);
        //    Assert.That(res.Value, Ist.Zero());
        //}



    }
}
