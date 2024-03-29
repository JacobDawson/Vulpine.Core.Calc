﻿/**
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Matrices
{
    /// <summary>
    /// For most purpouses, a vector can be thought of as a one-dimentional array 
    /// of real numbers. Vectors can be used to represent points and arrows (both
    /// direction and magnitude) in multi-dimentional space. More formaly, a vector
    /// can represent any member of a vector space that has a well-defined basis.
    /// In this library, vectors differ from points in that points are immutable
    /// where as vectors are not.
    /// </summary>
    /// <remarks>Last Update: 2016-07-09</remarks>
    public sealed class Vector : Euclidean<Vector, Double>, IEnumerable<Double>, IFormattable
    {
        //NOTE: Consider adding Contatinaiton and Subvector methods
        //this would be helpfull for nural-nets that build vectors

        #region Class Deffinitions...

        //containes the values for the vector
        private double[] vector;

        /// <summary>
        /// Constructs a new vector of given length. The values of
        /// the vector are initialy set to zero.
        /// </summary>
        /// <param name="length">The lenght of the vector</param>
        public Vector(int length)
        {
            //creats a vector with length atleast one
            if (length < 1) length = 1;
            vector = new double[length];

            //intiialises the vector to all zeroes
            for (int i = 0; i < length; i++) vector[i] = 0.0;
        }

        /// <summary>
        /// Constructs a new vector from a set of values.
        /// </summary>
        /// <param name="vals">The values of the vector</param>
        /// <exception cref="ArgumentException">If the length of
        /// the vector is less than one</exception>
        public Vector(params double[] vals)
        {
            //checks for valid length
            if (vals.Length < 1) throw new ArgumentShapeException("vals");

            //copies the values into the vector
            vector = new double[vals.Length];
            for (int i = 0; i < vals.Length; i++)
                vector[i] = vals[i];
        }

        /// <summary>
        /// Copy constructor, used to create identical copies of a
        /// given vector inorder to protect the original.
        /// </summary>
        /// <param name="other">The vector to be copyed</param>
        public Vector(Vector other)
        {
            //obtains the degree of the other vector
            int length = other.vector.Length;

            //copies the values into the vector
            vector = new double[length];
            for (int i = 0; i < length; i++)
                vector[i] = other.vector[i];
        }

        /// <summary>
        /// Generates a string representation of the vector, using the default
        /// formating for floating point values. If the vector is larger than 
        /// 4 elements, elipisis notation is used. 
        /// </summary>
        /// <returns>The vector fomated as a string</returns>
        public override string ToString()
        {
            //calls upon the method below
            return ToString("g5", null);
        }

        /// <summary>
        /// Generates a formated string representation of the vector, suplying
        /// the format information to each element of the vector in turn. If
        /// the vector is larger than 4 elements, elipisis notation is used.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The vector fomated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            //we are garenteed to have atleast one element
            sb.Append(vector[0].ToString(format, provider));

            if (vector.Length < 5)
            {
                for (int i = 1; i < vector.Length; i++)
                {
                    sb.Append(", ");
                    sb.Append(vector[i].ToString(format, provider));
                }
            }
            else
            {
                for (int i = 1; i < 3; i++)
                {
                    sb.Append(", ");
                    sb.Append(vector[i].ToString(format, provider));
                }

                sb.Append(" ... ");
                double last = vector[vector.Length - 1];
                sb.Append(last.ToString(format, provider));
            }

            sb.Append("]");
            return sb.ToString();
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
            Vector other = obj as Vector;
            if (other == null) return false;
            if (other.Length != Length) return false;

            for (int i = 0; i < vector.Length; i++)
                if (vector[i] != other.vector[i]) return false;

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

            for (int i = 0; i < vector.Length; i++)
            {
                unchecked
                {
                    temp = vector[i].GetHashCode();
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
        /// </summary>
        public int Length
        {
            get { return vector.Length; }
        }

        /// <summary>
        /// Accesses the vector elements by index. See the SetElement() 
        /// and GetElement() methods for more details.
        /// </summary>
        /// <param name="index">The index of the vector element</param>
        /// <returns>The requested element</returns>
        public double this[int index]
        {
            get { return GetElement(index); }
            set { SetElement(index, value); }
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Data Accessors...

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector, it throws an exception.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the index is
        /// outside the bounds of the vector</exception>
        public double GetElement(int index)
        {
            //checks for a valid index
            if (index < 0 || index >= vector.Length)
                throw new ArgumentOutOfRangeException("index");

            //simply sets the item
            return vector[index];
        }

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector, it returns zero.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element or zero</returns>
        public double GetExtended(int index)
        {
            if (index < 0 || index >= vector.Length) return 0.0;
            else return vector[index];
        }

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector it returns a default value.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <param name="def">Default value if index is invalid</param>
        /// <returns>The desired element or default value</returns>
        public double GetExtended(int index, double def)
        {
            if (index < 0 || index >= vector.Length) return def;
            else return vector[index];
        }

        /// <summary>
        /// Sets the desired element in the vector.
        /// </summary>
        /// <param name="index">Position of the element</param>
        /// <param name="value">Value to be set</param>
        /// <exception cref="ArgumentOutOfRangeException">If the index is
        /// outside the bounds of the vector</exception>
        public void SetElement(int index, double value)
        {
            //checks for a valid index
            if (index < 0 || index >= vector.Length)
                throw new ArgumentOutOfRangeException("index");

            //simply sets the item
            vector[index] = value;
        }

        /// <summary>
        /// Generates an enumeration of all the values in the vector.
        /// </summary>
        /// <returns>An enumeration of the vector</returns>
        public IEnumerator<Double> GetEnumerator()
        {
            for (int i = 0; i < vector.Length; i++)
                yield return vector[i];
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Vector Operations...

        /// <summary>
        /// Generates a new vector that is the sum of the current vector 
        /// and a second vector.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The sum of the two vectors</returns>
        /// <remarks>It overloads the (+) opperator</remarks>
        /// <exception cref="ArgumentShapeException">If the vectors are of
        /// differing length</exception>
        public Vector Add(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //builds the resultant vector
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = this.vector[i] + v.vector[i];

            return output;
        }

        /// <summary>
        /// Generates a new vector that is the diffrence of the current
        /// vector and a second vector.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The diffrence of the two vectors</returns>
        /// <remarks>It overloads the (-) opperator</remarks>
        /// <exception cref="ArgumentShapeException">If the vectors are of
        /// differing length</exception>
        public Vector Sub(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //builds the resultant vector
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = this.vector[i] - v.vector[i];

            return output;
        }

        /// <summary>
        /// Computes the dot product, or inner product, of the curent
        /// vector and a second given vector. The result is a floating
        /// point scalar.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The dot procuct of the two vectors</returns>
        /// <remarks>It overloads the (*) opperator</remarks>
        /// <exception cref="ArgumentShapeException">If the vectors are of
        /// differing length</exception>
        public double Mult(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //used to store the result
            double output = 0.0;

            //calcualtes the sum of the product
            for (int i = 0; i < vector.Length; i++)
                output += this.vector[i] * v.vector[i];

            return output;
        }

        /// <summary>
        /// Computes the product of a vector and a floating point scalar.
        /// This has the effect of multiplying the vector's magnitude
        /// by the same amount.
        /// </summary>
        /// <param name="s">Value by which to scale the vector</param>
        /// <returns>The product of this vector and a scalar</returns>
        /// <remarks>It overloads the (*) and (/) opperators, respectivly</remarks>
        public Vector Mult(double s)
        {
            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //multiplies each element componentwise
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = this.vector[i] * s;

            return output;
        }

        /// <summary>
        /// Computes the outer product of the curent vector and a second
        /// given vector. This results in a matrix who's elements are the
        /// pairwise products of the elements of each vector. Note that unlike
        /// the inner product, the vectors can be of differing length.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The outer product of the two vectors</returns>
        /// <remarks>It overloads the (%) opperator</remarks>
        public Matrix Outer(Vector v)
        {
            //constructs an array to hold the matrix
            int rows = this.vector.Length;
            int cols = v.vector.Length;
            double[] result = new double[rows * cols];

            //multiplies each pair of elements
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = j + (i * cols);
                    result[index] = vector[i] * v.vector[j];
                }
            }

            //generates the resultant matrix
            return new Matrix(rows, cols, result);
        }

        /// <summary>
        /// Computes the pairwise multiplicaiton of two vectors. This is not
        /// a standard vector operaiton, however it dose have certain aplicaitons.
        /// In particular, the pairwise multiple is equivlante to treating the
        /// vectors as if they were diagonal matricies.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The pairwise product of the two vectors</returns>
        public Vector MultPW(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //builds the resultant vector
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = this.vector[i] * v.vector[i];

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
            //used to store the result
            double output = 0.0;

            //computes the sum of the squares
            for (int i = 0; i < vector.Length; i++)
                output += this.vector[i] * this.vector[i];

            //returns the square root of the sum
            return Math.Sqrt(output);
        }

        /// <summary>
        /// Determins the euclidian distance between this vector and
        /// a diffrent, given vector. 
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The distance from the other vector</returns>
        /// <exception cref="ArgumentShapeException">If the vectors are 
        /// of differing length</exception>
        public double Dist(Vector other)
        {
            //checks that the vectors are the same length
            if (other.Length != this.Length) throw new ArgumentShapeException("other");

            double temp = 0.0;
            double output = 0.0;

            //computes the square of the distance
            for (int i = 0; i < vector.Length; i++)
            {
                temp = this.vector[i] - other.vector[i];
                output = output + (temp * temp);
            }

            //returns the acutal euclidian distance
            return Math.Sqrt(output);
        }

        /// <summary>
        /// Produces a new unit vector, pointing in the same direction as the
        /// original vector. This is done by dividing by the Norm, which is
        /// why it is sometimes refred to as normalising the vector.
        /// </summary>
        /// <returns>The corisponding unit vector</returns>
        public Vector Unit()
        {
            //uses the inverse magnitude to normalise
            double bottom = 1.0 / this.Norm();
            return this.Mult(bottom);
        }

        /// <summary>
        /// Computes the angle that is formed between the current vector 
        /// and another given vector. The output ranges between 0 and PI, 
        /// inclusive.
        /// </summary>
        /// <param name="other">The second vector</param>
        /// <returns>The angle between the two vectors</returns>
        public double Angle(Vector other)
        {
            //computes the angle using the dot product
            double a = this.Mult(other);
            double b = this.Norm() * other.Norm();
            return Math.Acos(a / b);
        }

        /// <summary>
        /// Computes the inverse of the current vector, by inverting each of the 
        /// vector's components. Note that this is not a standard vector operaiton, 
        /// and should not be thought of as a true inverse. However, it is useful 
        /// if the vector is being made to represent a diagonal matrix. 
        /// </summary>
        /// <returns>The conponentwise inverse of the vector</returns>
        public Vector Inv()
        {
            //creates a new vector to store the result
            Vector inv = new Vector(vector.Length);

            //inverts each of the elements
            for (int i = 0; i < vector.Length; i++)
                inv.vector[i] = 1.0 / this.vector[i];

            return inv;
        }

        /// <summary>
        /// Converts the vector to a probability distribution by dividing
        /// each term by the sum of absolute values. This is the same as treating
        /// the elemets as weights and then normalising the weights. It has the
        /// advantage that it preserves vectors that are already probability 
        /// distributions.
        /// </summary>
        /// <returns>The vector as a probablity distribution</returns>
        public Vector ToProb()
        {
            //stores the new vector and the sum of the terms
            Vector dist = new Vector(vector.Length);
            double sum = 0.0;

            //takes the absolute value and computes the sum
            for (int i = 0; i < vector.Length; i++)
            {
                dist.vector[i] = Math.Abs(vector[i]);
                sum += dist.vector[i];
            }

            //divides by the sum to normalise the vector
            for (int i = 0; i < vector.Length; i++)
                dist.vector[i] = dist.vector[i] / sum;

            return dist;
        }

        /// <summary>
        /// Converts the vector to a probablity distribution by taking the
        /// exponential of each term and dividing by their sum. This is similar
        /// to using the sigmoid function for a single value. It dose not
        /// preserve vectors that are already probability distributions.
        /// </summary>
        /// <returns>The vector as a probablity distribution</returns>
        public Vector Softmax()
        {
            //stores the new vector and the sum of the terms
            Vector dist = new Vector(vector.Length);
            double sum = 0.0;

            //takes the exponential and computes the sum
            for (int i = 0; i < vector.Length; i++)
            {
                dist.vector[i] = Math.Exp(vector[i]);
                sum += dist.vector[i];
            }

            //divides by the sum to normalise the vector
            for (int i = 0; i < vector.Length; i++)
                dist.vector[i] = dist.vector[i] / sum;

            return dist;
        }

        /// <summary>
        /// Computes the compoenent-wise minimum of the current vector and a
        /// given vector. That is, each element in the output vector is the
        /// minimum of the corsponding elements in the imput vectors.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The component-wise minimum of the two vectors</returns>
        /// <exception cref="ArgumentShapeException">If the vectors are 
        /// of differing length</exception>
        public Vector Min(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //computes the compoenetwise minimum
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = Math.Min(vector[i], v.vector[i]);

            return output;
        }

        /// <summary>
        /// Computes the compoenent-wise maximum of the current vector and a
        /// given vector. That is, each element in the output vector is the
        /// maximum of the corsponding elements in the imput vectors.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The component-wise maximum of the two vectors</returns>
        /// <exception cref="ArgumentShapeException">If the vectors are 
        /// of differing length</exception>
        public Vector Max(Vector v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //computes the compoenetwise maximum
            for (int i = 0; i < vector.Length; i++)
                output.vector[i] = Math.Max(vector[i], v.vector[i]);

            return output;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Opperator Overlodes...

        //refferences the Add(v) function
        public static Vector operator +(Vector v, Vector w)
        { return v.Add(w); }

        //references the Sub(v) function
        public static Vector operator -(Vector v, Vector w)
        { return v.Sub(w); }

        //references the Mult(v) function
        public static Double operator *(Vector v, Vector w)
        { return v.Mult(w); }

        //references the Mult(s) function
        public static Vector operator *(Vector v, Double s)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static Vector operator *(Double s, Vector v)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static Vector operator /(Vector v, Double s)
        { return v.Mult(1.0 / s); }

        //references the Outer(v) function
        public static Matrix operator %(Vector v, Vector w)
        { return v.Outer(w); }

        //refrences the Mult(-1) function
        public static Vector operator -(Vector v)
        { return v.Mult(-1.0); }

        //refrences the copy constructor
        public static Vector operator +(Vector v)
        { return new Vector(v); }

        #endregion /////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}
