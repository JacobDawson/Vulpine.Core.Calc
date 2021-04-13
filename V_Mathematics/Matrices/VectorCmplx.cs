/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2021 Benjamin Jacob Dawson
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;

namespace Vulpine.Core.Calc.Matrices
{
    public class VectorCmplx : Euclidean<VectorCmplx, Cmplx>, IEnumerable<Cmplx>
    {
        #region Class Deffinitions...

        //NOTE: There ought to be a way to treat a generic complex vector as
        //a collection of probability apmlituides (the complex extention of
        //a probabilty distribution) but I need to look more into it.

        //containes the values for the vector
        private double[] real;
        private double[] imag;

        /// <summary>
        /// Constructs a new complex vector of given length. The values 
        /// of the vector are initialy set to zero.
        /// </summary>
        /// <param name="length">The lenght of the vector</param>
        /// <exception cref="BoundsException">If the length of
        /// the vector is less than two</exception>
        public VectorCmplx(int length)
        {
            //makes shure we have a length of atleast one
            if (length < 1) length = 1;

            //creates the vector with zeros
            real = new double[length];
            imag = new double[length];

            //initialises all the values
            for (int i = 0; i < length; i++)
            {
                real[i] = 0.0;
                imag[i] = 0.0;
            }
        }

        /// <summary>
        /// Constructs a new complex vector from a set of values.
        /// </summary>
        /// <param name="vals">The values of the vector</param>
        /// <exception cref="BoundsException">If the length of
        /// the vector is less than two</exception>
        public VectorCmplx(params Cmplx[] vals)
        {
            //checks for valid length
            if (vals.Length < 1) throw new ArgumentShapeException("vals");

            //creates the vector with zeros
            real = new double[vals.Length];
            imag = new double[vals.Length];

            //copies the values into the vector
            for (int i = 0; i < vals.Length; i++)
            {
                real[i] = vals[i].CofR;
                imag[i] = vals[i].CofI;
            }
        }

        /// <summary>
        /// Constructs a new complex vector from a set of values.
        /// </summary>
        /// <param name="vals">The values of the vector</param>
        /// <exception cref="BoundsException">If the length of
        /// the vector is less than two</exception>
        /// <exception cref="MathException">If the length of
        /// the input is not divisible by two</exception>
        public VectorCmplx(params double[] vals)
        {
            //makes shure the input makes sense
            if (vals.Length < 2 || vals.Length % 2 != 0)
            throw new ArgumentShapeException("vals");

            //creates the vector with zeros
            real = new double[vals.Length / 2];
            imag = new double[vals.Length / 2];

            //copies the values into the vector
            for (int i = 0; i < real.Length; i++)
            {
                real[i] = vals[(i + i) + 0];
                imag[i] = vals[(i + i) + 1];
            }
        }

        /// <summary>
        /// Copy constructor, used to create identical copies of a
        /// given complex vector inorder to protect the original.
        /// </summary>
        /// <param name="other">The vector to be copyed</param>
        public VectorCmplx(VectorCmplx other)
        {
            //creates the vector with zeros
            real = new double[other.Length];
            imag = new double[other.Length];

            //copies the values into the vector
            for (int i = 0; i < other.Length; i++)
            {
                real[i] = other.real[i];
                imag[i] = other.imag[i];
            }
        }

        /// <summary>
        /// Conversion constructor, used to create a vector of complex
        /// values from a vector of floating point values.
        /// </summary>
        /// <param name="other">The vector to convert</param>
        public VectorCmplx(Vector other)
        {
            //creates the vector with zeros
            real = new double[other.Length];
            imag = new double[other.Length];

            //copies the values into the vector
            for (int i = 0; i < other.Length; i++)
            {
                real[i] = other[i];
                imag[i] = 0.0;
            }
        }


        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Determins if this vector is equal to another vector. Two vectors are
        /// considered equal if they have the same dimention and the values of
        /// their elements are equal. Note that if one of the vectors changes, 
        /// then the two vectors will no longer be equal.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            VectorCmplx other = obj as VectorCmplx;
            if (other == null) return false;
            if (other.Length != Length) return false;

