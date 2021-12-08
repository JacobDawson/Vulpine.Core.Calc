using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.RandGen;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Data.Extentions;

namespace QuickTests
{
    class JacobiTests
    {
        public const int COUNT = 100000; //000;
        public const double TOL = 1.0e-06;

        public static Cmplx RandCmplx(VRandom rng)
        {
            double a = rng.RandGauss(0.0, 2.5);
            double b = rng.RandGauss(0.0, 2.5);

            return new Cmplx(a, b);
        }

        public static Cmplx arcSN(Cmplx x, Cmplx m)
        {
            return Jacobi.FP(x, m);
        }

        public static void TestInversion1()
        {
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx m = RandCmplx(rng);

                //m = 0.5;

                Cmplx b = arcSN(a, m);
                Cmplx ap = Jacobi.SN(b, m);

                double error = (a - ap).Abs;
                error = error / a.Abs;
                stats.Add(error);

                if (error > TOL)
                {
                    //Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, ap, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   " + stats.Max[0]);
            Console.WriteLine("Mean:  " + stats.Mean[0]);
            Console.WriteLine("SD:    " + stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }
    }
}
