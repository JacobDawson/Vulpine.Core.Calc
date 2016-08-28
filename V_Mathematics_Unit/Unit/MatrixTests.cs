using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Calc_Tests.Unit
{
    [TestFixture]
    public class MatrixTests : AlgebraicTests
    {
        //IDEA: Should add tests for non-square matricies

        /// <summary>
        /// the tollarance used in desk calculations, the reason this is set 
        /// so high is due to the number of significant digits used.
        /// </summary>
        public const Double VTOL = 1.0e-5;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public MatrixTests() { tol = VTOL; }
        public MatrixTests(double tol) { this.tol = tol; }


        /// <summary>
        /// Obtains the additive ininty, or zero vector for the set of
        /// vectors that are being tested.
        /// </summary>
        /// <returns>The zero vector</returns>
        public override dynamic GetZero()
        {
            return new Matrix(3, 3);
        }

        /// <summary>
        /// Obtains the multiplicitive ininty, often refered to as the
        /// identiity element, or the one vector.
        /// </summary>
        /// <returns>The one vector</returns>
        public override dynamic GetOne()
        {
            return Matrix.Ident(3);
        }

        /// <summary>
        /// Method which generates samples to be used in tests.
        /// </summary>
        /// <param name="i">The index of the sample to obtain</param>
        /// <returns>A sample matrix sutable for testing</returns>
        public override dynamic GetSample(int i)
        {
            switch (i)
            {
                case 1:
                    return new Matrix(3, 3, 
                        3.08290, -2.27780, 0.786170, 
                        -0.499470, 1.225680, -0.984560, 
                        -0.545910, 1.59490, 1.123940);
                case 2:
                    return new Matrix(3, 3,
                        -1.09100, 4.06220, 3.97370,
                        -0.107740, 2.33910, -0.282610,
                        -0.443130, -1.73410, -1.76130);
                case 3:
                    return new Matrix(3, 3,
                        1.69530, -1.24120, -1.06780,
                        -0.513130, 1.05320, 1.03210,
                        0.909070, 1.27670, 4.73330);
                case 4:
                    return new Matrix(3, 3,
                        -0.64290, -1.00340, -1.14110,
                        -0.360880, 4.46480, 1.32160,
                        4.05170, 2.81320, -1.79900);
                case 5:
                    return new Matrix(3, 3,
                        4.84900, -2.59810, -0.366460,
                        -1.09070, 1.327660, 1.05890,
                        0.746620, -4.34850, 1.120770);

                case 6: //M1 + M2
                    return new Matrix(3, 3,
                        1.9919, 1.7844, 4.75987,
                        -0.60721, 3.56478, -1.26717,
                        -0.98904, -0.1392, -0.63736);
                case 7: //M2 + M3
                    return new Matrix(3, 3,
                        0.6043, 2.821, 2.9059,
                        -0.62087, 3.3923, 0.74949,
                        0.46594, -0.4574, 2.972);
                case 8: //M3 + M4
                    return new Matrix(3, 3,
                        1.0524, -2.2446, -2.2089,
                        -0.87401, 5.518, 2.3537,
                        4.96077, 4.0899, 2.9343);

                case 9: //M1 + 3.0
                    return new Matrix(3, 3,
                        6.0829, -2.2778, 0.78617,
                        -0.49947, 4.22568, -0.98456,
                        -0.54591, 1.5949, 4.12394);
                case 10: //M2 - 2.0
                    return new Matrix(3, 3,
                        -3.091, 4.0622, 3.9737,
                        -0.10774, 0.3391, -0.28261,
                        -0.44313, -1.7341, -3.7613);
                case 11: //M3 + 5.0
                    return new Matrix(3, 3,
                        6.6953, -1.2412, -1.0678,
                        -0.51313, 6.0532, 1.0321,
                        0.90907, 1.2767, 9.7333);

                case 12: //m1 - m2
                    return new Matrix(3, 3,
                        4.1739, -6.34, -3.18753,
                        -0.39173, -1.11342, -0.70195,
                        -0.10278, 3.329, 2.88524);
                case 13: //m2 - m3
                    return new Matrix(3, 3,
                        -2.7863, 5.3034, 5.0415,
                        0.40539, 1.2859, -1.31471,
                        -1.3522, -3.0108, -6.4946);
                case 14: //m3 - m4
                    return new Matrix(3, 3,
                        2.3382, -0.2378, 0.0733,
                        -0.15225, -3.4116, -0.2895,
                        -3.14263, -1.5365, 6.5323);

                case 15: //m1 * m2
                    return new Matrix(3, 3,
                        -3.46641, 5.83206, 11.5096,
                        0.849155, 2.54537, -0.597028,
                        -0.0742982, -0.435989, -4.59961);
                case 16: //m2 * m3
                    return new Matrix(3, 3,
                        -0.321638, 10.7057, 24.1663,
                        -1.63983, 2.23646, 1.19155,
                        -1.46256, -3.52499, -9.65335);
                case 17: //m3 * m4
                    return new Matrix(3, 3,
                        -4.96839, -10.2467, -1.6539,
                        4.13157, 8.12071, 0.120694,
                        18.1327, 18.1038, -7.86526);

                case 18: //m1 * 3.0
                    return new Matrix(3, 3,
                        9.2487, -6.8334, 2.35851,
                        -1.49841, 3.67704, -2.95368,
                        -1.63773, 4.7847, 3.37182);
                case 19: //m2 * -2.0
                    return new Matrix(3, 3,
                        2.182, -8.1244, -7.9474,
                        0.21548, -4.6782, 0.56522,
                        0.88626, 3.4682, 3.5226);
                case 20: //m3 * 1.5
                    return new Matrix(3, 3,
                        2.54295, -1.8618, -1.6017,
                        -0.769695, 1.5798, 1.54815,
                        1.36361, 1.91505, 7.09995);

                case 21: //trans(m1)
                    return new Matrix(3, 3,
                        3.08290, -0.499470, -0.545910,
                        -2.27780, 1.225680, 1.59490,
                        0.786170, -0.984560, 1.123940);
                case 22: //trans(m2)
                    return new Matrix(3, 3,
                        -1.09100, -0.107740, -0.443130,
                        4.06220, 2.33910, -1.73410,
                        3.97370, -0.282610, -1.76130);
                case 23: //trans(m3)
                    return new Matrix(3, 3,
                        1.69530, -0.513130, 0.909070,
                        -1.24120, 1.05320, 1.27670,
                        -1.06780, 1.03210, 4.73330);

                case 24: //inv(m1)
                    return new Matrix(3, 3,
                        0.454583, 0.588143, 0.197237,
                        0.169452, 0.600511, 0.407514,
                        -0.0196605, -0.566472, 0.407255);
                case 25: //inv(m2)
                    return new Matrix(3, 3,
                        -0.478777, 0.0274142, -1.08458,
                        -0.00670189, 0.38245, -0.0764864,
                        0.127055, -0.383441, -0.219586);
                case 26: //inv(m3)
                    return new Matrix(3, 3,
                        0.975357, 1.1999, -0.0416042,
                        0.895471, 2.39225, -0.31962,
                        -0.428859, -0.875705, 0.30547);
                case 27: //inv(m4)
                    return new Matrix(3, 3,
                        -0.476994, -0.203594, 0.15299,
                        0.191019, 0.234637, 0.0512087,
                        -0.775576, -0.0916174, -0.131224);
                case 28: //inv(m5)
                    return new Matrix(3, 3,
                        0.265609, 0.196415, -0.0987255,
                        0.087758, 0.248851, -0.206419,
                        0.163554, 0.834678, 0.15712);




            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains vairous vectors for use in testing matricies.
        /// </summary>
        /// <param name="i">The index of the sample to obtain</param>
        /// <returns>A sample vector to use in testing</returns>
        public dynamic GetVector(int i)
        {
            switch (i)
            {
                case 1: //matrix 1, row 0
                    return new Vector(3.08290, -2.27780, 0.786170);
                case 2: //matrix 2, row 1
                    return new Vector(-0.107740, 2.33910, -0.282610);
                case 3: //matrix 3, row 2
                    return new Vector(0.909070, 1.27670, 4.73330);
                case 4: //matrix 1, col 0
                    return new Vector(3.08290, -0.499470, -0.545910);
                case 5: //matrix 2, col 1
                    return new Vector(4.06220, 2.33910, -1.73410);
                case 6: //matrix 3, col 2
                    return new Vector(-1.06780, 1.03210, 4.73330);

                case 7:
                    return new Vector(3.6919, -1.2615, -4.9732);
                case 8:
                    return new Vector(-1.6951, -1.6745, 2.9663);
                case 9:
                    return new Vector(3.5326, -4.2052, -2.1320);
                case 10:
                    return new Vector(-0.39783, 2.7534, 7.1498);
                case 11:
                    return new Vector(5.0798, 8.0527, 1.3429);

                case 12: //m1 * v7
                    return new Vector(10.3454, 1.50623, -9.61699);
                case 13: //m2 * v8
                    return new Vector(6.83439, -4.5725, -1.56964);
                case 14: //m3 * v9
                    return new Vector(13.4849, -8.44204, -12.2488);
                case 15: //m4 * v10
                    return new Vector(-10.6656, 21.8861, -6.72851);
                case 16: //m5 * v11
                    return new Vector(3.21811, 6.57271, -29.7194);

            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }



        [TestCase(1, 0, 0, 3.08290)]
        [TestCase(2, 0, 2, 3.97370)]
        [TestCase(3, 2, 0, 0.909070)]
        public void GetElement_RandomMatrix_CorrectValue(int m, int i, int j, double expected)
        {
            dynamic matrix = GetSample(m);

            double x = matrix.GetElement(i, j);

            Assert.That(x, Ist.WithinTolOf(expected, VMath.ESP));
        }

        [TestCase(1, 0, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(3, 2, 3)]
        public void GetRow_RandomMatrix_CorrectValue(int m, int x, int exp)
        {
            dynamic matrix = GetSample(m);
            dynamic expected = GetVector(exp);

            dynamic output = matrix.GetRow(x);

            Assert.That(output, Ist.WithinTolOf(expected, VMath.ESP));
        }

        [TestCase(1, 0, 4)]
        [TestCase(2, 1, 5)]
        [TestCase(3, 2, 6)]
        public void GetColumn_RandomMatrix_CorrectValue(int m, int x, int exp)
        {
            dynamic matrix = GetSample(m);
            dynamic expected = GetVector(exp);

            dynamic output = matrix.GetColumn(x);

            Assert.That(output, Ist.WithinTolOf(expected, VMath.ESP));
        }

        [TestCase(1, 2, 6)]
        [TestCase(2, 3, 7)]
        [TestCase(3, 4, 8)]
        public void Add_WithMatrix_ExpectedResults(int ai, int bi, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic b = GetSample(bi);
            dynamic r = GetSample(ri);

            dynamic output = a.Add(b);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 3.0, 9)]
        [TestCase(2, -2.0, 10)]
        [TestCase(3, 5.0, 11)]
        public void Add_WithScalor_ExpectedResults(int ai, double s, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic r = GetSample(ri);

            dynamic output = a.Add(s);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 2, 12)]
        [TestCase(2, 3, 13)]
        [TestCase(3, 4, 14)]
        public void Sub_WithMatrix_ExpectedResults(int ai, int bi, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic b = GetSample(bi);
            dynamic r = GetSample(ri);

            dynamic output = a.Sub(b);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 2, 15)]
        [TestCase(2, 3, 16)]
        [TestCase(3, 4, 17)]
        public void Mult_WithMatrix_ExpectedResults(int ai, int bi, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic b = GetSample(bi);
            dynamic r = GetSample(ri);

            dynamic output = a.Mult(b);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 7, 12)]
        [TestCase(2, 8, 13)]
        [TestCase(3, 9, 14)]
        [TestCase(4, 10, 15)]
        [TestCase(5, 11, 16)]
        public void Mult_WithVector_ExpectedResults(int ai, int vi, int vri)
        {
            dynamic a = GetSample(ai);
            dynamic v = GetVector(vi);
            dynamic r = GetVector(vri);

            dynamic output = a.Mult(v);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 3.0, 18)]
        [TestCase(2, -2.0, 19)]
        [TestCase(3, 1.5, 20)]
        public void Mult_WithScalor_ExpectedResults(int ai, double s, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic r = GetSample(ri);

            dynamic output = a.Mult(s);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        //[Ignore]
        [TestCase(1, 12, 7)]
        [TestCase(2, 13, 8)]
        [TestCase(3, 14, 9)]
        [TestCase(4, 15, 10)]
        [TestCase(5, 16, 11)]
        public void InvAx_WithVector_ExpectedResults(int ai, int vi, int vri)
        {
            dynamic a = GetSample(ai);
            dynamic v = GetVector(vi);
            dynamic r = GetVector(vri);

            dynamic output = a.InvAx(v);

            Assert.That(output, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 4.70498)]
        [TestCase(2, 6.73428)]
        [TestCase(3, 5.73176)]
        public void Mag_RandomMatrix_ExpectedReuslts(int ai, double exp)
        {
            dynamic a = GetSample(ai);

            double mag = a.Mag();

            Assert.That(mag, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 2, 9.43826)]
        [TestCase(2, 3, 10.8594)]
        [TestCase(3, 4, 8.49606)]
        public void Dist_TwoMatrices_ExpectedResults(int ai, int bi, double exp)
        {
            dynamic a = GetSample(ai);
            dynamic b = GetSample(bi);

            double dist = a.Dist(b);

            Assert.That(dist, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 21)]
        [TestCase(2, 22)]
        [TestCase(3, 23)]
        public void Trans_RandomMatrix_ExpectedResults(int ai, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic r = GetSample(ri);

            dynamic trans = a.Trans();

            Assert.That(trans, Ist.WithinTolOf(r, tol));
        }

        //[Ignore]
        [TestCase(1, 6.484769902480346)]
        [TestCase(2, 9.62855780193796)]
        [TestCase(3, 3.7600877152468)]
        [TestCase(4, 24.6336468316816)]
        [TestCase(5, 22.938297072964532)]
        public void Det_RandomMatrix_ExpectedResults(int ai, double exp)
        {
            dynamic a = GetSample(ai);

            double det = a.Det();

            Assert.That(det, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 5.43252)]
        [TestCase(2, -0.5132)]
        [TestCase(3, 7.4818)]
        public void Trace_RandomMatrix_ExpectedResults(int ai, double exp)
        {
            dynamic a = GetSample(ai);

            double tr = a.Trace();

            Assert.That(tr, Ist.WithinTolOf(exp, tol));
        }

        //[Ignore]
        [TestCase(1, 24)]
        [TestCase(2, 25)]
        [TestCase(3, 26)]
        [TestCase(4, 27)]
        [TestCase(5, 28)]
        public void Invert_RandomMatrix_ExpectedResults(int ai, int ri)
        {
            dynamic a = GetSample(ai);
            dynamic r = GetSample(ri);

            dynamic inv = Matrix.Invert(a);

            Assert.That(inv, Ist.WithinTolOf(r, tol));
        }

    }
}
