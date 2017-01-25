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
    /// <remarks>Last Update: 2016-10-26</remarks>
    public class StatRunner : IFormattable
    {
        #region Class Definitions...

        //rembers the number of dimentions
        private int dim;

        //stores the sum of the weights
        private double t_weight;

        //stores the first and second central moments
        private Vector m1;
        private Vector m3;

        //stores the second and fourth central moments
        private double m2;
        private double m4;

        //stores the minimum and maximum bounds
        private Vector min;
        private Vector max;

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

        /// <summary>
        /// Generates a string representation of the stat-runner, using the default
        /// formating for floating point values.
        /// </summary>
        /// <returns>The stat-runner fomated as a string</returns>
        public override string ToString()
        {
            //calls upon the method below
            return ToString("g5", null);
        }

        /// <summary>
        /// Generates a formated string representation of the stat-runner, displaying
        /// the most comonly used statistics, passing the formating information on
        /// to each one. 
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The stat-runner fomated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            //takes care of an empty stat runner
            if (this.Empty) return "Empty Data-Set";

            StringBuilder sb = new StringBuilder();

            if (Dim == 1)
            {
                //if our data is univarant, we can report the stats as scalor values
                sb.AppendFormat("Min: {0}, ", Min[0].ToString(format, provider));
                sb.AppendFormat("Max: {0}, ", Max[0].ToString(format, provider));
                sb.AppendFormat("Avg: {0}, ", Mean[0].ToString(format, provider));
            }
            else
            {
                //we must report the stats as vectors for multidimentional data
                sb.AppendFormat("Min: {0}, ", Min.ToString(format, provider));
                sb.AppendFormat("Max: {0}, ", Max.ToString(format, provider));
                sb.AppendFormat("Avg: {0}, ", Mean.ToString(format, provider));
            }

            //the standard diviation and weight are always scalar values
            sb.AppendFormat("SD: {0}, ", SD.ToString(format, provider));
            sb.AppendFormat("WT: {0}", Weight.ToString(format, provider));

            return sb.ToString();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins if the dataset is currently empty, or if the stat-runner
        /// has been reset back to zero.
        /// </summary>
        public bool Empty
        {
            get { return (max == null) || (min == null); }
        }

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
        /// Represents the amount of skewness in the data set. Skewness can be thought of, 
        /// abstractly, as the distribution of the data about the mean. In single-valued 
        /// statistics, the distribution tends to lean to the left when the skew is negative, 
        /// or to the right when it is positive. In higher dimentions, this forms a vector 
        /// indicating the direction of the skew. It dose not relate the mean to the median,
        /// which are two diffrent types of central tendency.
        /// </summary>
        public Vector Skew
        {
            get
            {
                double temp = t_weight / (m2 * m2 * m2);
                return m3 * Math.Sqrt(temp);
            }
        }

        /// <summary>
        /// Represets the amount of kurtosis in the data set. Kurtosis, along with Skewness,
        /// help determin the shape of the distribution. Data sets with large kurtosis values,
        /// tend to have a lot of outliers. A normaly distributed random variable will have a
        /// kurtosis that tends tward zero.
        /// </summary>
        public double Kurt
        {
            get { return (t_weight * m4) / (m2 * m2) - 3.0; }
        }

        /// <summary>
        /// The minimum binding value in the data set. For univariate data, this is simply
        /// the smallest item in the dataset. In higher dimentions, it forms one corner of
        /// the bounding hyper-volume that contains the dataset. This hyper-volume can be
        /// uniquly defined by the minimum and maximum values.
        /// </summary>
        public Vector Min
        {
            get { return min; }
        }

        /// <summary>
        /// The maximum binding value in the data set. For univariate data, this is simply
        /// the largest item in the dataset. In higher dimentions, it forms one corner of
        /// the bounding hyper-volume that contains the dataset. This hyper-volume can be
        /// uniquly defined by the minimum and maximum values.
        /// </summary>
        public Vector Max
        {
            get { return max; }
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

            //updates the minimum and maximum bounds
            min = (min == null) ? x : min.Min(x);
            max = (max == null) ? x : max.Max(x);

            //used in computing the mean and varance
            double temp = 1.0 + t_weight;
            Vector delta = x - m1;
            Vector dn = delta / temp;
            double t1 = (delta * dn) * t_weight;
            double dn2 = dn * dn;

            //updates the fouth central moment
            m4 += t1 * dn2 * (temp * temp - 3.0 * temp + 3.0);
            m4 += 6.0 * dn2 * m2 - 4.0 * (dn * m3);

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

            //updates the minimum and maximum bounds
            min = (min == null) ? x : min.Min(x);
            max = (max == null) ? x : max.Max(x);

            //used in computing the mean and varance
            double temp = weight + t_weight;
            Vector delta = x - m1;
            Vector dn = delta * (weight / temp);
            double t1 = (delta * dn) * t_weight;
            double dn2 = dn * dn;

            //updates the fouth central moment
            m4 += t1 * dn2 * (temp * temp - 3.0 * temp + 3.0);
            m4 += 6.0 * dn2 * m2 - 4.0 * (dn * m3);

            //updates the third central moment
            m3 += dn * (t1 * (temp - 2.0));
            m3 += dn * (m2 * (-3.0));

            //updates the intermediates
            m2 += t1;
            m1 += dn;

            t_weight = temp;
        }


        //public void Add2(Vector x, double weight)
        //{
        //    //the dimentions of the input vector must match
        //    if (x.Length != dim) throw new ArgumentShapeException("x");

        //    //only allows positive weight values
        //    if (weight < 0.0) weight = -weight;

        //    //updates the minimum and maximum bounds
        //    min = (min == null) ? x : min.Min(x);
        //    max = (max == null) ? x : max.Max(x);

        //    //used in computing the mean and varance
        //    double temp = weight + t_weight;
        //    Vector delta = x.Sub(m1);
        //    Vector dn = delta.Mult(weight / temp);
        //    double t1 = delta.Mult(dn) * t_weight;
        //    double dn2 = dn.Mult(dn);

        //    //updates the fouth central moment
        //    m4 += t1 * dn2 * (temp * temp - 3.0 * temp + 3.0);
        //    m4 += 6.0 * dn2 * m2 - 4.0 * dn.Mult(m3);

        //    //updates the third central moment
        //    m3 = m3.Add(dn.Mult(t1 * (temp - 2.0)));
        //    m3 = m3.Add(dn.Mult(m2 * -3.0));

        //    //updates the intermediates
        //    m2 = m2 + t1;
        //    m1 = m1.Add(dn);

        //    t_weight = temp;
        //}



        /// <summary>
        /// Resets the StatRunner, clearing all previously acumuliated statistics.
        /// </summary>
        public void Reset()
        {
            //sets the initial weight to zero
            t_weight = 0.0;

            //sets the central moments to zero
            m2 = m4 = 0.0;
            m1 = new Vector(dim);
            m3 = new Vector(dim);

            //sets the maximum and minimum bounds to null
            max = min = null;
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
