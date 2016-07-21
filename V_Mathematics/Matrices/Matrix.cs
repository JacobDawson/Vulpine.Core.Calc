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
    /// <remarks>Last Update: 2013-11-16</remarks>
    public class Matrix : Algebraic<Matrix>, Euclidean<Matrix, Double>, 
        IEnumerable<Vector>, IFormattable
    {
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
        /// <returns>The matrix fomated as a string</returns>
        public string ToString(string format)
        {
            //calls upon the method below
            return ToString(format, null);
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

            int vpos = num_cols;
            Matrix aug = new Matrix(num_rows, vpos + 1);

            //bulids the augmentation matrix with the vector
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < vpos; j++)
                    aug[i, j] = this[i, j];
                aug[i, vpos] = b[i];
            }

            //generates the reduced echelon form
            aug.ToRowEchelon();

            //returns the resultent vector
            return aug.GetColumn(vpos);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Inline Opperators...

        /// <summary>
        /// Adds a matrix to the current matrix, overwriting the existing
        /// matrix. This method should only be called when the current matrix 
        /// will no longer be needed after adition.
        /// </summary>
        /// <param name="a">The matrix to add</param>
        /// <exception cref="ArgumentShapeException">If the matrices differ in
        /// either their number of rows or columns</exception>
        public void AddWith(Matrix a)
        {
            //makes shure the dimentions of the matrices match
            if (a.num_rows != num_rows || a.num_cols != num_cols)
                throw new ArgumentShapeException("a");

            //summs each element with the matching element
            for (int i = 0; i < matrix.Length; i++)
                matrix[i] = matrix[i] + a.matrix[i];
        }

        /// <summary>
        /// Adds a scalor value to the current matrix, overwriting the original
        /// matrix. This method should only be called when the current matrix 
        /// will no longer be needed after adition. See the standard Add method 
        /// for more details. 
        /// </summary>
        /// <param name="x">Scalar value to add to the matrix</param>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public void AddWith(double x)
        {
            //checks for non-square matricies
            if (num_rows != num_cols) throw new ArgumentShapeException();

            //used in computing the result
            int offset = num_rows + 1;

            //adds only the diagonal elements, copies all others
            for (int i = 0; i < matrix.Length; i++)
                if (i % offset == 0) matrix[i] += x;
        }

        /// <summary>
        /// DEPRICATED...
        /// </summary>
        public void MultBy(Matrix a)
        {
            //NOTE: This dose post multiplication, we require a funciton that
            //can do pre-multiplication, in other words A = B * A

            ////make shure the matricies can be multiplied
            //VectorLengthExcp.Check(a.num_rows, this.num_cols);
            //SquareMatrixExcp.Check(a.num_cols, a.num_rows);

            //stores the current row, as it's being edited
            double[] row = new double[num_cols];

            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    //copies the current element for later refrence
                    row[j] = GetElement(i, j);
                    double temp = 0.0;

                    //iterates over the interior product terms
                    for (int k = 0; k < num_cols; k++)
                    temp += row[k] * a.GetElement(k, i);

                    //sets the resulting element in the curent matrix
                    SetElement(i, j, temp);
                }
            }
        }

        /// <summary>
        /// Premultiplies the current matrix with the given matrix, overwriting the
        /// current matrix in the process. In other words, it computes A = B * A,
        /// where A is the curent matrix, and B is the given matrix.
        /// </summary>
        /// <param name="b">The given matrix</param>
        public void PreMult(Matrix b)
        {
            //stores the current row, as it's being edited
            double[] row = new double[num_cols];

            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                    row[j] = GetElement(i, j);

                for (int j = 0; j < num_cols; j++)
                {
                    ////copies the current element for later refrence
                    //row[j] = GetElement(i, j);
                    double temp = 0.0;

                    //iterates over the interior product terms
                    for (int k = 0; k < num_cols; k++)
                        temp += row[k] * b.GetElement(k, i);

                    //sets the resulting element in the curent matrix
                    SetElement(i, j, temp);
                }
            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Multiplies the curent matrix by a scalar, overwriting the original
        /// matrix in the process. This method should only be called when the 
        /// current matrix will no longer be needed after scaling.
        /// </summary>
        /// <param name="x">The scalar multiplier</param>
        public void MultBy(double x)
        {
            //multiplies each element by the scalar
            for (int i = 0; i < matrix.Length; i++)
                matrix[i] = matrix[i] * x;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Computes the magnitude as the Frobenius Norm of the current matrix.
        /// It is identical to the norm of the vectorised matrix, and is
        /// sub-multiplicitive with respect to the matrix product. 
        /// </summary>
        /// <returns>The magnitude of the matrix</returns>
        public double Mag()
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

            //makes a copy of the matrix in row-echelon form
            Matrix temp = new Matrix(this);
            double s = temp.ToRowEchelon();
            double det = 1.0;

            //computes the product of the diagonal elements
            for (int i = 0; i < num_rows; i++)
                det = det * temp.GetElement(i, i);

            //returns the corrected determinate
            return s * det;
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

            int vpos = a.num_cols;
            Matrix aug = new Matrix(a.num_rows, vpos * 2);

            //bulids the augmentation matrix with the identity
            for (int i = 0; i < a.num_rows; i++)
            {
                for (int j = 0; j < vpos; j++)
                    aug[i, j] = a.GetElement(i, j);

                for (int j = vpos; j < vpos * 2; j++)
                {
                    if (j == i) aug[i, j] = 1.0;
                    else aug[i, j] = 0.0;
                }
            }

            //generates the reduced echelon form
            aug.ToRowEchelon();

            //extracts the resulting inverse matrix
            Matrix result = new Matrix(a.num_rows, a.num_cols);
            for (int i = 0; i < a.num_rows; i++)
            {
                for (int j = 0; j < a.num_cols; j++)
                    result[i, j] = aug[i, vpos + j];
            }

            //returns the resultent vector
            return result;
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
        /// Swaps two rows in the current matrix, without having to
        /// store either of the rows being swaped as vectors.
        /// </summary>
        /// <param name="row1">First row to swap</param>
        /// <param name="row2">Second row to swap</param>
        private void SwapRows(int row1, int row2)
        {
            for (int i = 0; i < num_cols; i++)
            {
                double temp = this[row1, i];
                this[row1, i] = this[row2, i];
                this[row2, i] = temp;
            }
        }

        /// <summary>
        /// Reduces the current matrix to reduced row-echelon form. It dose
        /// this in-place, so the matrix must be copyed if the original matrix
        /// is to be preserved. The value returned is the modification to the
        /// determinate induced by the reduction.
        /// </summary>
        /// <returns>The coffecent to the determinate</returns>
        private double ToRowEchelon()
        {
            double v, maxv;
            int maxi;

            double s = 1.0;
            int i = 0;
            int j = 0;

            while (i < num_rows && j < num_cols)
            {
                maxi = i;
                v = maxv = 0.0;

                //findes the piviot index and max value
                for (int k = i; k < num_rows; k++)
                {
                    v = GetElement(k, j);
                    v = Math.Abs(v);
                    if (v > maxv) maxi = k;
                    if (v > maxv) maxv = v;
                }

                //if the row is escentialy zero, we skip it
                if (maxv == 0.0) { j++; continue; }

                //swaps the current row with the piviot
                if (maxi != i)
                {
                    SwapRows(maxi, i);
                    s = s * -1.0;
                }

                //re-obtains the maximum element
                maxv = GetElement(i, j);
                s = s / maxv;

                //normalises the pivot row
                for (int u = j; u < num_cols; u++)
                this[i, u] = this[i, u] / maxv;

                //eliminates the current column
                for (int k = 0; k < num_rows; k++)
                {
                    if (k == i) continue;
                    double temp = this[k, j];

                    for (int u = 0; u < num_cols; u++)
                    {
                        double x = this[i, u] * temp;
                        this[k, u] = this[k, u] - x;
                    }
                }

                //increments counters
                i++; j++;
            }

            return s;
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
            return (v => m.Mult(v));
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
