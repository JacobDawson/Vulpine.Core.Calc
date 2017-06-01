/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
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

using Vulpine.Core.Data;
using Vulpine.Core.Data.Tables;
using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Matrices
{
    public class MatrixSparse
    {
        #region Class Definitions...

        //stores the matrix in a table
        private Table<Cell, Double> matrix;

        //stores the size of the matrix
        private int num_rows;
        private int num_cols;

        /// <summary>
        /// Constructs an empty m x n matrix where all the entrys are initialsied
        /// to zero. The matrix can then be built dynamicaly.
        /// </summary>
        /// <param name="rows">Number of rows in the matrix</param>
        /// <param name="cols">Number of columns in the matrix</param>
        /// <exception cref="ArgRangeExcp">If either the number of rows or
        /// columns is less than one</exception>
        public MatrixSparse(int rows, int cols)
        {
            //checks that the size of the matrix is valid
            ArgRangeExcp.Atleast("rows", rows, 1);
            ArgRangeExcp.Atleast("cols", cols, 1);

            num_rows = rows;
            num_cols = cols;

            //instanciates the table for the first time
            matrix = new TableClosed<Cell, Double>();
        }

        /// <summary>
        /// Copy constructor, used to create identical copies of a
        /// given matrix inorder to protect the original.
        /// </summary>
        /// <param name="other">The matrix to be copyed</param>
        public MatrixSparse(MatrixSparse other)
        {
            //takes the dimentions of the other matrix
            num_rows = other.num_rows;
            num_cols = other.num_cols;
            int cap = other.matrix.Count;

            //instanciates the table for the first time
            matrix = new TableClosed<Cell, Double>(cap + 1);

            //copies each of the items into the new table
            foreach (var cell in other.matrix)
            matrix.Add(cell.Key, cell.Item);
        }

        /// <summary>
        /// Generates a deep copy of the current matrix by invoking
        /// the corisponding copy constructor.
        /// </summary>
        /// <returns>A copy of the mector</returns>
        public MatrixSparse Clone()
        {
            //calls upon the copy constructor
            return new MatrixSparse(this);
        }

        /// <summary>
        /// Generates a string representation of the matrix, displaying
        /// the number of rows and columns as it's dimentions.
        /// </summary>
        /// <returns>The matrix as a string</returns>
        public override string ToString()
        {
            //reports the dimentions of the matrix
            return String.Format("Matrix[{0}, {1}]",
                num_rows, num_cols);
        }

        #endregion //////////////////////////////////////////////////////////////


        private struct Cell : IComparable<Cell>
        {
            public int Row;
            public int Col;

            public Cell(int row, int col)
            {
                this.Row = row;
                this.Col = col;
            }

            public int CompareTo(Cell other)
            {
 	            int test = Row.CompareTo(other.Row);
                if (test != 0) return test;
                else return Col.CompareTo(other.Col);
            }
        }
    }
}
