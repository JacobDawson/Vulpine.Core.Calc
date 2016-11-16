using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;

namespace Vulpine_Core_Calc_Tests.Unit
{
    [TestFixture]
    public class MathTests
    {
        private Cmplx GetCmplx(int n)
        {
            switch (n)
            {
                case 0: return new Cmplx(-1.0445184600, -0.57773215910);
                case 1: return new Cmplx(-1.4976523500, -2.5360755830);
                case 2: return new Cmplx(0.62263369030, 1.4147873410);
                case 3: return new Cmplx(-3.4930822410, 0.42607424360);
                case 4: return new Cmplx(1.9324922290, 0.62974184760);
                case 5: return new Cmplx(4.2980018690, -3.1013584960);
                case 6: return new Cmplx(-3.3555079120, 7.9978442920);
                case 7: return new Cmplx(1.7137449450, 4.6592556790);
                case 8: return new Cmplx(5.4710230590, 1.3168351430);
                case 9: return new Cmplx(-2.1002182560, -1.5226184370);

                //results of the gamma function
                case 10: return new Cmplx(-0.142416877516903, -1.132059781631400);
                case 11: return new Cmplx(-0.00390104093073400, -0.00474677966930664);
                case 12: return new Cmplx(0.215009010126418, -0.183453949193736);
                case 13: return new Cmplx(0.112130608749238, 0.0784969554368719);
                case 14: return new Cmplx(0.826812551025766, 0.216168006211308);
                case 15: return new Cmplx(-0.764607350709280, 2.69231044936783);
                case 16: return new Cmplx(-2.81794073754196e-10, 2.50623038614254e-9);
                case 17: return new Cmplx(-0.00466537086579547, -0.00982974632802859);
                case 18: return new Cmplx(-22.2777231999429, 35.6868451800111);
                case 19: return new Cmplx(0.0342414817303455, 0.0122856408869635);

                //results of the error funciton
                case 20: return new Cmplx(-0.998103073230132, -0.185530903382701);
                case 21: return new Cmplx(-9.53933265834618, -9.67016230935939);
                case 22: return new Cmplx(2.75211150004256, 0.8126539401906065);
                case 23: return new Cmplx(-1.000000930031009, 4.82163348333148e-8);
                case 24: return new Cmplx(1.0081776973528331, 0.0038696318294437144);
            }

            return 0.0;
        }


        [TestCase(Double.NaN)]
        [TestCase(Double.NegativeInfinity)]
        [TestCase(Double.PositiveInfinity)]
        public void IsANumber_VariousInput_ExpectedFalse(double x)
        {
            bool test = VMath.IsANumber(x);
            Assert.That(test, Is.False, "Value was not a numeric value.");
        }

        [TestCase(2.0)]
        [TestCase(1.0 / 3.0)]
        [TestCase(Math.PI)]
        [TestCase(-0.0)]
        [TestCase(Double.Epsilon)]
        public void IsANumber_VariousInput_ExpectedTrue(double x)
        {
            bool test = VMath.IsANumber(x);
            Assert.That(test, Is.True, "Value was a nueric value.");
        }

        [TestCase(1.0)]
        [TestCase(-2.0)]
        [TestCase(1.0 / 3.0)]
        [TestCase(Double.PositiveInfinity)]
        [TestCase(Double.NaN)]
        public void IsZero_VariousInput_ExpectedFalse(double x)
        {
            bool test = VMath.IsZero(x);
            Assert.That(test, Is.False, "Value should not be considered zero.");
        }

