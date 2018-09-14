/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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
    /// Primarily, a matrix is a two-dimentional array of real numbers, but it is
    /// more than just an extention of vectors to two dimentions. Matrices form a
    /// non-comunitive ring, defining adition, subtraction, and multiplication.
    /// Square matrices can be inverted, though this is usualy costly and counter 
    /// productive. In adition, any matrix can be used to transform a vector by 
    /// multiplication. In this way it's possable to view a matrix as a liniar
    /// function that maps one set of vectors into another.
    /// </summary>
    /// <remarks>Last Update: 2016-07-21</remarks>
    public sealed class Matrix : Algebraic<Matrix, Double>, IEnumerable<Vector>, IFormattable
    {
        //IDEA: We could add methods to generate the characteristic polynomial,
        //from wich we can compute the eigenvalues and eigenvectors.

        #region Class Definitions...

        //contains the values for the matrix
        private double[] matrix;

        //stores the size of the matrix
        private int num_rows;
        private int num_cols;

        /// <summary>
        /// Constructs an empty m x n matrix where all the entrys are initialsied
        /// to zero. The matrix can then be built dynamicaly.
        /// </summary>
        /// <param name="rows">Number of rows in the matrix</param>
        /// <param name="cols">Number of columns in the matrix</param>
        public Matrix(int rows, int cols)
        {
            //checks that the size of the matrix is valid
            num_rows = (rows < 1) ? 1 : rows;
            num_cols = (cols < 1) ? 1 : cols;

            //creates matrix initialised to zero
            matrix = new double[rows * cols];
            for (int i = 0; i < matrix.Length; i++)
            matrix[i] = 0.0;
        }

        /// <summary>
        /// Constructs a new m x n matrix initialised with the given values.
        /// The values are read row-first, and must match the dimentions given.
        /// </summary>
        /// <param name="rows">Number of rows in the matrix</param>
        /// <param name="cols">Number of columns in the matrix</param>
        /// <param name="vals">The values to initialise the matrix</param>
        /// <exception cref="ArgumentShapeException">If the data given dose not 
        /// match the spesified number of rows and colums</exception>
        public Matrix(int rows, int cols, params double[] vals)
        {
            //checks that the size of the matrix is valid
            num_rows = (rows < 1) ? 1 : rows;
            num_cols = (cols < 1) ? 1 : cols;

            //checks that the values match the dimentions
            if (vals.Length < rows * cols) throw new ArgumentShapeException("vals");

            //creates matrix using the given values
            matrix = new double[rows * cols];
            for (int i = 0; i < matrix.Length; i++)
            matrix[i] = vals[i];
        }

        /// <summary>
        /// Copy constructor, used to create identical copies of a
        /// given matrix inorder to protect the original.
        /// </summary>
        /// <param name="other">The matrix to be copyed</param>
        public Matrix(Matrix other)
        {
            //takes the dimentions of the other matrix
            num_rows = other.num_rows;
            num_cols = other.num_cols;

            //copies the values from one array to another
            matrix = new double[num_rows * num_cols];
            for (int i = 0; i < matrix.Length; i++)
            matrix[i] = other.matrix[i];
        }

        /// <summary>
        /// Generates a string representation of the matrix, using the default
        /// formating for floating point values. It shows the complete matrix 
        /// for matricies that are less than 4x4. For larger matricies, 
        /// it only reports the dimentions of the matrix.
        /// </summary>
        /// <returns>The matrix as a string</returns>
        public override string ToString()
        {
            //calls upon the method below
            return ToString("g5", null);
        }

        /// <summary>
        /// Generates a formated string representation of the matrix. It shows the 
        /// complete matrix for matricies that are less than 4x4. For larger matricies, 
        /// it only reports the dimentions of the matrix.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The matrix fomated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (num_rows < 5 && num_cols < 5)
            {
                double cell = 0.0;
                StringBuilder sb = new StringBuilder();
                sb.Append("[");

                //itterates over the rows
                for (int i = 0; i < num_rows; i++)
                {
                    //appends the first ellement in the row
                    sb.Append((i == 0) ? "[" : ", [");
                    cell = GetElement(i, 0);
                    sb.Append(cell.ToString(format, provider));

                    //appends each subsequent element in the row
                    for (int j = 1; j < num_cols; j++)
                    {
                        sb.Append(", ");
                        cell = GetElement(i, j);
                        sb.Append(cell.ToString(format, provider));
                    }

                    sb.Append("]");
                }

                sb.Append("]");
                return sb.ToString();
            }
            else
            {
                //only reports the dimentions of the matrix
                return String.Format("Matrix: {0}x{1}", num_rows, num_cols);
            }
        }

        /// <summary>
        /// Determins if this matrix is equal to another matrix. Two matricies 
        /// are considered equal if they have the same shape and the values of
        /// their elements are equal. Note that if one of the matricies changes, 
        /// then the two matricies will no longer be equal.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            Matrix other = obj as Matrix;
            if (other == null) return false;

            if (other.num_cols != num_cols) return false;
            if (other.num_rows != num_rows) return false;

            for (int i = 0; i < matrix.Length; i++)
                if (matrix[i] != other.matrix[i]) return false;

            return true;
        }

        /// <summary>
        /// Generates a sudo-unique hash code for the current matrix, based on the 
        /// elements of the matrix. Note that if the value of the elements should 
        /// change, it's hash code will change acordingly.
        /// </summary>
        /// <returns>The hash of the current matrix</returns>
        public override int GetHashCode()
        {
            int hash = 1764;
            int temp = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                temp = matrix[i].GetHashCode();
                hash ^= (hash << 5) + (hash >> 2) + temp;
            }

            return hash;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Represents the number of rows in the matrix, or the length
        /// of a single column.
        /// </summary>
        public int NumRows
        {
            get { return num_rows; }
        }

        /// <summary>
        /// Represents the number of columns in the matrix, or the length
        /// of a single row.
        /// </summary>
        public int NumColumns
        {
            get { return num_cols; }
        }

        /// <summary>
        /// Represents the total number of cells in the matrix.
        /// </summary>
        public int NumCells
        {
            get { return num_rows * num_cols; }
        }

        /// <summary>
        /// Acceses the values of the matrix by row and column. See the
        /// SetElement() and GetElement() methods for more details.
        /// </summary>
        /// <param name="row">The row of the desired element</param>
        /// <param name="col">The column of the desired element</param>
        /// <returns>The desired element</returns>
        public double this[int row, int col]
        {
            get { return GetElement(row, col); }
            set { SetElement(row, col, value); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Data Accessors...

        /// <summary>
        /// Obtaines the matrix element at the given row and column.
        /// </summary>
        /// <param name="row">The row of the desired element</param>
        /// <param name="col">The column of the desired element</param>
        /// <returns>The desired element within the matrix</returns>
        /// <exception cref="ArgumentOutOfRangeException">If either the row or
        /// the column numbers lie outside the matrix</exception>
        public double GetElement(int row, int col)
        {
            //checks that the row and column are valid
            if (row < 0 || row >= num_rows)
                throw new ArgumentOutOfRangeException("row");
            if (col < 0 || col >= num_cols)
                throw new ArgumentOutOfRangeException("col");

            //obtains the desired cell value
            int index = col + (row * num_cols);
            return matrix[index];
        }

        /// <summary>
        /// Sets the value at the given row and column in the matrix.
        /// </summary>
        /// <param name="row">The row of the desired element</param>
        /// <param name="col">The column of the desired element</param>
        /// <param name="value">The new value of the element</param>
        /// <exception cref="ArgumentOutOfRangeException">If either the row or
        /// the column numbers lie outside the matrix</exception>
        public void SetElement(int row, int col, double value)
        {
            //checks that the row and column are valid
            if (row < 0 || row >= num_rows)
                throw new ArgumentOutOfRangeException("row");
            if (col < 0 || col >= num_cols)
                throw new ArgumentOutOfRangeException("col");

            //obtains the desired cell value
            int index = col + (row * num_cols);
            matrix[index] = value;
        }

        /// <summary>
        /// Obtains a single row from the current matrix, and
        /// returns it as a one-dimentional vector.
        /// </summary>
        /// <param name="row">The index of the desire row</param>
        /// <returns>The requested row as a vector</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the row number
        /// lies outside of the matrix</exception>
        public Vector GetRow(int row)
        {
            //checks that the row number is valid
            if (row < 0 || row >= num_rows)
                throw new ArgumentOutOfRangeException("row");

            //obtains the values in the given row
            double[] values = new double[num_cols];
            for (int i = 0; i < num_cols; i++)
                values[i] = GetElement(row, i);

            //returns the row vector
            return new Vector(values);
        }

        /// <summary>
        /// Obtains a single column from the current matrix, and
        /// returns it as a one-dimentional vector.
        /// </summary>
        /// <param name="col">The index of the desire column</param>
        /// <returns>The requested column as a vector</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the column number
        /// lies outside of the matrix</exception>
        public Vector GetColumn(int col)
        {
            //checks that the column number is valid
            if (col < 0 || col >= num_cols)
                throw new ArgumentOutOfRangeException("col");

            //obtains the values in the given column
            double[] values = new double[num_rows];
            for (int i = 0; i < num_rows; i++)
                values[i] = GetElement(i, col);

            //returns the column vector
            return new Vector(values);
        }

        /// <summary>
        /// Sets the indicated row in the current matrix.
        /// </summary>
        /// <param name="row">The index of the row</param>
        /// <param name="values">A vector to replace the indicated row</param>
        /// <exception cref="ArgumentShapeException">If the length of the vector
        /// dose not match the length of a row</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the row index lies outside
        /// the bounds of the matrix</exception>
        public void SetRow(int row, Vector values)
        {
            //makes shure the input vector is the correct shape
            if (row < 0 || row >= num_rows)
                throw new ArgumentOutOfRangeException("row");
            if (values.Length < num_cols)
                throw new ArgumentShapeException("values");

            //copies the row values into the matrix
            for (int i = 0; i < num_cols; i++)
            {
                double temp = values[i];
                SetElement(row, i, temp);
            }
        }

        /// <summary>
        /// Sets the indicated column in the current matrix.
        /// </summary>
        /// <param name="col">The index of the column</param>
        /// <param name="values">A vector to replace the indicated column</param>
        /// <exception cref="ArgumentShapeException">If the length of the vector
        /// dose not match the length of a column</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the column index lies outside
        /// the bounds of the matrix</exception>
        public void SetColumn(int col, Vector values)
        {
            //makes shure the input vector is the correct shape
            if (col < 0 || col >= num_cols)
                throw new ArgumentOutOfRangeException("col");
            if (values.Length < num_rows)
                throw new ArgumentShapeException("values");

            //copies the column values into the matrix
            for (int i = 0; i < num_rows; i++)
            {
                double temp = values[i];
                SetElement(i, col, temp);
            }
        }

        /// <summary>
        /// Treats the matrix as a collection of row vectors by itterating over
        /// the matrix one row at a time.
        /// </summary>
        /// <returns>An enumeration of the rows in the matrix</returns>
        public IEnumerator<Vector> GetEnumerator()
        {
            //used to store the values to build each row
            double[] temp = new double[num_cols];

            //itterates over each row in the matrix
            for (int i = 0; i < num_rows; i++)
            {
                //iterates over each element in the current row
                for (int j = 0; j < num_cols; j++)
                    temp[j] = GetElement(i, j);

                yield return new Vector(temp);
            }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Matrix Operations...

        /// <summary>
        /// Generates a new matrix that is the sum of the current
        /// matrix and a second matrix. It overlads the (+) opperator.
        /// </summary>
        /// <param name="a">The second matrix</param>
        /// <returns>The sum of the two matrices</returns>
        /// <exception cref="ArgumentShapeException">If the matrices differ in
        /// either their number of rows or columns</exception>
        public Matrix Add(Matrix a)
        {
            //makes shure the dimentions of the matrices match
            if (a.num_rows != num_rows || a.num_cols != num_cols)
                throw new ArgumentShapeException("a");

            //summs each element with the matching element
            double[] result = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
                result[i] = matrix[i] + a.matrix[i];

            //generates the resultant matrix
            return new Matrix(num_rows, num_cols, result);
        }

        /// <summary>
        /// Adds a scalor value to a square matrix. Scalor adition is
        /// defined such that (A + x) is equevenent to (A + Ix) where
        /// (I) is the identity and scalar multiplication is well defined.
        /// It overloads the (+) opperator.
        /// </summary>
        /// <param name="x">Scalar value to add to the matrix</param>
        /// <returns>The result of adding a scalor to the matrix</returns>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public Matrix Add(double x)
        {
            //checks for non-square matricies
            if (num_rows != num_cols) throw new ArgumentShapeException();

            //used in computing the result
            double[] result = new double[matrix.Length];
            int offset = num_rows + 1;

            //adds only the diagonal elements, copies all others
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i % offset != 0) result[i] = matrix[i];
                else result[i] = matrix[i] + x;
            }

            //generates the resultant matrix
            return new Matrix(num_rows, num_cols, result);
        }

        /// <summary>
        /// Generates a new matrix that is the diffrence of the current
        /// matrix and a second matrix. It overloads the (-) operator.
        /// </summary>
        /// <param name="a">The second matrix</param>
        /// <returns>The difrence of the two matrices</returns>
        /// <exception cref="ArgumentShapeException">If the matrices differ in
        /// either their number of rows or columns</exception>
        public Matrix Sub(Matrix a)
        {
            //makes shure the dimentions of the matrices match
            if (a.num_rows != num_rows || a.num_cols != num_cols)
                throw new ArgumentShapeException("a");

            //subtracts each element from the matching element
            double[] result = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
                result[i] = matrix[i] - a.matrix[i];

            //generates the resultant matrix
            return new Matrix(num_rows, num_cols, result);
        }

        /// <summary>
        /// Subtracts a scalor value from a matrix, using the same
        /// definition as scalor adition. It overloads the (-) operator.
        /// </summary>
        /// <param name="x">A scalar value to subtract from the matrix</param>
        /// <returns>The result of subtracting a scalor from the matrix</returns>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public Matrix Sub(double x)
        {
            //uses the additive inverse
            return this.Add(-x);
        }

        /// <summary>
        /// Generates a new matrix that is the product of the current matrix
        /// and a second matrix. Matrices can only be multiplied if the number
        /// of columns in the first matrix matches the number of rows in the
        /// second matrix. Because of this, the multiplication of matrices is
        /// not comunitive. It overloads the (*) operator.
        /// </summary>
        /// <param name="a">The second matrix</param>
        /// <returns>The product of the two matrices</returns>
        /// <exception cref="ArgumentShapeException">If the number of columns in the
        /// first matrix dose not match the number of rows in the second</exception>
        public Matrix Mult(Matrix a)
        {
            //make shure the matricies can be multiplied
            if (a.num_rows != num_cols) throw new ArgumentShapeException("a");

            int rows = num_rows;
            int cols = a.num_cols;
            double[] result = new double[rows * cols];

            //calculates each element in the new matrix
            for (int i = 0; i < result.Length; i++)
            {
                for (int k = 0; k < num_cols; k++)
                {
                    double temp = this[i / cols, k];
                    temp = temp * a[k, i % cols];
                    result[i] = result[i] + temp;
                }
            }

            //generates the resultant matrix
            return new Matrix(rows, cols, result);
        }

        /// <summary>
        /// Multiplies the curent matrix by a column vector, producing
        /// a new vector. This is usefull for transforming vectors, as the
        /// whole matrix can be seen as a function from one vector space to
        /// another. It overloads the (*) operator.
        /// </summary>
        /// <param name="v">The vector to be transformed</param>
        /// <returns>A new vector that is the product of the curent matrix 
        /// and the previous vector</returns>
        /// <exception cref="ArgumentShapeException">If the length of the vector 
        /// dose not match the number of columns in the matrix</exception>
        public Vector Mult(Vector v)
        {
            //make shure the matricies can be multiplied
            if (v.Length != num_cols) throw new ArgumentShapeException("v");

            double[] result = new double[num_rows];

            //calculates each element in the new matrix
            for (int i = 0; i < result.Length; i++)
            {
                for (int k = 0; k < num_cols; k++)
                {
                    double temp = this[i, k];
                    temp = temp * v[k];
                    result[i] = result[i] + temp;
                }
            }

            //generates the resultant vector
            return new Vector(result);
        }

        /// <summary>
        /// Generates a new matrix that is a scalar multiple of the
        /// current matrix. It overloads the (*) and (/) opperators,
        /// respectivly.
        /// </summary>
        /// <param name="x">Value by which to scale the matrix</param>
        /// <returns>The product of this matrix and a scalar</returns>
        public Matrix Mult(double x)
        {
            //multiplies each element by the scalar
            double[] result = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
                result[i] = matrix[i] * x;

            //generates the resultant matrix
            return new Matrix(num_rows, num_cols, result);
        }

        /// <summary>
        /// Atempts to solve the equation (Ax = b) for some unknown vector
        /// (x) given the curent matrix (A), by-passing the need to compute
        /// the matrix inverse directly. It overloads the (/) opperator.
        /// </summary>
        /// <param name="b">The vector (b) in the equaiton</param>
        /// <returns>The solution (x) to the equation</returns>
        /// <exception cref="SquareMatrixExcp">If the number of rows
        /// dose not match the number of columns</exception>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public Vector InvAx(Vector b)
        {
            //checks for non-square matricies
            if (num_rows != num_cols) throw new ArgumentShapeException();

            //obtains the LU decomposition
            Matrix upper, lower;
            Decomp(out upper, out lower);

            double[] x = new double[b.Length];
            double[] y = new double[b.Length];

            //Use Forward Propagation to sovle Ly = b
            for (int i = 0; i < b.Length; i++)
            {
                y[i] = b.GetElement(i);
                for (int j = 0; j < i; j++)
                    y[i] = y[i] - lower.GetElement(i, j) * y[j];
                y[i] = y[i] / lower.GetElement(i, i);

            }

            //Use Backward Propagation to solve Ux = y
            for (int i = b.Length - 1; i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = i + 1; j < b.Length; j++)
                    x[i] = x[i] - upper.GetElement(i, j) * x[j];
                x[i] = x[i] / upper.GetElement(i, i);
            }

            return new Vector(x);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Computes the Frobenius Norm of the current matrix. It is identical 
        /// to the Norm of the vectorised matrix, and is sub-multiplicitive 
        /// with respect to the matrix product. 
        /// </summary>
        /// <returns>The magnitude of the matrix</returns>
        public double Norm()
        {
            //used to store the result
            double output = 0.0;

            //computes the sum of the squares
            for (int i = 0; i < matrix.Length; i++)
                output += matrix[i] * matrix[i];

            //returns the square root of the sum
            return Math.Sqrt(output);
        }
        
        /// <summary>
        /// Computes the single-value diffrence between the current matrix 
        /// and another matrix using the Frobenius Norm. It is only defined
        /// for matrices that share the same dimentions.
        /// </summary>
        /// <param name="a">The other matrix</param>
        /// <returns>The distance between the matrices</returns>
        /// <exception cref="ArgumentShapeException">If the matrices differ in
        /// either their number of rows or columns</exception>
        public double Dist(Matrix a)
        {
            //makes shure the dimentions of the matrices match
            if (a.num_rows != num_rows || a.num_cols != num_cols)
                throw new ArgumentShapeException("a");

            double temp = 0.0;
            double output = 0.0;

            //computes the square of the distance
            for (int i = 0; i < matrix.Length; i++)
            {
                temp = this.matrix[i] - a.matrix[i];
                output = output + (temp * temp);
            }

            //returns the acutal euclidian distance
            return Math.Sqrt(output);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Special Methods...

        /// <summary>
        /// Generates the transpose of the current matrix, sutch that the
        /// row and column position of each element is swaped. Thus an
        /// m x n matrix becomes an n x m matrix.
        /// </summary>
        /// <returns>The transpose of the current matrix</returns>
        public Matrix Trans()
        {
            //sets up resluting matrix
            Matrix result = new Matrix(num_cols, num_rows);

            //copies the elments in transpose
            for (int i = 0; i < num_cols; i++)
            {
                for (int j = 0; j < num_rows; j++)
                {
                    double temp = GetElement(j, i);
                    result.SetElement(i, j, temp);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the determinate of a square matrix, relaying important
        /// information about the linear transformation it represents.
        /// </summary>
        /// <returns>The determinate of the matrix</returns>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public double Det()
        {
            //checks for non-square matricies
            if (num_rows != num_cols) throw new ArgumentShapeException();

            //obtains the LU decomposition
            Matrix upper, lower;
            Decomp(out upper, out lower);

            //computes the product of the upper diagonal
            double udet = 1.0;
            for (int i = 0; i < num_rows; i++)
                udet = udet * upper.GetElement(i, i);

            //computes the product of the lower diagonal
            double ldet = 1.0;
            for (int i = 0; i < num_rows; i++)
                ldet = ldet * lower.GetElement(i, i);

            //returnst the product of determinats
            return ldet * udet;
        }

        /// <summary>
        /// Computes the trace of a square matrix. It is equevlent to the sum of 
        /// the eigenvalues, and corisponds with the dirivitive of the determinate.
        /// </summary>
        /// <returns>The trace of the matrix</returns>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public double Trace()
        {
            //checks for non-square matricies
            if (num_rows != num_cols) throw new ArgumentShapeException();

            //computes the sum of the diagonal entries
            double trace = 0.0;
            for (int i = 0; i < num_rows; i++)
                trace = trace + GetElement(i, i);

            return trace;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Static Methods...

        /// <summary>
        /// Computes the inverse of a given square matrix. The inverse is
        /// defined as the matrix who's product with the original matrix
        /// yeilds the idenity matrix. For solving equations like (Ax = b)
        /// where (a) and (b) are vectors, the method InvAx should be 
        /// used instead.
        /// </summary>
        /// <param name="a">The matrix to invert</param>
        /// <returns>The inverse of the given matrix</returns>
        /// <exception cref="ArgumentShapeException">If the matrix given
        /// is not a square matrix</exception>
        public static Matrix Invert(Matrix a)
        {
            //checks for non-square matricies
            if (a.num_rows != a.num_cols) throw new ArgumentShapeException("a");

            //feteches the determinant
            double det = a.Det();

            //sets up the intermediate C matrix
            Matrix c = new Matrix(a.num_rows, a.num_cols);

            for (int i = 0; i < a.num_rows; i++)
            {
                for (int j = 0; j < a.num_cols; j++)
                {
                    double temp = a.Shrink(i, j);
                    if ((i + j) % 2 == 1) temp = -temp;
                    c.SetElement(i, j, temp);
                }
            }

            //computes the final matrix and returns    
            Matrix inverse = c.Mult(1 / det);
            return inverse.Trans();
        }

        /// <summary>
        /// Generates the identity matrix for a given size. The idenity is
        /// defined as being the matrix who's product with any other matrix
        /// yeilds itself. Thus, the idenity matrix is always square.
        /// </summary>
        /// <param name="size">The size of the (n x n) matrix</param>
        /// <returns>The idinity matrix for the requested size</returns>
        public static Matrix Ident(int size)
        {
            //checks for a size of atleast one
            if (size < 1) size = 1;

            double[] vals = new double[size * size];
            int test = size + 1;

            //places ones along the diagonal
            for (int i = 0; i < vals.Length; i++)
            {
                if (i % test == 0) vals[i] = 1.0;
                else vals[i] = 0.0;
            }

            //generates the identity matrix
            return new Matrix(size, size, vals);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Every invertable matrix can be decomposed into an upper
        /// and lower triangular matrix, whose product is the original
        /// matrix. Doing this first can greatly improve the running times
        /// of certain caluclations, like the determinate, which would
        /// otherwise be exponential.
        /// </summary>
        /// <param name="up">Is set to the upper triangular matrix</param>
        /// <param name="low">Is set to the lower triangular matrix</param>
        private void Decomp(out Matrix up, out Matrix low)
        {
            //copys the matrix so we don't mutate the original
            Matrix a = new Matrix(this);
            double val = 0.0;

            //initialises the two output matrices to zero
            up = new Matrix(num_rows, num_cols);
            low = new Matrix(num_rows, num_cols);

            //forms the L and U decomposition, updating A as it goes
            for (int k = 0; k < num_rows; k++)
            {
                //sets the U elements
                for (int j = k; j < num_rows; j++)
                {
                    val = a.GetElement(k, j);
                    up.SetElement(k, j, val);
                }

                //sets the L elements
                low.SetElement(k, k, 1);
                for (int i = k + 1; i < num_rows; i++)
                {
                    val = a.GetElement(i, k);
                    val = val / up.GetElement(k, k);
                    low.SetElement(i, k, val);
                }

                //updates the A elements
                for (int i = k + 1; i < num_rows; i++)
                {
                    for (int j = k + 1; j < num_rows; j++)
                    {
                        val = low.GetElement(i, k);
                        val = up.GetElement(k, j) * val;
                        val = a.GetElement(i, j) - val;
                        a.SetElement(i, j, val);
                    }
                }
            }
        }

        /// <summary>
        /// Helper method, shrinks the matrix by removing the indicated
        /// row and column, and then returns the determinate of the reduced
        /// matrix.
        /// </summary>
        /// <param name="row">Row to be removed</param>
        /// <param name="col">Column to be removed</param>
        /// <returns>the determinate of the reduced matrix</returns>
        private double Shrink(int row, int col)
        {
            if (num_rows <= 2)
            {
                //special case for size two
                int r = (row + 1) % 2;
                int c = (col + 1) % 2;
                return GetElement(r, c);
            }

            //sets up resluting matrix
            int rows = num_rows - 1;
            int cols = num_cols - 1;
            Matrix result = new Matrix(rows, cols);

            //used in itterations
            int x = 0;
            int y = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    //takes care of deleting the row
                    if (i < row) x = i;
                    else x = i + 1;

                    //takes care of deleting the col
                    if (j < col) y = j;
                    else y = j + 1;

                    //copies the desired values
                    double temp = GetElement(x, y);
                    result.SetElement(i, j, temp);
                }
            }

            //returns the determinat
            return result.Det();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Converts the curent matrix to a vector by leanerising the matrix.
        /// This process destroys informaiton about the shape of the matrix,
        /// which is why the conversion is explicit. To reverse the process
        /// you would need to call the constructor with the width and height
        /// of the original matrix.
        /// </summary>
        public static explicit operator Vector(Matrix m)
        {
            //vectorises the matrix
            return new Vector(m.matrix);
        }

        /// <summary>
        /// Treates the matrix as a funciton of vectors by multiplying column
        /// vectors on the right of the matrix. Note that the size of the input
        /// need not match the size of the output, unless the matrix is square.
        /// </summary>
        public static implicit operator VFunc<Vector>(Matrix m)
        {
            //treates the matrix as a function of vectors
            return v => m.Mult(v);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Operator Overloads...

        //refferences the Add(m) function
        public static Matrix operator +(Matrix a, Matrix b)
        { return a.Add(b); }

        //refferences the Add(s) function
        public static Matrix operator +(Matrix a, Double x)
        { return a.Add(x); }

        //references the Sub(m) function
        public static Matrix operator -(Matrix a, Matrix b)
        { return a.Sub(b); }

        //refferences the Sub(s) function
        public static Matrix operator -(Matrix a, Double x)
        { return a.Sub(x); }

        //references the Mult(m) function
        public static Matrix operator *(Matrix a, Matrix b)
        { return a.Mult(b); }

        //references the Mult(v) function
        public static Vector operator *(Matrix a, Vector v)
        { return a.Mult(v); }

        //references the Mult(s) function
        public static Matrix operator *(Matrix a, Double x)
        { return a.Mult(x); }

        //references the InvAx() function
        public static Vector operator /(Vector b, Matrix a)
        { return a.InvAx(b); }

        //references the Mult(s) function
        public static Matrix operator /(Matrix a, Double x)
        { return a.Mult(1.0 / x); }

        //refrences the Mult(-1) function
        public static Matrix operator -(Matrix a)
        { return a.Mult(-1.0); }

        //refrences the copy constructor
        public static Matrix operator +(Matrix a)
        { return new Matrix(a); }

        #endregion //////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}
