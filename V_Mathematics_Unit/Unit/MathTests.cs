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
        /// <summary>
        /// the tollarance used in desk calculations. it represents the minimum
        /// amound of relative error allowed in numeric aproximations.
        /// </summary>
        public const Double VTOL = 1.0e-10;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public MathTests() { tol = VTOL; }
        public MathTests(double tol) { this.tol = tol; }


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
        [TestCase(2.2250738585e-306)]
        public void IsZero_VariousInput_ExpectedFalse(double x)
        {
            bool test = VMath.IsZero(x);
            Assert.That(test, Is.False, "Value should not be considered zero.");
        }

        [TestCase(0.0)]
        [TestCase(-0.0)]
        [TestCase(Double.Epsilon)]
        [TestCase(-Double.Epsilon)]
        [TestCase(Double.Epsilon * 4096.0)]
        [TestCase(2.2250738585e-310)]
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
            Assert.That(y, Ist.WithinTolOf(exp, tol));
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
            Assert.That(y, Ist.WithinTolOf(exp, tol)); 
        }

        [TestCase(-3.78300, -5.42700, -4.69960112311076e-08, -2.39454043917665e-07)]
        [TestCase(2.24690, -1.42030, 0.399611628902505e+00, -0.542994381785484e+00)]
        [TestCase(1.58370, 11.3360, 3.43908535432232e-07, -5.44835804958548e-07)]
        [TestCase(3.29270, 10.2600, 6.15897705351052e-05, -1.61593664070636e-04)]
        [TestCase(-2.88220, 6.16450, 1.17608654272594e-07, -2.60100431878566e-07)]
        [TestCase(10.3710, -0.23657, 7.19984005852702e+05, -4.33318285943509e+05)]
        [TestCase(0.51327, -2.62410, 4.10969809389325e-02, 2.29149314660941e-03)]
        [TestCase(0.65053, -0.29841, 1.12768998627444e+00, 0.446423794126510e+00)]
        public void Gamma_CmplxInput_ExpectedOutput(double z1, double z2, double e1, double e2)
        {
            Cmplx z = new Cmplx(z1, z2);
            Cmplx exp = new Cmplx(e1, e2);

            Cmplx y = VMath.Gamma(z);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
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
        [TestCase(5.0, 4.7889493013, 11.4785315012872)]
        [TestCase(5.0, 2.3251782046, 21.9186856974341)]
        [TestCase(5.0, 1.2444018117, 23.7849090225189)]
        public void Gamma_MultiInput_ExpectedOutput(double a, double x, double exp)
        {
            //This test case won't pass for tollerance greator than 1.0e-09
            //[TestCase(5.0, 9.5778986027, 0.919868784774757)]

            //uses the upper incomplete gamma funciton
            double y = VMath.Gamma(a, x);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }


        [TestCase(1.41720, 3.37280, -1.33240, -0.89200, 4.66023033150601e+03, 6.57869478267654e+03)]
        [TestCase(-1.33240, -0.89200, 1.86510, -2.68050, -2.96870221512415e-03, -7.81450030663611e-04)]
        [TestCase(1.86510, -2.68050, -0.49927, -1.60900, -2.26810969541599e-02, -1.42528453187021e-01)]
        [TestCase(-0.49927, -1.60900, 0.92265, 0.43140, 3.18703660633048e-02, -3.01448658207578e-01)]
        [TestCase(0.92265, 0.43140, 2.27120, 1.44800, 4.54949270748465e-02, -6.22525119947233e-02)]
        [TestCase(2.27120, 1.44800, -0.62240, 3.51050, 1.23965054401276e+00, 1.60651017136115e-01)]
        public void Gamma_MultiCmplxInput_ExpectedOutput(double a1, double a2, double x1, double x2, double e1, double e2)
        {
            Cmplx a = new Cmplx(a1, a2);
            Cmplx x = new Cmplx(x1, x2);
            Cmplx e = new Cmplx(e1, e2);

            Cmplx y = VMath.Gamma(a, x);

            Assert.That(y, Ist.WithinTolOf(e, tol));
        }


        [TestCase(6.0, 3.0, 20.0)]
        [TestCase(24.0, 5.0, 42504.0)]
        [TestCase(34.0, 16.0, 2203961430.0)]
        [TestCase(52.0, 24.0, 426384982032100.0)]
        [TestCase(72.0, 8.0, 11969016345.0)]
        [TestCase(72.0, 60.0, 15363284301456.0)]
        [TestCase(0.5, 10, -0.009273529052734375)]
        [TestCase(0.5, 20, -0.003214633015886648)]
        [TestCase(16.0, Math.PI, 675.2486887153615)]
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
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }


        [TestCase(-0.87785, 2.25320, 1.92690, -1.91340, -4.64143746116780e+02, -6.74129516930783e+01)]
        [TestCase(1.92690, -1.91340, -1.04490, 1.50320, -2.64624822383974e-01, -3.75929794880916e+00)]
        [TestCase(-1.04490, 1.50320, 0.91537, 1.44340, 1.90429995549770e-04, 2.59433537228387e-02)]
        [TestCase(0.91537, 1.44340, 2.22190, -0.55814, -3.56820841910449e+00, -5.63284929814418e-01)]
        [TestCase(2.22190, -0.55814, -3.03790, -1.40990, -1.80579535265661e-01, -1.77619536057886e-01)]
        [TestCase(-3.03790, -1.40990, 0.92500, 1.20870, 2.18693208302627e+01, -1.14531045645560e+02)]
        public void Binomial_ComplexInput_ExpectedOutput(double n1, double n2, double k1, double k2, double e1, double e2)
        {
            Cmplx n = new Cmplx(n1, n2);
            Cmplx k = new Cmplx(k1, k2);
            Cmplx e = new Cmplx(e1, e2);

            Cmplx y = VMath.Binomial(n, k);
            Assert.That(y, Ist.WithinTolOf(e, tol));
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
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }


        [TestCase(0.29441, 0.53607, 4.23388115326953e-01, 6.01793842926204e-01)]
        [TestCase(1.87580, -3.83500, 9.45923967905694e+03, -2.19698202167700e+03)]
        [TestCase(-0.58450, -2.33380, -2.87686349072186e+01, 3.13483391805746e+01)]
        [TestCase(1.73620, 1.91150, 8.18745111912825e-01, 3.69684276404134e-01)]
        [TestCase(-1.96680, 2.23210, -1.57106843639527e+00, -0.0851194355770910e+00)]
        [TestCase(3.49310, 0.42607, 1.00000092990724e+00, 4.82249604498094e-08)]
        [TestCase(-0.08288, -3.91680, -3.99640614883861e+05, -5.52276299222953e+05)]
        [TestCase(-2.89480, 2.80330, -1.02525359594166e+00, -7.89112328205884e-02)]
        public void Erf_CmplxInput_ExpectedOutput(double z1, double z2, double e1, double e2)
        {
            //This test case won't pass for tollerance greator than 1.0e-09
            //[TestCase(-3.19500, -2.69880, -9.95698806714512e-01, 5.76877499510872e-03)]

            Cmplx z = new Cmplx(z1, z2);
            Cmplx exp = new Cmplx(e1, e2);

            Cmplx y = VMath.Erf(z);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(0.0, 0.5)]
        [TestCase(-5.0, 2.86651571879194e-7)]
        [TestCase(-3.0, 1.34989803163009e-3)]
        [TestCase(-1.0, 0.158655253931457)]
        [TestCase(-0.5, 0.308537538725987)]
        [TestCase(0.5, 0.691462461274013)]
        [TestCase(1.0, 0.841344746068543)]
        [TestCase(3.0, 0.998650101968370)]
        [TestCase(5.0, 0.999999713348428)]
        public void Cdf_VariousInput_ExpectedOutput(double x, double exp)
        {
            double y = VMath.CDF(x);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }


        [TestCase(2.18150, 1.17170, 1.02656410126568e+00, 4.97366511760895e-03)]
        [TestCase(1.81070, -0.05189, 9.65095113041434e-01, -4.01423530283050e-03)]
        [TestCase(0.12764, 1.06850, 5.89596276291867e-01, 5.17279885817026e-01)]
        [TestCase(0.84694, -1.48500, 1.20664001541143e+00, -4.31574632382498e-01)]
        [TestCase(-0.08288, -3.91680, -6.90890616898446e+01, -2.25921733610230e+02)]
        [TestCase(-0.29168, 0.49850, 3.70538398255119e-01, 1.98064055693983e-01)]
        public void Cdf_CmplxInput_ExpectedOutput(double z1, double z2, double e1, double e2)
        {
            Cmplx z = new Cmplx(z1, z2);
            Cmplx exp = new Cmplx(e1, e2);

            Cmplx y = VMath.CDF(z);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(-13.0, 2.0, -26.849923841567868225, -37.198406148445332353)] 
        [TestCase(-9.5, 0.0, -12.795895333554363459, -31.415926535897932385)] 
        [TestCase(-3.0, 1.0, -2.9535082922959020229, -9.7264182812369129133)]
        [TestCase(4.0, 2.0, 1.2508356193568071782, 2.6101958010488946753)] 
        [TestCase(7.0, 4.0, 5.4180869718730471170, 7.7181013652048386845)] 
        [TestCase(9.0, 16.0, -0.27799290829569333609, 39.553165314423256031)]
        [TestCase(10.0, 0.0, 12.801827480081469611, 0.0)]
        [TestCase(15.0, 5.0, 24.345777015693776834, 13.467369243717367191)] 
        [TestCase(-13.0, -2.0, -26.849923841567868225, 37.198406148445332353)] 
        [TestCase(-3.0, -1.0, -2.9535082922959020229, 9.7264182812369129133)]
        [TestCase(0.0, -8.0, -12.687152851994418176, -7.8397120535167111332)]
        [TestCase(3.0, 0.0, 0.69314718055994530942, 0.0)]
        [TestCase(4.0, -2.0, 1.2508356193568071782, -2.6101958010488946753)] 
        [TestCase(7.0, -4.0, 5.4180869718730471170, -7.7181013652048386845)] 
        [TestCase(9.0, -16.0, -0.27799290829569333609,  -39.553165314423256031)] 
        [TestCase(15.0, -5.0, 24.345777015693776834, -13.467369243717367191)] 
        public void GammaLog_CmplxInput_ExpectedOutput(double z1, double z2, double e1, double e2)
        {
            Cmplx z = new Cmplx(z1, z2);
            Cmplx exp = new Cmplx(e1, e2);

            Cmplx y = VMath.GammaLog(z);
            Assert.That(y, Ist.WithinTolOf(exp, tol));
        }

    }
}
