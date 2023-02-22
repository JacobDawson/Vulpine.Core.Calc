/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2023 Benjamin Jacob Dawson
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

namespace Vulpine.Core.Calc.Numbers
{
    /// <summary>
    /// Dual numbers are numbers of the form (a + be) where a and b are real values, and
    /// (e) is defined such that e^2 = 0. Thay are analogus to the complex numbers, except 
    /// that the "imaginary" component is defined diffrently. They can, in a sence, be
    /// though of as encoding infentessimal values, as (e) can be considered an infintessimal.
    /// There primary pourpus though is to aid in automatic diffenencation, as the (e) 
    /// component encodes information about the derivitive.
    /// </summary>
    public struct Dual : IComparable<Dual>, Algebraic<Dual, Double>, IFormattable
    {
        #region Class Definitions...

        //stores the real and dual components
        private double real;
        private double dual;

        /// <summary>
        /// Converts a real number to a dual number.
        /// </summary>
        /// <param name="num">Value to convert</param>
        public Dual(double num)
        {
            //initialises values
            real = num;
            dual = 0.0;
        }

        /// <summary>
        /// Constructs a dual number of the form (a + be) given
        /// it's real and dual components.
        /// </summary>
        /// <param name="r">The real compoent</param>
        /// <param name="e">The dual component</param>
        public Dual(double r, double e)
        {
            //initialises values
            real = r;
            dual = e;
        }

        /// <summary>
        /// Generates a string representation of the dual number.
        /// </summary>
        /// <returns>The number as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("g5", null);
        }

        /// <summary>
        /// Generates a formated string representation of the current dual
        /// number, by passing the format infromation to both the real and
        /// dual components. It handels the sign information independtly
        /// from the format string.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The number formated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            //if (IsInfinity()) return "Inf";
            //if (IsNaN()) return "NaN";

            StringBuilder sb = new StringBuilder();
            double r = Math.Abs(real);
            double e = Math.Abs(dual);

            sb.Append(real < 0.0 ? "-" : "");
            sb.Append(r.ToString(format, provider));

            sb.Append(dual < 0.0 ? " - " : " + ");
            sb.Append(e.ToString(format, provider));

