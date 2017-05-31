/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
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

namespace Vulpine.Core.Calc.RandGen
{

    /// <summary>
    /// The XOR-Shift Register is a rather small but fast PRNG, offering a long period
    /// of 2^128 - 1. It is reported to pass all of the DieHard tests, but fails some
    /// of the BigCrush tests. The shift values used in this particular Shift Register
    /// were listed in a paper by Geroge Marsaglia.
    /// </summary>
    /// <remarks>Last Update: 2016-02-06</remarks>
    public sealed class RandXOR : VRandom
    {
        #region Class Definitions...

        //stores the internal state of the XOR-Shift register
        private uint x, y, z, w;

        /// <summary>
        /// Constructs a new PRGN using the current system time
        /// as the intinal seed value.
        /// </summary>
        public RandXOR()
        {
            int time = (int)(DateTime.Now.Ticks % Int32.MaxValue);
            this.seed = time;
            Init(time);
        }

        /// <summary>
        /// Constructs a new PRGN useing the desired seed.
        /// </summary>
        /// <param name="seed">The initial seed</param>
        public RandXOR(int seed)
        {
            this.seed = seed;
            Init(seed);
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Random Implementation...

        /// <summary>
        /// Generates a psudo-random 32-bit value, that is between the
        /// maximum and minimum values for a 32-bit interger.
        /// </summary>
        /// <returns>A psudo-random interger</returns>
        public override int NextInt()
        {
            uint temp = x ^ (x << 11);
            temp = temp ^ (temp >> 8);

            x = y;
            y = z;
            z = w;

            w = w ^ (w >> 19);
            w = w ^ temp;

            return unchecked((int)w);
        }

        /// <summary>
        /// Generates a psudo-random floating-point value that is in
        /// between 0.0 inclusive, and 1.0 exclusive.
        /// </summary>
        /// <returns>A psudo-random double</returns>
        public override double NextDouble()
        {
            //grabs 32-bits stored as a long
            long next = unchecked((uint)NextInt());

            //builds a double in the interval [1, 2) then shifts to [0, 1)
            long bits = (next << 20) | (0x3FFL << 52);
            return BitConverter.Int64BitsToDouble(bits) - 1.0;
        }

        /// <summary>
        /// Resets the random number generator to the state that it was
        /// in when it was first initialised with its seed.
        /// </summary>
        public override void Reset()
        {
            //reinitianises the RNG
            Init(seed);
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region The XOR-Shift Register...

        /// <summary>
        /// Uses the 32-bit seed to mutate the starting 128-bit internal
        /// state of the XOR-Shift Register.
        /// </summary>
        /// <param name="seed">The initial seed</param>
        private void Init(int seed)
        {
            x = unchecked((uint)seed);

            y = x ^ 0x31415926U;
            z = x ^ 0x23581321U;
            w = x ^ 0x05101986U;
        }

        #endregion /////////////////////////////////////////////////////////////////

    }
}
