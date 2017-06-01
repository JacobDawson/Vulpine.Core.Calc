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

namespace Vulpine.Core.Calc.RandGen
{
    /// <summary>
    /// The Mersenne Twister is a famous PRNG, and is considered one of the best 
    /// PRNGs for non-cryptographic purpouses. It has an extremly long period 
    /// of 2^19937 - 1 and is quite robust agenst most tests for randomness. 
    /// Unfortuatly this robustness comes at the cost of a very large state-space, 
    /// taking up 2.4 KB of memory per instance. It is also some-what slower 
    /// than smaller PRNGs.
    /// </summary>
    /// <remarks>Last Update: 2016-02-04</remarks>
    public sealed class RandMT : VRandom
    {
        #region Class Definitions...

        //paramaters that define the Mersenne Twister
        private const int N = 624;
        private const int M = 397;

        //temporing bit-masks
        private const uint A = 0x9908B0DFU;
        private const uint B = 0x9D2C5680U;
        private const uint C = 0xEFC60000U;

        //the masks used to seperate the bits for twisting
        private const uint LowerMask = 0x7FFFFFFFU;
        private const uint UpperMask = 0x80000000U;

        //stores the internal state of the Mersenne Twister
        private uint[] state;
        private int index;

        /// <summary>
        /// Constructs a new PRGN using the current system time
        /// as the intinal seed value.
        /// </summary>
        public RandMT()
        {
            state = new uint[N];
            index = N + 1;

            seed = (int)(DateTime.Now.Ticks % Int32.MaxValue);
            Init(seed);
        }

        /// <summary>
        /// Constructs a new PRGN useing the desired seed.
        /// </summary>
        /// <param name="seed">The initial seed</param>
        public RandMT(int seed)
        {
            state = new uint[N];
            index = N + 1;

            this.seed = seed;
            Init(seed);
        }

        /// <summary>
        /// Constructs a new PRGN using an array of bytes. This allows
        /// the PRNG to use seeds mutch larger than 32-bits. 
        /// </summary>
        /// <param name="data">The data used to seed the generator</param>
        public RandMT(byte[] data)
        {
            state = new uint[N];
            index = N + 1;
            seed = 0;

            Init(data);
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
            //updates the next set of numbers
            if (index >= N)
            {
                Twist();
                index = 0;
            }

            //Goes through our Temporing Transform
            uint y = state[index];
            y ^= (y >> 11);
            y ^= (y << 7) & B;
            y ^= (y << 15) & C;
            y ^= (y >> 18);

            //Updates and returns
            index = index + 1;
            return unchecked((int)y);
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

        #region The Mersenne Twister...

        /// <summary>
        /// Initalises the Mersenne Twister with the given seed value.
        /// </summary>
        /// <param name="seed">Seed for the generator</param>
        private void Init(int seed)
        {
            //sets up the initial conditions
            index = state.Length;
            state[0] = unchecked((uint)seed);

            //fills out the rest of the state based on the seed
            for (int i = 1; i < N; i++)
            {
                unchecked
                {
                    state[i] = state[i - 1] ^ (state[i - 1] >> 30);
                    state[i] = 1812433253U * state[i] + (uint)i;
                }
            }
        }

        /// <summary>
        /// Initalises the Mersenne Twister using an array of bytes. This allows
        /// the PRNG to use seeds mutch larger than 32-bits. 
        /// </summary>
        /// <param name="data">The data used to seed the generator</param>
        private void Init(byte[] data)
        {
            //sets up the initial conditions
            index = state.Length;

            //fills up as mutch of the state as possable
            for (int i = 0; i < data.Length - 4; i+= 4)
            state[i / 4] = BitConverter.ToUInt32(data, i);

            //calculates the offset
            int off = data.Length / 4;

            //fills the rest of the state by permuting the data
            for (int i = off; i < state.Length; i++)
            {
                unchecked
                {
                    state[i] = state[i - off] ^ (state[i - off] >> 30);
                    state[i] = 1812433253U * state[i] + (uint)i;
                }
            }
        }

        /// <summary>
        /// Updates the internal state of the Mersenne Twister, 
        /// generating the next N words in the sequence.
        /// </summary>
        private void Twist()
        {
            uint x, a; 
            int t;

            for (int i = 0; i < N - 1; i++)
            {
                //grabes the upper and lower bits
                x = state[i] & UpperMask;
                x |= state[i + 1] & LowerMask;

                //updates the state
                t = (i + M) % state.Length;
                a = (x & 1U) == 0 ? 0 : A;
                state[i] = state[t] ^ ((x >> 1) ^ a); 
            }

            //grabes the upper and lower bits
            x = state[N - 1] & UpperMask;
            x |= state[0] & LowerMask;

            //updates the state
            a = (x & 1U) == 0 ? 0 : A;
            state[N - 1] = state[M - 1] ^ ((x >> 1) ^ a);
        }

        #endregion /////////////////////////////////////////////////////////////////

    }
}
