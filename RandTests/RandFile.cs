using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Vulpine.Core.Calc.RandGen
{
    public class RandFile : VRandom, IDisposable
    {
        private BinaryReader br;

        public RandFile(string file)
        {
            Stream s = File.Open(file, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(s);
            seed = 0;
        }

        public override int NextInt()
        {
            return br.ReadInt32();
        }

        public override double NextDouble()
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            br.Dispose();
        }
    }
}
