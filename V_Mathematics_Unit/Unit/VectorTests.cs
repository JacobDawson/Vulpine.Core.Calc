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
    public class VectorTests : EuclideanTests
    {
        /// <summary>
        /// the tollarance used in desk calculations, the reason this is set 
        /// so high is due to the number of significant digits used.
        /// </summary>
        public const Double VTOL = 1.0e-5;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public VectorTests() { tol = VTOL; }
        public VectorTests(double tol) { this.tol = tol; }

        /// <summary>
        /// Obtains the additive ininty, or zero vector for the set of
        /// vectors that are being tested.
        /// </summary>
        /// <returns>The zero vector</returns>
        public override dynamic GetZero()
        {
            return new Vector(5);
        }

        /// <summary>
        /// Method which generates samples to be used in tests.
        /// </summary>
        /// <param name="i">The index of the sample to obtain</param>
        /// <returns>A sample vector sutable for testing</returns>
        public override dynamic GetSample(int i)
        {
            switch (i)
            {
                case 1: 
                    return new Vector(-2.9227, 3.6919, -1.2615, -4.9732, 0.36378); 
                case 2:
                    return new Vector(-7.4335, -1.6951, -1.6745, 2.9663, -6.5169);
                case 3:
                    return new Vector(3.5326, -4.2052, -2.1320, -7.3201, -5.3228);
                case 4:
                    return new Vector(-7.8772, -0.39783, 2.7534, 7.1498, -8.7349);
                case 5:
                    return new Vector(5.0798, -3.1305, 8.0527, 1.3429, -0.85412);
                case 6:
                    return new Vector(-6.8623, -2.9050, 6.6572);
                case 7:
                    return new Vector(-5.1564, -4.1494, -7.2296);
                case 8:
                    return new Vector(4.9903, -12.618, 1.5343);
                case 9:
                    return new Vector(-0.44544, -7.7258, -3.4693);
                case 10:
                    return new Vector(-4.4418, -6.8257, -1.1869);


                case 11: //1 + 2
                    return new Vector(-10.3562, 1.9968, -2.936, -2.0069, -6.15312);
                case 12: //2 + 3
                    return new Vector(-3.9009, -5.9003, -3.8065, -4.3538, -11.8397);
                case 13: //3 + 4
                    return new Vector(-4.3446, -4.60303, 0.6214, -0.1703, -14.0577);
                case 14: //1 - 2
                    return new Vector(4.5108, 5.387, 0.413, -7.9395, 6.88068);
                case 15: //2 - 3
                    return new Vector(-10.9661, 2.5101, 0.4575, 10.2864, -1.1941);
                case 16: //3 - 4
                    return new Vector(11.4098, -3.80737, -4.8854, -14.4699, 3.4121);
                case 17: //1 * (2.0)
                    return new Vector(-5.8454, 7.3838, -2.523, -9.9464, 0.72756);
                case 18: //2 * (0.5)
                    return new Vector(-3.71675, -0.84755, -0.83725, 1.48315, -3.25845);
                case 19: //3 * (-1.0)
                    return new Vector(-3.5326, 4.2052, 2.132, 7.3201, 5.3228);
                case 20: //norm(1)
                    return new Vector(-0.419119, 0.529424, -0.180901, -0.713164, 0.0521666);
                case 21: //norm(2)
                    return new Vector(-0.701764, -0.160027, -0.158082, 0.280035, -0.615231);
                case 22: //norm(3)
                    return new Vector(0.327114, -0.389396, -0.197421, -0.677832, -0.492885);
                case 23: //prob(1)
                    return new Vector(0.221197, 0.279413, 0.0954736, 0.376385, 0.0275318);
                case 24: //prob(2)
                    return new Vector(0.36643, 0.0835589, 0.0825434, 0.146222, 0.321246);
                case 25: //prob(3)
                    return new Vector(0.156916, 0.186792, 0.0947021, 0.325154, 0.236435);

                case 26: //softmax(1)
                    return new Vector(0.00128362, 0.957457, 0.00675905, 0.000165164, 0.0343349);
                case 27: //softmax(2)
                    return new Vector(0.0000298649, 0.00927507, 0.00946812, 0.981152, 0.0000746852);
                case 28: //softmax(3)
                    return new Vector(0.995952, 0.000434265, 0.00345251, 0.0000192739, 0.000142032);
                
                    
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }

        [Test]
        public void ToString_LongVector_HasFirstElement()
        {
            dynamic v = GetSample(1);
            
            string s1 = v.ToString("0.0000", null);
            string s2 = v[0].ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [Test]
        public void ToString_LongVector_HasLastElement()
        {
            dynamic v = GetSample(2);

            string s1 = v.ToString("0.0000", null);
            string s2 = v[4].ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [Test]
        public void ToString_ShortVector_HasFirstElement()
        {
            dynamic v = GetSample(6);

            string s1 = v.ToString("0.0000", null);
            string s2 = v[0].ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [Test]
        public void ToString_ShortVector_HasLastElement()
        {
            dynamic v = GetSample(7);

            string s1 = v.ToString("0.0000", null);
            string s2 = v[2].ToString("0.0000");

            Assert.That(s1, Contains.Substring(s2));
        }

        [TestCase(1, 2, 11)]
        [TestCase(2, 3, 12)]
        [TestCase(3, 4, 13)]
        public void Add_TwoVectors_ExpectedResult(int xi, int yi, int result)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic r = GetSample(result);

            dynamic sum = x.Add(y);

            Assert.That(sum, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 2, 14)]
        [TestCase(2, 3, 15)]
        [TestCase(3, 4, 16)]
        public void Sub_TwoVectors_ExpectedResult(int xi, int yi, int result)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);
            dynamic r = GetSample(result);

            dynamic diff = x.Sub(y);

            Assert.That(diff, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 2, 0.457411)]
        [TestCase(2, 3, -2.58677)]
        [TestCase(3, 4, -37.86740)]
        public void Mult_ByVector_ExpectedResult(int xi, int yi, double result)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            double dot = x.Mult(y);

            Assert.That(dot, Ist.WithinTolOf(result, tol));
        }

        [TestCase(1, 2.0, 17)]
        [TestCase(2, 0.5, 18)]
        [TestCase(3, -1.0, 19)]
        public void Mult_ByScalor_ExpectedResult(int xi, double s, int result)
        {
            dynamic x = GetSample(xi);
            dynamic r = GetSample(result);

            dynamic prod = x.Mult(s);

            Assert.That(prod, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 6.97343)]
        [TestCase(2, 10.5926)]
        [TestCase(4, 14.0431)] 
        public void Norm_RandomVector_ExpectedResult(int xi, double result)
        {
            dynamic x = GetSample(xi);

            dynamic norm = x.Norm();

            Assert.That(norm, Ist.WithinTolOf(result, tol));
        }

        [TestCase(1, 2, 12.6458)]
        [TestCase(2, 3, 15.2971)]
        [TestCase(3, 4, 19.7375)]
        public void Dist_TwoVectors_ExpectedResult(int xi, int yi, double result)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            double dist = x.Dist(y);

            Assert.That(dist, Ist.WithinTolOf(result, tol));
        }

        [TestCase(1, 20)]
        [TestCase(2, 21)]
        [TestCase(3, 22)]
        public void Unit_RandomVector_ExpectedResult(int xi, int result)
        {
            dynamic x = GetSample(xi);
            dynamic r = GetSample(result);

            dynamic unit = x.Unit();

            Assert.That(unit, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 2, 1.5646)]
        [TestCase(2, 3, 1.59341)]
        [TestCase(3, 4, 1.82316)]
        public void Angle_TwoVectors_ExpectedResult(int xi, int yi, double result)
        {
            dynamic x = GetSample(xi);
            dynamic y = GetSample(yi);

            double t = x.Angle(y);

            Assert.That(t, Ist.WithinTolOf(result, tol));
        }

        [TestCase(1, 23)]
        [TestCase(2, 24)]
        [TestCase(3, 25)]
        public void ToProb_RandomVector_ExpectedResult(int xi, int result)
        {
            dynamic x = GetSample(xi);
            dynamic r = GetSample(result);

            dynamic prob = x.ToProb();

            Assert.That(prob, Ist.WithinTolOf(r, tol));
        }

        [TestCase(1, 26)]
        [TestCase(2, 27)]
        [TestCase(3, 28)]
        public void Softmax_RandomVector_ExpectedResult(int xi, int result)
        {
            dynamic x = GetSample(xi);
            dynamic r = GetSample(result);

            dynamic prob = x.Softmax();

            Assert.That(prob, Ist.WithinTolOf(r, tol));
        }
    }
}
