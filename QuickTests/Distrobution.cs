using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.RandGen;
using Vulpine.Core.Calc.Matrices;

namespace QuickTests
{
    public static class Distrobution
    {
        public static void Run()
        {
            VRandom rng = new RandMT();

            Vector dist = new Vector(2.0, 3.0, 1.0, 4.0, 5.0, 3.0, 2.0);
            int[] counts = new int[dist.Length];

            Console.WriteLine("runing tests...");
            int total = 10000000;

            for (int i = 0; i < total; i++)
            {
                int j = rng.SampleDesc(dist);
                counts[j] += 1;
            }

            for (int k = 0; k < counts.Length; k++)
            {
                double davg = dist[k] / 20.0;
                double cavg = counts[k] / (double)total;

                Console.Write(dist[k] + "  ");
                Console.Write(counts[k] + "  ");
                Console.Write(davg + "  ");
                Console.Write(cavg + "  ");
                Console.WriteLine();
            }
        }
    }
}
