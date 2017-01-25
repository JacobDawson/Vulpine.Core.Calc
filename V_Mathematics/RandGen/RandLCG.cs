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
    /// The Linear Congruential Generator is one of the simplest kind of PRNG availabe.
    /// Dispite it's simplicity it often dose well on tests, and is provided as the
    /// default generator for many programing packages. Thus, it has become somewhat
    /// of a standard by witch other PRNGs are judged. This particular generator uses
    /// the paramaters spesified by the Minimal Standard Generator.
    /// </summary>
    /// <remarks>Last Update: 2016-02-03</remarks>
    public sealed class RandLCG : VRandom
    {
        #region Class Definitions...

        //the paramaters that define the PRNG
        private const long A = 25214903917L;
        private const long C = 11L;
        
        //stores the state of the PRGN
        private ulong state;

        /// <summary>
        /// Constructs a new PRGN using the current system time
        /// as the intinal seed value.
        /// </summary>
        public RandLCG()
        {
            int time = (int)(DateTime.Now.Ticks % Int32.MaxValue);
            this.state = unchecked((ulong)time);
            this.seed = time;
        }

        /// <summary>
        /// Constructs a new PRGN useing the desired seed.
        /// </summary>
        /// <param name="seed">The initial seed</param>
        public RandLCG(int seed)
        {
            this.state = unchecked((ulong)seed);
            this.seed = seed;
        }

        #endregion ///////////////////////////////////////////////////////////////

        #region Random Implementation...

        /// <summary>
        /// Generates a psudo-random value that is between zero
        /// and the maximum value for 32-bit integers.
        /// </summary>
        /// <returns>A psudo-random interger</returns>
        public override int NextInt()
        {
            unchecked
            {
                state = (A * state + C) & 0xFFFFFFFFFFFF;
                return (int)(state >> 16);
            }
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
            state = unchecked((ulong)seed);
        }

        #endregion ///////////////////////////////////////////////////////////////
    }
}
