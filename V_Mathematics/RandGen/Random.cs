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

using Vulpine.Core.Data.Extentions;
using Vulpine.Core.Data.Lists;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.RandGen
{
    /// <summary>
    /// This is the parent class for all psudo-random number generators (PRNGs). It is
    /// ment to be a replacement for System.Random which is not the most ideal PRNG
    /// for all situations. It also provides convience methods for generating numbers
    /// in a particular range, rolling dice, and other common uses. Implementing classes
    /// need only to overide the abstract methods.
    /// </summary>
    /// <remarks>Last Update: 2016-02-10</remarks>
    public abstract class VRandom
    {
        #region Class Definitions...

        //NOTE: Consider adding a Clone method

        //used in generating normal distriubtion values
        private double last_norm = 0.0;
        private bool has_norm = false;

        //rembers the seed for the PRNG
        protected int seed;

        /// <summary>
        /// Generates a string representation of the current random number generator,
        /// using the seed value to uniquly identify the generator.
        /// </summary>
        /// <returns>The random number generator as a string</returns>
        public override string ToString()
        {
            Type t = base.GetType();
            return String.Format("{0}: {1:X8}", t.Name, seed);
        }

        /// <summary>
        /// Determins if this random number generator equals another object. Two RNGs
        /// are considered equal, if they are of the same type and have the same seed.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            VRandom rng = obj as VRandom;
            if (rng == null) return false;

            Type t1 = this.GetType();
            Type t2 = rng.GetType();

            return t1.Equals(t2) && seed.Equals(rng.seed); 
        }

        /// <summary>
        /// Gets a sudo-unique hash value for the current random number generator.
        /// This is just the seed for the generator.
        /// </summary>
        /// <returns>A sudo-unique hash value</returns>
        public override int GetHashCode()
        {
            return seed;
        }

        #endregion ////////////////////////////////////////////////////////

        #region Class properties...

        /// <summary>
        /// Represents the 32-bit seed value used to initialise this
        /// particular instance of Random.
        /// </summary>
        public int Seed
        {
            get { return seed; }
        }

        #endregion ////////////////////////////////////////////////////////

        #region Abstract Methods...

        /// <summary>
        /// Generates a psudo-random 32-bit value, that is between the
        /// maximum and minimum values for a 32-bit interger.
        /// </summary>
        /// <returns>A psudo-random interger</returns>
        public abstract int NextInt();

        /// <summary>
        /// Generates a psudo-random floating-point value that is in
        /// between 0.0 inclusive, and 1.0 exclusive.
        /// </summary>
        /// <returns>A psudo-random double</returns>
        public abstract double NextDouble();

        /// <summary>
        /// Resets the random number generator to the state that it was
        /// in when it was first initialised with its seed.
        /// </summary>
        public abstract void Reset();

        #endregion ////////////////////////////////////////////////////////

        #region Random Number Generation...

        /// <summary>
        /// Generates a random boolen value, with a close-to one to
        /// one chance of being either true or false.
        /// </summary>
        /// <returns>A psudo-random boolean value</returns>
        public bool RandBool()
        {
            int next = RandInt(2);
            return (next == 0);
        }

        /// <summary>
        /// Generates a random boolean value with the given probablity
        /// of the result being true. Probablity values are given in
        /// the range of 0.0 to 1.0 inclusive.
        /// </summary>
        /// <param name="prob">Probabilty of result being true</param>
        /// <returns>A psudo-random boolean value</returns>
        public bool RandBool(double prob)
        {
            //takes care of the extream cases
            if (prob < 0.0) return false;
            if (prob > 1.0) return true;

            //generates a true result with given probabilty
            return (NextDouble() < prob);
        }

        /// <summary>
        /// Generates a psudo-random ingerger value between zero and the given
        /// maximum, but not including the maximum value.
        /// </summary>
        /// <param name="max">Maximum vlaue to generate</param>
        /// <returns>A psudo-random value within the given bounds</returns>
        public int RandInt(int max)
        {
            //checks that the maximum value is positive
            if (max < 1) return 0;

            //grabs a random positive value
            int r = NextInt() & Int32.MaxValue;

            //uses the high order bits to generate the spread
            int x = (Int32.MaxValue / max) + 1;
            return r / x;
        }

        /// <summary>
        /// Generates a psudo-random interger value betwen two given bounds,
        /// including the lower bound, but excluding the upper bound. It swaps
        /// the bounds if given out of order.
        /// </summary>
        /// <param name="low">Lower bound, inclued in range</param>
        /// <param name="high">Upper bound, excluded from range</param>
        /// <returns>A psudo-random value within the given bounds</returns>
        public int RandInt(int low, int high)
        {
            //checks for default case
            if (low == high) return low;

            if (low > high)
            {
                //swaps the values and rolls again
                return RandInt(high, low);
            }
            else
            {
                //grabs a random positive value
                int r = NextInt() & Int32.MaxValue;

                //uses the high order bits to generate the spread
                int x = (Int32.MaxValue / (high - low)) + 1;
                return (r / x) + low;
            }
        }

        /// <summary>
        /// Generates a random, positive, floating-point value up to, but not
        /// including, the given maximum.
        /// </summary>
        /// <param name="rand">Generator for random numbers</param>
        /// <param name="max">Maximum vlaue to generate</param>
        /// <returns>A psudo-random value within the given bounds</returns>
        public double RandDouble(double max)
        {
            //simply multiply by our maximum
            return NextDouble() * max;
        }

        /// <summary>
        /// Generates a random floating-point vlaue between two given bounds,
        /// excluding the posiblility of returning the upper bound. The bracket
        /// is reversed if given out of order.
        /// </summary>
        /// <param name="low">Lower bound, inclued in range</param>
        /// <param name="high">Upper bound, excluded from range</param>
        /// <returns>A psudo-random value within the given bounds</returns>
        public double RandDouble(double low, double high)
        {
            if (low > high)
            {
                //swaps the values and rolls again
                return RandDouble(high, low);
            }
            else
            {
                //computes our random number
                double next = this.NextDouble();
                return ((next * (high - low)) + low);
            }
        }

        /// <summary>
        /// Generates random byte values, up to the count specified. If a negitive
        /// value is given, it will generate values indefinitaly.
        /// </summary>
        /// <param name="count">Number of bytes to generate</param>
        /// <returns>An enumeration of random bytes</returns>
        public IEnumerable<Byte> RandBytes(int count)
        {
            //must generate atleast one byte
            if (count == 0) yield break;

            //used in generating the bytes
            byte[] bits;
            int next;
            int n = 0;

            while (true)
            {
                //grabs the next four bytes
                next = NextInt();
                bits = BitConverter.GetBytes(next);

                for (int i = 0; i < 4; i++)
                {
                    yield return bits[i]; n++;
                    if (count > 0 && n >= count) yield break;
                }
            }
        }

        /// <summary>
        /// Generates a random value with Standard Normal Distribution, that is
        /// a normal distribution with a mean of 0.0 and a standard diviaiton of 1.0.
        /// </summary>
        /// <returns>A random value with normal distribution</returns>
        public double RandGauss()
        {
            if (has_norm)
            {
                has_norm = false;
                return last_norm;
            }
            else
            {
                //grabs two uniform random variables
                double u1 = NextDouble();
                double u2 = NextDouble();

                //computes the polar cordinates
                double r = Math.Sqrt(-2.0 * Math.Log(u1));
                double t = VMath.TAU * u2;

                has_norm = true;
                last_norm = r * Math.Cos(t);
                return r * Math.Sin(t);
            }
        }

        /// <summary>
        /// Generates a random value with normal distribution, given the mean value
        /// and standard diviation of the distribution.
        /// </summary>
        /// <param name="mean">Mean of the distribution</param>
        /// <param name="sd">Standard diviation of the distribution</param>
        /// <returns>A random value with normal distribution</returns>
        public double RandGauss(double mean, double sd)
        {
            double n = RandGauss();
            return n * sd + mean;
        }

        #endregion ////////////////////////////////////////////////////////

        #region Speciality Methods...

        /// <summary>
        /// Generates a random unit vector with the given number of dimentions. A
        /// unit vector is a vector with a length of one. It can be seen as representing
        /// a direciton in space, without a magnitude.
        /// </summary>
        /// <param name="dim">Dimintion of the output vector</param>
        /// <returns>A random unit vector</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the dimention is less 
        /// than one</exception>
        public Vector RandUnit(int dim)
        {
            if (dim < 1)
            {
                //we do not allow for dimentions less than one
                throw new ArgumentOutOfRangeException("dim");
            }
            else if (dim == 1)
            {
                //reduces the generator to a simple coin flip
                if (RandBool()) return new Vector(1.0);
                else return new Vector(-1.0);
            }
            else if (dim == 2)
            {
                //generates a random angle
                double theta = RandDouble(VMath.TAU);

                //computes the cordinates from the angle
                double x = Math.Cos(theta);
                double y = Math.Sin(theta);

                return new Vector(x, y);
            }
            else if (dim == 3)
            {
                //generates two uniform variables
                double t = RandDouble(VMath.TAU);
                double z = RandDouble(-1.0, 1.0);

                //uses the cylindrical equal-area projection
                double r = Math.Sqrt(1.0 - z * z);
                double x = r * Math.Cos(t);
                double y = r * Math.Sin(t);

                return new Vector(x, y, z);
            }
            else
            {
                //creates a vector to store our temproary values
                Vector v = new Vector(dim);

                //uses a gauss distribution for spherical data
                for (int i = 0; i < dim; i++) v[i] = RandGauss();

                //normalises the output
                return v.Unit();
            }
        }

        /// <summary>
        /// Selects a random item from a sequence of items. It is optomised for
        /// sequences that are indexable, but it will work for any sequence. If
        /// the sequence is empty, it will return the default item for the type.
        /// </summary>
        /// <typeparam name="T">Element Type</typeparam>
        /// <param name="source">Source of the original items</param>
        /// <returns>A random item</returns>
        /// <exception cref="ArgumentNullException">If the source is null</exception>
        public T RandElement<T>(IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            ICollection<T> colleciton = source as ICollection<T>;

            if (colleciton != null)
            {
                //we can't select a random element from zero elements
                int count = colleciton.Count;
                if (count == 0) return default(T);

                //simply finds the item by random index
                int index = RandInt(count);
                return source.FindByIndex(index);
            }

            using (var ittr = source.GetEnumerator())
            {
                //we can't select a random element from zero elements
                if (!ittr.MoveNext()) return default(T);

                //makes the first item our best guess
                int count = 1;
                int test = 0;
                T curr = ittr.Current;

                //replaces each guess with apropriate probability
                while (ittr.MoveNext())
                {
                    count = count + 1;
                    test = RandInt(count);
                    if (test == 0) curr = ittr.Current;
                }

                return curr;
            }
        }

        /// <summary>
        /// Generates a random permutation of all the items in a given sequence.
        /// This is similar, in effect, to shuffeling a deck of cards.
        /// </summary>
        /// <typeparam name="T">Element Type</typeparam>
        /// <param name="source">Source of the original items</param>
        /// <returns>A random sequence of items</returns>
        /// <exception cref="ArgumentNullException">If the source is null</exception>
        public IEnumerable<T> Shuffel<T>(IEnumerable<T> source)
        {
            //NOTE: I should be able to run this method using an array
            //instead of relying on VList, it also requires testing

            if (source == null) throw new ArgumentNullException("source");
            VList<T> shuffel = new VListArray<T>(source.Count());

            foreach (T item in source)
            {
                //produces a random index into the list
                int i = RandInt(shuffel.Count + 1);

                if (i == shuffel.Count)
                {
                    //adds the item to the end of the list
                    shuffel.Add(item);
                }
                else
                {
                    //swaps the item with the end of the list
                    shuffel.Add(shuffel[i]);
                    shuffel[i] = item;
                }
            }

            //returns the shuffeled list
            return shuffel.AsEnumerable();
        }

        /// <summary>
        /// Simulates rolling a set of dice, each with a given number of sides,
        /// and summing the values of all the dice plus a given offset.
        /// </summary>
        /// <param name="dice">Number of dice to roll</param>
        /// <param name="sides">Number of sides per die</param>
        /// <param name="offset">Offset to the sum</param>
        /// <returns>The total value of the dice roll</returns>
        public int RollDice(int dice, int sides, int offset)
        {
            //clamps the number of sides and dice
            if (sides < 1) sides = 1;
            if (dice < 1) dice = 1;

            //used in calculating the sum
            int x = (255 / sides) + 1;
            int sum = offset;

            //uses one byte for each die rolled
            foreach (byte b in RandBytes(dice))
            {
                sum += ((int)b / x) + 1;
            }

            return sum;
        }

        #endregion ////////////////////////////////////////////////////////
    }
}
