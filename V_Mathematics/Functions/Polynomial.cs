/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
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

using Vulpine.Core.Data;
using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Functions
{
    /// <summary>
    /// A polynomial is a special type of function, that is comprised as a sumation
    /// of monomials of the form (ax^n) where (x) is the input variable and (n) is 
    /// some positive interger. The set of all polynomials also forms a ring, in 
    /// that polynomials can be added, subtracted, and multiplied to produce new
    /// polynomials. In adition, polynomials can also be divided in a way similar
    /// to the intergers, producing both a quotient and a remainder. Polynomials
    /// cover a wide viraity of possable functions, and are often used to 
    /// aproximate more complicated functions, as they are eayser to work with.
    /// </summary>
    /// <remarks>Last Update: 2013-11-20</remarks>
    public class Polynomial : Algebraic<Polynomial, Double>
        //Function<Double>, Function<Cmplx>, Function<Matrix>
    {
        #region Class Definitions...

        //NOTE: Consider including a method to construct a polynomial from
        //its roots, unless this is covered by the Legrange form

        //stores the coefficients of the polynomial in inceacing order,
        //thus the index into the array is equivlent to the term's power
        private double[] coeff;

        /// <summary>
        /// Creates the simplest polynomial of given degree, with a leading 
        /// coefficient of one and all other coefficients initialised to zero. 
        /// The polynomial can then be built dynamicaly.
        /// </summary>
        /// <param name="degree">Degree of the ploynomial</param>
        /// <exception cref="ArgRangeExcp">If the degree is negative</exception>
        public Polynomial(int degree)
        {
            //checks for a vaild degree
            ArgRangeExcp.Atleast("degree", degree, 0);

            //sets up instance variables
            coeff = new double[degree + 1];
            coeff[degree] = 1.0;

            //fills in the remaning coffecents with zero
            for (int i = 0; i < degree - 1; i++) coeff[i] = 0.0;
        }

        /// <summary>
        /// Creates a new polynomial with the given coefficients. Note that
        /// the terms should be listed in increasing order, not the more common
        /// decreacing order. The degree is determined by the number of terms.
        /// </summary>
        /// <param name="coeff">The coefficients in increasing order</param>
        /// <exception cref="ArgRangeExcp">If no coefficients are given</exception>
        public Polynomial(params double[] coeff)
        {
            //checks for valid degree
            ArgRangeExcp.Atleast("coeff.Length", coeff.Length, 1);

            //instanciates the new row of coffecents
            this.coeff = new double[coeff.Length];

            //copies the coffecents from highest to lowest
            for (int i = 0; i < coeff.Length; i++)
            this.coeff[i] = coeff[i];
        }

        /// <summary>
        /// Copy Constructor, used for makeing a deep copy of another
        /// polynomial. This can be used to protect user access.
        /// </summary>
        /// <param name="other">Polynomial to be copied</param>
        public Polynomial(Polynomial other)
        {
            //instanciates the new row of coffecents
            this.coeff = new double[other.coeff.Length];

            //copies the coffecents into the array
            for (int i = 0; i < coeff.Length; i++)
            this.coeff[i] = other.coeff[i];
        }

        /// <summary>
        /// Generates a deep copy of the current polynomial by invoking
        /// the corisponding copy constructor.
        /// </summary>
        /// <returns>A copy of the polynomial</returns>
        public Polynomial Clone()
        {
            //calls upon the copy constructor
            return new Polynomial(this);
        }

        /// <summary>
        /// Generates a string representation of the polynomial, reporting
        /// the number of terms in the polynomial, not it's degree.
        /// </summary>
        /// <returns>The polynomial as a string</returns>
        public override string ToString()
        {
            //reports the dimention (not degree) of the polynomial
            return String.Format("Polynomial[{0}]", coeff.Length);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Properties Deffinition...

        /// <summary>
        /// Represents the degree of the polynomial, or the power of the
        /// leading term. Note that the degree is considered the same even 
        /// if the leading coefficient is zero. Read-Only.
        /// </summary>
        public int Degree
        {
            get { return coeff.Length - 1; }
        }

        /// <summary>
        /// Accesses the coefficients of the polynomial by the power of there
        /// associated terms. See the SetCoeff() and GetCoeff() methods for
        /// more details.
        /// </summary>
        /// <param name="power">Power of the desired term</param>
        /// <returns>The coefficient of the term</returns>
        public double this[int power]
        {
            get { return GetCoeff(power); }
            set { SetCoeff(power, value); }
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Coefficient Access...

        /// <summary>
        /// Returns the coefficient of a spesific term, indicated by the
        /// power of the desired term.
        /// </summary>
        /// <param name="power">Power of the desired term</param>
        /// <returns>The coefficient of the term</returns>
        public double GetCoeff(int power)
        {
            ArgRangeExcp.Check("power", power, coeff.Length - 1); 
            return coeff[power];
        }

        /// <summary>
        /// Sets the coefficient of a spesific term to the given vlaue. The
        /// term is indicated by it's power.
        /// </summary>
        /// <param name="power">Power of the desired term</param>
        /// <param name="value">New value of the coefficient</param>
        public void SetCoeff(int power, double value)
        {
            ArgRangeExcp.Check("power", power, coeff.Length - 1);
            coeff[power] = value;
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Polynomial Evaluation...

        /// <summary>
        /// Evaluates the polynomial at a given point.
        /// </summary>
        /// <param name="x">The input to the polynomial</param>
        /// <returns>The evaluation of the polynomial</returns>
        public double Evaluate(double x)
        {
            //stores the value as it is computed
            double value = coeff[coeff.Length - 1];

            //uses horners method to compute the polynomial
            for (int i = coeff.Length - 2; i >= 0; i--)
            value = coeff[i] + (x * value);

            return value;
        }

        /// <summary>
        /// Evaluates the polynomial at a paticular point within the
        /// complex plane. This is usefull for checking the non-real 
        /// roots of a polynomial.
        /// </summary>
        /// <param name="x">The input to the polynomial</param>
        /// <returns>The evaluation of the polynomial</returns>
        public Cmplx Evaluate(Cmplx x)
        {
            //stores the value as it is computed
            Cmplx value = coeff[coeff.Length - 1];

            //uses horners method to compute the polynomial
            for (int i = coeff.Length - 2; i >= 0; i--)
            value = coeff[i] + (x * value);

            return value;
        }

        //NOTE: This mehtod Almost works, except it requires knoladge
        //about the zero and one elements of the algebra (which every algebra
        //should have). Note also, that the multiply is not ambiguious, because
        //we start with the identity and multiply the same value over and over.

        public A Evaluate<A>(A x) where A : Algebraic<A, Double>
        {
            A power = default(A); //should be set to ONE
            A sum = default(A);   //should be set to ZERO

            for (int i = 0; i < coeff.Length; i++)
            {
                sum = sum.Add(power.Mult(coeff[i]));
                power = power.Mult(x);
            }

            return sum;
        }

        ///// <summary>
        ///// Treats the domain of the polynomial as the set of all square
        ///// matrices. Using an aproximating tailor series, this gives rise
        ///// to the deffinition of the matrix exponential and trigometric
        ///// functions.
        ///// </summary>
        ///// <param name="x">The input to the polynomial</param>
        ///// <returns>The evaluation of the polynomial</returns>
        ///// <exception cref="SquareMatrixExcp">If the input matrix is 
        ///// not a square matrix</exception>
        //public Matrix Evaluate(Matrix x)
        //{
        //    //makes shure that the input matrix is square
        //    SquareMatrixExcp.Check(x.NumColumns, x.NumRows);

        //    //stores the value as it is computed
        //    Matrix value = Matrix.Ident(x.NumRows);
        //    value.MultBy(coeff[coeff.Length - 1]);

        //    //uses horners method to compute the polynomial
        //    for (int i = coeff.Length - 2; i >= 0; i--)
        //    {
        //        value.MultBy(x);
        //        value.AddWith(coeff[i]);
        //    }

        //    return value;
        //}

        #endregion ////////////////////////////////////////////////////////////

        #region Uinary Operations...

        /// <summary>
        /// Computes the negative of the current polynomial. It
        /// overloads the uinary (-) operatoer.
        /// </summary>
        /// <returns>The negative polynomial</returns>
        public Polynomial Neg()
        {
            //creates a polynomial to store the negation
            Polynomial neg = new Polynomial(this.Degree);

            //negates each of the coeffecients in turn
            for (int i = 0; i < coeff.Length; i++)
            neg.coeff[i] = 0.0 - coeff[i];

            return neg;
        }

        /// <summary>
        /// Generates the derivative of the current polynomial as a new
        /// function. This is possable because the dirivitive of any
        /// polynomial is always another polynomial.
        /// </summary>
        /// <returns>The derivative function of the polynomial</returns>
        public Polynomial Deriv()
        {
            //takes care of single term polynomials
            if (coeff.Length < 2) return new Polynomial(0.0);

            //creates the new polynomial for drivitive
            Polynomial driv = new Polynomial(this.Degree - 1);

            //calculates the derivative funciton
            for (int term = coeff.Length - 1; term >= 1; term--)
            {
                double value = term * coeff[term];
                driv.coeff[term - 1] = value;
            }

            return driv;
        }

        /// <summary>
        /// Generates the primitive, or indefenate intergral, of the current
        /// polynomial. The primitive is always another polynomial and is
        /// unique up to a trailing constant term.
        /// </summary>
        /// <param name="c">The trailing constant</param>
        /// <returns>The primitive funciton of the polynomial</returns>
        public Polynomial Prim(double c = 0.0)
        {
            //creates a new polynomial for the intergral
            Polynomial intg = new Polynomial(this.Degree + 1);

            //calcuates the indefinate intergral
            for (int term = coeff.Length - 1; term >= 0; term--)
            {
                double value = coeff[term] / (term + 1.0);
                intg.coeff[term + 1] = value;
            }

            //sets the constant term and returns
            intg.coeff[0] = c; return intg;
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Binary Operations...

        /// <summary>
        /// Generates a new polynomial that is the sum of the current polynomial
        /// and second polynomial. It overloads the (+) operator.
        /// </summary>
        /// <param name="g">The other polynomial</param>
        /// <returns>The sum of the two polynomials</returns>
        public Polynomial Add(Polynomial g)
        {
            //assume that the curent polynomial is larger
            if (this.Degree < g.Degree) return g.Add(this);

            //used in generating the sum of the two polynomials
            int count = coeff.Length - 1;
            Polynomial sum = new Polynomial(count);

            //copies the single terms to the new polynomial
            while (count > g.Degree)
            {
                sum.coeff[count] = this.coeff[count];
                count = count - 1;
            }

            //adds the remaning terms together
            while (count >= 0)
            {
                double temp = this.coeff[count];
                sum.coeff[count] = temp + g.coeff[count];
                count = count - 1;
            }

            return sum;
        }

        /// <summary>
        /// Generates a new polynomial that is the diffrence of the current
        /// polynomal and a second polynomial. It overloads the (-) operator.
        /// </summary>
        /// <param name="g">The other polynomial</param>
        /// <returns>The product of the two polynomials</returns>
        public Polynomial Sub(Polynomial g)
        {
            //simply adds the negated polynomial
            return Add(g.Neg());
        }

        /// <summary>
        /// Generates a new polynomial that is the product of the current
        /// polynomial and a second polynomial. It overloads the (*) operator.
        /// </summary>
        /// <param name="g">The other polynomial</param>
        /// <returns>The product of the two polynomials</returns>
        public Polynomial Mult(Polynomial g)
        {
            //creates an array to store the output polynomial
            int degree = this.Degree + g.Degree;
            double[] output = new double[degree + 1];

            //used in calculating the output
            double temp = 0.0;

            //prefroms FOIL on the polynomials 
            for (int c1 = this.Degree; c1 >= 0; c1--)
            {
                for (int c2 = g.Degree; c2 >= 0; c2--)
                {
                    temp = this.coeff[c1] * g.coeff[c2];
                    temp = temp + output[c1 + c2];
                    output[c1 + c2] = temp;
                }
            }

            return new Polynomial(output);
        }

        /// <summary>
        /// Divides the current polynomial by another and returns the quotient. 
        /// For a more detailed explination see the LongDiv method. It overrides 
        /// the (/) opperator.
        /// </summary>
        /// <param name="g">The polynomial divisor</param>
        /// <returns>The quotient polynomial</returns>
        public Polynomial Div(Polynomial g)
        {
            Polynomial temp;
            return LongDiv(g, out temp);
        }

        /// <summary>
        /// Divides the current polynomial by another and returns the remainder. 
        /// For a more detailed explination see the LongDiv method. It overrides 
        /// the (%) opperator.
        /// </summary>
        /// <param name="g">The polynomial divisor</param>
        /// <returns>The remainder polynomial</returns>
        public Polynomial Mod(Polynomial g)
        {
            Polynomial temp;
            LongDiv(g, out temp);
            return temp;
        }

        /// <summary>
        /// Divides one polynomial by another resulting in a quotent polynomial
        /// and a remainder term. That is, the original rational function can be
        /// re-written as the sum of the quotient and the remainder over the
        /// original divisor [Q + (R / G)]. This method computes both the quotent
        /// and the remainder simultaniously, and is more effecent than computing
        /// either term seperatly. 
        /// </summary>
        /// <param name="g">The polynomial divisor</param>
        /// <param name="mod">Returns the remainder polynomial</param>
        /// <returns>The quotient polynomial</returns>
        public Polynomial LongDiv(Polynomial g, out Polynomial mod)
        {
            //takes care of cases where the bottom is larger
            if (g.Degree > this.Degree)
            {
                mod = new Polynomial(this);
                return new Polynomial(0.0);
            }

            //takes care of cases where the divisor is a constant
            if (g.Degree == 0)
            {
                mod = new Polynomial(0.0);

                Polynomial temp = new Polynomial(this.Degree);
                for (int i = 0; i <= temp.Degree; i++)
                temp.coeff[i] = this.coeff[i] / g.coeff[0];

                return temp;
            }

            //sets up double arrays as nessary
            double[] quo = new double[this.Degree - g.Degree + 1];
            double[] rem = new double[this.coeff.Length];

            //copys the dividend into the remainder
            for (int i = this.Degree; i >= 0; i--)
            rem[i] = this.coeff[i];

            for (int c1 = this.Degree; c1 >= g.Degree; c1--)
            {
                //divides to find quotent
                int c2 = c1 - g.Degree;
                quo[c2] = rem[c1] / g.coeff[g.Degree];

                //counters used in calculating the remander
                int c3 = c1 - 1;
                int c4 = g.Degree - 1;

                //calculates the lie and updates the remander
                while (c4 >= 0)
                {
                    double lie = quo[c2] * g.coeff[c4];
                    rem[c3] = rem[c3] - lie;

                    c3 = c3 - 1;
                    c4 = c4 - 1;
                }
            }

            //copies down the final form of the remainder
            mod = new Polynomial(g.Degree - 1);
            for (int i = 0; i < mod.coeff.Length; i++)
            mod.coeff[i] = rem[i];

            //returns the generated quotient
            return new Polynomial(quo);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Eucildian Implementation...

        /// <summary>
        /// Computes the Norm of the curent polynomial. This is defined as
        /// the square root, of the sum of the square coffecents, and is equivlent
        /// to the Norm of the corsponding n-dimentional vector.
        /// </summary>
        /// <returns>The magnitude of the polynomial</returns>
        public double Norm()
        {
            //used to store the result
            double output = 0.0;

            //computes the sum of the squares
            for (int i = 0; i < coeff.Length; i++)
                output += this.coeff[i] * this.coeff[i];

            //returns the square root of the sum
            return Math.Sqrt(output);
        }

        /// <summary>
        /// Computes the euclidian distance between any two polynomials,
        /// regardless of degree. It is defined using the Norm of the diffrence
        /// between the two polynomials.
        /// </summary>
        /// <param name="other">The other polynomial</param>
        /// <returns>The distance between the polynomials</returns>
        public double Dist(Polynomial other)
        {
            //uses the norm to define the metric
            return this.Sub(other).Norm();
        }

        /// <summary>
        /// Multiplies the current polynomial by a single constant value. This has
        /// the effect of scaling each coeffecent by the given value.
        /// </summary>
        /// <param name="s">The constant scalar value</param>
        /// <returns>The scaled polynomial</returns>
        public Polynomial Mult(double s)
        {
            //creates a new vector to store the result
            double[] output = new double[coeff.Length];

            //multiplies each element componentwise
            for (int i = 0; i < coeff.Length; i++)
                output[i] = coeff[i] * s;

            return new Polynomial(output);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the Add(g) function
        public static Polynomial operator +(Polynomial a, Polynomial b)
        { return a.Add(b); }

        //references the Sub(g) function
        public static Polynomial operator -(Polynomial a, Polynomial b)
        { return a.Sub(b); }

        //references the Mult(g) function
        public static Polynomial operator *(Polynomial a, Polynomial b)
        { return a.Mult(b); }

        //references the Div(g) function
        public static Polynomial operator /(Polynomial a, Polynomial b)
        { return a.Div(b); }

        //references the Mod(g) function
        public static Polynomial operator %(Polynomial a, Polynomial b)
        { return a.Mod(b); }

        //references the Neg() function
        public static Polynomial operator -(Polynomial a)
        { return a.Neg(); }

        //references the copy constructor
        public static Polynomial operator +(Polynomial a)
        { return new Polynomial(a); }

        #endregion ////////////////////////////////////////////////////////////
    }
}
