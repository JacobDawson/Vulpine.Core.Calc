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

namespace Vulpine_Core_Calc_Tests.Unit.Algorythims
{
    [TestFixture]
    public class IntegratorTests
    {
        private int max;
        private double tol;
        private double exp;

        private bool loging;

        


        public IntegratorTests()
        {
            max = 32; //28;
            tol = 1.0e-10;
            exp = 1.0e-09;

            loging = false;
        }


        public Integrator GetIntegrator()
        {
            var ing = new Integrator(max, tol);

            //ing.StepEvent += delegate(Object o, NumericStepEventArgs args)
            //{
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return ing;
        }


        public void LogResults(int f, Result<Double> res)
        {
            if (loging) Console.WriteLine
                ("function:{0} error:{1} ittr:{2}", f, res.Error, res.NumSteps);
        }


        public VFunc GetFunc(int index)
        {
            switch (index)
            {


                //////////  Basic Intergrals  //////////



                //INT [1 .. e] 1/x dx = 1
                case 1: return x => 1.0 / x;

                //INT [0 .. pi/2] sin(x) * cos(x) = 1/2
                case 2: return x => Math.Sin(x) * Math.Cos(x);

                //INT [1 .. 3] 4x - 3x^2 - 5 dx = -20
                case 3: return x => (4.0 * x) - (3.0 * x * x) - 5.0;

                //INT [-3 .. 5] x(x + 3)(x - 5)^2 dx = 1024/15
                case 4: return x => x * (x + 3.0) * (x - 5.0) * (x - 5.0);

                //INT [-1 .. 1] sqrt(1 - x^2) dx = pi/2
                case 5: return x => Math.Sqrt(1 - (x * x));

                //INT [0 .. 2] e^(x^2) dx = 1/2 sqrt(pi) erfi(2)
                case 6: return x => Math.Exp(x * x);

                //INT [2 .. 4] (1 + x - x^2) / x^2 dx = ln(2) - 7/4
                case 7: return x => (1.0 + x - (x * x)) / (x * x);

                //INT [0 to pi] 1 / (1 + sin^2(x)) dx = pi/sqrt(2)
                case 8: return x => 1.0 / (1.0 + Math.Sin(x) * Math.Sin(x));

                //INT [-2pi .. 2pi] sinc(x) dx = 2 * Si(2pi)
                case 9: return x => VMath.Sinc(x);

                //INT [-1 .. 2] sqrt(1 + x^3) dx = ??
                case 10: return x => Math.Sqrt(1 + (x * x * x));

                //INT [-8 to 8] ln(1 + x^2) - 2 dx = ??
                case 11: return x => Math.Log(1.0 + (x * x)) - 2.0;

                //INT [0 .. 10] cos(sqrt(x)) dx = ??
                case 12: return x => Math.Cos(Math.Sqrt(x));


            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }



        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, 0.0, Math.PI / 2.0, 0.5)]
        [TestCase(3, 1.0, 3.0, -20.0)]
        [TestCase(4, -3.0, 5.0, 1024.0 / 15.0)]
        [TestCase(5, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(6, 2.0, 0.0, -16.452627765507230225)]
        [TestCase(7, 4.0, 2.0, 1.0568528194400546906)]
        [TestCase(8, Math.PI, 0.0, -2.2214414690791831235)]
        [TestCase(9, -VMath.TAU, VMath.TAU, 2.8363031522652569005)]
        [TestCase(10, -1.0, 2.0, 4.0826185263905451134)]
        [TestCase(11, 8.0, -8.0, -8.5759616473227345073)]
        [TestCase(12, 0.0, 10.0, -4.1303862851374246194)]
        public void Trapezoid_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Trapezoid(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        /*************************************************************************/



        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, 0.0, Math.PI / 2.0, 0.5)]
        [TestCase(3, 1.0, 3.0, -20.0)]
        [TestCase(4, -3.0, 5.0, 1024.0 / 15.0)]
        [TestCase(5, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(6, 2.0, 0.0, -16.452627765507230225)]
        [TestCase(7, 4.0, 2.0, 1.0568528194400546906)]
        [TestCase(8, Math.PI, 0.0, -2.2214414690791831235)]
        [TestCase(9, -VMath.TAU, VMath.TAU, 2.8363031522652569005)]
        [TestCase(10, -1.0, 2.0, 4.0826185263905451134)]
        [TestCase(11, 8.0, -8.0, -8.5759616473227345073)]
        [TestCase(12, 0.0, 10.0, -4.1303862851374246194)]
        public void Romberg_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Romberg(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        /*************************************************************************/


        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, 0.0, Math.PI / 2.0, 0.5)]
        [TestCase(3, 1.0, 3.0, -20.0)]
        [TestCase(4, -3.0, 5.0, 1024.0 / 15.0)]
        [TestCase(5, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(6, 2.0, 0.0, -16.452627765507230225)]
        [TestCase(7, 4.0, 2.0, 1.0568528194400546906)]
        [TestCase(8, Math.PI, 0.0, -2.2214414690791831235)]
        [TestCase(9, -VMath.TAU, VMath.TAU, 2.8363031522652569005)]
        [TestCase(10, -1.0, 2.0, 4.0826185263905451134)]
        [TestCase(11, 8.0, -8.0, -8.5759616473227345073)]
        [TestCase(12, 0.0, 10.0, -4.1303862851374246194)]
        public void Gauss_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Gauss(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        /*************************************************************************/



        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, 0.0, Math.PI / 2.0, 0.5)]
        [TestCase(3, 1.0, 3.0, -20.0)]
        [TestCase(4, -3.0, 5.0, 1024.0 / 15.0)]
        [TestCase(5, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(6, 2.0, 0.0, -16.452627765507230225)]
        [TestCase(7, 4.0, 2.0, 1.0568528194400546906)]
        [TestCase(8, Math.PI, 0.0, -2.2214414690791831235)]
        [TestCase(9, -VMath.TAU, VMath.TAU, 2.8363031522652569005)]
        [TestCase(10, -1.0, 2.0, 4.0826185263905451134)]
        [TestCase(11, 8.0, -8.0, -8.5759616473227345073)]
        [TestCase(12, 0.0, 10.0, -4.1303862851374246194)]
        public void Kronrod_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            VFunc f = GetFunc(fx);
            double res = Integrator.Kronrod(f, a, b);
            double error = Math.Abs((res - act) / act);

            if (loging) Console.WriteLine("function:{0:00} accuracy:{1:G5}", fx, error);
        
            Assert.That(res, Ist.WithinTolOf(act, exp));
        }





    }
}
