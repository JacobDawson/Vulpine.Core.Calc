using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Matrices
{
    public abstract class Vector<T> : Euclidean<T, Double>, IEnumerable<Double>, IFormattable
        where T : Vector<T>
    {
        //NOTE: Consider adding Contatinaiton and Subvector methods
        //this would be helpfull for nural-nets that build vectors

        #region Class Deffinitions...

        ////containes the values for the vector
        //private double[] vector;


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
            sb.Append(this[0].ToString(format, provider));

            if (Length < 5)
            {
                for (int i = 1; i < Length; i++)
                {
                    sb.Append(", ");
                    sb.Append(this[i].ToString(format, provider));
                }
            }
            else
            {
                for (int i = 1; i < 3; i++)
                {
                    sb.Append(", ");
                    sb.Append(this[i].ToString(format, provider));
                }

                sb.Append(" ... ");
                double last = this[Length - 1];
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
            T other = obj as T;
            if (other == null) return false;
            if (other.Length != Length) return false;

            for (int i = 0; i < Length; i++)
            {
                //if (vector[i] != other.vector[i]) return false;
                double a = this.GetElement(i);
                double b = other.GetElement(i);

                if (a != b) return false;
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

            for (int i = 0; i < Length; i++)
            {
                unchecked
                {
                    //temp = vector[i].GetHashCode();
                    temp = GetElement(i).GetHashCode();
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
        public abstract int Length { get; }

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
        public abstract double GetElement(int index);

        /// <summary>
        /// Obtains the desired element in the vector. If the index
        /// is outside the range of the vector, it returns zero.
        /// </summary>
        /// <param name="index">Position of desired element</param>
        /// <returns>The desired element or zero</returns>
        public double GetExtended(int index)
        {
            if (index < 0 || index >= Length) return 0.0;
            else return GetElement(index);
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
            if (index < 0 || index >= Length) return def;
            else return GetElement(index);
        }

        /// <summary>
        /// Sets the desired element in the vector.
        /// </summary>
        /// <param name="index">Position of the element</param>
        /// <param name="value">Value to be set</param>
        /// <exception cref="ArgumentOutOfRangeException">If the index is
        /// outside the bounds of the vector</exception>
        public abstract void SetElement(int index, double value);

        /// <summary>
        /// Generates an enumeration of all the values in the vector.
        /// </summary>
        /// <returns>An enumeration of the vector</returns>
        public IEnumerator<Double> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return GetElement(i);
        }

        //protected abstract Vector<T> CreateNew();

        protected abstract T CreateNew();

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
        public T Add(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            T output = CreateNew();

            //builds the resultant vector
            for (int i = 0; i < Length; i++)
            {
                //output[i] = this[i] + v[i];
                double temp = GetElement(i);
                temp = temp + v.GetElement(i);
                output.SetElement(i, temp);
            }

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
        public T Sub(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            T output = CreateNew();

            //builds the resultant vector
            for (int i = 0; i < Length; i++)
            {
                //output[i] = this[i] - v[i];
                double temp = GetElement(i);
                temp = temp - v.GetElement(i);
                output.SetElement(i, temp);
            }
                
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
        public double Mult(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //used to store the result
            double output = 0.0;

            //calcualtes the sum of the product
            for (int i = 0; i < Length; i++)
            {
                //output += this[i] * v[i];
                double temp = GetElement(i);
                temp = temp * v.GetElement(i);
                output += temp;
            }

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
        public T Mult(double s)
        {
            //creates a new vector to store the result
            T output = CreateNew();

            //multiplies each element componentwise
            for (int i = 0; i < Length; i++)
            {
                //output[i] = this[i] * s;
                double temp = GetElement(i);
                output.SetElement(i, temp * s);
            }

            return output;
        }

        ///// <summary>
        ///// Computes the outer product of the curent vector and a second
        ///// given vector. This results in a matrix who's elements are the
        ///// pairwise products of the elements of each vector. Note that unlike
        ///// the inner product, the vectors can be of differing length.
        ///// </summary>
        ///// <param name="v">The second vector</param>
        ///// <returns>The outer product of the two vectors</returns>
        ///// <remarks>It overloads the (%) opperator</remarks>
        //public Matrix Outer(Vector v)
        //{
        //    //constructs an array to hold the matrix
        //    int rows = this.vector.Length;
        //    int cols = v.vector.Length;
        //    double[] result = new double[rows * cols];

        //    //multiplies each pair of elements
        //    for (int i = 0; i < rows; i++)
        //    {
        //        for (int j = 0; j < cols; j++)
        //        {
        //            int index = j + (i * cols);
        //            result[index] = vector[i] * v.vector[j];
        //        }
        //    }

        //    //generates the resultant matrix
        //    return new Matrix(rows, cols, result);
        //}

        /// <summary>
        /// Computes the pairwise multiplicaiton of two vectors. This is not
        /// a standard vector operaiton, however it dose have certain aplicaitons.
        /// In particular, the pairwise multiple is equivlante to treating the
        /// vectors as if they were diagonal matricies.
        /// </summary>
        /// <param name="v">The second vector</param>
        /// <returns>The pairwise product of the two vectors</returns>
        public T MultPW(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            T output = CreateNew();

            //builds the resultant vector
            for (int i = 0; i < Length; i++)
            {
                //output[i] = this[i] * v[i];
                double temp = GetElement(i);
                temp = temp * v.GetElement(i);
                output.SetElement(i, temp);
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
            //used to store the result
            double output = 0.0;

            //computes the sum of the squares
            for (int i = 0; i < Length; i++)
            {
                //output += this[i] * this[i];
                double temp = GetElement(i);
                output += temp * temp;
            }

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
        public double Dist(T other)
        {
            //checks that the vectors are the same length
            if (other.Length != this.Length) throw new ArgumentShapeException("other");

            double temp = 0.0;
            double output = 0.0;

            //computes the square of the distance
            for (int i = 0; i < Length; i++)
            {
                temp = GetElement(i);
                temp = temp - other.GetElement(i);
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
        public T Unit()
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
        public double Angle(T other)
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
        public T Inv()
        {
            //creates a new vector to store the result
            T inv = CreateNew();

            //inverts each of the elements
            for (int i = 0; i < Length; i++)
            {
                //inv[i] = 1.0 / this[i];
                double temp = GetElement(i);
                inv.SetElement(i, 1.0 / temp);
            }

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
        public T ToProb()
        {
            //stores the new vector and the sum of the terms
            T dist = CreateNew();
            double sum = 0.0;

            //takes the absolute value and computes the sum
            for (int i = 0; i < Length; i++)
            {
                //dist.vector[i] = Math.Abs(vector[i]);
                //sum += dist.vector[i];

                double temp = GetElement(i);
                temp = Math.Abs(temp);
                dist.SetElement(i, temp);
                sum += temp;
            }

            //divides by the sum to normalise the vector
            for (int i = 0; i < Length; i++)
            {
                //dist.vector[i] = dist.vector[i] / sum;

                double temp = dist.GetElement(i);
                dist.SetElement(i, temp / sum);
            }

            return dist;
        }

        /// <summary>
        /// Converts the vector to a probablity distribution by taking the
        /// exponential of each term and dividing by their sum. This is similar
        /// to using the sigmoid function for a single value. It dose not
        /// preserve vectors that are already probability distributions.
        /// </summary>
        /// <returns>The vector as a probablity distribution</returns>
        public T Softmax()
        {
            //stores the new vector and the sum of the terms
            T dist = CreateNew();
            double sum = 0.0;

            //takes the exponential and computes the sum
            for (int i = 0; i < Length; i++)
            {
                //dist.vector[i] = Math.Exp(vector[i]);
                //sum += dist.vector[i];

                double temp = GetElement(i);
                temp = Math.Exp(temp);
                dist.SetElement(i, temp);
                sum += temp;
            }

            //divides by the sum to normalise the vector
            for (int i = 0; i < Length; i++)
            {
                //dist.vector[i] = dist.vector[i] / sum;

                double temp = dist.GetElement(i);
                dist.SetElement(i, temp / sum);
            }

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
        public T Min(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            T output = CreateNew();

            //computes the compoenetwise minimum
            for (int i = 0; i < Length; i++)
            {
                //output[i] = Math.Min(this[i], v[i]);
                double temp = GetElement(i);
                temp = Math.Min(temp, v.GetElement(i));
                output.SetElement(i, temp);
            }

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
        public T Max(T v)
        {
            //checks that the vectors are the same length
            if (v.Length != this.Length) throw new ArgumentShapeException("v");

            //creates a new vector to store the result
            T output = CreateNew();

            //computes the compoenetwise maximum
            for (int i = 0; i < Length; i++)
            {
                //output[i] = Math.Max(this[i], v[i]);
                double temp = GetElement(i);
                temp = Math.Max(temp, v.GetElement(i));
                output.SetElement(i, temp);
            }

            return output;
        }

        #endregion /////////////////////////////////////////////////////////////

        #region Opperator Overlodes...

        ////refferences the Add(v) function
        //public static T operator +(T v, T w)
        //{ return v.Add(w); }

        ////references the Sub(v) function
        //public static T operator -(T v, T w)
        //{ return v.Sub(w); }

        ////references the Mult(v) function
        //public static Double operator *(T v, T w)
        //{ return v.Mult(w); }

        ////references the Mult(s) function
        //public static T operator *(T v, Double s)
        //{ return v.Mult(s); }

        ////references the Mult(s) function
        //public static T operator *(Double s, T v)
        //{ return v.Mult(s); }

        ////references the Mult(s) function
        //public static T operator /(T v, Double s)
        //{ return v.Mult(1.0 / s); }

        ////references the Outer(v) function
        //public static Matrix operator %(Vector v, Vector w)
        //{ return v.Outer(w); }

        ////refrences the Mult(-1) function
        //public static T operator -(T v)
        //{ return v.Mult(-1.0); }

        ////refrences the copy constructor
        //public static T operator +(T v)
        //{ return new Vector(v); }

        #endregion /////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }

    }
}
