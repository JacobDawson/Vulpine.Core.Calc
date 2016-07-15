using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.RandGen
{
    public class RandSystem : VRandom
    {
        //stores our internal PRNG
        private Random rng;

        public RandSystem()
        {
            seed = (int)(DateTime.Now.Ticks % Int32.MaxValue);
            rng = new Random(seed);
        }

        public RandSystem(int seed)
        {
            this.seed = seed;
            this.rng = new Random(seed);
        }

        public override double NextDouble()
        {
            return rng.NextDouble();
        }

        public override int NextInt()
        {
            //int x1 = rng.Next();
            //int x2 = rng.Next();

            //return (x1 << 1) | (x2 & 1);

            byte[] bits = new byte[4];
            rng.NextBytes(bits);
            return BitConverter.ToInt32(bits, 0);
        }

        public override void Reset()
        {
            rng = new Random(seed);            
        }
    }
}
