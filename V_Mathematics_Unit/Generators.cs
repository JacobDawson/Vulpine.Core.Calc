using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine_Core_Calc_Tests
{
    public static class Generators
    {
        //stores several seeds taken from a true RNG
        private static readonly uint[] seeds = {
            0x09d2d6a6, 0x3875b961, 0x8d7bc51c, 0x1c376d98, 0x6aeaea8b,
            0xd0e7df84, 0x28b811e0, 0x84980757, 0x5ef8d581, 0x2e5eb340,
            0x5cc3a2e0, 0x3bafda2c, 0x48bce535, 0x7c88917f, 0x1ee869d3,
            0xbd8c2043, 0xc33a35e0, 0xa4e817f6, 0x261d9870, 0xd8f1d1ad,
            0x38869b86, 0x5af6b846, 0x2b197a7a, 0x042eb6df, 0x51f0fa75,
            0x6aac1aa6, 0x553ad36a, 0x14133ef8, 0x4fdbbe0e, 0xea1dc207,
            0x4f96e37b, 0x2523ea22, 0xef7c06a4, 0x6f28669b, 0x4f6abdac,
            0xa83e3eee, 0x8d0917c2, 0x6d400565, 0x9cdbb476, 0x9084d931,
            0xb9276042, 0x00ee227b, 0x32e804be, 0xbded6564, 0x57f11be3,
            0xb5d335ac, 0x1ba3d95b, 0x320413dd, 0x6fdda0e4, 0xa34f9e76,
        };

        /// <summary>
        /// Obtains a random number generator corisponding to the given index.
        /// The same generator will always output the same sequence of numbers.
        /// </summary>
        /// <param name="index">Index of the generator</param>
        /// <returns>A psudo-random number generator</returns>
        public static Random GetGenerator(int index)
        {
            int n = Math.Abs(index) % seeds.Length;
            int seed = unchecked((int)seeds[n]);

            return new Random(seed);
        }

    }
}
