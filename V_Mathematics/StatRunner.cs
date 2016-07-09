using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// this is the basic stat runner, it computes stats on single values with equal weights
    /// </summary>
    public class StatRunner
    {
        #region Class Definitions...

        //counts the number of entries
        private long n;

        //used in computing the mean and variance
        private double k, ex, ex2;

        private double max, min;


        public StatRunner()
        {
            Reset();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...




        public double Total
        {
            get { return ex + n * k; }
        }

        public double Mean
        {
            get { return (ex / n) + k; }
        }

        public double Variance
        {
            get { return (ex2 - (ex * ex) / n) / n; }
        }

        public double SD
        {
            get { return Math.Sqrt(Variance); }
        }

        public double Max
        {
            get { return max; }
        }

        public double Min
        {
            get { return min; }
        }

        public double MidRange
        {
            get { return (max + min) / 2.0; }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Methods...


        public void AddValue(double x)
        {
            //use the first value as our control
            if (n <= 0) max = min = k = x;

            //computes the diffrence with our control
            double diff = x - k;

            //updates our internal values
            n = n + 1;
            ex += diff;
            ex2 += diff * diff;

            //updates the max and min as nessary
            if (x > max) max = x;
            if (x < min) min = x;
        }

        public void Reset()
        {
            n = 0;
            k = ex = ex2 = 0.0;
            max = min = 0.0;

        }

        #endregion //////////////////////////////////////////////////////////////

    }




    /// <summary>
    /// This is the vector-based stat runner
    /// </summary>
    public class StatRunner2
    {
        #region Class Definitions...

        //rembers the number of dimentions
        private int dim;

        //counts the number of entries
        private long n;

        //stores the offset and the sum
        private Vector k, ex;

        //stores the sum of the squares
        private double ex2;

        public StatRunner2()
        {
            this.dim = 1;
            Reset();
        }

        public StatRunner2(int dim)
        {
            if (dim < 1) dim = 1;
            this.dim = dim;
            Reset();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        public int Dimentions
        {
            get { return dim; }
        }

        public int Count
        {
            get { return (int)n; }
        }

        public Vector Total
        {
            get { return ex + n * k; }
        }

        public Vector Mean
        {
            get { return (ex / n) + k; }
        }

        public double Variance
        {
            get { return (ex2 - (ex * ex) / n) / n; }
        }

        public double SD
        {
            get { return Math.Sqrt(Variance); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Methods...

        public void AddValue(double x)
        {
            //calls the method below
            AddValue(new Vector(x));
        }


        public void AddValue(Vector x)
        {
            //the dimentions of the input vector must match
            if (x.Length != dim) throw new ArgumentException(); 

            //use the first value as our control
            if (n <= 0)
            {
                k = x;
                ex = new Vector(dim);
            }

            //computes the diffrence with our control
            Vector diff = x - k;

            //updates our internal values
            n = n + 1;
            ex += diff;
            ex2 += diff * diff;
        }

        public void Reset()
        {
            n = 0;
            k = ex = null;
            ex2 = 0.0;
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
