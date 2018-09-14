/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
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