            for (int i = 0; i < real.Length; i++)
            {
                if (real[i] != other.real[i]) return false;
                if (imag[i] != other.imag[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Generates a sudo-unique hash code for the current vector, based on the 
        /// elements of the vector. Note that if the value of the elements should 
        /// change, it's hash code will change acordingly.
        /// </summary>
        /// <returns>The hash of the current vector</returns>
        public override int GetHashCode()
        {
            int hash = 1764;
            int temp = 0;

            for (int i = 0; i < real.Length; i++)
            {
                unchecked
                {
                    temp = real[i].GetHashCode();
                    hash ^= (hash << 5) + (hash >> 2) + temp;

                    temp = imag[i].GetHashCode();
                    hash ^= (hash << 5) + (hash >> 2) + temp;
                }
            }

            return hash;
        }


        #endregion /////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Represents the length, or number of elements,
        /// present in the vector. Also known as the dimention.
        /// Read-Only.
        /// </summary>
        public int Length
        {
            get { return real.Length; }
        }

        public Cmplx this[int index]
        {
            get { return GetElement(index); }
            set { SetElement(index, value); }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Data Accessors...

        /// <summary>
        /// Obtains the desired element in the complex vector.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element</returns>
        /// <exception cref="BoundsException">If the index is
        /// outside the bounds of the vector</exception>
        public Cmplx GetElement(int index)
        {
            //checks for a valid index
            if (index < 0 || index >= real.Length)
                throw new ArgumentOutOfRangeException("index");

            //obtains the value
            double a = real[index];
            double b = imag[index];
            return new Cmplx(a, b);
        }

        /// <summary>
        /// Sets the desired element in the complex vector.
        /// </summary>
        /// <param name="index">Position of the element</param>
        /// <param name="value">Value to be set</param>
        /// <exception cref="BoundsException">If the index is
        /// outside the bounds of the vector</exception>
        public void SetElement(int index, Cmplx value)
        {
            //checks for a valid index
            if (index < 0 || index >= real.Length)
                throw new ArgumentOutOfRangeException("index");

            //sets the value
            real[index] = value.CofR;
            imag[index] = value.CofI;
        }

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector, it returns zero.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element or zero</returns>
        public Cmplx GetExtended(int index)
        {
            //calls upon the method below
            return GetExtended(index, new Cmplx(0.0));
        }

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector it returns a default value.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <param name="def">Default value if index is invalid</param>
        /// <returns>The desired element or default value</returns>
        public Cmplx GetExtended(int index, Cmplx def)
        {
            //returns the default if the index is out of range
            if (index < 0 || index >= real.Length) return def;

            //obtains the value
            double a = real[index];
            double b = imag[index];
            return new Cmplx(a, b);
        }

        /// <summary>
        /// Generates an enumeration of all the values in the vector.
        /// </summary>
        /// <returns>An enumeration of the vector</returns>
        public IEnumerator<Cmplx> GetEnumerator()
        {
            for (int i = 0; i < real.Length; i++)
            {
                Cmplx c = new Cmplx(real[i], imag[i]);
                yield return c;
            }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Vector Operations...

        /// <summary>
        /// Generates a new vector that is the sum of the current
        /// vector and a second vector. It overides the '+' opperator.
        /// </summary>
        /// <param name="v">The second opperand</param>
        /// <returns>The sum of the two vectors</returns>
        /// <exception cref="MathException">If the vectors are of
        /// differing length</exception>
        public VectorCmplx Add(VectorCmplx v)
        {
            //checks that the vectors are the same length
            if (v.real.Length != real.Length) 
                throw new ArgumentShapeException("v");

            VectorCmplx output = new VectorCmplx(real.Length);

            //builds the resultant vector
            for (int i = 0; i < real.Length; i++)
            {
                output.real[i] = this.real[i] + v.real[i];
                output.imag[i] = this.imag[i] + v.imag[i];
            }

            return output;
        }

        /// <summary>
        /// Generates a new vector that is the diffrence of the current
        /// vector and a second vector. It overides the '-' opperator.
        /// </summary>
        /// <param name="v">The second opperand</param>
        /// <returns>The diffrence of the two vectors</returns>
        /// <exception cref="MathException">If the vectors are of
        /// differing length</exception>
        public VectorCmplx Sub(VectorCmplx v)
        {
            //checks that the vectors are the same length
            if (v.real.Length != real.Length)
                throw new ArgumentShapeException("v");

            VectorCmplx output = new VectorCmplx(real.Length);

            //builds the resultant vector
            for (int i = 0; i < real.Length; i++)
            {
                output.real[i] = this.real[i] - v.real[i];
                output.imag[i] = this.imag[i] - v.imag[i];
            }

            return output;
        }

        /// <summary>
        /// Computes the dot product, or inner product, of the curent
        /// vector and a second given vector. The result is a floating
        /// point scalar. It overides the '*' opperator.
        /// </summary>
        /// <param name="v">The second opperand</param>
        /// <returns>The dot procuct of the two vectors</returns>
        /// <exception cref="MathException">If the vectors are of
        /// differing length</exception>
        public Cmplx Mult(VectorCmplx v)
        {
            //checks that the vectors are the same length
            if (v.real.Length != real.Length)
                throw new ArgumentShapeException("v");

            double temp1, temp2;
            double out_r = 0.0;
            double out_i = 0.0;

            //NOTE: This method computes ab* not a*b [a*b = (ab*)*] 

            //computes the loop for the real component
            for (int i = 0; i < real.Length; i++)
            {
                temp1 = this.real[i] * v.real[i];
                temp2 = this.imag[i] * v.imag[i];
                out_r = out_r + (temp1 + temp2);
            }

            //computes the loop for the imaganry component
            for (int i = 0; i < imag.Length; i++)
            {
                temp1 = this.imag[i] * v.real[i];
                temp2 = this.real[i] * v.imag[i];
                out_i = out_i + (temp1 - temp2);
            }

            //returns the value generated
            return new Cmplx(out_r, out_i);
        }

        /// <summary>
        /// Computes the product of a vector and a complex scalar.
        /// It overides the '*' and '/' opperators.
        /// </summary>
        /// <param name="s">The scalar value opperand</param>
        /// <returns>The product of this vector and a scalar</returns>
        public VectorCmplx Mult(Cmplx s)
        {
            VectorCmplx output = new VectorCmplx(real.Length);
            double temp1, temp2;

            //multiplies each element
            for (int i = 0; i < real.Length; i++)
            {
                temp1 = (real[i] * s.CofR) - (imag[i] * s.CofI);
                temp2 = (real[i] * s.CofI) + (real[i] * s.CofR);
                output.real[i] = temp1;
                output.imag[i] = temp2;
            }

            return output;
        }


        //NOTE: Add an outer product once we have MatrixCmplx defined


        /// <summary>
        /// Computes the pairwise multiplicaiton of two vectors. This is not
        /// a standard vector operaiton, however it dose have certain aplicaitons.
        /// In particular, the pairwise multiple is equivlante to treating the
        /// vectors as if they were diagonal matricies.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The pairwise product of the two vectors</returns>
        public VectorCmplx MultPW(VectorCmplx v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            VectorCmplx output = new VectorCmplx(real.Length);

            //builds the resultant vector
            for (int i = 0; i < real.Length; i++)
            {
                double a = this.real[i];
                double b = this.imag[i];
                double c = v.real[i];
                double d = v.imag[i];

                output.real[i] = (a * c) - (b * d);
                output.imag[i] = (a * d) + (b * c);
            }

            return output;
        }


        #endregion /////////////////////////////////////////////////////////////

        #region Special Methods...

        /// <summary>
        /// Calculates the Norm of the current vector. The Norm is equivlent 
        /// to the distance from the origin or zero vector. It's also defined
        /// as the square root of the vector's dot product with itself. 
        /// </summary>
        /// <returns>The magnitude of the vector</returns>
        public double Norm()
        {
            double temp1, temp2;
            double out_r = 0.0;

            //computes the loop for the real component
            for (int i = 0; i < real.Length; i++)
            {
                temp1 = real[i] * real[i];
                temp2 = imag[i] * imag[i];
                out_r = out_r + (temp1 + temp2);
            }

            //the output is always real
            return Math.Sqrt(out_r);
        }

        /// <summary>
        /// Determins the euclidian distance between this vector and
        /// a diffrent, given vector. 
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The distance from the other vector</returns>
        /// <exception cref="ArgumentShapeException">If the vectors are 
        /// of differing length</exception>
        public double Dist(VectorCmplx v)
        {
            //computes magnitude of the diffrence
            VectorCmplx temp = this.Sub(v);
            return temp.Norm();
        }

        /// <summary>
        /// Produces a new unit vector, pointing in the same direction as the
        /// original vector. This is done by dividing by the Norm, which is
        /// why it is sometimes refred to as normalising the vector.
        /// </summary>
        /// <returns>The corisponding unit vector</returns>
        public VectorCmplx Unit()
        {
            //precomputes the scaling factor
            double[] values = new double[real.Length * 2];
            double scale = 1.0 / this.Norm();

            //scales the values independntly of each other
            for (int i = 0; i < real.Length; i++)
            {
                int j = i * 2;
                values[j + 0] = real[i] * scale;
                values[j + 1] = imag[i] * scale;
            }

            //constructs the new complex vector
            return new VectorCmplx(values);
        }

        /// <summary>
        /// Computes the Hermitian angle between two complex vectors. Note that this
        /// is diffrent from the Euclidian angle, defined on the decomposed real vector
        /// space. In particular, vectors that are orthoginal in real vector space, may
        /// be considered parallel in complex vector space. The output is in the range
        /// of 0 to pi/2 radians, comprising a half-angle.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The Hermitian angle between the two vectors</returns>
        public double AngleH(VectorCmplx v)
        {
            //starts with the Hermitian dot produt
            Cmplx dot = this.Mult(v);

            //computes the norm of both vectors
            double a = this.Norm();
            double b = v.Norm();

            //computes the cosine of the angle
            double cos = dot.Abs / (a * b);
            return Math.Acos(cos);
        }

        /// <summary>
        /// Computes the Euclidian angle between two complex vectors. This is the same
        /// angle you get by treating the coplex vectors as a real vectors with twice 
        /// the dimentionality, and taking the angle of that.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The Euclidian angle between the two vectors</returns>
        public double AngleE(VectorCmplx v)
        {
            //starts with the Hermitian dot produt
            Cmplx dot = this.Mult(v);

            //computes the norm of both vectors
            double a = this.Norm();
            double b = v.Norm();

            //computes the cosine of the angle
            double cos = dot.CofR / (a * b);
            return Math.Acos(cos);
        }

        /// <summary>
        /// Computes the inverse of the current vector, by inverting each of the 
        /// vector's components. Note that this is not a standard vector operaiton, 
        /// and should not be thought of as a true inverse. However, it is useful 
        /// if the vector is being made to represent a diagonal matrix. 
        /// </summary>
        /// <returns>The conponent-wise inverse of the vector</returns>
        public VectorCmplx Inv()
        {
            //creates a new vector to store the result
            VectorCmplx inv = new VectorCmplx(real.Length);

            //inverts each of the elements
            for (int i = 0; i < real.Length; i++)
            {
                Cmplx c = this.GetElement(i).Inv();

                inv.real[i] = c.CofR;
                inv.imag[i] = c.CofI;
            }

            return inv;
        }

        /// <summary>
        /// Computes the complex conjiguate of the curent vector, by preforming
        /// piece-wise conjugation of each of the vector elements.
        /// </summary>
        /// <returns>The component-wise conjugate of the vector</returns>
        public VectorCmplx Conj()
        {
            //creates a new vector to store the result
            VectorCmplx con = new VectorCmplx(real.Length);

            //conjugates each of the elements
            for (int i = 0; i < real.Length; i++)
            {
                con.real[i] = this.real[i];
                con.imag[i] = -this.imag[i];
            }

            return con;
        }

        /// <summary>
        /// Generates the equivlent real valued vector by ignoring the 
        /// imaginary component of the elements alltogether.
        /// </summary>
        /// <returns>The equivlent real vector</returns>
        public Vector Real()
        {
            //returns a copy of the real values
            return new Vector(real);
        }

        /// <summary>
        /// Generates the spectrum, described as the real vector
        /// formed by takeing the absolute value of each of the 
        /// complex elements. Usefull in Foyer analisis.
        /// </summary>
        /// <returns>The spectrum as a real vector</returns>
        public Vector Spectrum()
        {
            //used in computeing the spectrum values
            Vector spec = new Vector(real.Length);
            double temp = 0.0;

            //computes the spectrum values
            for (int i = 0; i < real.Length; i++)
            {
                temp = (real[i] * real[i]) + (imag[i] * imag[i]);
                spec[i] = Math.Sqrt(temp);
            }

            return spec;
        }

        /// <summary>
        /// Generates the phase vector by takeing the argument of
        /// each of the complex elements. Together with the spectrum
        /// it defines the entire complex vector.
        /// </summary>
        /// <returns>The phase vector</returns>
        public Vector Phase()
        {
            Vector phase = new Vector(real.Length);

            //computes the phase values
            for (int i = 0; i < real.Length; i++)
                phase[i] = Math.Atan2(imag[i], real[i]);

            return phase;
        }

        /// <summary>
        /// Expands the complex valued vector into a real valued vector
        /// with twice the number of enterys. For example a vector in C4
        /// becomes a vector in R8. There are some useful benifits to
        /// viewing complex vectors this way.
        /// </summary>
        /// <returns>The expanded real vector</returns>
        public Vector Expand()
        {
            Vector ex = new Vector(real.Length * 2);

            for (int i = 0; i < real.Length; i++)
            {
                ex[i * 2 + 0] = real[i];
                ex[i * 2 + 1] = imag[i];
            }

            return ex;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Class Conversions...

        /**
         *  Allows Explicit conversion from a complex vector to
         *  a floting point vector, by ignoring the imaginary part.
         */
        public static explicit operator Vector(VectorCmplx v)
        {
            return v.Real();
        }

        /**
         *  Allows an Implicit conversion from a floting point vector
         *  to a complex vector as all rational numbers can also
         *  be treated as complex numbers.
         */
        public static implicit operator VectorCmplx(Vector v)
        {
            return new VectorCmplx(v);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Opperator Overlodes...

        //refferences the Add(v) function
        public static VectorCmplx operator +(VectorCmplx v, VectorCmplx w)
        { return v.Add(w); }

        //references the Sub(v) function
        public static VectorCmplx operator -(VectorCmplx v, VectorCmplx w)
        { return v.Sub(w); }

        //references the Mult(v) function
        public static Cmplx operator *(VectorCmplx v, VectorCmplx w)
        { return v.Mult(w); }

        //references the Mult(s) function
        public static VectorCmplx operator *(VectorCmplx v, Cmplx s)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static VectorCmplx operator *(Cmplx s, VectorCmplx v)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static VectorCmplx operator /(VectorCmplx v, Cmplx s)
        { return v.Mult(1.0 / s); }

        #endregion /////////////////////////////////////////////////////////////

        //object ICloneable.Clone()
        //{ return new VectorCmplx(this); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }

    }
}
