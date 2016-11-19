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
    public class CmplxTexts : EuclideanTests
    {
        /// <summary>
        /// the tollarance used in desk calculations, the reason this is set 
        /// so high is due to the number of significant digits used.
        /// </summary>
        public const Double VTOL = 1.0e-9;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public CmplxTexts() { tol = VTOL; }
        public CmplxTexts(double tol) { this.tol = tol; }

        /// <summary>
        /// Obtains the complex number zero.
        /// </summary>
        /// <returns>Returns zero</returns>
        public override dynamic GetZero()
        {
            return new Cmplx(0.0, 0.0);
        }

        public override dynamic GetSample(int i)
        {
            switch (i)
            {
                case 1: return new Cmplx(-0.3359978120, -0.2883891882);
                case 2: return new Cmplx(1.665660902, 3.183652389);
                case 3: return new Cmplx(2.145846053, -2.082449443);
                case 4: return new Cmplx(-1.180137148, 2.194679590);
                case 5: return new Cmplx(3.091222659, -3.340571706);
                case 6: return new Cmplx(1.785173315, -1.859005973);
                case 7: return new Cmplx(3.329525492, -1.483974099);
                case 8: return new Cmplx(0.5737155175, 0.5494237726);
                case 9: return new Cmplx(-3.265105997, 1.782076995);
                case 10: return new Cmplx(0.9630845028, -0.2288886457);

                //neg(z)
                case 11: return new Cmplx(0.3359978120, 0.2883891882);
                case 12: return new Cmplx(-1.665660902, -3.183652389);
                case 13: return new Cmplx(-2.145846053, 2.082449443);
                case 14: return new Cmplx(1.180137148, -2.194679590);
                case 15: return new Cmplx(-3.091222659, 3.340571706);

                //conj(z)
                case 16: return new Cmplx(-0.3359978120, 0.2883891882);
                case 17: return new Cmplx(1.665660902, -3.183652389);
                case 18: return new Cmplx(2.145846053, 2.082449443);
                case 19: return new Cmplx(-1.180137148, -2.194679590);
                case 20: return new Cmplx(3.091222659, 3.340571706);

                //z^-1
                case 21: return new Cmplx(-1.71372499142173, 1.47090171847358);
                case 22: return new Cmplx(0.129020296568687, -0.246602279555933);
                case 23: return new Cmplx(0.239993940579611, 0.232903589325374);
                case 24: return new Cmplx(-0.190058320467283, -0.353448001824298);
                case 25: return new Cmplx(0.149225739364511, 0.161262819834945);

                //z*i
                case 26: return new Cmplx(0.2883891882, -0.335997812);
                case 27: return new Cmplx(-3.183652389, 1.665660902);
                case 28: return new Cmplx(2.082449443, 2.145846053);
                case 29: return new Cmplx(-2.19467959, -1.180137148);
                case 30: return new Cmplx(3.340571706, 3.091222659);


                case 31: return new Cmplx(1.32966309, 2.8952632008); //1 + 2
                case 32: return new Cmplx(3.811506955, 1.101202946); //2 + 3
                case 33: return new Cmplx(0.965708905, 0.112230147); //3 + 4

                case 34: return new Cmplx(-2.001658714, -3.4720415772); //1 - 2
                case 35: return new Cmplx(-0.480185151, 5.266101832);  //2 - 3
                case 36: return new Cmplx(3.325983201, -4.277129033); //3 - 4

                case 37: return new Cmplx(0.358472509368754, -1.55005883221683);  //1 * 2
                case 38: return new Cmplx(10.2040470163718, 3.36297329546289);    //2 * 3 
                case 39: return new Cmplx(2.03791664872449, 7.16702048231737);    //3 * 4

                case 40: return new Cmplx(-0.114467968560075, 0.0456497677762387);  //1 / 2
                case 41: return new Cmplx(-0.341735545022031, 1.15199568494654);    //2 / 3
                case 42: return new Cmplx(-1.14387349134300, -0.362658156060797);   //3 / 4
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }

        [Test]
        public void ToString_Normal_ContainsReal()
        {
            dynamic z = GetSample(1);

            string s1 = z.ToString("0.0000", null);
            string s2 = Math.Abs(z.CofR).ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [Test]
        public void ToString_Normal_ContainsImag()
        {
            dynamic z = GetSample(1);

            string s1 = z.ToString("0.0000", null);
            string s2 = Math.Abs(z.CofI).ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [Test]
        public void ToString_Infinity_ReturnsInf()
        {
            dynamic z = Cmplx.Inf;

            string s1 = z.ToString("0.0000", null);

            Assert.That(s1, Contains.Substring("Inf").IgnoreCase);
        }

        [Test]
        public void ToString_NaN_ReturnsNaN()
        {
            dynamic z = Cmplx.NaN;

            string s1 = z.ToString("0.0000", null);

            Assert.That(s1, Contains.Substring("NaN").IgnoreCase);
        }

        [TestCase(1, 0.442789852570542)]
        [TestCase(2, 3.59305841511613)]
        [TestCase(3, 2.99019246300747)]
        [TestCase(4, 2.49185517052098)]
        [TestCase(5, 4.55138186163749)]
        public void Abs_VariousInput_ExpectedValue(int a, double exp)
        {
            dynamic z1 = GetSample(a);

            dynamic res = z1.Abs;

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, -2.43229582033145)]
        [TestCase(2, 1.08876786193320)]
        [TestCase(3, -0.77040586682652)]
        [TestCase(4, 2.06416761741589)]
        [TestCase(5, -0.82414695164776)]
        public void Arg_VariousInput_ExpectedValue(int a, double exp)
        {
            dynamic z1 = GetSample(a);

            dynamic res = z1.Arg;

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 11)]
        [TestCase(2, 12)]
        [TestCase(3, 13)]
        [TestCase(4, 14)]
        [TestCase(5, 15)]
        public void Neg_VariousInput_ExpectedValue(int a, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(exp);

            dynamic res = z1.Neg();

            Assert.That(res, Ist.WithinTolOf(z2, tol));
        }

        [TestCase(1, 16)]
        [TestCase(2, 17)]
        [TestCase(3, 18)]
        [TestCase(4, 19)]
        [TestCase(5, 20)]
        public void Conj_VariousInput_ExpectedValue(int a, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(exp);

            dynamic res = z1.Conj();

            Assert.That(res, Ist.WithinTolOf(z2, tol));
        }

        [TestCase(1, 21)]
        [TestCase(2, 22)]
        [TestCase(3, 23)]
        [TestCase(4, 24)]
        [TestCase(5, 25)]
        public void Inv_VariousInput_ExpectedValue(int a, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(exp);

            dynamic res = z1.Inv();

            Assert.That(res, Ist.WithinTolOf(z2, tol));
        }

        [TestCase(1, 26)]
        [TestCase(2, 27)]
        [TestCase(3, 28)]
        [TestCase(4, 29)]
        [TestCase(5, 30)]
        public void MultI_VariousInput_ExpectedValue(int a, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(exp);

            dynamic res = z1.MultI();

            Assert.That(res, Ist.WithinTolOf(z2, tol));
        }

        [TestCase(1, 2, 31)]
        [TestCase(2, 3, 32)]
        [TestCase(3, 4, 33)]
        public void Add_TwoNumbers_ExpectedResult(int a, int b, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(b);
            dynamic z0 = GetSample(exp);

            dynamic res = z1.Add(z2);

            Assert.That(res, Ist.WithinTolOf(z0, tol));
        }

        [TestCase(1, 2, 34)]
        [TestCase(2, 3, 35)]
        [TestCase(3, 4, 36)]
        public void Sub_TwoNumbers_ExpectedResult(int a, int b, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(b);
            dynamic z0 = GetSample(exp);

            dynamic res = z1.Sub(z2);

            Assert.That(res, Ist.WithinTolOf(z0, tol));
        }

        [TestCase(1, 2, 37)]
        [TestCase(2, 3, 38)]
        [TestCase(3, 4, 39)]
        public void Mult_TwoNumbers_ExpectedReuslt(int a, int b, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(b);
            dynamic z0 = GetSample(exp);

            dynamic res = z1.Mult(z2);

            Assert.That(res, Ist.WithinTolOf(z0, tol));
        }

        [TestCase(1, 2, 40)]
        [TestCase(2, 3, 41)]
        [TestCase(3, 4, 42)]
        public void Div_TwoNumbers_ExpectedReuslt(int a, int b, int exp)
        {
            dynamic z1 = GetSample(a);
            dynamic z2 = GetSample(b);
            dynamic z0 = GetSample(exp);

            dynamic res = z1.Div(z2);

            Assert.That(res, Ist.WithinTolOf(z0, tol));
        }

        [TestCase(-0.45423, 3.24610, 2.83560, 3.09200, 3.293437147)]
        [TestCase(2.83560, 3.09200, -0.56796, -0.35781, 4.846174750)]
        [TestCase(-0.56796, -0.35781, 1.43850, -0.25795, 2.008943442)]
        [TestCase(1.43850, -0.25795, -1.24770, 1.53060, 3.227163080)]
        [TestCase(-1.24770, 1.53060, -0.38713, 1.73560, 0.8846500579)]
        public void Dist_TwoNumbers_ExpectedResult(double x1, double x2, double y1, double y2, double exp)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic y = new Cmplx(y1, y2);

            double res = x.Dist(y);

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(0.18149, 3.65690, 1.0, 0.18149, 3.6569)]
        [TestCase(-0.07039, 0.30866, 1.5, -0.1604673326, 0.07733145352)]
        [TestCase(0.56104, 2.49800, 3.0, -10.32607928, -13.22867448)]
        [TestCase(0.78610, -0.38247, -2.5, 0.5944954942, 1.266927992)]
        [TestCase(2.83560, 3.09200, 0.75, 2.383309608, 1.706760358)]
        public void Pow_RealPower_ExpectedResults(double x1, double x2, double p, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic res = Cmplx.Pow(x, (double)p);

            Assert.That(res, Ist.WithinTolOf(e, tol));
        }

        [TestCase(3.16830, 2.94940, -0.49478, 0.36996, 0.3616656357, 0.06252249002)]
        [TestCase(-0.49478, 0.36996, -1.50490, -1.08130, -30.64819156, 3.051395033)]
        [TestCase(-1.50490, -1.08130, 0.60931, -0.95943, -0.06854953545, -0.1104132480)]
        [TestCase(0.60931, -0.95943, 4.33760, 1.44200, -3.801474717, 6.374309847)]
        [TestCase(4.33760, 1.44200, 0.18149, 3.65690, 0.3200130436, -0.2521905668)]
        public void Pow_ComplexPower_ExpectedResults(double x1, double x2, double y1, double y2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic y = new Cmplx(y1, y2);
            dynamic e = new Cmplx(e1, e2);

            dynamic res = Cmplx.Pow(x, y);

            Assert.That(res, Ist.WithinTolOf(e, tol));
        }

        [TestCase(0.05898, 2.76690, -0.9871590708, 0.3882216782)]
        [TestCase(3.89180, -0.93200, 29.21465483, -39.33708776)]
        [TestCase(-2.85380, -4.58770, -0.007166589736, 0.05717755240)]
        [TestCase(2.32160, 0.42280, 9.294500708, 4.181922620)]
        [TestCase(3.16830, 2.94940, -23.32944176, 4.539782304)]
        public void Exp_DefaultBase_ExpectedResult(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Exp(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(-1.07330, 4.62700, 1.0, 1.0, 0.0)]
        [TestCase(-0.38713, 1.73560, 2.0, 0.2749185595, 0.7135182104)]
        [TestCase(-0.70963, -1.73950, 0.5, 0.5838514507, 1.527612654)]
        [TestCase(0.18889, 0.78849, 3.25, 0.7475565330, 1.001036653)]
        [TestCase(3.57710, 1.63570, 0.75, 0.3185020902, -0.1620132752)]
        public void Exp_RealBase_ExpectedResult(double x1, double x2, double b, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Exp(x, b);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(-1.18260, 1.87790, 0.7971673322, 2.132801214)]
        [TestCase(4.32890, -0.69287, 1.477961239, -0.1587106706)]
        [TestCase(-0.24995, -1.43550, 0.3764469444, -1.743188515)]
        [TestCase(3.13280, 0.26062, 1.145375605, 0.08299963605)]
        [TestCase(1.89270, -2.52110, 1.148195902, -0.9268194052)]
        public void Log_DefaultBase_ExpectedResult(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Log(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(-2.74380, -1.64030, 2.0, 1.676592651, -3.755013814)]
        [TestCase(-2.35210, 3.84870, 0.5, -2.173296440, -3.057601414)]
        [TestCase(0.12178, 1.13700, 3.25, 0.1137707445, 1.242175770)]
        [TestCase(0.41289, -1.18620, 0.75, -0.7923193029, 4.295835553)]
        [TestCase(2.47180, -0.21595, 5.0, 0.5646371916, -0.05414574581)]
        public void Log_RealBase_ExpectedResult(double x1, double x2, double b, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Log(x, b);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(-1.0, 0.0, 0.0, 1.0)]
        [TestCase(2.47180, -0.21595, 1.573692371, -0.06861252047)]
        [TestCase(2.58510, 0.68769, 1.621743885, 0.2120217645)]
        [TestCase(-2.05520, -1.07470, 0.3633385866, -1.478923571)]
        [TestCase(-0.20818, 0.71192, 0.5165044912, 0.6891711612)]
        public void Sqrt_RandomValues_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Sqrt(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(-0.25790, 0.79360, -0.3396708419, 0.8504753207)]
        [TestCase(-2.74020, -1.06710, -0.6350725975, 1.179612445)]
        [TestCase(0.39988, -1.26300, 0.7433478348, -1.498278093)]
        public void Sin_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Sin(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(0.39988, -1.26300, 1.758771353, 0.6332498961)]
        [TestCase(-0.75110, -1.44470, 1.635985581, -1.366511638)]
        [TestCase(-1.71780, 0.20009, -0.1494167133, 0.1992552904)]
        public void Cos_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Cos(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(1.49030, -2.28490, 0.003390363030, -1.020657742)]
        [TestCase(-0.11054, -0.12740, -0.1091886694, -0.1282508556)]
        [TestCase(-2.17570, 1.61050, 0.07672615560, 1.025693089)]
        public void Tan_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Tan(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(0.566690, -0.023549, 0.6022023366, -0.02857187192)]
        [TestCase(0.280710, -0.223210, 0.2769718560, -0.2300203299)]
        [TestCase(-0.350510, 0.904380, -0.2588402205, 0.8350551975)]
        public void Asin_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Asin(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(0.255670, 0.887060, 1.379928912, -0.8114416545)]
        [TestCase(-0.571750, 0.129550, 2.171025842, -0.1563532794)]
        [TestCase(-0.145570, -0.697560, 1.690191862, 0.6547639015)]
        public void Acos_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Acos(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }

        [TestCase(0.475200, 0.787940, 0.7054184286, 0.6342040784)]
        [TestCase(-0.266330, -0.037366, -0.2606126446, -0.03490226404)]
        [TestCase(0.363710, -0.794030, 0.6277726588, -0.7384608668)]
        public void Atan_RandomInput_ExpectedResults(double x1, double x2, double e1, double e2)
        {
            dynamic x = new Cmplx(x1, x2);
            dynamic e = new Cmplx(e1, e2);

            dynamic r = Cmplx.Atan(x);

            Assert.That(r, Ist.WithinTolOf(e, tol));
        }
    }
}