        [TestCase(0.0)]
        [TestCase(-Double.Epsilon)]
        [TestCase(Double.Epsilon * 64.0)]
        public void IsZero_VariousInput_ExpectedTrue(double x)
        {
            bool test = VMath.IsZero(x);
            Assert.That(test, Is.True, "Value should be considered zero.");
        }

        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(0.0)]
        [TestCase(Double.Epsilon)]
        [TestCase(-Double.Epsilon)]
        [TestCase(Double.MaxValue)]
        [TestCase(Double.MinValue)]
        public void NextUp_VariousInput_OutputIncreases(double x)
        {
            double y = VMath.NextUp(x);
            Assert.That(y, Is.GreaterThan(x));
        }

        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(0.0)]
        [TestCase(Double.Epsilon)]
        [TestCase(-Double.Epsilon)]
        [TestCase(Double.MaxValue)]
        [TestCase(Double.MinValue)]
        public void NextDown_VariousInput_OutputDecreases(double x)
        {
            double y = VMath.NextDown(x);
            Assert.That(y, Is.LessThan(x));
        }

        [TestCase(0.0, 1.0)]
        [TestCase(1.0 / 3.0, 0.98158409038845673252000)]
        [TestCase(-5.0, -0.191784854932627693779)]
        [TestCase(7.5, 0.1250666635699651810598)]
        [TestCase(0.000005, 0.99999999999583333333333854166667)]
        [TestCase(Math.PI + 0.0001, -0.0000318299753857431926)]
        public void Sinc_VariousInput_ExpectedOutput(double x, double exp)
        {
            double y = VMath.Sinc(x);
            Assert.That(y, Ist.WithinTolOf(exp, VMath.TOL));
        }

        [TestCase(5.0, 24.0)]
        [TestCase(6.25, 184.86096222719834995)] 
        [TestCase(10.0, 362880.0)]
        [TestCase(73.0, 6.12344583768860868615e+103)]
        [TestCase(0.5, 1.7724538509055160273)]
        [TestCase(2.5, 1.3293403881791370205)]
        [TestCase(-0.5, -3.5449077018110320546)]
        [TestCase(0.25,  3.6256099082219083119)]
        [TestCase(1.4616321449683623413, 0.88560319441088870028)]
        [TestCase(-1.5, 2.3632718012073547030)]
        [TestCase(-1.0001, 9999.577356843892449)]
        public void Gamma_VariousInput_ExpectedOutput(double x, double exp)
        {
            double y = VMath.Gamma(x);
            Assert.That(y, Ist.WithinTolOf(exp, VMath.TOL)); 
        }

        [TestCase(0, 10)]
        [TestCase(1, 11)]
        [TestCase(2, 12)]
        [TestCase(3, 13)]
        [TestCase(4, 14)]
        [TestCase(5, 15)]
        [TestCase(6, 16)]
        [TestCase(7, 17)]
        [TestCase(8, 18)]
        [TestCase(9, 19)]
        public void Gamma_CmplxInput_ExpectedOutput(int zn, int en)
        {
            Cmplx z = GetCmplx(zn);
            Cmplx exp = GetCmplx(en);

            Cmplx y = VMath.Gamma(z);
            Assert.That(y, Ist.WithinTolOf(exp, VMath.TOL));
        }


        [TestCase(0.5, 0.52049987781304653768)]
        [TestCase(-1.0, -0.84270079294971486934)]
        [TestCase(Math.PI, 0.99999112385363235839473162)]
        [TestCase(0.75, 0.71115563365351513160)]
        [TestCase(-0.25, -0.276326390168236932985)]
        [TestCase(0.0001, 0.000112837916333424869486)]
        [TestCase(-4.0, -0.999999984582742099719981148)]
        [TestCase(5.0, 0.9999999999984625402055719651498)]
        [TestCase(2.99, 0.999976474396919359773184)]
        [TestCase(0.50001, 0.520508665594896616722)]
        public void Erf_VariousInput_ExpectedOutput(double x, double exp)
        {
            double y = VMath.Erf(x);
            Assert.That(y, Ist.WithinTolOf(exp, 1.0e-10));
        }

        [TestCase(0, 20)]
        [TestCase(1, 21)]
        [TestCase(2, 22)]
        [TestCase(3, 23)]
        [TestCase(4, 24)]
        public void Erf_CmplxInput_ExpectedOutput(int zn, int en)
        {
            Cmplx z = GetCmplx(zn);
            Cmplx exp = GetCmplx(en);

            Cmplx y = VMath.Erf(z);
            Assert.That(y, Ist.WithinTolOf(exp, 1.0e-10));
        }

        [TestCase(0.5, 1.3738007307, 0.172637787023159)]
        [TestCase(0.5, 3.3659000732, 0.0167867281951844)]
        [TestCase(0.5, 0.6784674614, 0.432601736096022)]
        [TestCase(0.5, 0.8638064724, 0.334490460917159)]
        [TestCase(1.0, 2.6666677558, 0.0694833755460999)]
        [TestCase(1.0, 4.6270786063, 0.00978329826089111)]
        [TestCase(1.0, 3.8299245024, 0.0217112547155983)]
        [TestCase(1.0, 1.0383773820, 0.354028669443186)]
        [TestCase(2.5, 1.0789390067, 1.09922682168843)]
        [TestCase(2.5, 3.5118755422, 0.290965884060299)]
        [TestCase(2.5, 6.5899012671, 0.0289142711761357)]
        [TestCase(2.5, 0.9679350635, 1.14050172234326)]
        [TestCase(5.0, 5.9410732885, 7.03251483283762)]
        [TestCase(5.0, 9.5778986027, 0.919868784774757)]
        [TestCase(5.0, 2.3251782046, 21.9186856974341)]
        [TestCase(5.0, 1.2444018117, 23.7849090225189)]
        public void Gamma_MultiInput_ExpectedOutput(double a, double x, double exp)
        {
            //uses the upper incomplete gamma funciton
            double y = VMath.Gamma(a) - VMath.Gamma(a, x);
            Assert.That(y, Ist.WithinTolOf(exp, 1.0e-10));
        }

        [TestCase(6.0, 3.0, 20.0)]
        [TestCase(24.0, 5.0, 42504.0)]
        [TestCase(34.0, 16.0, 2203961430.0)]
        [TestCase(52.0, 24.0, 426384982032100.0)]
        [TestCase(72.0, 8.0, 11969016345.0)]
        [TestCase(72.0, 60.0, 15363284301456.0)]
        [TestCase(0.5, 10, -0.009273529052734375)]
        [TestCase(0.5, 20, -0.003214633015886648)]
        [TestCase(16, Math.PI, 675.2486887153615)]
        [TestCase(VMath.TAU, Math.E, 22.68726548792432)]
        [TestCase(-1.515572593, -0.1361058568, 0.847702938770886)]
        [TestCase(-1.45927868, 0.7657063568, -0.895069929286101)]
        [TestCase(-0.3998426792, -1.360171874, -0.388066067805052)]
        [TestCase(1.627713632, -2.038288681, 0.00385792748132843)]
        [TestCase(0.6412807012, 0.04666553127, 1.03228770026497)]
        [TestCase(1.379424623, 0.6331685928, 1.48705896022135)]
        public void Binomial_VariousInput_ExpectedOutput(double n, double k, double exp)
        {
            //for some reason this returns NaN in debug mode, but passes normaly
            //[TestCase(100.0, 5.0, 75287520.0)]

            double y = VMath.Binomial(n, k);
            Assert.That(y, Ist.WithinTolOf(exp, 1.0e-12));
        }
    }
}
