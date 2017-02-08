/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
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

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Numbers
{
    /// <summary>
    /// Complex numbers are numbers that contain both a real part and an imaginary
    /// part. They are based on the imaginary unit (I) who's square is negative one.
    /// Any multiple of (I) is said to be imaginary, froming a line of numbers
    /// orthognal to the real number line. In this way, complex numbers extend the
    /// real numbers into a two-dimentional feild that is algebraicly complete.
    /// </summary>
    /// <remarks>Last Update: 2016-11-18</remarks>
    public struct Cmplx : Algebraic<Cmplx, Double>, IFormattable
    {
        #region Class Definitions...

        //stores the real and imaginary components
        private double real;
        private double imag;

        /// <summary>
        /// Converts a real number to a complex number.
        /// </summary>
        /// <param name="num">Value to convert</param>
        public Cmplx(double num)
        {
            //initialises values
            real = num;
            imag = 0.0;
        }

        /// <summary>
        /// Constructs a complex number of the form (a + bi) given
        /// it's real and imaginary components.
        /// </summary>
        /// <param name="r">The real compoent</param>
        /// <param name="i">The imaginary component</param>
        public Cmplx(double r, double i)
        {
            //initialises values
            real = r;
            imag = i;
        }

        /// <summary>
        /// Generates a string representation of the complex number.
        /// </summary>
        /// <returns>The number as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("g10", null);
        }

        /// <summary>
        /// Generates a formated string representation of the current complex
        /// number, by passing the format infromation to both the real and
        /// imaginary components. It handels the sign information independtly
        /// from the format string.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The number formated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (IsInfinity()) return "Inf";
            if (IsNaN()) return "NaN";

            StringBuilder sb = new StringBuilder();
            double r = Math.Abs(real);
            double i = Math.Abs(imag);

            sb.Append(real < 0.0 ? "-" : "");
            sb.Append(r.ToString(format, provider));

            sb.Append(imag < 0.0 ? " - " : " + ");
            sb.Append(i.ToString(format, provider));

            sb.Append("i");
            return sb.ToString();
        }

        /// <summary>
        /// Determins if another complex number Exactly equals the current number
        /// based on their respective floating point representations. In order to
        /// determin if two numbers are aproximatly equal, an alternate method 
        /// should be employed.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Cmplx)
            {
                Cmplx other = (Cmplx)obj;

                if (real != other.real) return false;
                if (imag != other.imag) return false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a sudo-unique hash code for the current complex number.
        /// </summary>
        /// <returns>The hash of the complex number</returns>
        public override int GetHashCode()
        {
            int a1 = real.GetHashCode();
            int a2 = imag.GetHashCode();

            return unchecked((a1 * 907) ^ a2);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Constant Constructors...

        /// <summary>
        /// The imaginary constant, and the basis of the imaginary numbers. It
        /// is defined as the square root of negative one.
        /// </summary>
        public static Cmplx I
        {
            get { return new Cmplx(0.0, 1.0); }
        }

        /// <summary>
        /// Represnents the value 2 * PI * I, which apeares frequently in many
        /// formulas invlovling complex numbers, usualy represented by the
        /// lowercase greek letter omega.
        /// </summary>
        public static Cmplx W
        {
            get { return new Cmplx(0.0, VMath.TAU); }
        }

        /// <summary>
        /// Returns the complex equevlent of a NaN (Not a Number) value, borrowed
        /// from the conventions used by real floating-point numbers.
        /// </summary>
        public static Cmplx NaN
        {
            get { return new Cmplx(Double.NaN, Double.NaN); }
        }

        /// <summary>
        /// Returns the singluar point at infinity as described by the Riemann 
        /// Sphere, which can be thought of as an extention to the complex plane.
        /// </summary>
        public static Cmplx Inf
        {
            get
            {
                double inf = Double.PositiveInfinity;
                return new Cmplx(inf, inf);
            }
        } 

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The coffecent of the real compoent (1) making up the
        /// complex number.
        /// </summary>
        public double CofR
        {
            get 
            {
                if (!Double.IsNaN(imag)) return real;
                else return Double.NaN;
            }
        }

        /// <summary>
        /// The coffecent of the imaginary component (I) makeing up
        /// the complex number.
        /// </summary>
        public double CofI
        {
            get 
            {
                if (!Double.IsNaN(real)) return imag;
                else return Double.NaN;
            }
        }

        /// <summary>
        /// The absolute value of the complex number. This is equivlent
        /// to the magnitude or radius in polar cordinats.
        /// </summary>
        public double Abs
        {
            get
            {
                //uses the distace fourmual
                double z = (real * real) + (imag * imag);
                return Math.Sqrt(z);
            }
        }

        /// <summary>
        /// The priniciple argument of the complex number, ranging from
        /// just above negative PI to PI. It is equivlent to the
        /// anglular mesure in polar cordinats.
        /// </summary>
        public double Arg
        {
            get { return Math.Atan2(imag, real); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Urinary Operations...

        /// <summary>
        /// Generates the real compoent of the complex number. This is
        /// diffrent from the coffecent as the result is another complex 
        /// number. Use CofR for the coffecent.
        /// </summary>
        /// <returns>An exclusivly real number</returns>
        public Cmplx Real()
        {
            //returns the exclusivly real component
            return new Cmplx(real, 0.0);
        }

        /// <summary>
        /// Generates the imaginary compoent of the complex number. This is
        /// diffrent from the coffecent as the result is another complex
        /// number. Use CofI for the coffecent.
        /// </summary>
        /// <returns>An exclusivly imaginary number</returns>
        public Cmplx Imag()
        {
            //returns the exclusivly imaginary component
            return new Cmplx(0.0, imag);
        }

        /// <summary>
        /// Computes the negative of the current complex number.
        /// </summary>
        /// <returns>The negative complex number</returns>
        /// <remarks>It overloads the uinary (-) operatoer</remarks>
        public Cmplx Neg()
        {
            //negates the complex number
            return new Cmplx(-real, -imag);
        }

        /// <summary>
        /// Computes the conjgate of the curent complex number, defined
        /// by the negation of the imaginary coefecent.
        /// </summary>
        /// <returns>The complex conjigate</returns>
        /// <remarks>It overloads the (~) opperator</remarks>
        public Cmplx Conj()
        {
            //produces the complex conjagate
            return new Cmplx(real, -imag);
        }

        /// <summary>
        /// Computes the multiplicitve inverse of the complex number. 
        /// </summary>
        /// <returns>The inverse of the complex number</returns>
        public Cmplx Inv()
        {
            //uses the conjgate to invert the number
            double temp = (real * real) + (imag * imag);
            double re = real / temp;
            double ie = imag / temp;

            //returns the complex inverse
            return new Cmplx(re, -ie);
        }

        /// <summary>
        /// Computes the multiplication of the complex number by the
        /// imaginary constant (I), saving on opperations. 
        /// </summary>
        /// <returns>The complex number multiplyed by I</returns>
        public Cmplx MultI()
        {
            //computes the multily in one opperation
            return new Cmplx(-imag, real);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Binary Operations...

        /// <summary>
        /// Adds the opperand to the curent complex number. 
        /// </summary>
        /// <param name="z">The number to add</param>
        /// <returns>The sum of the complex numbers</returns>
        /// <remarks>It overloads the (+) opperator</remarks>
        public Cmplx Add(Cmplx z)
        {
            //dose peicewise addition of coffecents
            double a = real + z.real;
            double b = imag + z.imag;

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Subtracts the opperand from the curent complex number.
        /// </summary>
        /// <param name="z">The number to subtract</param>
        /// <returns>The diffrence of the complex numbers</returns>
        /// <remarks>It overloads the (-) opperator</remarks>
        public Cmplx Sub(Cmplx z)
        {
            //dose peicewise addition of coffecents
            double a = real - z.real;
            double b = imag - z.imag;

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Multiplys the current complex number by the opperand.
        /// </summary>
        /// <param name="z">The number used to mutiply</param>
        /// <returns>The product of the complex numbers</returns>
        /// <remarks>It overloads the (*) opperator</remarks>
        public Cmplx Mult(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly
            double a = (real * z.real) - (imag * z.imag);
            double b = (real * z.imag) + (imag * z.real);

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Divides the current complex number by the opperand.
        /// </summary>
        /// <param name="z">The number used to devide</param>
        /// <returns>The quotient of the two complex numbers</returns>
        /// <remarks>It overloads the (/) opperator</remarks>
        public Cmplx Div(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly
            double temp = (z.real * z.real) + (z.imag * z.imag);
            double a = (real * z.real + imag * z.imag) / temp;
            double b = (imag * z.real - real * z.imag) / temp;

            return new Cmplx(a, b);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Conputes the euclidian distance between two complex numbers
        /// within the complex feild. This is the same as the absolute
        /// value of the number's diffrence.
        /// </summary>
        /// <param name="z">The other number</param>
        /// <returns>The distance between the numbers in the complex feild</returns>
        public double Dist(Cmplx z)
        {
            //subtracts the two parts seperatly
            double xr = z.real - real;
            double yr = z.imag - imag;

            //computes the sum of the squares
            return Math.Sqrt((xr * xr) + (yr * yr));
        }

        /// <summary>
        /// Computes the Norm of the current complex number. This
        /// is exactly the same as the absolute value.
        /// </summary>
        /// <returns>The magnitude of the complex number</returns>
        public double Norm()
        {
            //simply returns the asbolute value
            return this.Abs;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Transindental Functions...

        /// <summary>
        /// This method extends the domain of the exponintial function
        /// (defined by euler's constant) into the complex plain. It is a
        /// single-valued, continous function for all complex numbers.
        /// </summary>
        /// <param name="z">Power of the exponential</param>
        /// <returns>Euler's constant raised to a complex power</returns>
        public static Cmplx Exp(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly 
            double power = Math.Exp(z.real);
            double outR = power * Math.Cos(z.imag);
            double outI = power * Math.Sin(z.imag);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This metnod generalises the complex exponential function, allowing
        /// for any positive real base. It is also uniquly defined for all
        /// complex numbers under these restrictions.
        /// </summary>
        /// <param name="z">Power of the exponential</param>
        /// <param name="bass">Base of the exponential</param>
        /// <returns>A given base raised to a complex power</returns>
        public static Cmplx Exp(Cmplx z, double bass)
        {
            //takes care of the special casses
            if (bass.IsZero()) return new Cmplx(0.0, 0.0);
            if (bass < 0.0) return Cmplx.NaN;

            //calculates the result storing temporary values
            double temp1 = Math.Pow(bass, z.real);
            double temp2 = z.imag * Math.Log(bass);
            double outR = temp1 * Math.Cos(temp2);
            double outI = temp1 * Math.Sin(temp2);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// The complex logarythim is the inverse of the complex exponential.
        /// In general, there are infinatly many solutions to the complex
        /// logarythim. This method returns the principle branch, with the
        /// branch cut lying along the negative real axis.
        /// </summary>
        /// <param name="z">Argument of the logarythim</param>
        /// <returns>The natural log of a complex number</returns>
        public static Cmplx Log(Cmplx z)
        {
            //calculates the resulting components seperatly
            double outR = Math.Log(z.Abs);
            double outI = z.Arg;

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This method generalises the complex logarythim, allowing for
        /// any positive real base (except for base-1). Like the natural
        /// logarythim, it also has infinatly many solutions. This method 
        /// returns the principle branch, with the branch cut lying along 
        /// the negative real axis. 
        /// </summary>
        /// <param name="z">Argument of the complex logarythim</param>
        /// <param name="bass">Base of the complex logarythim</param>
        /// <returns>The log of a complex number in the given base</returns>
        public static Cmplx Log(Cmplx z, double bass)
        {
            //return NaN if the bass is invalid
            if (bass <= 0.0) return Cmplx.NaN;

            //computes the radius and the log of the bass
            double outR = Math.Log(z.Abs);
            double outI = Math.Log(bass);

            //calculates the log with the change of base formula
            outR = outR / outI;
            outI = z.Arg / outI;
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Raises a complex number to an arbitrary, real power. In general, this
        /// function may have more that one solution. The value returned by this
        /// method is the principle value.
        /// </summary>
        /// <param name="z">Base of the exponintial</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A complex number raised to a given power</returns>
        public static Cmplx Pow(Cmplx z, double exp)
        {
            //calculates the result storing temporary values
            double rad = Math.Pow(z.Abs, exp);
            double arg = z.Arg * exp;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This is the most general form of the expnential, as it raises any
        /// complex number to any complex power, and is in general multi-valued.
        /// Consider using one of the other functions if either the base or 
        /// the exponent are known to be real.
        /// </summary>
        /// <param name="z">Base of the exponential</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A complex number raised to a complex power</returns>
        public static Cmplx Pow(Cmplx z, Cmplx exp)
        {
            //allows for computation of zero, avoiding ln(0)
            if (z.Abs.IsZero()) return new Cmplx(0.0, 0.0);

            //uses the definition of the general exponential
            return Cmplx.Exp(Cmplx.Log(z).Mult(exp));
        }

        /// <summary>
        /// This method calculates the square root of a complex number. All
        /// numbers in the complex plain have exactly two square roots. This
        /// method returns the principle value, with a positive imaginary
        /// component.
        /// </summary>
        /// <param name="z">Argument of the radical</param>
        /// <returns>The square root of a complex number</returns>
        public static Cmplx Sqrt(Cmplx z)
        {
            //calculates the result storing temporary values
            double rad = Math.Sqrt(z.Abs);
            double arg = z.Arg / 2.0;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Trigimetric Functions...

        /// <summary>
        /// Computes the sine of a complex number. The sine function extends
        /// naturaly to the coplex numbers, due to the complex exponential
        /// and Euler's identity.
        /// </summary>
        /// <param name="arg">Argument of the complex sine</param>
        /// <returns>The sine of the complex number</returns>
        public static Cmplx Sin(Cmplx arg)
        {
            //uses the hyperboloc definition to compute the sine
            double outR = Math.Sin(arg.real) * Math.Cosh(arg.imag);
            double outI = Math.Cos(arg.real) * Math.Sinh(arg.imag);
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Computes the cosign of a complex number. The cosign function 
        /// extends naturaly to the coplex numbers, due to the complex 
        /// exponential and Euler's identity.
        /// </summary>
        /// <param name="arg">Argument of the complex cosine</param>
        /// <returns>The cosine of the complex number</returns>
        public static Cmplx Cos(Cmplx arg)
        {
            //uses the hyperboloc definition to compute the cosine
            double outR = Math.Cos(arg.real) * Math.Cosh(arg.imag);
            double outI = Math.Sin(arg.real) * Math.Sinh(arg.imag);
            return new Cmplx(outR, -outI);
        }

        /// <summary>
        /// Computes the tangent of a complex number. Like the tangent
        /// function on the real numbers, it is defined as the ratio of
        /// the sign over the cosign.
        /// </summary>
        /// <param name="arg">Argument of the complex tangent</param>
        /// <returns>The tangent of the complex number</returns>
        public static Cmplx Tan(Cmplx arg)
        {
            //computes the raito of the sign over the cosine
            return Cmplx.Sin(arg) / Cmplx.Cos(arg);
        }

        /// <summary>
        /// Computes the principle arcsine of a complex number. That is,
        /// it finds a complex value who's sign is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arcsine</param>
        /// <returns>The principle arcsine of the given number</returns>
        public static Cmplx Asin(Cmplx arg)
        {
            //uses the logarthim definition of arcsign
            Cmplx temp = 1.0 - (arg * arg);
            temp = arg.MultI() + Cmplx.Sqrt(temp);
            return -Cmplx.Log(temp).MultI();
        }

        /// <summary>
        /// Computes the principle arcosine of a complex number. That is,
        /// it finds a complex value who's cosign is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arcosine</param>
        /// <returns>The principle arcosine of the given number</returns>
        public static Cmplx Acos(Cmplx arg)
        {
            //uses the logarthim definition of arcosine
            Cmplx temp = 1.0 - (arg * arg);
            temp = arg + Cmplx.Sqrt(temp).MultI();
            return -Cmplx.Log(temp).MultI();
        }

        /// <summary>
        /// Computes the principle arctangent of a complex number. That is,
        /// it finds a complex value who's tangent is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arctangent</param>
        /// <returns>The principle arctangent of the given number</returns>
        public static Cmplx Atan(Cmplx arg)
        {
            //uses the logarithm definition of arctangent
            Cmplx temp1 = Cmplx.Log(1.0 - arg.MultI());
            Cmplx temp2 = Cmplx.Log(1.0 + arg.MultI());
            return (temp1 - temp2) * new Cmplx(0.0, 0.5);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Hyperbolic Functions...

        /// <summary>
        /// Computes the hyperbolic sine of a complex number. It releates 
        /// imaginary angles to a hyperbola, similar to how the trigometric 
        /// sine releates angles to circles.
        /// </summary>
        /// <param name="arg">Argument of complex hyperbolic sine</param>
        /// <returns>The hyperbolic sine of the complex number</returns>
        public static Cmplx Sinh(Cmplx arg)
        {
            //inverts the formula used for complex sine
            double outR = Math.Sinh(arg.real) * Math.Cos(arg.imag);
            double outI = Math.Cosh(arg.real) * Math.Sin(arg.imag);

            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Computes the hyperbolic cosine of a complex number. It releates 
        /// imaginary angles to a hyperbola, similar to how the trigometric 
        /// cosine releates angles to circles.
        /// </summary>
        /// <param name="arg">Argument of complex hyperbolic cosine</param>
        /// <returns>The hyperbolic cosine of the complex number</returns>
        public static Cmplx Cosh(Cmplx arg)
        {
            //inverts the formula used for complex cosine
            double outR = Math.Cosh(arg.real) * Math.Cos(arg.imag);
            double outI = Math.Sinh(arg.real) * Math.Sin(arg.imag);

            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Computes the hyperbolic tangent of a complex number. It releates 
        /// imaginary angles to a hyperbola, similar to how the trigometric 
        /// tangent releates angles to circles.
        /// </summary>
        /// <param name="arg">Argument of complex hyperbolic tangent</param>
        /// <returns>The hyperbolic tangent of the complex number</returns>
        public static Cmplx Tanh(Cmplx arg)
        {
            //computes the raito of the sign over the cosine
            return Cmplx.Sinh(arg) / Cmplx.Cosh(arg);
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic sine. That is it finds a
        /// value who's hyperbolic sine is equal to the argument. Since more
        /// than one value may satisify this contidtion, this function returns
        /// what is considered the "principle value".
        /// </summary>
        /// <param name="arg">Argument of the inverse hyperbolic sine</param>
        /// <returns>The principle value of the inverse hyperbolic sine</returns>
        public static Cmplx Asinh(Cmplx arg)
        {
            //uses the default mathmatic definition 
            Cmplx temp = (arg * arg) + 1.0;
            temp = arg + Cmplx.Sqrt(temp);
            return Cmplx.Log(temp);
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic cosine. That is it finds a
        /// value who's hyperbolic cosine is equal to the argument. Since more
        /// than one value may satisify this contidtion, this function returns
        /// what is considered the "principle value".
        /// </summary>
        /// <param name="arg">Argument of the inverse hyperbolic cosine</param>
        /// <returns>The principle value of the inverse hyperbolic cosine</returns>
        public static Cmplx Acosh(Cmplx arg)
        {
            //factors the square root for the principle value
            Cmplx a = Cmplx.Sqrt(arg + 1.0);
            Cmplx b = Cmplx.Sqrt(arg - 1.0);
            return Cmplx.Log(arg + (a * b));
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic tangent. That is it finds a
        /// value who's hyperbolic tangent is equal to the argument. Since more
        /// than one value may satisify this contidtion, this function returns
        /// what is considered the "principle value".
        /// </summary>
        /// <param name="arg">Argument of the inverse hyperbolic tangent</param>
        /// <returns>The principle value of the inverse hyperbolic tangent</returns>
        public static Cmplx Atanh(Cmplx arg)
        {
            //distributes the log for grreator stability
            Cmplx a = Cmplx.Log(1.0 + arg);
            Cmplx b = Cmplx.Log(1.0 - arg);
            return (a - b) * 0.5;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Value Checking Functions...

        /// <summary>
        /// Determins if a complex number exists at one of the points of
        /// infinity. This set of points can be viewed as the outter edge 
        /// of the extended complex plane, or as a single point on the 
        /// Riemann Sphere.
        /// </summary>
        /// <returns>True if the number is at one of the points of infinity,
        /// and false if otherwise</returns>
        public bool IsInfinity()
        {
            if (Double.IsInfinity(real)) return true;
            else if (Double.IsInfinity(imag)) return true;
            else return false;
        }

        /// <summary>
        /// Determins if a complex number is a valid number. If either the
        /// real or imaginary component is invalid, then the number as a whole
        /// is invalid. It returns true for invlalid numbers.
        /// </summary>
        /// <returns>True if the number is invalid, and false otherwise</returns>
        public bool IsNaN()
        {
            if (Double.IsNaN(real)) return true;
            else if (Double.IsNaN(imag)) return true;
            else return false;
        }     

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Converts a double to a complex number through the constructor
        /// </summary>
        public static implicit operator Cmplx(Double n)
        { 
            return new Cmplx(n); 
        }

        /// <summary>
        /// Converts the complex number to a floating point number if it lies 
        /// on the real number line. Otherwise it reutrns NaN.
        /// </summary>
        public static explicit operator Double(Cmplx n)
        {
            bool is_real = Math.Abs(n.imag) < VMath.TOL;
            return (is_real) ? n.real : Double.NaN;
        }

        /// <summary>
        /// Converts the complex number to a 2D vector containing the real
        /// and imaginary components as elements.
        /// </summary>
        public static implicit operator Vector(Cmplx n)
        { 
            return new Vector(n.real, n.imag); 
        }

        /// <summary>
        /// Converts a vector to a complex number by treating the first and second
        /// elements as the real and imaginary parts respectivly.
        /// </summary>
        public static explicit operator Cmplx(Vector v)
        {
            double r = v.GetExtended(0);
            double i = v.GetExtended(1);
            return new Cmplx(r, i);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the Add(z) function
        public static Cmplx operator +(Cmplx a, Cmplx b)
        { return a.Add(b); }

        //references the Sub(z) function
        public static Cmplx operator -(Cmplx a, Cmplx b)
        { return a.Sub(b); }

        //references the Mult(z) function
        public static Cmplx operator *(Cmplx a, Cmplx b)
        { return a.Mult(b); }

        //references the Div(z) function
        public static Cmplx operator /(Cmplx a, Cmplx b)
        { return a.Div(b); }

        //refrences the Neg() function
        public static Cmplx operator -(Cmplx n)
        { return n.Neg(); }

        //refrences the Conj() function
        public static Cmplx operator ~(Cmplx n)
        { return n.Conj(); }

        #endregion //////////////////////////////////////////////////////////////

        Cmplx Euclidean<Cmplx, Double>.Mult(double scalar)
        { return this.Mult((Cmplx)scalar); }
    }
}
