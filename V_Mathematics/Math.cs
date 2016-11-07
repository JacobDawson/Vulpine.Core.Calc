﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Data.Extentions;

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
        /// The default error tollarence used within the mathamatics library.
        /// It is desined to be greater that the precision of a single floating
        /// point value, but less than the presision of a double. Note this value
        /// is only sutable for comparing releative error, for absolute error
        /// a diffrent metric should be used.
        /// </summary>
        public const double ERR = 1e-12;

        #endregion //////////////////////////////////////////////////////////////////

        #region Floating Point Manipulation...

        /// <summary>
        /// Determins if the floating point number indicates an actual numeric value,
        /// rather than an infinate value, or a special NaN value.
        /// </summary>
        /// <param name="x">Floating point vlaue to test</param>
        /// <returns>True if it is an actual numeric value, otherwise false</returns>
        public static bool IsANumber(double x)
        {
            //checks for special cases
            if (Double.IsNaN(x)) return false;
            if (Double.IsInfinity(x)) return false;

            //must be a numeric value 
            return true;
        }

        /// <summary>
        /// Determins if the given value should be treated as zero, for all intents and
        /// purpouses. One should tipicaly avoid directly comparing two floating point
        /// numbers, due to possable round-off error. However, in many instances in code,
        /// it is usefull to compare a floating point value to zero. This method helps
        /// to alieviate this problem.
        /// </summary>
        /// <param name="x">Floating point vlaue to test</param>
        /// <returns>True if the value should be considered zero, false if otherwise</returns>
        public static bool IsZero(double x)
        {
            //determins if the number is invertable
            double test = 1.0 / x;
            return Double.IsInfinity(test);
        }

        /// <summary>
        /// Increments a double-presesion floating-point value by the smallest amount
        /// possable. It basicly produces the next largest double value that can be
        /// represented in the system. 
        /// </summary>
        /// <param name="x">Double value to increment</param>
        /// <returns>The next largest double value</returns>
        public static double NextUp(double x)
        {
            //we do not update NaNs or Infinities
            if (Double.IsNaN(x)) return x;
            if (Double.IsInfinity(x)) return x;

            //must treat zero as a special case
            if (x == 0.0) return Double.Epsilon;

            //manipulates the bit-pattern and returns
            long bits = BitConverter.DoubleToInt64Bits(x);
            bits = (x > 0.0) ? bits + 1 : bits - 1;
            return BitConverter.Int64BitsToDouble(bits);
        }

        /// <summary>
        /// Decrements a double-presesion floating-point value by the smallest amount
        /// possable. It basicly produces the next smallest double value that can be
        /// represented in the system. 
        /// </summary>
        /// <param name="x">Double value to decrement</param>
        /// <returns>The next dmalest double value</returns>
        public static double NextDown(double x)
        {
            //we do not update NaNs or Infinities
            if (Double.IsNaN(x)) return x;
            if (Double.IsInfinity(x)) return x;

            //must treat zero as a special case
            if (x == 0.0) return -Double.Epsilon;

            //manipulates the bit-pattern and returns
            long bits = BitConverter.DoubleToInt64Bits(x);
            bits = (x > 0.0) ? bits - 1 : bits + 1;
            return BitConverter.Int64BitsToDouble(bits);
        }

        /// <summary>
        /// Clamps the floating point value to be between zero and one.
        /// </summary>
        /// <param name="val">Value to be clamped</param>
        /// <returns>The clamped value</returns>
        public static double Clamp(double val)
        {
            if (val < 0.0) return 0.0;
            if (val > 1.0) return 1.0;
            return val;
        }

        /// <summary>
        /// Clamps the floating point value to be between the given minimum and
        /// maximum values, respectivly. 
        /// </summary>
        /// <param name="val">Value to be clamped</param>
        /// <param name="min">Minimum possable output</param>
        /// <param name="max">Maximum possable output</param>
        /// <returns>The clamped value</returns>
        public static double Clamp(double val, double min, double max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Statistics...

        /// <summary>
        /// Computes the maximum from a set of values, as opposed to the
        /// standard Max function, which only returns the maximium of two values
        /// </summary>
        /// <param name="vals">The values to compare</param>
        /// <returns>The maximum value</returns>
        public static double Max(params double[] vals)
        {
            //takes care of the obvious cases
            if (vals.Length == 0) return Double.NaN;
            if (vals.Length == 1) return vals[0];

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
            if (vals.Length == 0) return Double.NaN;
            if (vals.Length == 1) return vals[0];

            //assumes the first is minimunm
            double min = vals[0];

            //preformes the itterative calculation
            for (int i = 1; i < vals.Length; i++)
                if (vals[i] < min) min = vals[i];

            return min;
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Special Functions...

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

        /// <summary>
        /// Computes the logarythim base 2 of the given value, that is, it finds
        /// the exponent of 2 which produces the given value. This is a usefull
        /// mesurment for base-2 binary systems.
        /// </summary>
        /// <param name="x">Number to evaluate</param>
        /// <returns>Base-2 Logarythim of the imput</returns>
        public static double Log2(double x)
        {
            return Math.Log(x) / VMath.LN2;
        }

        /// <summary>
        /// Computes the logarythim base 2 of the given value, that is, it finds
        /// the exponent of 2 which produces the given value. This is a usefull
        /// mesurment for base-2 binary systems.
        /// </summary>
        /// <param name="z">Number to evaluate</param>
        /// <returns>Base-2 Logarythim of the imput</returns>
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
            return (double)Gamma(new Cmplx(x));
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

        /// <summary>
        /// Computes the sigmoid funciton, which takes any value on the real
        /// number line and maps it to the interval [0, 1], this is known
        /// as flatening the funciton.
        /// </summary>
        /// <param name="x">Input to the sigmoid function</param>
        /// <returns>The result of the sigmoid funciton</returns>
        public static double Sgmd(double x)
        {
            //calculates the standard logistic funciton
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        /// <summary>
        /// Computes the inverse sigmoid funciton, which undoes the action
        /// taken by the sigmoid funciton. If the input is outside the range
        /// of zero to one, the funciton maps the value to infinity.
        /// </summary>
        /// <param name="x">Input to the inverse sigmoid funciton</param>
        /// <returns>The result of the inverse sigmoid funciton</returns>
        public static double Asgmd(double x)
        {
            //takes care of the extrem cases
            if (x <= 0.0) return Double.NegativeInfinity;
            if (x >= 1.0) return Double.PositiveInfinity;

            double temp = x / (x - 1);
            return Math.Log(-x);
        }

        /// <summary>
        /// Evalueates the error function at the given value. Note this is not the same
        /// as computing the amount of error in an estimated value. The error funciton
        /// arrises from stastics and has a similar shape to the sigmoid funciton. The
        /// error function, however, cannot be defined in terms of elementry operattors,
        /// thus a numerical aproximation is used.
        /// </summary>
        /// <param name="x">Input to the error fuction</param>
        /// <returns>The result of the error funciton</returns>
        public static double Erf(double x)
        {
            if (Math.Abs(x) <= 0.5)
            {
                double x2 = x * x;
                double p = -3.5609843701815385e-2;
                double q = 1.0;

                //computes the polynomial
                p = (p * x2) + 6.9963834886191355e+0;
                p = (p * x2) + 2.1979261618294152e+1;
                p = (p * x2) + 2.4266795523053175e+2;

                //computest the quotient
                q = (q * x2) + 1.5082797630407787e+1;
                q = (q * x2) + 9.1164905404514901e+1;
                q = (q * x2) + 2.1505887586986120e+2;

                return x * (p / q);
            }
            else
            {
                double a = Math.Abs(x);
                double p = -6.0858151959688e-6;
                double q = 1.0;

                //computes the polynomial
                p = (p * a) + 5.6437160686381e-1;
                p = (p * a) + 4.2677201070898e+0;
                p = (p * a) + 1.4571898596926e+1;
                p = (p * a) + 2.6094746956075e+1;
                p = (p * a) + 2.2898992851659e+1;

                //computest the quotient
                q = (q * a) + 7.5688482293618e+0;
                q = (q * a) + 2.6288795758761e+1;
                q = (q * a) + 5.0273202863803e+1;
                q = (q * a) + 5.1933570687552e+1;
                q = (q * a) + 2.2898985749891e+1;

                double t = (p / q) * Math.Exp(-x * x);
                return (x >= 0.0) ? 1.0 - t : t - 1.0;
            }
        }

        /// <summary>
        /// Evalueates the error function at the given value. Note this is not the same
        /// as computing the amount of error in an estimated value. The error funciton
        /// arrises from stastics and has a similar shape to the sigmoid funciton. The
        /// error function, however, cannot be defined in terms of elementry operattors,
        /// thus a numerical aproximation is used.
        /// </summary>
        /// <param name="z">Input to the complex error fuction</param>
        /// <returns>The result of the complex error funciton</returns>
        public static Cmplx Erf(Cmplx z)
        {
            //Uses the talor series for the complex error function
            double error = Double.PositiveInfinity;
            int n = 0;

            Cmplx sum1 = 0.0;
            Cmplx sum2 = 0.0;
            Cmplx prod = 1.0;

            while (error > VMath.ERR && n < 64)
            {
                //computes the inner product
                for (int k = 1; k <= n; k++)
                    prod *= -(z * z) / (double)k;

                sum1 = sum2;

                //updates the sum and calculates the relitive error
                sum2 += (z / (2.0 * n + 1.0)) * prod;
                error = (sum1 - sum2).Abs / sum1.Abs;

                prod = 1.0;
                n++;
            }

            //sum * 2/sqrt(pi)
            return sum2 * 1.1283791670955125739;
        }

        #endregion //////////////////////////////////////////////////////////////////





        /// <summary>
        /// Converts from radians to degrees.
        /// </summary>
        /// <param name="rad">Input in radians</param>
        /// <returns>The equivlent value in degrees</returns>
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

    }
}
