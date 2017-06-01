/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Data.Extentions;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// This class contains a variaty of diffrent constants and functions which crop
    /// up in statistics and mathmatical analisis, which have been excluded from the 
    /// default math library. Many of these funcitons cannot be expressed as elementry
    /// functions, and therefore must employ numerical methods inorder to be solved.
    /// It also includs several methods for the managment of floating point numbers,
    /// which have been omitted from the corisponding structors.
    /// </summary>
    /// <remarks>Last Update: 2016-11-14</remarks>
    public static class VMath
    {
        //NOTES: Perhaps a sigmoid function ranging from -1 to 1 would be more benificial?
        //Adding A Permutation funciton would help complete the Gamma Funciton set.
        //I still need to replace the complex error function

        #region Mathmatical Constants...

        /// <summary>
        /// The circile constant that is the ratio of the cercumfrence to
        /// the radius. It is also precicesly twice the value of PI, which
        /// is the ratio of the cercumfrence to the diamater.
        /// It is aproximatly (6.2831853).
        /// </summary>
        public const double TAU = Math.PI * 2.0;

        /// <summary>
        /// The nautual lograthim of two. This is of primary importance for 
        /// takeing base 2 lograthims, using the change of base formula.
        /// It is aproximatly (0.6931472).
        /// </summary>
        public const double LN2 = 0.69314718055994530942;

        /// <summary>
        /// The suqare root of two. One of the first irational numbers to be
        /// discovered, it often occors in geometric probelms in two dimetions.
        /// It is aproximatly (1.4142136)
        /// </summary>
        public const double R2 = 1.4142135623730950488;

        /// <summary>
        /// The golden ratio, or golden mean. It is the ratio generated when
        /// two lengths have the same ratio as do there sum and there maximum.
        /// It is aproximatly (1.6180340).
        /// </summary>
        public const double PHI = 1.6180339887498948482;

        /// <summary>
        /// The Euler–Mascheroni constant, not to be confused with Euler's number
        /// which is the base of the natural logarythim. It is defined as the limiting 
        /// difference between the harmonic series and the natural logarithm, and is
        /// usualy denoted by the lower greek letter gamma.
        /// It is aproximatly (0.5772157).
        /// </summary>
        public const double EM = 0.57721566490153286061;

        /// <summary>
        /// The square root of twice pi, a value which comes up frequently in many
        /// formulas, and is therefore convenient to have defined as a constant.
        /// It is aproximatly (2.5066283).
        /// </summary>
        public const double RTP = 2.5066282746310005024;

        /// <summary>
        /// The default error tollarence used within the mathamatics library.
        /// It is desined to be greater that the precision of a single floating
        /// point value, but less than the presision of a double. Note this value
        /// is only sutable for comparing releative error, for absolute error
        /// a diffrent tollerence value should be used.
        /// </summary>
        public const double TOL = 1e-12;

        #endregion //////////////////////////////////////////////////////////////////

        #region Floating Point Manipulation...

        /// <summary>
        /// Determins if the given floating point value is one of the NaN (Not
        /// a number values). NaN values are often used to comunicate some sort
        /// of error in floating point arithmatic.
        /// </summary>
        /// <param name="x">Value to test</param>
        /// <returns>True if it is a NaN value</returns>
        public static bool IsNaN(this double x)
        {
            return Double.IsNaN(x);
        }

        /// <summary>
        /// Determins if the given floating point value is one of the infinity
        /// values, either postive or negative infinity.
        /// </summary>
        /// <param name="x">Value to test</param>
        /// <returns>True if it is infinite</returns>
        public static bool IsInfinity(this double x)
        {
            return Double.IsInfinity(x);
        }

        /// <summary>
        /// Determins if the floating point number indicates an actual numeric value,
        /// rather than an infinate value, or a special NaN value.
        /// </summary>
        /// <param name="x">Vlaue to test</param>
        /// <returns>True if it is an actual numeric value</returns>
        public static bool IsANumber(this double x)
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
        /// <param name="x">Vlaue to test</param>
        /// <returns>True if the value should be considered zero</returns>
        public static bool IsZero(this double x)
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
        /// <param name="x">Value to increment</param>
        /// <returns>The next largest double value</returns>
        public static double NextUp(this double x)
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
        /// <param name="x">Value to decrement</param>
        /// <returns>The next smalest double value</returns>
        public static double NextDown(this double x)
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
        /// Converts the given floating point value to the next smallest interger
        /// value. Note that even though the result is an interger, it is returned 
        /// as a  floating point, because the value may exceed the range of an int.
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>The next smallest interger</returns>
        public static double Floor(this double val)
        {
            return Math.Floor(val);
        }

        /// <summary>
        /// Rounds off the given floating point value to the closest interger value.
        /// Note that even though the result is an interger, it is returned as a 
        /// floating point, because the value may exceed the range of an int.
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>The closest interger value</returns>
        public static double Round(this double val)
        {
            return Math.Floor(val + 0.5);
        }

        /// <summary>
        /// Extracts the fractional part of a floating point number. For positive
        /// real numbers, this is equivilent to the part of the number that apears
        /// after the radix point. It creates a sawtooth-like wave funciton.
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>The fractional part of the value</returns>
        public static double Frac(this double val)
        {
            return val - Math.Floor(val);
        }

        /// <summary>
        /// Clamps the floating point value to be between zero and one.
        /// </summary>
        /// <param name="val">Value to be clamped</param>
        /// <returns>The clamped value</returns>
        public static double Clamp(this double val)
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
        public static double Clamp(this double val, double min, double max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        /// <summary>
        /// Computes the floating point modulus of a given number. The function is
        /// designed sutch that the output is always between zero and the modulus,
        /// regardless of the sign of the input.
        /// </summary>
        /// <param name="val">Value to modulate</param>
        /// <param name="mod">The modulus</param>
        /// <returns>A value between zero and the modulus</returns>
        public static double Mod(this double val, double mod)
        {
            double s = Math.Floor(val / mod);
            return val - (mod * s);
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
        /// Determins the sign of a floating point number, returning negative one
        /// for negative numbers, and positive one for positive numbers. It returns
        /// zero if the number is zero. This method also propagates NaN values.
        /// </summary>
        /// <param name="x">Input to the sign funciton</param>
        /// <returns>The sign value of the input</returns>
        public static double Sign(double x)
        {
            if (Double.IsNaN(x)) return x;
            if (x < 0.0) return -1.0;
            if (x > 0.0) return 1.0;
            return 0.0;
        }

        /// <summary>
        /// This method extends the sign funciton to the complex domain. It returns
        /// the point on the unit circle closest to the input. It returns zero if
        /// the input is zero.
        /// </summary>
        /// <param name="z">Input to the sign funciton</param>
        /// <returns>The normalised complex vector</returns>
        public static Cmplx Sign(Cmplx z)
        {
            double abs = z.Abs;
            if (abs.IsZero()) return 0.0;
            return z / abs;
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
        /// Computes the probability density function for the standard normal
        /// distribution. That is a distribution with a mean of zero, and a
        /// variance of one. It is characterised by it's bell-shaped curve.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static double Gauss(double x)
        {
            double temp = Math.Exp(-0.5 * x * x);
            return temp / VMath.RTP;
        }

        /// <summary>
        /// Computes the probability density function for the standard normal
        /// distribution. That is a distribution with a mean of zero, and a
        /// variance of one. It is characterised by it's bell-shaped curve.
        /// </summary>
        /// <param name="z">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static Cmplx Gauss(Cmplx z)
        {
            Cmplx temp = Cmplx.Exp(-0.5 * z * z);
            return temp / VMath.RTP;
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

        #endregion //////////////////////////////////////////////////////////////////

        #region Advanced Functions...

        /// <summary>
        /// Computes the gamma function applied to the real number line.
        /// The gamma funciton can be thought of as a continuation of the
        /// factorial function where Fact(n) is equal to Gamma(n + 1).
        /// </summary>
        /// <param name="x">Input to the gamma function</param>
        /// <returns>Output from the gamma funciton</returns>
        public static double Gamma(double x)
        {
            if (x < 0.5)
            {
                //uses the reflection formula for the other half
                return Math.PI / (Math.Sin(Math.PI * x) * Gamma(1.0 - x));
            }
            else
            {
                double p, t;

                //initialise the values
                p = 0.99999999999980993;
                x = x - 1.0;

                //uses a bunch of magic numbers
                p += 676.5203681218851 / (x + 1);
                p += -1259.1392167224028 / (x + 2);
                p += 771.32342877765313 / (x + 3);
                p += -176.61502916214059 / (x + 4);
                p += 12.507343278686905 / (x + 5);
                p += -0.13857109526572012 / (x + 6);
                p += 9.9843695780195716e-6 / (x + 7);
                p += 1.5056327351493116e-7 / (x + 8);

                t = x + 7.5;

                //finalises the result
                p *= Math.Pow(t, x + 0.5);
                p *= Math.Exp(-t) * VMath.RTP;

                return p;
            }
        }

        /// <summary>
        /// Computes the gamma funciton applied to the complex plane.
        /// The gamma funciton can be thought of as a continuation of the
        /// factorial function where Fact(n) is equal to Gamma(n + 1).
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
                Cmplx p, t;

                //initialise the values
                p = 0.99999999999980993;
                z = z - 1.0;

                //uses a bunch of magic numbers
                p += 676.5203681218851 / (z + 1);
                p += -1259.1392167224028 / (z + 2);
                p += 771.32342877765313 / (z + 3);
                p += -176.61502916214059 / (z + 4);
                p += 12.507343278686905 / (z + 5);
                p += -0.13857109526572012 / (z + 6);
                p += 9.9843695780195716e-6 / (z + 7);
                p += 1.5056327351493116e-7 / (z + 8);

                t = z + 7.5;

                //finalises the result
                p *= Cmplx.Pow(t, z + 0.5);
                p *= Cmplx.Exp(-t) * VMath.RTP;

                return p;
            }
        }

        /// <summary>
        /// Evaluates the upper incomplete gamma function. It is related to 
        /// the gamma function in that both can be expressed as an intergral 
        /// of the same soultion. It is a special funciton which arises as
        /// the solution to various mathmatical problems.
        /// </summary>
        /// <param name="a">Value to integrate</param>
        /// <param name="x">Lower limit of the intergral</param>
        /// <returns>The solution to the incomplete gamma function</returns>
        public static double Gamma(double a, double x)
        {
            //the function is undefined for negative x
            if (x < 0.0) return Double.NaN;

            //values used in the itteration below
            double pow = Math.Pow(x, a) * Math.Exp(-x);
            double error = Double.PositiveInfinity;
            int k = 0;

            double p = 1.0;
            double q = 1.0;
            double sum1 = 0.0;
            double sum2 = 0.0;

            //uses the power series expansion
            while (error > VMath.TOL && k < 64)
            {
                sum1 = sum2;

                q = q * (a + k);
                sum2 += (pow * p) / q;
                p = p * x;

                error = Math.Abs(sum1 - sum2) / Math.Abs(sum1);
                k++;
            }

            //returns the upper incomplete gamma
            return VMath.Gamma(a) - sum2;
        }

        /// <summary>
        /// Evaluates the upper incomplete gamma function. It is related to 
        /// the gamma function in that both can be expressed as an intergral 
        /// of the same soultion. It is a special funciton which arises as
        /// the solution to various mathmatical problems.
        /// </summary>
        /// <param name="a">Value to integrate</param>
        /// <param name="z">Lower limit of the intergral</param>
        /// <returns>The solution to the incomplete gamma function</returns>
        public static Cmplx Gamma(Cmplx a, Cmplx z)
        {
            //values used in the itteration below
            Cmplx pow = Cmplx.Pow(z, a) * Cmplx.Exp(-z);
            double error = Double.PositiveInfinity;
            int k = 0;

            Cmplx p = 1.0;
            Cmplx q = 1.0;
            Cmplx sum1 = 0.0;
            Cmplx sum2 = 0.0;

            //uses the power series expansion
            while (error > VMath.TOL && k < 64)
            {
                sum1 = sum2;

                q = q * (a + k);
                sum2 += (pow * p) / q;
                p = p * z;

                error = (sum1 - sum2).Abs / sum1.Abs;
                k++;
            }

            //returns the upper incomplete gamma
            return VMath.Gamma(a) - sum2;
        }

        /// <summary>
        /// Computes the binomial coffecent. When n and k are postive intergers, 
        /// this is equal to the number of ways to choose k items from n total 
        /// items. This can be expanded to the real and complex numbers by use 
        /// of the gamma funciton.
        /// </summary>
        /// <param name="n">Total number of items</param>
        /// <param name="k">Size of the subset</param>
        /// <returns>Number of possable combinations</returns>
        public static double Binomial(double n, double k)
        {
            if (n > k && k > 0.0)
            {
                //uses the log-gamma function to avoid multiplication
                double a = Math.Log(VMath.Gamma(n + 1.0));
                double b = Math.Log(VMath.Gamma(k + 1.0));
                double c = Math.Log(VMath.Gamma(n - k + 1.0));

                return Math.Exp(a - b - c);
            }
            else
            {
                //calls upon the complex method
                return (double)Binomial(new Cmplx(n), new Cmplx(k));
            }
        }

        /// <summary>
        /// Computes the binomial coffecent. When n and k are postive intergers, 
        /// this is equal to the number of ways to choose k items from n total 
        /// items. This can be expanded to the real and complex numbers by use 
        /// of the gamma funciton.
        /// </summary>
        /// <param name="n">Total number of items</param>
        /// <param name="k">Size of the subset</param>
        /// <returns>Number of possable combinations</returns>
        public static Cmplx Binomial(Cmplx n, Cmplx k)
        {
            //uses the log-gamma function to avoid multiplication
            Cmplx a = Cmplx.Log(VMath.Gamma(n + 1.0));
            Cmplx b = Cmplx.Log(VMath.Gamma(k + 1.0));
            Cmplx c = Cmplx.Log(VMath.Gamma(n - k + 1.0));

            return Cmplx.Exp(a - b - c);
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

            while (error > VMath.TOL && n < 128)
            {
                //updates the sum and calculates the relitive error
                sum2 += (z / (2.0 * n + 1.0)) * prod;
                error = (sum1 - sum2).Abs / sum2.Abs;

                //updates the inner product
                prod *= -(z * z) / (double)(n + 1);

                sum1 = sum2;
                n++;
            }

            //sum * 2/sqrt(pi)
            return sum2 * 1.1283791670955125739;
        }

        /// <summary>
        /// Computes the cumulitive distribution funciton for the standard normal
        /// distribution, with mean of 0 and variance of 1. It returns the probability 
        /// that a random number will appear below the input value.
        /// </summary>
        /// <param name="x">Input value X to test</param>
        /// <returns>Probablity that some value is less than X</returns>
        public static double Cdf(double x)
        {
            //computes x / sqrt(2)
            double phi = x * 0.7071067811865475244;

            phi = 1.0 + VMath.Erf(phi);
            return phi * 0.5;
        }

        /// <summary>
        /// Computes the cumulitive distribution funciton for the standard normal
        /// distribution, with mean of 0 and variance of 1. It returns the probability 
        /// that a random number will appear below the input value.
        /// </summary>
        /// <param name="z">Input value Z to test</param>
        /// <returns>Probablity that some value is less than Z</returns>
        public static Cmplx Cdf(Cmplx z)
        {
            //computes z / sqrt(2)
            Cmplx phi = z * 0.7071067811865475244;

            phi = 1.0 + VMath.Erf(phi);
            return phi * 0.5;
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Conversion Methods...

        /// <summary>
        /// Converts from radians to degrees.
        /// </summary>
        /// <param name="rad">Input in radians</param>
        /// <returns>The equivlent value in degrees</returns>
        public static double ToDeg(double rad)
        {
            return (rad * (180.0 / Math.PI));
        }

        /// <summary>
        /// Converts from degrees to radians.
        /// </summary>
        /// <param name="deg">Input in degrees</param>
        /// <returns>The equivlent value in radians</returns>
        public static double ToRad(double deg)
        {
            return (deg * (Math.PI / 180.0));
        }

        #endregion //////////////////////////////////////////////////////////////////

    }
}
