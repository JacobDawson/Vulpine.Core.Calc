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
            max = 128; //1048576;
            tol = 1.0e-12;
            exp = 1.0e-11;

            loging = true;
        }


        public Integrator GetIntegrator()
        {
            var ing = new Integrator(max, tol);

            //rf.StepEvent += delegate(Object o, NumericStepEventArgs args)
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
                //Basic Intergrals


                //INT [1 .. e] 1/x dx = 1
                case 1: return x => 1.0 / x;

                //INT [-1 .. 1] sqrt(1 - x^2) dx = pi/2
                case 2: return x => Math.Sqrt(1 - (x * x));

                //INT [0 .. 2] e^(x^2) dx = 1/2 sqrt(pi) erfi(2)
                case 3: return x => Math.Exp(x * x);

                //INT [2 .. 4] (1 + x - x^2) / x^2 dx = ln(2) - 7/4
                case 4: return x => (1.0 + x - (x * x)) / (x * x);

                //INT [-1 .. 1] e^(acos(x)) dx = 1/2 * (1 + e^pi)
                case 5: return x => Math.Exp(Math.Acos(x));

                //INT [-1 .. 2] sqrt(1 + x^3) dx = ??
                case 6: return x => Math.Sqrt(1 + (x * x * x));



                //Improper Intergrals Type1


                //INT [0 .. 1] (sqrt(x) - 2x^2) / x dx = 1
                case 7: return x => (Math.Sqrt(x) - (2.0 * x * x)) / x;

                //INT [0 .. 1] x^2 * ln(x) dx = -1/9
                case 8: return x => x * x * Math.Log(x);

                //INT [0 .. 2] x / sqrt(4 - x^2) = 2
                case 9: return x => x / Math.Sqrt(4.0 - (x * x));

                //INT [0 .. 1] 1 / (x - 1)^(1/5) dx = -5/4
                case 10: return x => 1.0 / Math.Pow(x - 1.0, 0.2);

                //INT [3 .. 9] 1 / sqrt(x^2 - 9) dx = ln(3 + 2 * sqrt(2))
                case 11: return x => 1.0 / Math.Sqrt((x * x) - 9.0);

                //INT [0 .. 3pi] sin(x) / x dx = Si(3pi)
                case 12: return x => Math.Sin(x) / x;



                //Improper Intergrals Type2


                //INT [1 .. inf] 1/x^2 dx = 1
                case 13: return x => 1.0 / (x * x);

                //INT [0 .. inf] 1/((x+1) * sqrt(x)) dx = pi
                case 14: return x => 1.0 / ((x + 1.0) * Math.Sqrt(x));

                //INT [-inf .. 0] x * e^(-x^2) dx = -1/2
                case 15: return x => x * Math.Exp(-x * x);

                //INT [-inf .. 0] 4 / (36 + x^2) dx = pi/3
                case 16: return x => 4.0 / (36.0 + (x * x));

                //INT [0 .. inf] x / (e^x - 1) dx = (pi^2)/6
                case 17: return x => x / (Math.Exp(x) - 1.0);

                //INT [0 .. inf] x^4.2 * e^(-x) dx = gamma(26/5)
                case 18: return x => Math.Pow(x, 4.2) * Math.Exp(-x);


            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }



        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(3, 0.0, 2.0, 16.452627765507230225)]
        [TestCase(4, 2.0, 4.0, -1.0568528194400546906)]
        [TestCase(5, -1.0, 1.0, 12.070346316389634503)]
        [TestCase(6, -1.0, 2.0, 4.0826185263905451134)]
        public void Trapezoid_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Trapezoid(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(7, 0.0, 1.0, 1.0)]
        [TestCase(8, 0.0, 1.0, -1.0 / 9.0)]
        [TestCase(9, 0.0, 2.0, 2.0)]
        [TestCase(10, 0.0, 1.0, -1.25)]
        [TestCase(11, 3.0, 9.0, 1.7627471740390860505)]
        [TestCase(12, 0.0, 3.0 * Math.PI, 1.6747617989799612659)]
        public void Trapezoid_VerticalAsymptote_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Trapezoid(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(13, 1.0, VMath.InfP, 1.0)]
        [TestCase(14, 0.0, VMath.InfP, Math.PI)]
        [TestCase(15, VMath.InfN, 0.0, -0.5)]
        [TestCase(16, VMath.InfN, 0.0, 1.0471975511965977462)]
        [TestCase(17, 0.0, VMath.InfP, 1.6449340668482264365)]
        [TestCase(18, 0.0, VMath.InfP, 32.578096050331346083)]
        public void Trapezoid_HorizontalAsymptote_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Trapezoid(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }



        /*************************************************************************/



        [TestCase(1, 1.0, Math.E, 1.0)]
        [TestCase(2, -1.0, 1.0, 1.5707963267948966192)]
        [TestCase(3, 0.0, 2.0, 16.452627765507230225)]
        [TestCase(4, 2.0, 4.0, -1.0568528194400546906)]
        [TestCase(5, -1.0, 1.0, 12.070346316389634503)]
        [TestCase(6, -1.0, 2.0, 4.0826185263905451134)]
        public void Romberg_NormalIntergral_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Romberg(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(7, 0.0, 1.0, 1.0)]
        [TestCase(8, 0.0, 1.0, -1.0 / 9.0)]
        [TestCase(9, 0.0, 2.0, 2.0)]
        [TestCase(10, 0.0, 1.0, -1.25)]
        [TestCase(11, 3.0, 9.0, 1.7627471740390860505)]
        [TestCase(12, 0.0, 3.0 * Math.PI, 1.6747617989799612659)]
        public void Romberg_VerticalAsymptote_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Romberg(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }


        [TestCase(13, 1.0, VMath.InfP, 1.0)]
        [TestCase(14, 0.0, VMath.InfP, Math.PI)]
        [TestCase(15, VMath.InfN, 0.0, -0.5)]
        [TestCase(16, VMath.InfN, 0.0, 1.0471975511965977462)]
        [TestCase(17, 0.0, VMath.InfP, 1.6449340668482264365)]
        [TestCase(18, 0.0, VMath.InfP, 32.578096050331346083)]
        public void Romberg_HorizontalAsymptote_ExpectedValue(int fx, double a, double b, double act)
        {
            Integrator ing = GetIntegrator();
            VFunc f = GetFunc(fx);

            var res = ing.Romberg(f, a, b);

            LogResults(fx, res);
            Assert.That(res.Value, Ist.WithinTolOf(act, exp));
        }

    }
}
