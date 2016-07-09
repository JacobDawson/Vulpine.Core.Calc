using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;
using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;

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
    /// <remarks>Last Update: 2013-09-16</remarks>
    public class Vector : Euclidian<Vector, Double>, Cloneable<Vector>
    {
        #region Class Deffinitions...

        //containes the values for the vector
        private double[] vector;

        /// <summary>
        /// Constructs a new vector of given length. The values of
        /// the vector are initialy set to zero.
        /// </summary>
        /// <param name="length">The lenght of the vector</param>
        /// <exception cref="ArgRangeExcp">If the length of 
        /// the vector is less than one</exception>
        public Vector(int length)
        {
            //creats a vector with length atleast one
            ArgRangeExcp.Atleast("length", length, 1);
            vector = new double[length];

            //intiialises the vector to all zeroes
            for (int i = 0; i < length; i++) vector[i] = 0.0;
        }

        /// <summary>
        /// Constructs a new vector from a set of values.
        /// </summary>
        /// <param name="vals">The values of the vector</param>
        /// <exception cref="ArgRangeExcp">If the length of
        /// the vector is less than one</exception>
        public Vector(params double[] vals)
        {
            //checks for valid length
            ArgRangeExcp.Atleast("vals.Length", vals.Length, 1);

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
        /// Generates a deep copy of the current vector by invoking
        /// the corisponding copy constructor.
        /// </summary>
        /// <returns>A copy of the vector</returns>
        public Vector Clone()
        {
            //calls upon the copy constructor
            return new Vector(this);
        }

        /// <summary>
        /// Generates a string representation of the vector, displaying
        /// it's sise and spatial dimention.
        /// </summary>
        /// <returns>The vector as a string</returns>
        public override string ToString()
        {
            //reports the dimentions of the vector
            return String.Format("Vector[{0}]", vector.Length);
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
        /// Obtains the desired element in the vector.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element</returns>
        /// <exception cref="ArgRangeExcp">If the index is
        /// outside the bounds of the vector</exception>
        public double GetElement(int index)
        {
            ArgRangeExcp.Check("index", index, vector.Length - 1);
            return vector[index];
        }

        /// <summary>
        /// Sets the desired element in the vector.
        /// </summary>
        /// <param name="index">Position of the element</param>
        /// <param name="value">Value to be set</param>
        /// <exception cref="ArgRangeExcp">If the index is
        /// outside the bounds of the vector</exception>
        public void SetElement(int index, double value)
        {
            ArgRangeExcp.Check("index", index, vector.Length - 1);
            vector[index] = value;
        }

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is greator than the length of the vector, it returns zero.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element</returns>
        /// <exception cref="ArgRangeExcp">If the index is
        /// less than zero</exception>
        public double GetExtended(int index)
        {
            ArgRangeExcp.Atleast("index", index, 0);
            if (index >= vector.Length) return 0.0;
            else return vector[index];
        }

        /// <summary>
        /// Copies the vector into an array of floating points.
        /// </summary>
        /// <returns>The vector as an array</returns>
        public double[] ToArray()
        {
            //makes a deep copy inorder to protect the vector
            double[] copy = new double[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            copy[i] = vector[i];

            //returnes the newly copied array
            return copy;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Vector Operations...

        /// <summary>
        /// Generates a new vector that is the sum of the current vector 
        /// and a second vector. It overloads the (+) opperator.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The sum of the two vectors</returns>
        /// <exception cref="VectorLengthExcp">If the vectors are of
        /// differing length</exception>
        public Vector Add(Vector v)
        {
            //checks that the vectors are the same length
            VectorLengthExcp.Check(v.Length, this.Length);

            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //builds the resultant vector
            for (int i = 0; i < vector.Length; i++)
            output.vector[i] = this.vector[i] + v.vector[i];

            return output;
        }

        /// <summary>
        /// Generates a new vector that is the diffrence of the current
        /// vector and a second vector. It overloads the (-) opperator.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The diffrence of the two vectors</returns>
        /// <exception cref="VectorLengthExcp">If the vectors are of
        /// differing length</exception>
        public Vector Sub(Vector v)
        {
            //checks that the vectors are the same length
            VectorLengthExcp.Check(v.Length, this.Length);

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
        /// point scalar. It overloads the (*) opperator.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The dot procuct of the two vectors</returns>
        /// <exception cref="VectorLengthExcp">If the vectors are of
        /// differing length</exception>
        public double Mult(Vector v)
        {
            //checks that the vectors are the same length
            VectorLengthExcp.Check(v.Length, this.Length);

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
        /// by the same amount. It overloads the (*) and (/) opperators,
        /// respectivly.
        /// </summary>
        /// <param name="s">Value by which to scale the vector</param>
        /// <returns>The product of this vector and a scalar</returns>
        public Vector Mult(double s)
        {
            //creates a new vector to store the result
            Vector output = new Vector(vector.Length);

            //multiplies each element componentwise
            for (int i = 0; i < vector.Length; i++)
            output.vector[i] = this.vector[i] * s;

            return output;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Calculates the magnitude of the current vector. The magnitude
        /// is equivlent to the distance from the origin or zero vector.
        /// </summary>
        /// <returns>The magnitude of the vector</returns>
        public double Mag()
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
        /// <exception cref="VectorLengthExcp">If the vectors are 
        /// of differing length</exception>
        public double Dist(Vector other)
        {
            //checks that the vectors are the same length
            VectorLengthExcp.Check(other.Length, this.Length);

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

        #endregion /////////////////////////////////////////////////////////////

        #region Special Methods...

        /// <summary>
        /// Calculates the magnitude of the given vector. The magnitude
        /// is equivlent to the distance from the origin or zero vector.
        /// </summary>
        /// <param name="v">The vector in question</param>
        /// <returns>The magnitude of the vector</returns>
        public static double Mag(Vector v)
        {
            //the magnitude is the sum of the squares
            double temp = v.Mult(v);
            return Math.Sqrt(temp);
        }

        /// <summary>
        /// Produces a new normalised version of the curent vector, by
        /// seting it's magnitude to 1, preserving the vector's direction.
        /// It overloads the (~) opperator.
        /// </summary>
        /// <param name="v">The vector in question</param>
        /// <returns>The normalised vector</returns>
        public static Vector Norm(Vector v)
        {
            //uses the inverse magnitude to normalise
            double bottom = 1.0 / v.Mag();
            return v.Mult(bottom);     
        }

        /// <summary>
        /// Determins the eucildian distance between two vectors.
        /// </summary>
        /// <param name="v">The first vector</param>
        /// <param name="w">The second vector</param>
        /// <returns>The distance between the two vectors</returns>
        /// <exception cref="VectorLengthExcp">If the vectors are 
        /// of differing length</exception>
        public static double Dist(Vector v, Vector w)
        {
            //computes magnitude of the diffrence
            Vector temp = v.Sub(w);
            return Mag(temp);
        }

        /// <summary>
        /// Computes the angle that is formed between two vectors.
        /// The output ranges between 0 and PI, inclusive.
        /// </summary>
        /// <param name="v">The first vector</param>
        /// <param name="w">The second vector</param>
        /// <returns>The angle between the two vectors</returns>
        /// <exception cref="VectorLengthExcp">If the vectors are 
        /// of differing length</exception>
        public static double Angle(Vector v, Vector w)
        {
            //computes the angle using the dot product
            Vector nv = Norm(v);
            Vector nw = Norm(w);
            return Math.Acos(nv.Mult(nw));
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

        //refrences the Mult(-1) function
        public static Vector operator -(Vector v)
        { return v.Mult(-1.0); }

        //refrences the copy constructor
        public static Vector operator +(Vector v)
        { return new Vector(v); }

        //refrences the Norm(v) function
        public static Vector operator ~(Vector v)
        { return Vector.Norm(v); }

        #endregion /////////////////////////////////////////////////////////////

        object ICloneable.Clone()
        { return new Vector(this); }
    }
}
