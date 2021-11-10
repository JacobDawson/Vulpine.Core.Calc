using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;

namespace Vulpine.Core.Calc.Matrices
{
    public sealed class MatrixCmplx
    {
        //uses two real matrices to store the real and imaginary parts
        private Matrix real;
        private Matrix imag;

        public MatrixCmplx(int rows, int cols)
        {
            real = new Matrix(rows, cols);
            imag = new Matrix(rows, cols);
        }

        public MatrixCmplx(int rows, int cols, params Cmplx[] vals)
        {
            //checks that the values match the dimentions
            if (vals.Length < rows * cols) throw new ArgumentShapeException("vals");

            //spits the complex numbers into real and imaginary parts
            double[] rv = new double[vals.Length];
            double[] iv = new double[vals.Length];

            for (int k = 0; k < vals.Length; k++)
            {
                rv[k] = vals[k].CofR;
                iv[k] = vals[k].CofI;
            }

            //creates the matrix pair with the given values
            real = new Matrix(rows, cols, rv);
            imag = new Matrix(rows, cols, iv);
        }

        public MatrixCmplx(MatrixCmplx other)
        {
            //simply copies the component matricies
            real = new Matrix(other.real);
            imag = new Matrix(other.imag);
        }

        private MatrixCmplx(Matrix real, Matrix imag)
        {
            //copy the matrices by rererence
            this.real = real;
            this.imag = imag;
        }

        #region Class Properties...

        /// <summary>
        /// Represents the number of rows in the matrix, or the length
        /// of a single column.
        /// </summary>
        public int NumRows
        {
            get { return real.NumRows; }
        }

        /// <summary>
        /// Represents the number of columns in the matrix, or the length
        /// of a single row.
        /// </summary>
        public int NumColumns
        {
            get { return real.NumColumns; }
        }

        /// <summary>
        /// Represents the total number of cells in the matrix.
        /// </summary>
        public int NumCells
        {
            get { return real.NumCells; }
        }

        /// <summary>
        /// Acceses the values of the matrix by row and column. See the
        /// SetElement() and GetElement() methods for more details.
        /// </summary>
        /// <param name="row">The row of the desired element</param>
        /// <param name="col">The column of the desired element</param>
        /// <returns>The desired element</returns>
        public Cmplx this[int row, int col]
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
        public Cmplx GetElement(int row, int col)
        {
            //checks that the row and column are valid
            if (row < 0 || row >= real.NumRows)
                throw new ArgumentOutOfRangeException("row");
            if (col < 0 || col >= real.NumColumns)
                throw new ArgumentOutOfRangeException("col");

            //obtains the desired cell value
            double rv = real.GetElement(row, col);
            double iv = imag.GetElement(row, col);

            return new Cmplx(rv, iv);
        }

        /// <summary>
        /// Sets the value at the given row and column in the matrix.
        /// </summary>
        /// <param name="row">The row of the desired element</param>
        /// <param name="col">The column of the desired element</param>
        /// <param name="value">The new value of the element</param>
        /// <exception cref="ArgumentOutOfRangeException">If either the row or
        /// the column numbers lie outside the matrix</exception>
        public void SetElement(int row, int col, Cmplx value)
        {
            //checks that the row and column are valid
            if (row < 0 || row >= real.NumRows)
                throw new ArgumentOutOfRangeException("row");
            if (col < 0 || col >= real.NumColumns)
                throw new ArgumentOutOfRangeException("col");

            //sets the desired cell value
            real.SetElement(row, col, value.CofR);
            imag.SetElement(row, col, value.CofI);
        }

        /// <summary>
        /// Obtains a single row from the current matrix, and
        /// returns it as a one-dimentional vector.
        /// </summary>
        /// <param name="row">The index of the desire row</param>
        /// <returns>The requested row as a vector</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the row number
        /// lies outside of the matrix</exception>
        public VectorCmplx GetRow(int row)
        {
            //checks that the row number is valid
            if (row < 0 || row >= real.NumRows)
                throw new ArgumentOutOfRangeException("row");

            //used to store the values in the row
            int size = real.NumColumns;
            double[] values = new double[size * 2];

            //obtains the values in the given row
            for (int k = 0; k < size; k++)
            {
                int j = 2 * k;

                values[j + 0] = real.GetElement(row, k);
                values[j + 1] = imag.GetElement(row, k);
            }

            //returns the complex row vector
            return new VectorCmplx(values);
        }

