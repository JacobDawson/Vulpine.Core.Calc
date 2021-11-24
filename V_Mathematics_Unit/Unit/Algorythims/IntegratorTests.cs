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
            max = 28; //28;
            tol = 1.0e-10;
            exp = 1.0e-08;

            loging = true;
        }


        public Integrator GetIntegrator()
        {
            var ing = new Integrator(max, tol);

            //ing.StepEvent += delegate(Object o, StepEventArgs args)
            //{
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return ing;
        }


        public void LogResults(int f, Result<Double> res)
        {
            if (loging) Console.WriteLine
                ("function:{0} error:{1} ittr:{2}", f, res.Error, res.Iterations);
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


        public VFunc GetFunc2(int index)
        {
            switch (index)
            {


                //////////  Improper Intergrals  //////////



                //INT [0 .. 1] (sqrt(x) - 2x^2) / x dx = 1
                case 1: return x => (Math.Sqrt(x) - (2.0 * x * x)) / x;

                //INT [0 .. 1] x^2 * ln(x) dx = -1/9
                case 2: return x => x * x * Math.Log(x);

                //INT [0 .. 2] x / sqrt(4 - x^2) = 2
                case 3: return x => x / Math.Sqrt(4.0 - (x * x));

                //INT [0 .. 1] 1 / (x - 1)^(1/5) dx = -5/4
                //case 4: return x => 1.0 / Math.Pow(x - 1.0, 0.2);
                case 4: return x => 1.0 / -Math.Pow(-(x - 1.0), 0.2);

                //INT [3 .. 9] 1 / sqrt(x^2 - 9) dx = ln(3 + 2 * sqrt(2))
                case 5: return x => 1.0 / Math.Sqrt((x * x) - 9.0);

                //INT [0 .. 3pi] sin(x) / x dx = Si(3pi)
                case 6: return x => Math.Sin(x) / x;

                //INT [0 .. 1] sqrt(x) * ln(x) dx = -4/9
                case 7: return x => Math.Sqrt(x) * Math.Log(x);

                //INT [0 .. 1] sqrt(1 - x^2) dx = pi / 4
                case 8: return x => Math.Sqrt(1.0 - (x * x));

                //INT [0 .. 1] sqrt(x) / sqrt(1 - x^2) dx = 1.19814023473559220744 
                case 9: return x => Math.Sqrt(x) / Math.Sqrt(1.0 - (x * x));

                //INT [0 .. 1] log(x)^2 dx = 2
                case 10: return x => Math.Log(x) * Math.Log(x);

                //INT [0 .. pi/2] log(cos(x)) dx = -pi Log(2) / 2
                case 11: return x => Math.Log(Math.Cos(x));

                //INT [0 .. pi/2] sqrt(tan(x)) dx = pi sqrt(2) / 2
                case 12: return x => Math.Sqrt(Math.Tan(x));


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
        public void RombergGause_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.RombergGause(f, a, b);

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
        public void DubExp_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.DubExp(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(1, 0.0, 1.0, 1.0)]
        [TestCase(2, 0.0, 1.0, -1.0 / 9.0)]
        [TestCase(3, 0.0, 2.0, 2.0)]
        [TestCase(4, 0.0, 1.0, -1.25)]
        //[TestCase(5, 3.0, 9.0, 1.7627471740390860505)]
        [TestCase(6, 0.0, 3.0 * Math.PI, 1.6747617989799612659)]
        [TestCase(7, 0.0, 1.0, -4.0 / 9.0)]
        [TestCase(8, 0.0, 1.0, Math.PI / 4.0)]
        [TestCase(9, 0.0, 1.0, 1.19814023473559220744)]
        [TestCase(10, 0.0, 1.0, 2.0)]
        [TestCase(11, 0.0, Math.PI / 2.0, -1.08879304515180106525)]
        [TestCase(12, 0.0, Math.PI / 2.0, 2.22144146907918312351)]
        public void DubExp_VerticleAsymptote_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc2(fx);

            var res = ing.DubExp(f, a, b);

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


        /*************************************************************************/

        //[Test]
        //public void Trapizoid_ZeroIntergral_ReturnsZero()
        //{
        //    Integrator ing = GetIntegrator();
        //    //VFunc f = GetFunc(fx);

        //    var res = ing.Gauss(x => Math.Sin(x), 0.0, VMath.TAU);

        //    //LogResults(fx, res);
        //    Assert.That(res.Value, Ist.Zero());
        //}


    }
}
