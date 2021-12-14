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
        public const bool LOG = true;

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

        public static Cmplx arcSC(Cmplx x, Cmplx m)
        {
            Cmplx phi = Cmplx.Atan(x);
            return Jacobi.F(phi, m);
        }

        public static void ListSamples()
        {
            VRandom rng = new RandXOR();

            for (int i = 0; i < 100; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                //Console.WriteLine("a = {0:G5}  b = {1:G5}", a, b);

                Console.WriteLine();
                Console.WriteLine("JacobiCD({0:G5}, {1:G5})", a, b);
                Console.WriteLine("new double[] {{ {0:G5}, {1:G5}, {2:G5}, {3:G5}, x, y }},", a.CofR, a.CofI, b.CofR, b.CofI);
                
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

            DateTime start = DateTime.Now;

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
                    if (LOG) Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, m, error);
                    failed = failed + 1;
                }

            }

            TimeSpan runtime = DateTime.Now - start;         


            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Runtime: " + runtime);
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


        public static void TestInversion4()
        {
            Console.WriteLine("Testing Inversion of the Complex-Complex Jacobi CN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            DateTime start = DateTime.Now;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx p = RandCmplx(rng);
                Cmplx m = RandCmplx(rng);
                Cmplx x1 = Cmplx.Cos(p);

                //m = 0.5;

                //Cmplx b = Jacobi.ArcCN(a, m);
                //Cmplx ap = Jacobi.CN(b, m);

                Cmplx u = Jacobi.F(p, m);
                Cmplx x2 = Jacobi.CN(u, m);
                Cmplx p2 = Cmplx.Acos(x2);
                

                double error = (x1 - x2).Abs;
                error = error / x1.Abs;
                stats.Add(error);

                if (error > TOL)
                {
                    if (LOG) Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", x1, x2, error);
                    failed = failed + 1;
                }

            }

            TimeSpan runtime = DateTime.Now - start;


            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Runtime: " + runtime);
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }

        public static void TestInversion5()
        {
            Console.WriteLine("Testing Inversion of the Complex-Complex Jacobi DN");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            DateTime start = DateTime.Now;

            for (int i = 0; i < COUNT; i++)
            {
                //Cmplx a = RandCmplx(rng);
                //Cmplx m = RandCmplx(rng);

                ////m = 0.5;

                //Cmplx b = Jacobi.ArcDN(a, m);
                //Cmplx ap = Jacobi.DN(b, m);

                Cmplx p = RandCmplx(rng);
                Cmplx m = RandCmplx(rng);

                Cmplx u = Jacobi.F(p, m);
                Cmplx x1 = Jacobi.DN(u, m);

                Cmplx x2 = Cmplx.Sin(p);
                x2 = m * x2 * x2;
                x2 = Cmplx.Sqrt(1.0 - x2);

                double error = (x1 - x2).Abs;
                error = error / x2.Abs;
                stats.Add(error);

                if (error > TOL)
                {
                    if (LOG) Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", x1, x2, error);
                    failed = failed + 1;
                }

            }

            TimeSpan runtime = DateTime.Now - start;


            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Runtime: " + runtime);
            Console.WriteLine();

            Console.WriteLine("Max:   {0:E5}", stats.Max[0]);
            Console.WriteLine("Mean:  {0:E5}", stats.Mean[0]);
            Console.WriteLine("SD:    {0:E5}", stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }

        public static void TestInversion6()
        {
            Console.WriteLine("Testing Inversion of the Complex-Complex Jacobi SC");
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;

            DateTime start = DateTime.Now;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx m = RandCmplx(rng);

                //m = 0.5;

                Cmplx b = arcSC(a, m);
                Cmplx ap = Jacobi.SC(b, m);

                double error = (a - ap).Abs;
                error = error / a.Abs;
                stats.Add(error);

                if (error > TOL)
                {
                    if (LOG) Console.WriteLine("{0:0.00}, {1:0.00} = {2:G5}", a, ap, error);
                    failed = failed + 1;
                }

            }

            TimeSpan runtime = DateTime.Now - start;


            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Runtime: " + runtime);
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