            sb.Append(" e");
            return sb.ToString();
        }

        /// <summary>
        /// Determins if another dual number Exactly equals the current number
        /// based on their respective floating point representations. In order to
        /// determin if two numbers are aproximatly equal, an alternate method 
        /// should be employed.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Dual)
            {
                Dual other = (Dual)obj;

                if (real != other.real) return false;
                if (dual != other.dual) return false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a sudo-unique hash code for the current dual number.
        /// </summary>
        /// <returns>The hash of the dual number</returns>
        public override int GetHashCode()
        {
            int a1 = real.GetHashCode();
            int a2 = dual.GetHashCode();

            return unchecked((a1 * 907) ^ a2);
        }

        public int CompareTo(Dual other)
        {
            //compairse the real components first
            if (real < other.real) return -1;
            if (real > other.real) return 1;

            //compares the dual components only if the real components match
            if (dual < other.dual) return -1;
            if (dual > other.dual) return 1;

            //the numbers must be the same
            return 0;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Constant Constructors...

        /// <summary>
        /// The dual constant episolon, and the basis of the dual numbers. It
        /// is defined such that it's square is is equal to zero, but it is not
        /// equal to zero itself.
        /// </summary>
        public static Dual E
        {
            get { return new Dual(0.0, 1.0); }
        }

        /// <summary>
        /// Returns the dual equevlent of a NaN (Not a Number) value, borrowed
        /// from the conventions used by real floating-point numbers.
        /// </summary>
        public static Dual NaN
        {
            get { return new Dual(Double.NaN, Double.NaN); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The coffecent of the real compoent (1) making up the composite 
        /// dual number.
        /// </summary>
        public double Real
        {
            get
            {
                if (!Double.IsNaN(dual)) return real;
                else return Double.NaN;
            }
        }

        /// <summary>
        /// The coffecent of the dual (epsilon) component makeing up the 
        /// composite dual number.
        /// </summary>
        public double Eps
        {
            get
            {
                if (!Double.IsNaN(real)) return dual;
                else return Double.NaN;
            }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Urinary Operations...

        /// <summary>
        /// Computes the negative of the current dual number.
        /// </summary>
        /// <returns>The negative dual number</returns>
        /// <remarks>It overloads the uinary (-) operatoer</remarks>
        public Dual Neg()
        {
            //negates both the real and dual components
            return new Dual(-real, -dual);
        }

        /// <summary>
        /// Computes the conjgate of the curent dual number, defined
        /// by the negation of the imaginary coefecent.
        /// </summary>
        /// <returns>The dual conjigate</returns>
        /// <remarks>It overloads the (~) opperator</remarks>
        public Dual Conj()
        {
            //negates only the dual component
            return new Dual(real, -dual);
        }

        /// <summary>
        /// Computes the multiplicitve inverse of the dual number. 
        /// </summary>
        /// <returns>The inverse of the dual number</returns>
        public Dual Inv()
        {
            //division is undifined for pure imaginary numbers
            double r2 = real * real;
            if (r2.IsZero()) return Dual.NaN;

            //uses the conjugate to invert the dual number
            double r = 1.0 / real;
            double e = -dual / r2;
            return new Dual(r, e);
        }



        #endregion //////////////////////////////////////////////////////////////

        #region Binary Operations...

        /// <summary>
        /// Adds the opperand to the curent dual number. 
        /// </summary>
        /// <param name="x">The dual number to add</param>
        /// <returns>The sum of the two dual numbers</returns>
        /// <remarks>It overloads the (+) opperator</remarks>
        public Dual Add(Dual x)
        {
            //dose peicewise addition of coffecents
            double a = real + x.real;
            double b = dual + x.dual;

            return new Dual(a, b);
        }

        /// <summary>
        /// Subtracts the opperand from the curent dual number.
        /// </summary>
        /// <param name="x">The dual number to subtract</param>
        /// <returns>The diffrence of the two dual numbers</returns>
        /// <remarks>It overloads the (-) opperator</remarks>
        public Dual Sub(Dual x)
        {
            //dose peicewise addition of coffecents
            double a = real - x.real;
            double b = dual - x.dual;

            return new Dual(a, b);
        }

        /// <summary>
        /// Multiplys the current dual number by the opperand.
        /// </summary>
        /// <param name="x">The dual number used to mutiply</param>
        /// <returns>The product of the two dual numbers</returns>
        /// <remarks>It overloads the (*) opperator</remarks>
        public Dual Mult(Dual x)
        {
            //we do not need to compute the product of the dual parts
            //since their product is, by definition, zero
            double a = real * x.real;
            double b = real * x.dual;
            double c = dual * x.real;

            return new Dual(a, b + c);
        }

        /// <summary>
        /// Divides the current dual number by the opperand.
        /// </summary>
        /// <param name="x">The dual number used to devide</param>
        /// <returns>The quotient of the two dual numbers</returns>
        /// <remarks>It overloads the (/) opperator</remarks>
        public Dual Div(Dual x)
        {
            //division is undifined for pure imaginary numbers
            double r2 = x.real * x.real;
            if (r2.IsZero()) return Dual.NaN;

            //stores the computed dual values
            double r, e;

            //the method is derived using the dual conjiguate, similar to how
            //complex division is derived using the complex conjuigate
            r = real / x.real;
            e = (dual * x.real) - (real * x.dual);
            e = e / r2;

            return new Dual(r, e);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Computes the "distance" between two dual space numbers, defined by
        /// taking the norm of their diffrence. This is equivilent to computing
        /// the distance between just the real components.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Dist(Dual x)
        {
            //computes the distance between the real components
            return Math.Abs(real - x.real);
        }

        /// <summary>
        /// Computes the Norm of a dual space number, defined by multiplying the
        /// number with it's conjugate. This results in only the real portion of
        /// the dual number contributing to the norm, the dual part is ignored.
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            //simply returns the absolute value of the real component
            return Math.Abs(real);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Transindental Functions...

        /// <summary>
        /// This method extends the domain of the exponintial function
        /// (defined by euler's constant) into the dual domain. 
        /// </summary>
        /// <param name="x">Power of the exponential</param>
        /// <returns>Euler's constant raised to a dual power</returns>
        public static Dual Exp(Dual x)
        {
            //the exponential function is it's own dirivitive
            double exp = Math.Exp(x.real);
            return new Dual(exp, x.dual * exp);
        }

        /// <summary>
        /// This metnod generalises the dual exponential function, allowing
        /// for any positive real base.
        /// </summary>
        /// <param name="x">Power of the exponential</param>
        /// <param name="bass">Base of the exponential</param>
        /// <returns>A given base raised to a dual power</returns>
        public static Dual Exp(Dual x, double bass)
        {
            //takes care of the special casses
            if (bass.IsZero()) return new Dual(0.0, 0.0);
            if (bass < 0.0) return Dual.NaN;

            //computes the exponential and it's derivitive
            double fx = Math.Pow(bass, x.real);
            double dx = fx * Math.Log(bass);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// This method extends the natural logrythim (defined by euler's constant)
        /// into the dual domain. Like in the real domain, the logryhtim is only
        /// defined for dual numbers with positive real components.
        /// </summary>
        /// <param name="x">Argument of the logarythim</param>
        /// <returns>The natural log of a dual number</returns>
        public static Dual Log(Dual x)
        {
            //the logarythim is only defined for positive values
            if (x.real.IsZero() || x.real < 0.0) return Dual.NaN;

            //computes the logarythim and it's derivitive
            double fx = Math.Log(x.real);
            double dx = 1.0 / x.real;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// This metnod generalises the dual logrythim function, allowing
        /// for any positive real base. Again, like the natural logrythim, it
        /// is only defined for dual numbers with positive real compoments.
        /// </summary>
        /// <param name="x">Argument of the logarythim</param>
        /// <param name="bass">Base of the logarythim</param>
        /// <returns>The log of a dual number in the given base</returns>
        public static Dual Log(Dual x, double bass)
        {
            //the logarythim is only defined for positive values
            if (x.real.IsZero() || x.real < 0.0) return Dual.NaN;
            if (bass.IsZero() || bass < 0.0) return Dual.NaN;

            //computes the log of the bass
            double lb = Math.Log(bass);

            //computes the logarythim and it's derivitive
            double fx = Math.Log(x.real) / lb;
            double dx = 1.0 / (x.real * lb);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Raises a dual number to an arbitary real power.
        /// </summary>
        /// <param name="x">Base of the power funciton</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A dual number raised to a given power</returns>
        public static Dual Pow(Dual x, double exp)
        {
            //computes the power function and it's derivitive
            double fx = Math.Pow(x.real, exp);
            double dx = exp * Math.Pow(x.real, exp - 1.0);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// This is the most general form of the expnential, as it raises any
        /// dual number to any other dual power. Consider using one of the other 
        /// functions if either the base or the exponent are known to be real.
        /// </summary>
        /// <param name="x">Base of the exponential</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A complex number raised to a complex power</returns>
        public static Dual Pow(Dual x, Dual exp)
        {
            //uses the definition of the general exponential
            return Dual.Exp(Dual.Log(x).Mult(exp));
        }

        /// <summary>
        /// Computes the square root of a dual number. Like the real valued square
        /// root, it is only defined for dual numbers with positive real components.
        /// </summary>
        /// <param name="x">Argument of the radical</param>
        /// <returns>The square root of a dual number</returns>
        public static Dual Sqrt(Dual x)
        {
            //the square root is only defined for positive values
            if (x.real.IsZero() || x.real < 0.0) return Dual.NaN;

            //computes the square root and it's derivitive
            double fx = Math.Sqrt(x.real);
            double dx = 1.0 / (2.0 * fx);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Piecewise Funcitons..

        /// <summary>
        /// Computes the absolute value function of the given dual number. In this 
        /// case the "absolute value" dose not refer to a Norm on the space of dual 
        /// numbers, but is an extention of the real-valued function to their domain.
        /// </summary>
        /// <param name="x">Input to the ABS function</param>
        /// <returns>The ABS of the dual number</returns>
        public static Dual Abs(Dual x)
        {
            //computes the absolute value and it's derivitive
            double fx = Math.Abs(x.real);
            double dx = VMath.Sign(x.real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Extends the Floor function to the dual realm. Because the drivitive of
        /// the floor funciton is essentily zero, this causes the dual portion
        /// of the number to vanish.
        /// </summary>
        /// <param name="x">Input to the Floor funciton</param>
        /// <returns>The Floor of the dual number</returns>
        public static Dual Floor(Dual x)
        {
            double fx = Math.Floor(x.real);
            return new Dual(fx, 0.0);
        }

        /// <summary>
        /// Extends the Ceiling function to the dual realm. Because the drivitive of
        /// the floor funciton is essentily zero, this causes the dual portion
        /// of the number to vanish.
        /// </summary>
        /// <param name="x">Input to the Ceiling funciton</param>
        /// <returns>The Ceiling of the dual number</returns>
        public static Dual Ceiling(Dual x)
        {
            double fx = Math.Ceiling(x.real);
            return new Dual(fx, 0.0);
        }

        /// <summary>
        /// Computes the Max function of two dual numbers. This dose not produce a
        /// maximum value in the truest sense, but rather is an extention of the 
        /// function to the dual realm. It is useful when doing Max-pooling.
        /// </summary>
        /// <param name="x">First argument to the Max function</param>
        /// <param name="y">Second argument to the Max function</param>
        /// <returns>The retult of the Max function</returns>
        public static Dual Max(Dual x, Dual y)
        {
            double fx = Math.Max(x.real, y.real);

            //computes the gradient of the "Max" function
            double dx = (x.real >= y.real) ? 1.0 : 0.0;
            double dy = (x.real >= y.real) ? 0.0 : 1.0;

            //dot product of the dual component and the gradient
            double dot = (x.dual * dx) + (y.dual * dy);

            return new Dual(fx, dot);
        }

        /// <summary>
        /// Computes the Min function of two dual numbers. This dose not produce a
        /// minimum value in the truest sense, but rather is an extention of the 
        /// function to the dual realm. It is useful when doing Min-pooling.
        /// </summary>
        /// <param name="x">First argument to the Min function</param>
        /// <param name="y">Second argument to the Min function</param>
        /// <returns>The retult of the Min function</returns>
        public static Dual Min(Dual x, Dual y)
        {
            double fx = Math.Min(x.real, y.real);

            //computes the gradient of the "Max" function
            double dx = (x.real <= y.real) ? 1.0 : 0.0;
            double dy = (x.real <= y.real) ? 0.0 : 1.0;

            //dot product of the dual component and the gradient
            double dot = (x.dual * dx) + (y.dual * dy);

            return new Dual(fx, dot);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Trigimetric Functions...

        /// <summary>
        /// Computes the sine of a dual number.
        /// </summary>
        /// <param name="x">Argument of the sine</param>
        /// <returns>The sine of the dual number</returns>
        public static Dual Sin(Dual x)
        {
            //computes the sin function and it's derivitive
            double fx = Math.Sin(x.real);
            double dx = Math.Cos(x.real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the cosin of a dual number.
        /// </summary>
        /// <param name="x">Argument of the cosine</param>
        /// <returns>The cosine of the dual number</returns>
        public static Dual Cos(Dual x)
        {
            //computes the sin function and it's derivitive
            double fx = Math.Cos(x.real);
            double dx = -Math.Sin(x.real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the tangent of a dual number. 
        /// </summary>
        /// <param name="x">Argument of the tangent</param>
        /// <returns>The tangent of the dual number</returns>
        public static Dual Tan(Dual x)
        {
            //computes both the sin and the cosine
            double sin = Math.Sin(x.real);
            double cos = Math.Cos(x.real);

            //computes the tangent function and it's derivitive
            double fx = sin / cos;
            double dx = 1.0 / (cos * cos);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);

            ////computes the raito of the sign over the cosine
            //return Dual.Sin(x) / Dual.Cos(x);
        }

        /// <summary>
        /// Computes the arcsine of a dual number. That is, it finds a 
        /// dual value who's sign is the given number. 
        /// </summary>
        /// <param name="x">Argument of the arcsine</param>
        /// <returns>The arcsine of the given number</returns>
        public static Dual Asin(Dual x)
        {
            //stores the funciton and it's derivitive
            double fx, dx;

            //computes the inverse sign and it's derivitive
            fx = Math.Asin(x.real);
            dx = 1.0 - (x.real * x.real);
            dx = 1.0 / Math.Sqrt(dx);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the arccosine of a dual number. That is, it finds a 
        /// dual value who's cosign is the given number. 
        /// </summary>
        /// <param name="x">Argument of the arccosine</param>
        /// <returns>The arccosine of the given number</returns>
        public static Dual Acos(Dual x)
        {
            //stores the funciton and it's derivitive
            double fx, dx;

            //computes the inverse cosine and it's derivitive
            fx = Math.Acos(x.real);
            dx = 1.0 - (x.real * x.real);
            dx = -1.0 / Math.Sqrt(dx);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the arctangent of a dual number. That is, it finds a 
        /// dual value who's tangent is the given number. 
        /// </summary>
        /// <param name="x">Argument of the arctangent</param>
        /// <returns>The arctangent of the given number</returns>
        public static Dual Atan(Dual x)
        {
            //stores the funciton and it's derivitive
            double fx, dx;

            //computes the inverse cosine and it's derivitive
            fx = Math.Atan(x.real);
            dx = 1.0 + (x.real * x.real);
            dx = 1.0 / dx;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Hyperbolic Functions...

        /// <summary>
        /// Computes the hyperbolic sine of a dual number.
        /// </summary>
        /// <param name="x">Argument of the hyperbolic sine</param>
        /// <returns>The hyperbolic sine of the dual number</returns>
        public static Dual Sinh(Dual x)
        {
            //computes the hyperbolic sine and it's derivitive
            double fx = Math.Sinh(x.real);
            double dx = Math.Cosh(x.real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the hyperbolic cosine of a dual number.
        /// </summary>
        /// <param name="x">Argument of the hyperbolic cosine</param>
        /// <returns>The hyperbolic cosine of the dual number</returns>
        public static Dual Cosh(Dual x)
        {
            //computes the hyperbolic sine and it's derivitive
            double fx = Math.Cosh(x.real);
            double dx = Math.Sinh(x.real);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the hyperbolic tangent of a dual number. 
        /// </summary>
        /// <param name="x">Argument of the hyperbolic tangent</param>
        /// <returns>The hyperbolic tangent of the dual number</returns>
        public static Dual Tanh(Dual x)
        {
            //computes both the sin and the cosine
            double sinh = Math.Sinh(x.real);
            double cosh = Math.Cosh(x.real);

            //computes the tangent function and it's derivitive
            double fx = sinh / cosh;
            double dx = 1.0 / (cosh * cosh);

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic sine. That is it finds a
        /// value who's hyperbolic sine is equal to the argument. 
        /// </summary>
        /// <param name="x">Argument of the inverse hyperbolic sine</param>
        /// <returns>The value of the inverse hyperbolic sine</returns>
        public static Dual Asinh(Dual x)
        {
            //the value sqrt(x^2 + 1) is ued in both the function and the dirivitive
            double rad = x.real * x.real;
            rad = Math.Sqrt(rad + 1.0);

            //computes both the function and the dirivitive
            double fx = Math.Log(rad + x.real);
            double dx = 1.0 / rad;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic cosine. That is it finds a
        /// value who's hyperbolic sine is equal to the argument. It is undefined
        /// for dual numbers who's real component is less than one.
        /// </summary>
        /// <param name="x">Argument of the inverse hyperbolic cosine</param>
        /// <returns>The value of the inverse hyperbolic cosine</returns>
        public static Dual Acosh(Dual x)
        {
            //the function is only defined for real values greator that one
            if (x.real <= 1.0) return Dual.NaN;

            //the value is ued in both the function and the dirivitive
            double rad = Math.Sqrt(x.real + 1.0);
            rad = rad * Math.Sqrt(x.real - 1.0);

            //computes both the function and the dirivitive
            double fx = Math.Log(rad + x.real);
            double dx = 1.0 / rad;

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        /// <summary>
        /// Computes the inverse of the hyperbolic tangent. That is it finds a
        /// value who's hyperbolic tangent is equal to the argument. It is only
        /// defined for real values in the range of [-1 .. 1].
        /// </summary>
        /// <param name="x">Argument of the inverse hyperbolic sine</param>
        /// <returns>The value of the inverse hyperbolic sine</returns>
        public static Dual Atanh(Dual x)
        {
            //the funciton is only defined for real values in the range [-1 .. 1]
            if (x.real <= -1.0 || x.real >= 1.0) return Dual.NaN;

            //logarythims used in the inverse hyperbolic tangent
            double ln1 = Math.Log(x.real + 1.0);
            double ln2 = Math.Log(1.0 - x.real);

            //computes both the function and the dirivitive
            double fx = (ln1 - ln2) * 0.5;
            double dx = 1.0 / (1.0 - (x.real * x.real));

            //constructs the dual number from the derivitive
            return new Dual(fx, x.dual * dx);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Value Checking Functions...

        /// <summary>
        /// Determins if a complex number is a valid number. If either the
        /// real or imaginary component is invalid, then the number as a whole
        /// is invalid. It returns true for invlalid numbers.
        /// </summary>
        /// <returns>True if the number is invalid, and false otherwise</returns>
        public bool IsNaN()
        {
            if (Double.IsNaN(real)) return true;
            else if (Double.IsNaN(dual)) return true;
            else return false;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Converts a double to a dual number through the constructor
        /// </summary>
        public static implicit operator Dual(Double n)
        {
            return new Dual(n);
        }

        /// <summary>
        /// Converts a dual number to a floating point number by disregarding
        /// the dual portion of the number.
        /// </summary>
        public static explicit operator Double(Dual n)
        {
            return n.real; 
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the Add(z) function
        public static Dual operator +(Dual a, Dual b)
        { return a.Add(b); }

        //references the Sub(z) function
        public static Dual operator -(Dual a, Dual b)
        { return a.Sub(b); }

        //references the Mult(z) function
        public static Dual operator *(Dual a, Dual b)
        { return a.Mult(b); }

        //references the Div(z) function
        public static Dual operator /(Dual a, Dual b)
        { return a.Div(b); }

        //refrences the Neg() function
        public static Dual operator -(Dual n)
        { return n.Neg(); }

        //refrences the Conj() function
        public static Dual operator ~(Dual n)
        { return n.Conj(); }

        #endregion //////////////////////////////////////////////////////////////

        Dual Euclidean<Dual, Double>.Mult(double scalar)
        { return this.Mult((Dual)scalar); }

    }
}
