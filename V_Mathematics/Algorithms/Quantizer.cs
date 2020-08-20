using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Data;
using Vulpine.Core.Calc.RandGen;

namespace Vulpine.Core.Calc.Algorithms
{
    public class Quantizer : Algorithm
    {
        #region Class Definitions...

        //NOTE: Consider implementing k-means++ or a similar algorythim to
        //automaticaly select the starting conditon for k-means

        //uses a PRNG for selecting random samples
        private VRandom rng;

        /// <summary>
        /// Creates a new Quantizer with default stoping criteria.
        /// </summary>
        public Quantizer()
        {
            base.max = 256;
            base.tol = VMath.TOL;

            this.rng = new RandXOR();
        }

        /// <summary>
        /// Creates a new Quantizer with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Quantizer(int max, double tol)
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);

            this.rng = new RandXOR();
        }

        /// <summary>
        /// Creates a new Quantizer with the given maximum number of
        /// itterations, the minimal error allowed, and a designated
        /// PRNG for selecting random samples
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="rng">Random Number Generator for selecting samples</param>
        public Quantizer(int max, double tol, VRandom rng)
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);

            this.rng = rng;
        }

        #endregion //////////////////////////////////////////////////////////////

        public ResultMulti<Vector> KMeans(Matrix data, int means)
        {
            //used to store the random centers we will generate
            Vector[] centers = new Vector[means];

            int i = 0;

            while (i < means)
            {
                //grabs a random point from the data
                int index = rng.RandInt(data.NumRows);
                Vector test = data.GetRow(index);
                bool fail = false;

                ////makes certain that the point is unique
                //for (int k = 0; k < i; k++)
                //    if (test.Equals(centers[k])) continue;

                //makes certain that the point is unique
                for (int k = 0; k < i; k++)
                {
                    fail |= test.Equals(centers[k]);
                    if (fail) break;
                }

                if (!fail)
                {
                    //adds the new point to our list
                    centers[i] = test; i++;
                }
            }

            //calls the method below with our random centers
            return KMeans(data, centers);
        }

        /// <summary>
        /// Uses the K-Means algorythim to partiton the data set, represented as a matrix
        /// with data points for the rows. Initial values for the centers (or means) of
        /// each partition must be provided up front.
        /// </summary>
        /// <param name="data">Matrix storing the data points as rows</param>
        /// <param name="means">List of starting points for the means</param>
        /// <returns>A list of mean values that evenly partion the data set</returns>
        public ResultMulti<Vector> KMeans(Matrix data, params Vector[] means)
        {
            //determins the dimentions from the matrix
            int dim = data.NumColumns;

            //creates a KD tree for fast look-up of the mean values
            TreeVector<Int32> current = new TreeKD<Int32>(dim);
            Vector[] update = new Vector[means.Length];

            //NOTE: Should we consider using a ball-tree 
            //for when both K and D are large?

            for (int i = 0; i < means.Length; i++)
            {
                //enshures that all the starting points are valid
                if (means[i].Length != dim) throw new ArgumentException();
                Vector temp = new Vector(means[i]);

                //initialises the current and update data sets
                current.Add(temp, i);
                //update[i] = new Vector(dim);
                update[i] = new Vector(means[i]);
            }

            Initialise();

            while (true)
            {
                //builds the KD tree structor
                current.Build();

                int count = 1; // 0;
                double motion = 0.0;

                //sorts the datapoints by the closest mean
                //and computes the new mean from the datapoints
                foreach (Vector x in data)
                {
                    int i = current.GetNearest(x).Value;
                    Vector delta = x - update[i];

                    count += 1;
                    update[i] += delta / count;
                }

                //computes the amount of motion for each pair of means
                foreach (var pair in current)
                {
                    int i = pair.Value;
                    Vector prev = pair.Location;
                    motion += prev.Dist(update[i]);
                }

                //clears the existing KD-Tree
                current.Clear();

                //we end the process once the means are no longer moving
                if (Step(motion)) break;

                //adds the new means for the next itteration 
                for (int i = 0; i < update.Length; i++)
                {
                    current.Add(update[i], i);
                    //update[i] = new Vector(dim);
                }
            }

            //returns the latest set of update mean positons
            return Finish(update.AsEnumerable());
        }
    }
}
