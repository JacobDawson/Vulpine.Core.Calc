using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc
{

    /// <summary>
    /// This class represents a set of running statistics for a given data set. Data
    /// points can be added, and the corisponding stats will be updated on the fly. It
    /// is able to handle both single and multi-dimentional data, as well as weighted
    /// statistics, if a weight is given for each data point. It can track the mean,
    /// variance, and skew, as well as other noteworthy stats. If all one needs is the
    /// average from a fixed set of datapoints, this class might be overkill, but for
    /// it's more advanced feautres it can be quite usefull.
    /// </summary>
    /// <remarks>Last Update: 2016-08-26</remarks>
    public class StatRunner
    {
        #region Class Definitions...

        //rembers the number of dimentions
        private int dim;

        //stores the sum of the weights
        private double t_weight;

        //used in computing the mean and sum
        private Vector m1;

        //used in computing the variance
        private double m2;

        //used in computing the skew
        private Vector m3;

        /// <summary>
        /// Creates a single-valued StatRunner for acumuliating single-vaued 
        /// statistics. All input data points are real numbers.
        /// </summary>
        public StatRunner()
        {
            this.dim = 1;
            Reset();
        }

        /// <summary>
        /// Creates a multi-valued StatRunner with the given dimentinality.
        /// All input data points are vectors.
        /// </summary>
        /// <param name="dim">Dimentionality of the input data</param>
        public StatRunner(int dim)
        {
            if (dim < 1) dim = 1;
            this.dim = dim;
            Reset();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The dimentionality of the input data
        /// </summary>
        public int Dim
        {
            get { return dim; }
        }

        /// <summary>
        /// The total sum of all the weights. In unweighted statistics,
        /// this is simply the count of the data points.
        /// </summary>
        public double Weight
        {
            get { return t_weight; }
        }

        /// <summary>
        /// The weighted sum of the data points.
        /// </summary>
        public Vector Sum
        {
            get { return m1 * t_weight; } 
        }

        /// <summary>
        /// The arithemetic mean, or centralised value, of the data points.
        /// </summary>
        public Vector Mean
        {
            get { return m1; }
        }

        /// <summary>
        /// The variance of the data points. It is equivlent to the square of
        /// the standard diviation.
        /// </summary>
        public double Var
        {
            get { return m2 / t_weight; }
        }

        /// <summary>
        /// The standard diviation, or how mutch (on average) the data points
        /// tend to deviate from the mean.
        /// </summary>
        public double SD
        {
            get { return Math.Sqrt(Var); }
        }


        /// <summary>
        /// Represents the amount of skewness in the data set, in both magnitude and
        /// direction. Skewness can be thought of, abstractly, as the distance between
        /// the mean and the median, though this is not always the case.
        /// </summary>
        public Vector Skew
        {
            get
            {
                double temp = t_weight / (m2 * m2 * m2);
                return m3 * Math.Sqrt(temp);
            }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Methods...

        /// <summary>
        /// Adds a single data point to the running statistics. The default
        /// weight for un-weighted data is 1.0.
        /// </summary>
        /// <param name="x">Data point to add</param>
        public void Add(double x)
        {
            //calls the mehtod below
            Add(new Vector(x));
        }

        /// <summary>
        /// Adds a single data point to the running statistics with the given
        /// weight. The weight controls how mutch this data point influnces the
        /// rest of the stats. The default weight is 1.0.
        /// </summary>
        /// <param name="x">Data point to add</param>
        /// <param name="weight">Weight of the given data point</param>
        public void Add(double x, double weight)
        {
            //calls the mehtod below
            Add(new Vector(x), weight);
        }

        /// <summary>
        /// Adds a single data point to the running statistics. The default
        /// weight for un-weighted data is 1.0.
        /// </summary>
        /// <param name="x">Data point to add</param>
        /// <exception cref="ArgumentShapeException">If the input vector dose
        /// not have the same dimention as the rest of the data points</exception>
        public void Add(Vector x)
        {
            //the dimentions of the input vector must match
            if (x.Length != dim) throw new ArgumentShapeException("x");

            //used in computing the mean and varance
            double temp = 1.0 + t_weight;
            Vector delta = x - m1;
            Vector dn = delta * (1.0 / temp);
            double t1 = (delta * dn) * t_weight;

            //updates the third central moment
            m3 += dn * (t1 * (temp - 2.0));
            m3 += dn * (m2 * (-3.0));

            //updates the intermediates
            m2 += t1;
            m1 += dn;

            t_weight = temp;
        }

        /// <summary>
        /// Adds a single data point to the running statistics with the given
        /// weight. The weight controls how mutch this data point influnces the
        /// rest of the stats. The default weight is 1.0.
        /// </summary>
        /// <param name="x">Data point to add</param>
        /// <param name="weight">Weight of the given data point</param>
        /// <exception cref="ArgumentShapeException">If the input vector dose
        /// not have the same dimention as the rest of the data points</exception>
        public void Add(Vector x, double weight)
        {
            //the dimentions of the input vector must match
            if (x.Length != dim) throw new ArgumentShapeException("x");

            //only allows positive weight values
            if (weight < 0.0) weight = -weight;

            //used in computing the mean and varance
            double temp = weight + t_weight;
            Vector delta = x - m1;
            Vector dn = delta * (weight / temp);
            double t1 = (delta * dn) * t_weight;

            //updates the third central moment
            m3 += dn * (t1 * (temp - 2.0));
            m3 += dn * (m2 * (-3.0));

            //updates the intermediates
            m2 += t1;
            m1 += dn;

            t_weight = temp;
        }

        /// <summary>
        /// Resets the StatRunner, clearing all previously acumuliated statistics.
        /// </summary>
        public void Reset()
        {
            t_weight = 0.0;
            m2 = 0.0;
            m1 = new Vector(dim);
            m3 = new Vector(dim);
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
