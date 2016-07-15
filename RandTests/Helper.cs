using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.RandGen;

namespace RandTests
{
    public static class Helper
    {
        public static int Get32bits(this VRandom rng)
        {
            ////extracts a 32-bit value from a 31-bit rng
            //int x1 = rng.NextInt();
            //int x2 = rng.NextInt();

            //return (x1 << 1) | (x2 & 1);


            //all RNGs are now 32 bits
            return rng.NextInt();
        }
    }
}
