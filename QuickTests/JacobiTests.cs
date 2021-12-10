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
        public const double TOL = 1.0e-8;

        public static Cmplx RandCmplx(VRandom rng)
        {
            double a = rng.RandGauss(0.0, 2.5);
            double b = rng.RandGauss(0.0, 2.5);

            return new Cmplx(a, b);
        }

        public static Cmplx arcSN(Cmplx x, Cmplx m)
        {
            Cmplx phi = Cmplx.Asin(x);
            return Jacobi.F(phi, m);

            //return Jacobi.FP(x, m);
        }

        public static double arcSN(double x, double m)
        {
            double phi = Math.Asin(x);
            return Jacobi.F(phi, m);

            //return Jacobi.FP(x, m);
        }

        public static void ListSamples()
        {
            VRandom rng = new RandXOR();

            for (int i = 0; i < 100; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                Console.WriteLine("a = {0:G5}  b = {1:G5}", a, b);
            }

            Console.WriteLine();
            Console.WriteLine();

        }

        public static void TestInversion1()
        {
            Console.WriteLine("Testing Inversion of the Complex-Complex Jacobi SN");
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
                    Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, m, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }

        public static void TestInversion2()
        {
            Console.WriteLine("Testing Inversion of the Real-Real Jacobi SN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            for (int i = 0; i < COUNT; i++)
            {
                double a = rng.RandDouble(-1.0, 1.0);
                double m = rng.RandDouble(0.0, 1.0);

                //Cmplx b = arcSN(a, m);
                //Cmplx ap = Jacobi.SN(b, m);

                double b = arcSN(a, m);
                double ap = Jacobi.SN(b, m);

                double error = Math.Abs(a - ap);
                error = error / Math.Abs(a);
                stats.Add(error);

                //Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, ap, error);

                if (error > TOL)
                {
                    Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, ap, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }

        public static void TestInversion3()
        {
            Console.WriteLine("Testing Inversion of the Real-Real Jacobi SN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            for (int i = 0; i < COUNT; i++)
            {
                double a = rng.RandGauss(0.0, 2.5);
                double m = rng.RandDouble(0.0, 1.0);

                double b = Jacobi.SN(a, m);
                double c = arcSN(b, m);
                double bp = Jacobi.SN(c, m);

                double error = Math.Abs(b - bp);
                error = error / Math.Abs(a);
                stats.Add(error);

                //Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", b, bp, error);

                if (error > TOL)
                {
                    Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", b, bp, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }


        public static void TestCompatablity1()
        {
            Console.WriteLine("Testing Compatablity of the Real-Real Jacobi SN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            for (int i = 0; i < COUNT; i++)
            {
                double a = rng.RandGauss(0.0, 2.5);
                double m = rng.NextDouble();

                Cmplx b1 = Jacobi.SN(a, m);
                Cmplx b2 = Jacobi.SN((Cmplx)a, (Cmplx)m);

                double error = (b1 - b2).Abs;
                error = error / b2.Abs;
                stats.Add(error);

                double e2 = Math.Abs(b2.CofI);

                if (error > TOL || e2 > TOL)
                {
                    Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", b1.CofR, b2.CofR, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }


        public static void TestCompatablity2()
        {
            Console.WriteLine("Testing Compatablity of the Complex-Real Jacobi SN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                double m = rng.NextDouble();

                Cmplx b1 = Jacobi.SN(a, m);
                Cmplx b2 = Jacobi.SN(a, (Cmplx)m);

                double error = (b1 - b2).Abs;
                error = error / b2.Abs;
                stats.Add(error);

                //double e2 = Math.Abs(b2.CofI);

                if (error > TOL) // || e2 > TOL)
                {
                    Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", b1, b2, error);
                    failed = failed + 1;
                }

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }
    }
}
