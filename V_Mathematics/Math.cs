using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;

namespace Vulpine.Core.Calc
{
    public static class VMath
    {
        #region Mathmatical Constants...

        /// <summary>
        /// The circile constant that is the ratio of the cercumfrence to
        /// the radius. It is also precicesly twice the value of PI, which
        /// is the ratio of the cercumfrence to the diamater.
        /// It is aproximatly (6.2831853).
        /// </summary>
        public const double TAU = Math.PI * 2.0;

        /// <summary>
        /// The nautual lograthim of 2. This is of primary importance for 
        /// takeing base 2 lograthims, using the change of base formula.
        /// It is aproximatly (0.6931472).
        /// </summary>
        public const double LN2 = 0.69314718055994530942;

        /// <summary>
        /// The golden ratio, or golden mean. It is the ratio generated when
        /// two lengths have the same ratio as do there sum and there maximum.
        /// It is aproximatly (1.6180340).
        /// </summary>
        public const double PHI = 1.6180339887498948482;

        /// <summary>
        /// The square root of 2, aproximatly (1.4142136).
        /// </summary>
        public const double ROOT2 = 1.4142135623730950488;

        /// <summary>
        /// A very small number, sutable for spesifing the maximum expected
        /// error when comparing two floating point values for equality. 
        /// </summary>
        public const double TOL = 1e-12;

        #endregion //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Converts from radians to degrees.
        /// </summary>
        /// <param name="rad">Input in radians</param>
        /// <returns>The ewuivlent value in degrees</returns>
        public static double ToDegrees(double rad)
        {
            return (rad * (180.0 / Math.PI));
        }

        /// <summary>
        /// Converts from degrees to radians.
        /// </summary>
        /// <param name="deg">Input in degrees</param>
        /// <returns>The equivlent value in radians</returns>
        public static double ToRadians(double deg)
        {
            return (deg * (Math.PI / 180.0));
        }

        /// <summary>
        /// Computes the maximum from a set of values, as opposed to the
        /// standard Max function, which only returns the maximium of two values
        /// </summary>
        /// <param name="vals">The values to compare</param>
        /// <returns>The maximum value</returns>
        public static double Max(params double[] vals)
        {
            //takes care of the obvious cases
            if (vals.Length == 1) return vals[0];
            if (vals.Length == 0) return Double.NaN;

            //assumes the first is maximum
            double max = vals[0];

            //preformes the itterative calculation
            for (int i = 1; i < vals.Length; i++)
            if (vals[i] > max) max = vals[i];

            return max;
        }

        /// <summary>
        /// Computes the minimum from a set of values, as opposed to the
        /// standard Min function, which only returns the minimum of two values
        /// </summary>
        /// <param name="vals">The values to compare</param>
        /// <returns>The minimum value</returns>
        public static double Min(params double[] vals)
        {
            //takes care of the obvious cases
            if (vals.Length == 1) return vals[0];
            if (vals.Length == 0) return Double.NaN;

            //assumes the first is minimunm
            double min = vals[0];

            //preformes the itterative calculation
            for (int i = 1; i < vals.Length; i++)
            if (vals[i] < min) min = vals[i];

            return min;
        }

        /// <summary>
        /// Computes the standard sinc function, which is defined as sin(x) / x,
        /// except when x is zero where it is equal to one.
        /// </summary>
        /// <param name="x">Input to the sinc function</param>
        /// <returns>Evaluation of the sinc function</returns>
        public static double Sinc(double x)
        {
            if (Math.Abs(x) < 0.001)
            {
                //use the tailor series for small x
                double x2 = x * x;
                double x4 = x2 * x2;
                return 1.0 - (x2 / 6.0) + (x4 / 120.0);
            }
            else
            {
                //uses the standard formula
                return Math.Sin(x) / x;
            }
        }

        /// <summary>
        /// Computes the standard sinc function, which is defined as sin(z) / z,
        /// except when z is zero where it is equal to one.
        /// </summary>
        /// <param name="x">Input to the sinc function</param>
        /// <returns>Evaluation of the sinc function</returns>
        public static Cmplx Sinc(Cmplx z)
        {
            if (z.Abs < 0.001)
            {
                //use the tailor series for small x
                Cmplx z2 = z * z;
                Cmplx z4 = z2 * z2;
                return 1.0 - (z2 / 6.0) + (z4 / 120.0);
            }
            else
            {
                //uses the standard formula
                return Cmplx.Sin(z) / z;
            }
        }