        /// <summary>
        /// Obtains a single column from the current matrix, and
        /// returns it as a one-dimentional vector.
        /// </summary>
        /// <param name="col">The index of the desire column</param>
        /// <returns>The requested column as a vector</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the column number
        /// lies outside of the matrix</exception>
        public VectorCmplx GetColumn(int col)
        {
            //checks that the row number is valid
            if (col < 0 || col >= real.NumColumns)
                throw new ArgumentOutOfRangeException("col");

            //used to store the values in the row
            int size = real.NumRows;
            double[] values = new double[size * 2];

            //obtains the values in the given row
            for (int k = 0; k < size; k++)
            {
                int j = 2 * k;

                values[j + 0] = real.GetElement(k, col);
                values[j + 1] = imag.GetElement(k, col);
            }

            //returns the complex row vector
            return new VectorCmplx(values);
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
        public void SetRow(int row, VectorCmplx values)
        {
            //makes shure the input vector is the correct shape
            if (row < 0 || row >= real.NumRows)
                throw new ArgumentOutOfRangeException("row");
            if (values.Length < real.NumColumns)
                throw new ArgumentShapeException("values");

            //copies the row values into the matrix
            for (int i = 0; i < real.NumColumns; i++)
            {
                Cmplx temp = values[i];
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
        public void SetColumn(int col, VectorCmplx values)
        {
            //makes shure the input vector is the correct shape
            if (col < 0 || col >= real.NumColumns)
                throw new ArgumentOutOfRangeException("col");
            if (values.Length < real.NumRows)
                throw new ArgumentShapeException("values");

            //copies the column values into the matrix
            for (int i = 0; i < real.NumRows; i++)
            {
                Cmplx temp = values[i];
                SetElement(i, col, temp);
            }
        }

        /// <summary>
        /// Treats the matrix as a collection of row vectors by itterating over
        /// the matrix one row at a time.
        /// </summary>
        /// <returns>An enumeration of the rows in the matrix</returns>
        public IEnumerator<VectorCmplx> GetEnumerator()
        {
            //determins the number of rows and columns
            int rows = real.NumRows;
            int cols = real.NumColumns;

            //used to store the values to build each row
            double[] temp = new double[cols * 2];

            //itterates over each row in the matrix
            for (int i = 0; i < rows; i++)
            {
                //iterates over each element in the current row
                for (int j = 0; j < cols; j++)
                {
                    int k = j * 2;
                    temp[k + 0] = real.GetElement(i, j);
                    temp[k + 1] = imag.GetElement(i, j);
                }

                yield return new VectorCmplx(temp);
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
        public MatrixCmplx Add(MatrixCmplx a)
        {
            //makes shure the dimentions of the matrices match
            if (a.NumRows != NumRows || a.NumColumns != NumColumns)
                throw new ArgumentShapeException("a");

            //adds the real and imaginary matrices component-wise
            Matrix mr = real.Add(a.real);
            Matrix mi = imag.Add(a.imag);

            return new MatrixCmplx(mr, mi);
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
        public MatrixCmplx Add(Cmplx x)
        {
            //checks for non-square matricies
            if (real.NumRows != real.NumColumns) 
                throw new ArgumentShapeException();

            //adds the scalar value compnent-wise
            Matrix mr = real.Add(x.CofR);
            Matrix mi = imag.Add(x.CofI);

            return new MatrixCmplx(mr, mi);
        }

        /// <summary>
        /// Generates a new matrix that is the diffrence of the current
        /// matrix and a second matrix. It overloads the (-) operator.
        /// </summary>
        /// <param name="a">The second matrix</param>
        /// <returns>The difrence of the two matrices</returns>
        /// <exception cref="ArgumentShapeException">If the matrices differ in
        /// either their number of rows or columns</exception>
        public MatrixCmplx Sub(MatrixCmplx a)
        {
            //makes shure the dimentions of the matrices match
            if (a.NumRows != NumRows || a.NumColumns != NumColumns)
                throw new ArgumentShapeException("a");

            //subtracts the real and imaginary matrices component-wise
            Matrix mr = real.Sub(a.real);
            Matrix mi = imag.Sub(a.imag);

            return new MatrixCmplx(mr, mi);
        }

        /// <summary>
        /// Subtracts a scalor value from a matrix, using the same
        /// definition as scalor adition. It overloads the (-) operator.
        /// </summary>
        /// <param name="x">A scalar value to subtract from the matrix</param>
        /// <returns>The result of subtracting a scalor from the matrix</returns>
        /// <exception cref="ArgumentShapeException">If matrix is not square</exception>
        public MatrixCmplx Sub(Cmplx x)
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
        /// <param name="m">The second matrix</param>
        /// <returns>The product of the two matrices</returns>
        /// <exception cref="ArgumentShapeException">If the number of columns in the
        /// first matrix dose not match the number of rows in the second</exception>
        public MatrixCmplx Mult(MatrixCmplx m)
        {
            //make shure the matricies can be multiplied
            if (m.real.NumRows != this.real.NumColumns) 
                throw new ArgumentShapeException("m");

            //computes each term with four matrix multiplies
            Matrix ac = real.Mult(m.real);
            Matrix bd = imag.Mult(m.imag);
            Matrix ad = real.Mult(m.imag);
            Matrix bc = imag.Mult(m.real);

            //computes the resulting matrix
            return new MatrixCmplx(ac - bd, bc + ad); 
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
        public VectorCmplx Mult(VectorCmplx v)
        {
            //make shure the matricies can be multiplied
            if (v.Length != NumColumns) throw new ArgumentShapeException("v");

            Vector vr = v.Real();
            Vector vi = null; //v.Imag():

            //computes each term with four matrix multiplies
            Vector ac = real.Mult(vr);
            Vector bd = imag.Mult(vi);
            Vector ad = real.Mult(vi);
            Vector bc = imag.Mult(vr);

            //computes the resulting matrix
            //return new VectorCmplx(ac - bd, bc + ad); 
            return null;
        }

        /// <summary>
        /// Generates a new matrix that is a scalar multiple of the
        /// current matrix. It overloads the (*) and (/) opperators,
        /// respectivly.
        /// </summary>
        /// <param name="x">Value by which to scale the matrix</param>
        /// <returns>The product of this matrix and a scalar</returns>
        public MatrixCmplx Mult(Cmplx x)
        {
            //computes each term with four scalar multiplies
            Matrix ac = real.Mult(x.CofR);
            Matrix bd = imag.Mult(x.CofI);
            Matrix ad = real.Mult(x.CofI);
            Matrix bc = imag.Mult(x.CofR);

            //computes the resulting matrix
            return new MatrixCmplx(ac - bd, bc + ad); 
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
