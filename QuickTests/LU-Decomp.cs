using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vulpine.Core.Calc.Matrices;

namespace QuickTests
{
    public class LU_Decomp
    {
        public static Matrix M1
        {
            get { return new Matrix(2, 2, 
                1.0, 2.0, 
                2.0, 1.0); }
        }

        public static Matrix M2
        {
            get { return new Matrix(3, 3, 
                0.0, 0.0, 1.0, 
                1.0, 0.0, 0.0, 
                0.0, 1.0, 0.0); }
        }

        public static Matrix M3
        {
            get { return new Matrix(3, 3,
                2.0, 0.0, 0.0,
                6.0, 1.0, 0.0,
                -8.0, 5.0, 3.0); }
        }

        public static Matrix M4
        {
            get { return new Matrix(2, 2,
                    1.0, 1.0,
                    -1.0, 1.0); }
        }

        public static Matrix M5
        {
            get { return new Matrix(5, 5,
                 7,   -4,  9,  12,  -2,
                 6,    6,  4,  -2,   0,
                -9,  0.9, 17,   4,   8,
                 3,  0.5,  1, -14,  20,
                19,   -6,  0,  -7,  22); }
        }

        public static Matrix M6
        {
            get { return new Matrix(3, 3,
                    0.8571, 0.3943, -0.3314,
                    0.4286, -0.9029, 0.0343,
                    -0.2857, -0.1714, -0.9429); }
        }

        public static void Run()
        {
            double x;

            x = TestMatrix(M1);
            Console.WriteLine("M1 = " + x);

            x = TestMatrix(M2);
            Console.WriteLine("M2 = " + x);

            x = TestMatrix(M3);
            Console.WriteLine("M3 = " + x);

            x = TestMatrix(M4);
            Console.WriteLine("M4 = " + x);

            x = TestMatrix(M5);
            Console.WriteLine("M5 = " + x);

            x = TestMatrix(M6);
            Console.WriteLine("M6 = " + x);

            Console.ReadKey(true);
        }

        public static double TestMatrix(Matrix m)
        {
            //decomposes and recomposes the matrix
            Matrix b, upper, lower;
            Decomp(m, out upper, out lower);
            b = lower * upper;

            //computes the diffrence between the matrices
            //Vector v1 = m.ToVector();
            //Vector v2 = b.ToVector();
            Vector v1 = (Vector)m;
            Vector v2 = (Vector)b;
            return v1.Dist(v2);
        }

        public static void Decomp(Matrix m, out Matrix up, out Matrix low)
        {
            //copys the matrix so we don't mutate the original
            Matrix a = new Matrix(m);
            double val = 0.0;

            //initialises the two output matrices to zero
            up = new Matrix(m.NumRows, m.NumRows);
            low = new Matrix(m.NumRows, m.NumRows);

            //forms the L and U decomposition, updating A as it goes
            for (int k = 0; k < m.NumRows; k++)
            {
                //sets the U elements
                for (int j = k; j < m.NumRows; j++)
                {
                    val = a.GetElement(k, j);
                    up.SetElement(k, j, val);
                }

                //sets the L elements
                low.SetElement(k, k, 1);
                for (int i = k + 1; i < m.NumRows; i++)
                {
                    val = a.GetElement(i, k);
                    val = val / up.GetElement(k, k);
                    low.SetElement(i, k, val);
                }

                //updates the A elements
                for (int i = k + 1; i < m.NumRows; i++)
                {
                    for (int j = k + 1; j < m.NumRows; j++)
                    {
                        val = low.GetElement(i, k);
                        val = up.GetElement(k, j) * val;
                        val = a.GetElement(i, j) - val;
                        a.SetElement(i, j, val);
                    }
                }
            }
        }
    }
}