        public static double Log2(double x)
        {
            return Math.Log(x) / VMath.LN2;
        }

        public static Cmplx Log2(Cmplx z)
        {
            //seperates the funciton into real and imaginary
            double real = Math.Log(z.Abs) / VMath.LN2;
            double imag = z.Arg / VMath.LN2;

            return new Cmplx(real, imag);
        }

        /// <summary>
        /// Computes the gamma function applied to the real number line.
        /// The gamma funciton can be thought of as a continuation of the
        /// factorial function where Gamma(n) is equal to Fact(n - 1).
        /// </summary>
        /// <param name="x">Input to the gamma function</param>
        /// <returns>Output from the gamma funciton</returns>
        public static double Gamma(double x)
        {
            //calls upon the complex method
            Cmplx temp = Gamma(new Cmplx(x));

            //returns the result if it lies on the real axis
            bool test = Math.Abs(temp.CofI) < VMath.TOL;
            return (test) ? temp.CofR : Double.NaN; 
        }

        /// <summary>
        /// Computes the gamma funciton applied to the complex plane.
        /// The gamma funciton can be thought of as a continuation of the
        /// factorial function where Gamma(n) is equal to Fact(n - 1).
        /// </summary>
        /// <param name="z">Input to the gamma function</param>
        /// <returns>Output from the gamma funciton</returns>
        public static Cmplx Gamma(Cmplx z)
        {
            if (z.CofR < 0.5)
            {
                //uses the reflection formula for the other half
                return Math.PI / (Cmplx.Sin(Math.PI * z) * Gamma(1.0 - z));
            }
            else
            {
                Cmplx x, t;

                //initialise the values
                x = 0.99999999999980993;
                z = z - 1.0;

                //uses a bunch of magic numbers
                x += 676.5203681218851 / (z + 1);
                x += -1259.1392167224028 / (z + 2);
                x += 771.32342877765313 / (z + 3);
                x += -176.61502916214059 / (z + 4);
                x += 12.507343278686905 / (z + 5);
                x += -0.13857109526572012 / (z + 6);
                x += 9.9843695780195716e-6 / (z + 7);
                x += 1.5056327351493116e-7 / (z + 8);

                t = z + 7.5;

                //finalises the result
                x *= 2.506628274631;  //SQRT(2*PI); 
                x *= Cmplx.Pow(t, z + 0.5);
                x *= Cmplx.Exp(-t);

                return x;
            }
        }

        public static double Sigmoid(double x)
        {
            //calculates the standard logistic funciton
            return 1.0 / (1.0 - Math.Exp(-x));
        }

        public static Cmplx Sigmoid(Cmplx x)
        {
            //calculates the standard logistic funciton
            return 1.0 / (1.0 - Cmplx.Exp(-x));
        }










        public static double Clamp(this double val, double min, double max)
        {
            if (val < min) return min;
            if (val > max) return max;

            return val;
        }

        public static float Clamp(this float val, float min, float max)
        {
            if (val < min) return min;
            if (val > max) return max;

            return val;
        }


        public static string FormatShort(double val)
        {
            //takes care of NaN and infinity cases
            if (Double.IsNaN(val)) return "NaN";
            if (Double.IsInfinity(val)) return "Inf";

            //uses the absolute value to determin the correct format
            double abs = Math.Abs(val);

            //retruns five significant digits regardless of size
            if (abs < 10) return val.ToString("0.0000");
            if (abs < 100) return val.ToString("00.000");
            if (abs < 1000) return val.ToString("000.00");
            if (abs < 10000) return val.ToString("0000.0");
            if (abs < 100000) return val.ToString("00000");

            //returns five significant digits in sinentific notation
            return val.ToString("0.0000e+00");
        }

        public static string FormatFixed(double val)
        {        
            return val.ToString("0.00e+00");
        }
    }
}
