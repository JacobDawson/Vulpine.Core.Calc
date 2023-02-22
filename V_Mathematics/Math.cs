/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2023 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
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
        /// The default error tollarence used within the mathamatics library.
        /// It is desined to be greater that the precision of a single floating
        /// point value, but less than the presision of a double. 
        /// </summary>
        public const double TOL = 1e-12;
        
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
        /// Represents the imaginary constant i raised to the i-th power (i^i).
        /// Dispite originating in the complex plane, this value is real for all
        /// branches of the complex lograthim. The principle branch is represented
        /// here. It is aproximatly (0.2078796).
        /// </summary>
        public const double II = 0.20787957635076190855;

        /// <summary>
        /// Gauss's Constant, which has relationships with the Arethmetic-Geometric
        /// mean, the gamma function, the beta function, the lemniscate constants,
        /// and the eleptic intergrals. It is aproximatly (0.8346268).
        /// </summary>
        public const double GA = 0.83462684167407318628; 

        /// <summary>
        /// Represents the Leminscate Constant which relates to the geometry of a
        /// Leminscate in much the same way that PI relates to the geometry of a
        /// circle. It is aproximatly (2.6220576).
        /// </summary>
        public const double LC = 2.6220575542921198105;

        /// <summary>
        /// Represnets the value -1/e which is the lower bound of the Lambert W
        /// function. It is aproximatly (-0.367879).
        /// </summary>
        public const double LW = -0.3678794411714423216;

        /// <summary>
        /// Represents the omega constant which is defined as the unique real number
        /// that satisfies the equation (w e^w = 1), it is also the value of W(1)
        /// where W(x) is the Lambert W function. It is aproximatly (0.567143). 
        /// </summary>
        public const double W = 0.56714329040978387300; 

        /// <summary>
        /// Stores a list of all the even indexed Bernouli numbers up to B_24.
        /// The odd indexed Bernouli numbers are all zero, except for B_1 which
        /// simply evaluates to -1/2.
        /// </summary>
        private static readonly double[] B2N =
        {
            1.0,
            1.0 / 6.0, 
            -1.0 / 30.0,
            1.0 / 42.0,
            -1.0 / 30.0,
            5.0 / 66.0,
            -691.0 / 2730.0,
            7.0 / 6.0,
            -3617.0 / 510.0,
            54.971177944862155388, 
            -529.12424242424242424,
            6192.1231884057971014,
            -86580.253113553113553,
        };

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
        /// Determins if the given value should be considered equal to zero, for all intents 
        /// and purpouses. It works by determing if the number is invertable. Basicly, this
        /// method should be used if you need to be as precice as possable while avoiding
        /// division by zero. Otherwise, it should be sufficent to determin if the magnitude
        /// is less than some tollerence.
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

        #region Floating Point Comparison...

        /// <summary>
        /// Computes the reletive error between some estimated value and
        /// an actual known value. Not to be confused with the error function
        /// denoted Erf(). Care must be taken if the actual known value
        /// is close to zero. 
        /// </summary>
        /// <param name="est">The estimated value</param>
        /// <param name="act">The known value</param>
        /// <returns>The absolute error in the estimate</returns>
        public static double Error(double est, double act)
        {
            double error = est - act;
            error = error / act;

            return Math.Abs(error);
        }

        /// <summary>
        /// Computes the reletive error between some estimated value and
        /// an actual known value. Not to be confused with the error function
        /// denoted Erf(). Care must be taken if the actual known value
        /// is close to zero. 
        /// </summary>
        /// <param name="est">The estimated value</param>
        /// <param name="act">The known value</param>
        /// <returns>The absolute error in the estimate</returns>
        public static double Error(Cmplx est, Cmplx act)
        {
            Cmplx error = act - est;
            error = error / est;

            return error.Abs;
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
        /// the point on the unit circle closest to the input, except when the input
        /// is zero, then it returns zero.
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
        /// This method extends the sign funciton to the dual domain. For simplicity,
        /// the derivitive is treated as being zero everywhere, and the discontinuity
        /// at zero is ignored.
        /// </summary>
        /// <param name="x">Input to the sign funciton</param>
        /// <returns>The dual value of the function</returns>
        public static Dual Sign(Dual x)
        {
            //the derivitive is essentialy zero for all points
            double fx = Sign(x.Real);
            return new Dual(fx, 0.0);
        }

        /// <summary>
        /// Computes the real-valued cubed root as a continious function. In particular
        /// it allows for the computation of negative values, returning real values in
        /// sutch cases. The result is an S-shaped curve, or a rotation of the graph of
        /// x cubed by 90 degrees.
        /// </summary>
        /// <param name="x">Input to the cubed root funciton</param>
        /// <returns>The real valued cubed root</returns>
        public static double Curt(double x)
        {
            //seperates the input into sign and magnitude
            double s = VMath.Sign(x);
            double m = Math.Abs(x);

            //computes the real valued cube root
            return s * Math.Pow(m, 1.0 / 3.0);
        }

        /// <summary>
        /// Computes the principle branch of the complex cubed root. Note that this is
        /// inherently diffrent from the real valued cube root, in that negative real
        /// values result in complex output. The result is continious in the complex
        /// plane, except for the negative real axis. 
        /// </summary>
        /// <param name="z">Input to the complex cubed root</param>
        /// <returns>The complex cubed root</returns>
        public static Cmplx Curt(Cmplx z)
        {
            //calculates the result storing temporary values
            double rad = Math.Pow(z.Abs, 1.0 / 3.0);
            double arg = z.Arg / 3.0;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Extends the cube root function, as defined on the real numbers, to the dual
        /// reaml. It is defined for all dual numbers, except at zero where the derivitive
        /// vanishes.
        /// </summary>
        /// <param name="x">Input to the cubed root</param>
        /// <returns>The dual value of the cubed root</returns>
        public static Dual Curt(Dual x)
        {
            //computes both the cube root and it's derivitive
            double fx = Curt(x.Real);
            double dx = 1.0 / (3.0 * fx * fx);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
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
        /// <param name="z">Input to the sinc function</param>
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
        /// Computes the standard sinc function, which is defined as sin(x) / x,
        /// except when x is zero where it is equal to one.
        /// </summary>
        /// <param name="x">Input to the sinc function</param>
        /// <returns>Evaluation of the sinc function</returns>
        public static Dual Sinc(Dual x)
        {
            double r = x.Real;
            double fx, dx;

            if (Math.Abs(r) < 0.001)
            {
                double r2 = r * r;
                double r4 = r2 * r2;

                //uses the tailor series for the function and its derivitive
                fx = 1.0 - (r2 / 6.0) + (r4 / 120.0);
                dx = r * (r2 - 10.0) * (1.0 / 30.0);
            }
            else
            {
                double sin = Math.Sin(r);
                double cos = Math.Cos(r);

                //uses the standard formula and its derivitive
                fx = sin / r;
                dx = (r * cos) - sin;
                dx = dx / (r * r);
            }

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
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
        /// Computes the logarythim base 2 of the given value, that is, it finds
        /// the exponent of 2 which produces the given value. This is a usefull
        /// mesurment for base-2 binary systems.
        /// </summary>
        /// <param name="x">Number to evaluate</param>
        /// <returns>Base-2 Logarythim of the imput</returns>
        public static Dual Log2(Dual x)
        {
            //computes the log base 2 and its derivitive
            double fx = Math.Log(x.Real) / VMath.LN2;
            double dx = 1.0 / (x.Real * VMath.LN2);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the standard gaussian curve, which has a maximum value of one
        /// at the origin. It is sometimes called a bell-shaped curve due to it's
        /// aperance on the real axis.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static double Gauss(double x)
        {
            return Math.Exp(-x * x);
        }

        /// <summary>
        /// Computes the standard gaussian curve, which has a maximum value of one
        /// at the origin. It is sometimes called a bell-shaped curve due to it's
        /// aperance on the real axis.
        /// </summary>
        /// <param name="z">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static Cmplx Gauss(Cmplx z)
        {
            return Cmplx.Exp(-z * z);
        }

        /// <summary>
        /// Computes the standard gaussian curve, which has a maximum value of one
        /// at the origin. It is sometimes called a bell-shaped curve due to it's
        /// aperance on the real axis.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static Dual Gauss(Dual x)
        {
            double r = x.Real;

            //computes the gauss function and its derivitive
            double fx = Math.Exp(-r * r);
            double dx = -2.0 * fx * r;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the sigmoid funciton, which takes any value on the real
        /// number line and maps it to the interval [0, 1], this is known
        /// as flatening the funciton.
        /// </summary>
        /// <param name="x">Input to the sigmoid function</param>
        /// <returns>The result of the sigmoid funciton</returns>
        public static double Sigmoid(double x)
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
        public static double Asigmoid(double x)
        {
            //takes care of the extrem cases
            if (x <= 0.0) return Double.NegativeInfinity;
            if (x >= 1.0) return Double.PositiveInfinity;

            double temp = x / (x - 1);
            return Math.Log(-x);
        }


        public static double SoftPlus(double x)
        {
            double temp = Math.Exp(x) + 1.0;
            return Math.Log(temp);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Advanced Functions...

        /// <summary>
        /// Computes the Lambert W function, defined such that [W(x) e^(W(x) = x].
        /// it is deffined for all real values greator that [-1/e]. For input
        /// values less than this constant, it simply returns NaN.
        /// </summary>
        /// <param name="x">Input to the Lambert W funciton</param>
        /// <returns>The result of the Lambert W function</returns>
        public static double Lambert(double x)
        {
            //atempts to find an aproximate value to W(z)
            double x0 = 0.0;

            if (x > 0.0) //x > 0
            {
                double ln = Math.Log(1.0 + x);

                x0 = Math.Log(1.0 + ln);
                x0 = x0 / (2.0 + ln);
                x0 = ln * (1.0 - x0);
            }
            else if (x > VMath.LW) //LW > x > 0
            {
                double ex = x * Math.E;
                double sr = Math.Sqrt(1.0 + ex);

                x0 = 1.0 + ex + sr;
                x0 = ex / x0;
                x0 = x0 * Math.Log(1.0 + sr);
            }
            else //x < LW
            {
                //the function is undefined for this range
                return Double.NaN;
            }

            double wn, e1, e2;

            //uses newton's method to fine-tune the aproximation
            for (int i = 0; i < 6; i++)
            {
                e1 = Math.Exp(x0);
                e2 = x0 * e1;

                wn = (e2 - x) / (e1 + e2);
                wn = x0 - wn;

                x0 = wn;
            }

            return x0;
        }

        /// <summary>
        /// Computes the Lambert W function, defined such that [W(x) e^(W(x) = x].
        /// It returns the principle branch of this complex function, with a branch
        /// cut allong the negative real axis.
        /// </summary>
        /// <param name="z">Input to the Lambert W funciton</param>
        /// <returns>The result of the Lambert W function</returns>
        public static Cmplx Lambert(Cmplx z)
        {
            Cmplx z0, y, lnB, lnC;

            //atempts to find an aproximate value to W(z)
            y = 2.0 * Math.E * z + 2.0;
            y = Cmplx.Sqrt(y);

            lnB = Cmplx.Log(1.0 + 0.8842 * y) * 2.0;
            lnC = Cmplx.Log(1.0 + 0.5106 * y) * 0.9294;

            z0 = lnB - Cmplx.Log(1.0 + lnC) - 1.213;
            z0 = z0 / (1.0 + (1.0 / (lnB + 4.688)));

            Cmplx wn, e1, e2;

            //uses newton's method to fine-tune the aproximation
            for (int i = 0; i < 6; i++)
            {
                e1 = Cmplx.Exp(z0);
                e2 = z0 * e1;

                wn = (e2 - z) / (e1 + e2);
                wn = z0 - wn;

                z0 = wn;
            }

            return z0;
        }

        /// <summary>
        /// Computes the Lambert W function, defined such that [W(x) e^(W(x) = x].
        /// it is deffined for all real values greator that [-1/e]. For input
        /// values less than this constant, it simply returns NaN.
        /// </summary>
        /// <param name="x">Input to the Lambert W funciton</param>
        /// <returns>The result of the Lambert W function</returns>
        public static Dual Lambert(Dual x)
        {
            //computes the Lambert W function and it's derivitive
            double fx = Lambert(x.Real);
            double dx = 1.0 / (x.Real + Math.Exp(fx));

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

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
        /// Computes the gamma function applied to the dual number domain.
        /// The gamma funciton can be thought of as a continuation of the
        /// factorial function where Fact(n) is equal to Gamma(n + 1).
        /// </summary>
        /// <param name="x">Input to the gamma function</param>
        /// <returns>Output from the gamma funciton</returns>
        public static Dual Gamma(Dual x)
        {
            //computes the gamma funciton and it's derivitive
            double fx = Gamma(x.Real);
            double dx = Digamma(x.Real) * fx;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
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
            //returns the upper incomplete gamma
            return Gamma(a) - GammaLow(a, x);
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
            //returns the upper incomplete gamma
            return Gamma(a) - GammaLow(a, z);
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
        public static Dual Gamma(Dual a, Dual x)
        {
            //returns the upper incomplete gamma
            return Gamma(a) - GammaLow(a, x);
        }

        /// <summary>
        /// Evaluates the lower incomplete gamma function. It is related to 
        /// the gamma function in that both can be expressed as an intergral 
        /// of the same soultion. It is a special funciton which arises as
        /// the solution to various mathmatical problems.
        /// </summary>
        /// <param name="a">Value to integrate</param>
        /// <param name="x">Upper limit of the intergral</param>
        /// <returns>The solution to the incomplete gamma function</returns>
        public static double GammaLow(double a, double x)
        {
            //the function is undefined for negative x
            if (x < 0.0) return Double.NaN;

            //values used in the itteration below
            double pow = Math.Pow(x, a) * Math.Exp(-x);

            double p = 1.0;
            double q = 1.0;
            double sum = 0.0;

            for (int k = 0; k < 32; k++)
            {
                q = q * (a + k);
                sum += (pow * p) / q;
                p = p * x;
            }

            return sum;
        }

        /// <summary>
        /// Evaluates the lower incomplete gamma function. It is related to 
        /// the gamma function in that both can be expressed as an intergral 
        /// of the same soultion. It is a special funciton which arises as
        /// the solution to various mathmatical problems.
        /// </summary>
        /// <param name="a">Value to integrate</param>
        /// <param name="z">Upper limit of the intergral</param>
        /// <returns>The solution to the incomplete gamma function</returns>
        public static Cmplx GammaLow(Cmplx a, Cmplx z)
        {
            //values used in the itteration below
            Cmplx pow = Cmplx.Pow(z, a) * Cmplx.Exp(-z);

            Cmplx p = 1.0;
            Cmplx q = 1.0;
            Cmplx sum = 0.0;

            for (int k = 0; k < 32; k++)
            {
                q = q * (a + k);
                sum += (pow * p) / q;
                p = p * z;
            }

            return sum;
        }

        /// <summary>
        /// Evaluates the lower incomplete gamma function. It is related to 
        /// the gamma function in that both can be expressed as an intergral 
        /// of the same soultion. It is a special funciton which arises as
        /// the solution to various mathmatical problems.
        /// </summary>
        /// <param name="a">Value to integrate</param>
        /// <param name="x">Upper limit of the intergral</param>
        /// <returns>The solution to the incomplete gamma function</returns>
        public static Dual GammaLow(Dual a, Dual x)
        {
            //the function is undefined for negative x
            if (x.Real < 0.0) return Dual.NaN;

            //values used in the itteration below
            Dual pow = Dual.Pow(x, a) * Dual.Exp(-x);

            Dual p = 1.0;
            Dual q = 1.0;
            Dual sum = 0.0;

            for (int k = 0; k < 32; k++)
            {
                q = q * (a + k);
                sum += (pow * p) / q;
                p = p * x;
            }

            return sum;
        }

        /// <summary>
        /// Evaluates the natural logarythim of the Gamma function. Because the
        /// Gamma function grows quite quickly, this method can be used to avoid
        /// numeric overflow in certain cercomstances. This function is undefined
        /// for real values less than zero.
        /// </summary>
        /// <param name="x">Input to the Gamma function</param>
        /// <returns>The logrythim of the Gamma function</returns>
        public static double GammaLog(double x)
        {
            double lg, sum, xk;

            if (x >= 7.0) //x >= x0
            {
                lg = (x - 0.5) * Math.Log(x);
                lg = lg - x + 0.91893853320467274178; // ln(tau) / 2

                xk = x;
                sum = 0.0;

                for (int k = 1; k <= 12; k++)
                {
                    int dim = 2 * k;
                    dim = dim * (dim - 1);

                    double bk = B2N[k] / (double)dim;
                    double s = bk / (xk * xk);

                    sum += s;
                    xk *= x;
                }

                return lg + (x * sum);

            }
            else if (x >= 0.0) //0 <= x < x0
            {
                int n = 7 - (int)Math.Floor(x);

                //recursivly calls the GammaLog() function
                lg = GammaLog(x + n);
                sum = 1.0;

                for (int k = 0; k < n; k++)
                {
                    sum *= (x + k);
                }

                return lg - Math.Log(sum);

            }
            else //x < 0
            {
                //the function is undefined for negative values
                return Double.NaN;
            }
        }

        /// <summary>
        /// Evaluates the natural logarythim of the Gamma function. Because the
        /// Gamma function grows quite quickly, this method can be used to avoid
        /// numeric overflow in certain cercomstances. It also features a single
        /// brach-cut along the negative real axis.
        /// </summary>
        /// <param name="z">Input to the Gamma function</param>
        /// <returns>The logrythim of the Gamma function</returns>
        public static Cmplx GammaLog(Cmplx z)
        {
            double x = z.CofR;
            Cmplx lg, sum, zk;

            if (x >= 7.0) //x >= x0
            {
                lg = (z - 0.5) * Cmplx.Log(z);
                lg = lg - z + 0.91893853320467274178; // ln(tau) / 2

                zk = z;
                sum = 0.0;

                for (int k = 1; k <= 12; k++)
                {
                    int dim = 2 * k;
                    dim = dim * (dim - 1);

                    double bk = B2N[k] / (double)dim;
                    Cmplx s = (Cmplx)bk / (zk * zk);

                    sum += s;
                    zk *= z;
                }

                return lg + (z * sum);

            }
            else if (x >= 0.0) //0 <= x < x0
            {
                int n = 7 - (int)Math.Floor(x);
                double a = 0.0;

                //recursivly calls the GammaLog() function
                lg = GammaLog(z + n);
                sum = 1.0;
                
                for (int k = 0; k < n; k++)
                {
                    sum *= (z + k);
                    a += Math.Atan2(z.CofI, x + k);
                }

                double b = 1.0 * Math.Log(sum.Abs);
                Cmplx prod = new Cmplx(b, a);

                return lg - prod;

            }
            else //x < 0
            {
                bool y_neg = z.CofI < 0.0;
                if (y_neg) z = z.Conj();

                //recursivly calls the GammaLog() function
                lg = GammaLog(1.0 - z);
                lg = 1.1447298858494 - lg; // ln(pi) - lg

                double xb, yb, e, sin, ey, re, im;

                xb = Math.Floor(x);
                yb = Math.Abs(z.CofI);
                e = x - xb;

                sin = Math.Sin(Math.PI * e);
                ey = Math.Exp(-VMath.TAU * yb);

                //the real part is computed to avoid overflow
                re = 1.0 - ey;
                re = 0.25 * re * re;
                re = (ey * sin * sin) + re;

                re = 0.5 * Math.Log(re);
                re = re + (Math.PI * yb);

                //the imaginary part enshures the correct branch-cut
                im = Math.Tanh(Math.PI * yb);
                im = im / Math.Tan(Math.PI * e);
                im = Math.Atan(im) - (xb * Math.PI);

                Cmplx lsz = new Cmplx(re, im);

                lg = lg - lsz;
                lg = y_neg ? lg.Conj() : lg;

                return lg;
            }
        }

        /// <summary>
        /// Evaluates the natural logarythim of the Gamma function. Because the
        /// Gamma function grows quite quickly, this method can be used to avoid
        /// numeric overflow in certain cercomstances. This function is undefined
        /// for real values less than zero.
        /// </summary>
        /// <param name="x">Input to the Gamma function</param>
        /// <returns>The logrythim of the Gamma function</returns>
        public static Dual GammaLog(Dual x)
        {
            //takes care of negative real values
            if (x.Real <= 0.0) return Dual.NaN;

            //computes the log of the gamma funciton and it's derivitive
            double fx = GammaLog(x.Real);
            double dx = Digamma(x.Real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the Digamma funciton, which is defined as the derivitive of
        /// the LogGamma function. However, unlike the LogGamma funciton, it is
        /// defined for all real values.
        /// </summary>
        /// <param name="x">Input to the Digamma funciton</param>
        /// <returns>The result of the Digamma funciton</returns>
        public static double Digamma(double x)
        {
            if (x >= 7.0) //x >= x0 > 0
            {
                double xk = x;
                double sum = 0.0;

                for (int k = 1; k <= 10; k++)
                {
                    double m = B2N[k] / (2.0 * k);
                    double s = m / (xk * xk);

                    sum += s;
                    xk *= x;
                }

                double dig = Math.Log(x) - (1.0 / (x + x));
                return dig - sum;
            }
            else if (x >= 0.0) //0 <= x < x0
            {
                int n = 7 - (int)Math.Floor(x);
                double sum = 0.0;

                for (int k = 0; k < n; k++)
                {
                    sum += 1.0 / (x + k);
                }

                double dig = Digamma(x + n);
                return dig - sum;
            }
            else //x < 0
            {
                double dig = Digamma(1.0 - x);
                dig = dig - Math.PI / Math.Tan(Math.PI * x);

                return dig;
            }
        }

        /// <summary>
        /// Computes the Digamma funciton, which is defined as the derivitive of
        /// the LogGamma function.
        /// </summary>
        /// <param name="z">Input to the Digamma funciton</param>
        /// <returns>The result of the Digamma funciton</returns>
        public static Cmplx Digamma(Cmplx z)
        {
            double x = z.CofR;

            if (x >= 7.0) //x >= x0 > 0
            {                
                Cmplx zk = z;
                Cmplx sum = 0.0;

                for (int k = 1; k <= 10; k++)
                {
                    double m = B2N[k] / (2.0 * k);
                    Cmplx s = (Cmplx)m / (zk * zk);

                    sum += s;
                    zk *= z;
                }

                Cmplx dig = Cmplx.Log(z) - (1.0 / (z + z));
                return dig - sum;
            }
            else if (x >= 0.0) //0 <= x < x0
            {
                int n = 7 - (int)Math.Floor(x);
                Cmplx sum = 0.0;

                for (int k = 0; k < n; k++)
                {
                    sum += 1.0 / (z + k);
                }

                Cmplx dig = Digamma(z + n);
                return dig - sum;
            }
            else //x < 0
            {
                Cmplx dig = Digamma(1.0 - z);
                dig = dig - Math.PI / Cmplx.Tan(Math.PI * z);

                return dig;
            }
        }

        /// <summary>
        /// Computes the Digamma funciton, which is defined as the derivitive of
        /// the LogGamma function. However, unlike the LogGamma funciton, it is
        /// defined for all real values.
        /// </summary>
        /// <param name="z">Input to the Digamma funciton</param>
        /// <returns>The result of the Digamma funciton</returns>
        public static Dual Digamma(Dual z)
        {
            double x = z.Real;

            if (x >= 7.0) //x >= x0 > 0
            {
                Dual zk = z;
                Dual sum = 0.0;

                for (int k = 1; k <= 10; k++)
                {
                    double m = B2N[k] / (2.0 * k);
                    Dual s = (Dual)m / (zk * zk);

                    sum += s;
                    zk *= z;
                }

                Dual dig = Dual.Log(z) - (1.0 / (z + z));
                return dig - sum;
            }
            else if (x >= 0.0) //0 <= x < x0
            {
                int n = 7 - (int)Math.Floor(x);
                Dual sum = 0.0;

                for (int k = 0; k < n; k++)
                {
                    sum += 1.0 / (z + k);
                }

                Dual dig = Digamma(z + n);
                return dig - sum;
            }
            else //x < 0
            {
                Dual dig = Digamma(1.0 - z);
                dig = dig - Math.PI / Dual.Tan(Math.PI * z);

                return dig;
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
        public static double Binomial(double n, double k)
        {
            if (n > k && k > 0.0)
            {
                //uses the log-gamma function to avoid multiplication
                double a = VMath.GammaLog(n + 1.0);
                double b = VMath.GammaLog(k + 1.0);
                double c = VMath.GammaLog(n - k + 1.0);

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
            Cmplx a = VMath.GammaLog(n + 1.0);
            Cmplx b = VMath.GammaLog(k + 1.0);
            Cmplx c = VMath.GammaLog(n - k + 1.0);

            return Cmplx.Exp(a - b - c);
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
        public static Dual Binomial(Dual n, Dual k)
        {
            double x = n.Real;
            double y = k.Real;

            //computes the Binomial and it's gradient
            double fx = Binomial(x, y);
            double xy = Digamma(x - y + 1.0);
            double dx = fx * (Digamma(x + 1.0) - xy);
            double dy = fx * (xy - Digamma(y + 1.0));

            //dot product of the dual component and the gradient
            double dot = (n.Eps * dx) + (k.Eps * dy);

            return new Dual(fx, dot);
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
            //NOTE: It should be possable to re-write this method without
            //the while loop, and get greator accuracy!

            //Uses the talor series for the complex error function
            double error = Double.PositiveInfinity;
            int n = 0;

            Cmplx sum1 = 0.0;
            Cmplx sum2 = 0.0;
            Cmplx prod = 1.0;

            while (error > VMath.TOL && n < 128) //128
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
        /// Evalueates the error function at the given value. Note this is not the same
        /// as computing the amount of error in an estimated value. The error funciton
        /// arrises from stastics and has a similar shape to the sigmoid funciton. The
        /// error function, however, cannot be defined in terms of elementry operattors,
        /// thus a numerical aproximation is used.
        /// </summary>
        /// <param name="x">Input to the error fuction</param>
        /// <returns>The result of the error funciton</returns>
        public static Dual Erf(Dual x)
        {
            //computes the error function and its derivitive
            double fx = Erf(x.Real);
            double dx = Gauss(x.Real) * 1.1283791670955125739;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the cumulitive distribution funciton for the standard normal
        /// distribution, with mean of 0 and variance of 1. It returns the probability 
        /// that a random number will appear below the input value.
        /// </summary>
        /// <param name="x">Input value X to test</param>
        /// <returns>Probablity that some value is less than X</returns>
        public static double CDF(double x)
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
        public static Cmplx CDF(Cmplx z)
        {
            //computes z / sqrt(2)
            Cmplx phi = z * 0.7071067811865475244;

            phi = 1.0 + VMath.Erf(phi);
            return phi * 0.5;
        }

        /// <summary>
        /// Computes the cumulitive distribution funciton for the standard normal
        /// distribution, with mean of 0 and variance of 1. It returns the probability 
        /// that a random number will appear below the input value.
        /// </summary>
        /// <param name="x">Input value X to test</param>
        /// <returns>Probablity that some value is less than X</returns>
        public static Dual CDF(Dual x)
        {
            //computes both the CDF and the PDF
            double fx = CDF(x.Real);
            double dx = PDF(x.Real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the probability density function for the standard normal
        /// distribution, with mean of 0 and variance of 1. It is the derivitive
        /// of the standard cumulitive distribution function.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static double PDF(double x)
        {
            double temp = Math.Exp(-0.5 * x * x);
            return temp / VMath.RTP;
        }

        /// <summary>
        /// Computes the probability density function for the standard normal
        /// distribution, with mean of 0 and variance of 1. It is the derivitive
        /// of the standard cumulitive distribution function.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static Cmplx PDF(Cmplx z)
        {
            Cmplx temp = Cmplx.Exp(-0.5 * z * z);
            return temp / VMath.RTP;
        }

        /// <summary>
        /// Computes the probability density function for the standard normal
        /// distribution, with mean of 0 and variance of 1. It is the derivitive
        /// of the standard cumulitive distribution function.
        /// </summary>
        /// <param name="x">Input to the normal distribution funciton</param>
        /// <returns>The result of the normal distribution function</returns>
        public static Dual PDF(Dual x)
        {
            double r = x.Real;

            //computes the gauss function and its derivitive
            double fx = Math.Exp(-0.5 * r * r) / VMath.RTP;
            double dx = -fx * r;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.Eps * dx);
        }

        /// <summary>
        /// Computes the Arethmetic-Geometric mean of two numbers, defined as the limit of
        /// repeated applications of both the Arethmetic and Geometric means. For real values,
        /// this limit is only defined when a and b are both positive. Inputing negative values
        /// into the Arethemetic-Geometric mean returns NaN.
        /// </summary>
        /// <param name="a">First paramater of the AGM</param>
        /// <param name="b">Second paramater of the AGM</param>
        /// <returns>The AGM of the two paramaters</returns>
        public static double AGM(double a, double b)
        {
            //checks that a and b are both positive real values
            if (a < 0.0 || b < 0.0) return Double.NaN;

            double m;

            //itterativly computes the arehmentic and geometric means
            for (int i = 0; i < 10; i++)
            {
                m = (a + b) / 2.0;
                b = Math.Sqrt(a * b);
                a = m;
            }

            return a;
        }

        /// <summary>
        /// Extends the notion of the Arethmetic-Geometric mean to the complex numbers. 
        /// At each step we select the branch of the Geometric mean that is garenteed 
        /// to reach convergence. The result of this selective process is known as the 
        /// Simplest Value for the AGM.
        /// </summary>
        /// <param name="a">First paramater of the AGM</param>
        /// <param name="b">Second paramater of the AGM</param>
        /// <returns>the Simplest Value of the AGM</returns>
        public static Cmplx AGM(Cmplx a, Cmplx b)
        {
            Cmplx an, bn;
            double t1, t2;

            for (int i = 0; i < 10; i++)
            {
                //computes the aretmentic and geometric means
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);

                //makes shure we choose the right branch of sqrt
                t1 = (an - bn).Abs;
                t2 = (an + bn).Abs;
                if (t1 > t2) bn = -bn;

                a = an;
                b = bn;
            }

            return a;

        }

        /// <summary>
        /// Extends the Arithemtic-Geometric Mean into the Dual domain. As in the
        /// Real domain, the AGM is only defined when the real parts of both inputs
        /// are positive. Inputing negative values into the AGM returns NaN.
        /// </summary>
        /// <param name="a">First paramater of the AGM</param>
        /// <param name="b">Second paramater of the AGM</param>
        /// <returns>The AGM of the two paramaters</returns>
        public static Dual AGM(Dual a, Dual b)
        {
            //checks that a and b are both positive real values
            if (a.Real < 0.0) return Dual.NaN;
            if (b.Real < 0.0) return Dual.NaN;

            Dual m;

            //itterativly computes the arehmentic and geometric means
            for (int i = 0; i < 10; i++)
            {
                m = (a + b) / 2.0;
                b = Dual.Sqrt(a * b);
                a = m;
            }

            return a;
        }

        #endregion //////////////////////////////////////////////////////////////////       

    }
}
