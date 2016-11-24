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
    public class QtrinTests : AlgebraicTests
    {
        /// <summary>
        /// the tollarance used in desk calculations, the reason this is set 
        /// so high is due to the number of significant digits used.
        /// </summary>
        public const Double VTOL = 1.0e-5;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public QtrinTests() { tol = VTOL; }
        public QtrinTests(double tol) { this.tol = tol; }

        public override dynamic GetZero()
        {
            return new Qtrin(0.0);
        }

        public override dynamic GetOne()
        {
            return new Qtrin(1.0);
        }

        public override dynamic GetSample(int i)
        {
            switch (i)
            {
                case 1: return new Qtrin(-0.04755, 0.05709, -1.74540, 0.71854);
                case 2: return new Qtrin(-0.67505, -1.15020, -0.68752, 0.77692);
                case 3: return new Qtrin(2.55340, 0.86221, 1.43190, -0.20076);
                case 4: return new Qtrin(1.96840, -2.41370, -4.78920, 3.37100);
                case 5: return new Qtrin(1.48430, -0.17492, 3.20390, 2.60800);
                case 6: return new Qtrin(-2.60690, 0.67205, 0.67762, -2.43590);
                case 7: return new Qtrin(-1.70290, -0.59567, -1.70170, -0.44760);
                case 8: return new Qtrin(2.25710, -4.43930, -1.88070, -0.79924);
                case 9: return new Qtrin(-1.34290, -2.44440, 1.34300, 0.22223);
                case 10: return new Qtrin(2.43570, 1.05200, -0.14820, 2.34210);


                //Unit(q)
                case 11: return new Qtrin(-0.0251723, 0.0302227, -0.923991, 0.380385);
                case 12: return new Qtrin(-0.399519, -0.68073, -0.406899, 0.459809);
                case 13: return new Qtrin(0.834877, 0.281914, 0.468184, -0.0656419);
                case 14: return new Qtrin(0.296745, -0.363876, -0.721994, 0.508194);
                case 15: return new Qtrin(0.337862, -0.0398159, 0.729283, 0.593642);

                //Inverse(q)
                case 16: return new Qtrin(-0.0133259, -0.0159995, 0.489149, -0.201371);
                case 17: return new Qtrin(-0.23645, 0.40288, 0.240818, -0.272132);
                case 18: return new Qtrin(0.272977, -0.0921765, -0.153081, 0.0214627);
                case 19: return new Qtrin(0.0447357, 0.054856, 0.108844, -0.0766126);
                case 20: return new Qtrin(0.0769052, 0.00906303, -0.166002, -0.135127);

                //Versor(q)
                case 21: return new Qtrin(0.0, 0.0302323, -0.924284, 0.380506);
                case 22: return new Qtrin(0.0, -0.742567, -0.443862, 0.501578);
                case 23: return new Qtrin(0.0, 0.512164, 0.850568, -0.119254);
                case 24: return new Qtrin(0.0, -0.38104, -0.756049, 0.532164);
                case 25: return new Qtrin(0.0, -0.0423035, 0.774847, 0.630732);


                case 26: return new Qtrin(-0.7226, -1.09311, -2.43292, 1.49546);        //1 + 2
                case 27: return new Qtrin(1.87835, -0.28799, 0.74438, 0.57616);         //2 + 3
                case 28: return new Qtrin(4.5218, -1.55149, -3.3573, 3.17024);          //3 + 4

                case 29: return new Qtrin(0.6275, 1.20729, -1.05788, -0.05838);         //1 - 2
                case 30: return new Qtrin(-3.22845, -2.01241, -2.11942, 0.97768);       //2 - 3
                case 31: return new Qtrin(0.585, 3.27591, 6.2211, -3.57176);            //3 - 4

                case 32: return new Qtrin(-1.66048, -0.845872, 0.340105, -2.5688);      //1 * 2
                case 33: return new Qtrin(0.408476, -4.4934, -2.28316, 1.06513);        //2 * 3
                case 34: return new Qtrin(14.6416, -0.600512, -11.8321, 7.53922);       //3 * 4

                case 35: return new Qtrin(-0.0951, 0.11418, -3.4908, 1.43708);          //1 * (2.0)
                case 36: return new Qtrin(-0.337525, -0.5751, -0.34376, 0.38846);       //2 * (0.5)
                case 37: return new Qtrin(-7.6602, -2.58663, -4.2957, 0.60228);         //3 * (-3.0)


            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new NotImplementedException();
        }







        [TestCase(1, 1.88898)]
        [TestCase(2, 1.68966)]
        [TestCase(3, 3.05841)]
        [TestCase(4, 6.63330)]
        [TestCase(5, 4.39322)]
        public void Abs_VariousInput_ExpectedValue(int n, double exp)
        {
            dynamic q = GetSample(n);

            dynamic abs = q.Abs;

            Assert.That(abs, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 1.595971)]
        [TestCase(2, 1.981788)]
        [TestCase(3, 0.582887)]
        [TestCase(4, 1.269514)]
        [TestCase(5, 1.226152)]
        public void Arg_VariousInput_ExpectedValue(int n, double exp)
        {
            dynamic q = GetSample(n);

            dynamic arg = q.Arg;

            Assert.That(arg, Ist.WithinTolOf(exp, tol));
        }


        [TestCase(1, 16)]
        [TestCase(2, 17)]
        [TestCase(3, 18)]
        [TestCase(4, 19)]
        [TestCase(5, 20)]
        public void Inv_VariousInput_ExpectedValue(int n, int m)
        {
            dynamic q = GetSample(n);
            dynamic exp = GetSample(m);

            dynamic res = q.Inv();

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, -0.04755, 1.88838)]
        [TestCase(2, -0.67505, 1.54895)]
        [TestCase(3, 2.55340, 1.68346)]
        [TestCase(4, 1.96840, 6.33451)]
        [TestCase(5, 1.48430, 4.13488)]
        public void Decomp_VariousInput_ExpectedValue(int n, double e1, double e2)
        {
            dynamic q = GetSample(n);
            dynamic exp = new Cmplx(e1, e2);

            dynamic res = q.Decomp();

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 21)]
        [TestCase(2, 22)]
        [TestCase(3, 23)]
        [TestCase(4, 24)]
        [TestCase(5, 25)]
        public void Versor_VariousInput_ExpectedValue(int n, int m)
        {
            dynamic q = GetSample(n);
            dynamic exp = GetSample(m);

            dynamic res = q.Versor();

            Assert.That(res, Ist.WithinTolOf(exp, tol)); 
        }

        [TestCase(1, 2, 26)]
        [TestCase(2, 3, 27)]
        [TestCase(3, 4, 28)]
        public void Add_VariousInput_ExpectedResult(int n, int m, int ei)
        {
            dynamic p = GetSample(n);
            dynamic q = GetSample(m);
            dynamic exp = GetSample(ei);

            dynamic res = p.Add(q);

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 2, 29)]
        [TestCase(2, 3, 30)]
        [TestCase(3, 4, 31)]
        public void Sub_VariousInput_ExpectedResult(int n, int m, int ei)
        {
            dynamic p = GetSample(n);
            dynamic q = GetSample(m);
            dynamic exp = GetSample(ei);

            dynamic res = p.Sub(q);

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 2, 32)]
        [TestCase(2, 3, 33)]
        [TestCase(3, 4, 34)]
        public void Mult_VariousInput_ExpectedResult(int n, int m, int ei)
        {
            dynamic p = GetSample(n);
            dynamic q = GetSample(m);
            dynamic exp = GetSample(ei);

            dynamic res = p.Mult(q);

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 2.0, 35)]
        [TestCase(2, 0.5, 36)]
        [TestCase(3, -3.0, 37)]
        public void Mult_ScalarInput_ExpectedResult(int n, double s, int ei)
        {
            dynamic p = GetSample(n);
            dynamic exp = GetSample(ei);

            dynamic res = p.Mult(s);

            Assert.That(res, Ist.WithinTolOf(exp, tol));
        }





        [TestCase(1, -2.977712155e-01, 9.058776435e-01)]
        [TestCase(2, 1.112136658e-02, 5.090094823e-01)]
        [TestCase(3, -1.444789207e+00, 1.276924591e+01)]
        [TestCase(4, 7.149784921e+00, 3.672878208e-01)]
        [TestCase(5, -2.408611779e+00, 3.696381920e+00)]  //-
        [TestCase(6, -6.381372223e-02, 3.699685888e-02)]
        [TestCase(7, -5.154217622e-02, 1.747102439e-01)]
        [TestCase(8, 1.660418435e+00, 9.409968322e+00)]   //-
        public void Exp_SingleInput_IsScalable(int n, double cr, double ci)
        {
            dynamic q = GetSample(n);

            dynamic c1 = Qtrin.Exp(q).Decomp();
            dynamic c2 = new Cmplx(cr, ci);

            Assert.That(c1, Ist.WithinTolOf(c2, tol));
        }

        [TestCase(1, 2.0, 2.504944207e-01, 9.345907647e-01)]
        [TestCase(2, 2.0, 2.986990722e-01, 5.504940713e-01)]
        [TestCase(3, 0.5, 6.695138531e-02, 1.566450970e-01)]
        [TestCase(4, 0.5, -8.078105116e-02, 2.424318640e-01)]
        [TestCase(5, 3.25, 9.231955483e-01, 5.677022258e+00)]   //-
        [TestCase(6, 3.25, -4.622107796e-02, 2.684390262e-03)]
        [TestCase(7, 0.75, 1.404564212e+00, 8.313180810e-01)]   //-
        [TestCase(8, 0.75, 8.574378209e-02, 5.153122930e-01)]   //-
        public void Exp_CustomeBase_IsScalable(int n, double bass, double cr, double ci)
        {
            dynamic q = GetSample(n);

            dynamic c1 = Qtrin.Exp(q, bass).Decomp();
            dynamic c2 = new Cmplx(cr, ci);

            Assert.That(c1, Ist.WithinTolOf(c2, tol));
        }


        [TestCase(2.0, -0.98943, 0.44006, -1.73640, 0.19960)]
        [TestCase(2.0, 1.49770, 2.07120, 1.72120, 1.20740)]
        [TestCase(0.5, 2.15460, 0.22362, -0.93243, 0.05281)]
        [TestCase(0.5, 2.16670, 1.29520, -1.01000, -2.21390)]
        [TestCase(3.25, 0.64393, 0.22777, 1.92610, 2.07760)]
        [TestCase(3.25, 0.04469, -0.34234, 0.79609, -1.07940)]
        [TestCase(0.75, 0.90999, -0.71533, -0.58142, 1.15930)]
        [TestCase(0.75, -1.31280, -0.65119, 2.31820, 1.62250)]
        public void Exp_CustomeBase_SameAsLong(double bass, double qr, double qi, double qj, double qk)
        {
            dynamic q = new Qtrin(qr, qi, qj, qk);

            dynamic p1 = Qtrin.Exp(q, bass);
            dynamic p2 = Qtrin.Exp(q * Math.Log(bass));

            Assert.That(p1, Ist.WithinTolOf(p2, tol));
        }


        [TestCase(1, 6.360364014e-01, 1.595971316e+00)]
        [TestCase(2, 5.245255851e-01, 1.981788355e+00)]
        [TestCase(3, 1.117896675e-00, 5.828870392e-01)]
        [TestCase(4, 1.892101907e+00, 1.269513639e+00)]
        [TestCase(5, 1.480062463e+00, 1.226152451e+00)]
        [TestCase(6, 1.306516394e+00, 2.354416627e+00)]
        [TestCase(7, 9.242919542e-01, 2.312753097e+00)]
        [TestCase(8, 1.683263318e+00, 1.138128614e+00)]
        public void Log_SingleInput_IsScalable(int n, double cr, double ci)
        {
            dynamic q = GetSample(n);

            dynamic c1 = Qtrin.Log(q).Decomp();
            dynamic c2 = new Cmplx(cr, ci);

            Assert.That(c1, Ist.WithinTolOf(c2, tol));
        }


        [TestCase(0.63681, 0.94402, 0.65269, 1.21750)]
        [TestCase(-1.13220, 1.76600, -0.65036, -1.78190)]
        [TestCase(2.04340, -1.27790, 0.67602, -0.55447)]
        [TestCase(0.36385, 0.64493, 3.16400, -2.24250)]
        [TestCase(-0.08664, 0.64548, 0.29402, -1.86530)]
        [TestCase(-1.32820, -0.91298, -0.05527, -1.21790)]
        [TestCase(1.30220, 0.68109, 0.92065, 3.56040)]
        [TestCase(3.35870, 0.75695, -1.46600, 2.33140)]
        public void Log_SingleInput_IsReversable(double qr, double qi, double qj, double qk)
        {
            dynamic q = new Qtrin(qr, qi, qj, qk);
            dynamic p = Qtrin.Exp(Qtrin.Log(q));

            Assert.That(q, Ist.WithinTolOf(p, tol));
        }


        [TestCase(1, 2.0, 9.176065621e-01, 2.302499903e+00)]
        [TestCase(2, 2.0, 7.567304604e-01, 2.859116232e+00)]
        [TestCase(3, 0.5, -1.612783990e+00, 8.409282409e-01)]   //-
        [TestCase(4, 0.5, -2.729726037e+00, 1.831521032e+00)]   //-
        [TestCase(5, 3.25, 1.255721536e+00, 1.040298013e+00)]
        [TestCase(6, 3.25, 1.108480766e+00, 1.997545197e+00)]
        [TestCase(7, 0.75, -3.212893825e+00, 8.039267367e+00)]   //-
        [TestCase(8, 0.75, -5.851123443e+00, 3.956202776e+00)]   //-
        public void Log_CustomeBase_IsScalable(int n, double bass, double cr, double ci)
        {
            dynamic q = GetSample(n);

            dynamic c1 = Qtrin.Log(q, bass).Decomp();
            dynamic c2 = new Cmplx(cr, ci);

            Assert.That(c1, Ist.WithinTolOf(c2, tol));
        }

        [TestCase(2.0, 2.33400, 2.37840, -0.39328, 1.37610)]
        [TestCase(2.0, -1.04140, 1.72840, 0.78966, -0.53665)]
        [TestCase(0.5, -0.72504, 1.86240, 2.88660, -4.18040)]
        [TestCase(0.5, 2.29240, -1.55940, -1.60320, 0.17235)]
        [TestCase(3.25, 2.01730, -0.42504, 0.32780, -0.28980)]
        [TestCase(3.25, -0.49086, 0.77223, 1.09880, -0.25531)]
        [TestCase(0.75, -0.63173, -0.37186, 1.11470, 1.00710)]
        [TestCase(0.75, -0.98105, 0.61427, 1.17720, 0.73994)]
        public void Log_CutsomeBase_IsReversable(double bass, double qr, double qi, double qj, double qk)
        {
            dynamic q = new Qtrin(qr, qi, qj, qk);

            dynamic p0 = Qtrin.Log(q, bass);
            dynamic p1 = Qtrin.Exp(p0, bass);

            Assert.That(q, Ist.WithinTolOf(p1, tol));
        }


        //1.5, 3.0, -2.5, 0.75


        [TestCase(1, 1.5, -1.903800371e+00, 1.765184493e+00)]
        [TestCase(2, 1.5, -2.165074365e+00, 3.692210392e-01)]
        [TestCase(3, 3.0, -5.061587598e+00, 2.815677417e+01)]
        [TestCase(4, 3.0, -2.293254450e+02, 1.805476776e+02)]     //-
        [TestCase(5, -2.5, -2.464791243e-02, 1.882100333e-03)]    //-
        [TestCase(6, -2.5, 3.517860477e-02, 1.475497772e-02)]
        [TestCase(7, 0.75, -3.260982654e-01, 1.973381424e+00)]
        [TestCase(8, 0.75, 2.322856437e+00, 2.663441660e+00)]
        public void Pow_RealPower_IsScalable(int n, double pow, double cr, double ci)
        {
            dynamic q = GetSample(n);

            dynamic c1 = Qtrin.Pow(q, pow).Decomp();
            dynamic c2 = new Cmplx(cr, ci);

            Assert.That(c1, Ist.WithinTolOf(c2, tol));
        }


        [TestCase(1.5, 3.56750, -2.53070, -0.91451, 1.06310)]
        [TestCase(1.5, -0.55176, 2.98850, 1.05350, 0.36312)]
        [TestCase(3.0, 1.20540, 2.05440, 0.27027, -0.02017)]
        [TestCase(3.0, 1.09320, -1.89870, 1.97660, 1.49450)]
        [TestCase(-2.5, 1.88770, 1.02850, 2.95810, 1.74470)]
        [TestCase(-2.5, 0.13607, -0.80494, -2.11000, -0.86335)]
        [TestCase(0.75, -3.76880, -0.28243, -0.56477, 1.14670)]
        [TestCase(0.75, 0.89588, 1.92290, 1.57630, -1.72750)] 
        public void Pow_RealPower_SameAsLong(double pow, double qr, double qi, double qj, double qk)
        {
            dynamic q = new Qtrin(qr, qi, qj, qk);

            dynamic p1 = Qtrin.Pow(q, pow);
            dynamic p2 = Qtrin.Exp(Qtrin.Log(q).Mult(pow));

            Assert.That(p1, Ist.WithinTolOf(p2, tol));
        }


        [TestCase(0.0, 1.0)]
        [TestCase(1.0, Math.E)]
        [TestCase(-1.0, 1.0 / Math.E)]
        public void Exp_RealInput_ExpectedResult(double input, double exp)
        {
            dynamic q = new Qtrin(input, VMath.TOL, 0.0, 0.0);
            dynamic e = new Qtrin(exp);

            dynamic p = Qtrin.Exp(q);

            Assert.That(p, Ist.WithinTolOf(e, tol));
        }



    }
}
