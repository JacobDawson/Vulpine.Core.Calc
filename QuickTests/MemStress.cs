using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.RandGen;

namespace QuickTests
{
    public static class MemStress
    {
        public const int size = 20000;

        public static void Run01()
        {
            Process proc = Process.GetCurrentProcess();
            string mem = GetMemUsage(proc);

            VRandom rng = new RandLCG();

            Console.WriteLine("Current Memory Usage: " + mem);
            Console.WriteLine();

            //Console.ReadKey(true);
            //Console.WriteLine();

            Console.WriteLine("Generating 10,000 by 10,000 matrix...");
            Console.WriteLine();

            Matrix a = new Matrix(size, size);

            mem = GetMemUsage(proc);
            Console.WriteLine("Current Memory Usage: " + mem);
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    a[i, j] = rng.RandDouble(-10.0, 10.0);
                }
            }

            Console.WriteLine("Generating Another 10,000 by 10,000 matrix...");
            Console.WriteLine();

            Matrix b = new Matrix(size, size);

            mem = GetMemUsage(proc);
            Console.WriteLine("Current Memory Usage: " + mem);
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    b[i, j] = rng.RandDouble(0.0, 25.0);
                }
            }

            Console.WriteLine("Computing The Sum Of The Two Matrices...");
            Console.WriteLine();

            Matrix c = a + b;

            mem = GetMemUsage(proc);
            Console.WriteLine("Current Memory Usage: " + mem);
            Console.WriteLine();

            Console.WriteLine("Computing The Determinate Of The Product...");
            Console.WriteLine();

            double det = c.Det();

            mem = GetMemUsage(proc);
            Console.WriteLine("Current Memory Usage: " + mem);
            Console.WriteLine();

            Console.WriteLine("The Determinate Is: " + det);
            Console.WriteLine();

        }


        public static string GetMemUsage(Process proc)
        {
            proc = Process.GetCurrentProcess();
       

            long bytes = proc.WorkingSet64;
            double large = bytes / 1024.0;
            string unit = " KB";

            if (large > 900.0)
            {
                large = large / 1024.0;
                unit = " MB";
            }

            if (large > 900.0)
            {
                large = large / 1024.0;
                unit = "GB";
            }

            return large.ToString("0.0") + unit;
        }
    }
}
