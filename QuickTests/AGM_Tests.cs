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
    public static class AGM_Tests
    {
        public const int COUNT = 100000; //000;
        public const double TOL = 1.0e-12;

        public static void CountTest()
        {
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int ittr = 0;

            int b0 = 0;
            int b1 = 0;
            int b2 = 0;
            int b3 = 0;
            int b4 = 0;
            int b5 = 0;
            int b6 = 0;
            int b7 = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                AGM(a, b, out ittr);

                Console.Write("{0}, ", ittr);
                stats.Add(ittr);

                if (ittr < 8) b0++;
                else if (ittr < 16) b1++;
                else if (ittr < 24) b2++;
                else if (ittr < 32) b3++;
                else if (ittr < 40) b4++;
                else if (ittr < 48) b5++;
                else if (ittr < 56) b6++;
                else b7++;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:   " + stats.Max[0]);
            Console.WriteLine("Mean:  " + stats.Mean[0]);
            Console.WriteLine("SD:    " + stats.SD);
            Console.WriteLine();

            Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", b0, b1, b2, b3, b4, b5, b6, b7);
            Console.WriteLine();
        }


        public static void TestError()
        {
            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            StatRunner results = new StatRunner(2);

            double error;

            Console.WriteLine("Running {0} tests per level: ", COUNT);
            Console.WriteLine();

            for (int i = 1; i < 16; i++)
            {
                rng.Reset();
                stats.Reset();
                results.Reset();

                for (int j = 0; j < COUNT; j++)
                {
                    Cmplx a = RandCmplx(rng);
                    Cmplx b = RandCmplx(rng);

                    Cmplx res = AGM(a, b, i, out error);

                    stats.Add(error);
                    results.Add(res);
                }

                Console.WriteLine("Level: {0:00} \t Max: {1} \t Avg: {2} \t SD: {3}",
                    i, stats.Max[0], stats.Mean[0], stats.SD);

                Cmplx min = (Cmplx)results.Min;
                Cmplx max = (Cmplx)results.Max;
                Cmplx avg = (Cmplx)results.Mean;

                Console.WriteLine("Level: {0:00} \t {1} \t {2}",
                    i, avg, results.SD);
            }

            Console.WriteLine();
            Console.WriteLine();
        }


        public static void TestComp1()
        {
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;
            int skipped = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                Cmplx t1 = AGM(a, b);
                Cmplx t2 = AGM3(a, b);

                double e = (t1 - t2).Abs / t1.Abs;
                
                if (e > 4.0)
                {
                    skipped++;
                    continue;
                }

                if (e > TOL)
                {
                    Console.WriteLine("{0}, {1}, {2}", a, b, e);
                    failed++;
                }

                stats.Add(e);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:     " + stats.Max[0]);
            Console.WriteLine("Mean:    " + stats.Mean[0]);
            Console.WriteLine("SD:      " + stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Skipped: " + skipped);
            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }


        public static void TestComp2()
        {
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;
            int skipped = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                Cmplx t1 = AGM(a, b);
                Cmplx t2 = AGM2(a, b);

                if (t2.IsNaN())
                {
                    skipped++;
                    continue;
                }

                double e = (t1 - t2).Abs / t1.Abs;

                if (e > TOL)
                {
                    Console.WriteLine("{0}, {1}, {2}", a, b, e);
                    failed++;
                }

                stats.Add(e);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:     " + stats.Max[0]);
            Console.WriteLine("Mean:    " + stats.Mean[0]);
            Console.WriteLine("SD:      " + stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            Console.WriteLine("Skipped: " + skipped);
            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }



        public static void TestComp4()
        {
            Console.WriteLine("Running {0} Itterations: ", COUNT);
            Console.WriteLine();

            VRandom rng = new RandXOR();
            StatRunner stats = new StatRunner();
            int failed = 0;
            //int skipped = 0;

            for (int i = 0; i < COUNT; i++)
            {
                Cmplx a = RandCmplx(rng);
                Cmplx b = RandCmplx(rng);

                Cmplx t1 = AGM(a, b);
                Cmplx t2 = AGM5(a, b);

                //if (t2.IsNaN())
                //{
                //    skipped++;
                //    continue;
                //}

                double e = (t1 - t2).Abs / t1.Abs;

                if (e > TOL)
                {
                    //Console.WriteLine("{0}, {1}, {2}", a, b, e);
                    failed++;
                }

                stats.Add(e);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Max:     " + stats.Max[0]);
            Console.WriteLine("Mean:    " + stats.Mean[0]);
            Console.WriteLine("SD:      " + stats.SD);
            Console.WriteLine();

            double percent = (double)failed / (double)COUNT;

            //Console.WriteLine("Skipped: " + skipped);
            Console.WriteLine("Failed:  " + failed);
            Console.WriteLine("Percent: " + (percent * 100.0));
        }




        public static Cmplx RandCmplx(VRandom rng)
        {
            double a = rng.RandGauss(0.0, 2.5);
            double b = rng.RandGauss(0.0, 2.5);

            return new Cmplx(a, b);
        }

        public static Cmplx AGM(Cmplx a, Cmplx b, out int count)
        {
            Cmplx an, bn;
            double t1, t2;

            count = 0;

            for (int i = 0; i < 64; i++)
            {
                //computes the aretmentic and geometric means
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);

                //makes shure we choose the right brance of sqrt
                t1 = (an - bn).Abs;
                t2 = (an + bn).Abs;
                if (t1 > t2)
                {
                    bn = -bn;
                    count++;
                }

                //takes care of special edge cases
                if (t1 == t2)
                {
                    t1 = (bn / an).CofI;
                    if (t1 <= 0) bn = -bn;
                }

                a = an;
                b = bn;
            }

            return a;

        }

        public static Cmplx AGM(Cmplx a, Cmplx b)
        {
            Cmplx an, bn;
            double t1, t2;

            //count = 0;

            for (int i = 0; i < 64; i++)
            {
                //computes the aretmentic and geometric means
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);

                //makes shure we choose the right brance of sqrt
                t1 = (an - bn).Abs;
                t2 = (an + bn).Abs;
                if (t1 > t2)
                {
                    bn = -bn;
                    //count++;
                }

                //takes care of special edge cases
                if (t1 == t2)
                {
                    t1 = (bn / an).CofI;
                    if (t1 <= 0) bn = -bn;
                }

                a = an;
                b = bn;
            }

            return a;

        }

        public static Cmplx GM(Cmplx a, Cmplx b)
        {
            double t1 = a.Arg;
            double t2 = b.Arg;

            //NOTE: need to ensure |t1 - t2| < pi


            if (Math.Abs(t1 - t2) > Math.PI)
            {
                return Cmplx.NaN;
            }


            double sr = (a * b).Abs;
            sr = Math.Sqrt(sr);

            double ta = (t1 + t2) / 2.0;
            Cmplx tai = new Cmplx(0.0, ta);

            Cmplx c = Cmplx.Exp(tai) * sr;

            return c;
        }

        public static Cmplx AGM2(Cmplx a, Cmplx b)
        {
            Cmplx an, bn;

            for (int i = 0; i < 64; i++)
            {
                an = (a + b) / 2.0;
                bn = GM(a, b);

                if (bn.IsNaN()) return bn;

                a = an;
                b = bn;
            }

            return a;
        }

        public static Cmplx AGM3(Cmplx a, Cmplx b)
        {
            Cmplx xx = a * a;
            Cmplx yy = b * b;

            VFunc<Cmplx> intgrand = delegate(Cmplx z)
            {
                Cmplx cos2 = Cmplx.Cos(z);
                Cmplx sin2 = Cmplx.Sin(z);

                cos2 = cos2 * cos2;
                sin2 = sin2 * sin2;

                Cmplx rad = (xx * cos2) + (yy * sin2);
                return 1.0 / Cmplx.Sqrt(rad);
            };

            Integrator ing = new Integrator();

            Cmplx c = ing.DubExp(intgrand, 0.0, Math.PI / 2.0);

            return Math.PI / (2.0 * c);

        }


        public static Cmplx AGM4(Cmplx a, Cmplx b)
        {
            Cmplx an = (a + b) / 2.0;
            Cmplx bn = Cmplx.Sqrt(a * b);

            //makes shure we select the correct branch
            double t1 = (an - bn).Abs;
            double t2 = (an + bn).Abs;
            bool branch = t1 > t2;

            //if (t1 > t2)
            //{
            //    bn = -bn;
            //    branch = true;
            //}

            //iterativly computes the aretmentic and geometric means
            for (int i = 0; i < 64; i++)
            {
                a = an;
                b = branch ? -bn : bn;
               
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);
            }

            return an;

        }

        public static Cmplx AGM5(Cmplx a, Cmplx b)
        {
            Cmplx an, bn;
            double t1, t2;

            //count = 0;

            for (int i = 0; i < 64; i++)
            {
                //computes the aretmentic and geometric means
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);

                //makes shure we choose the right brance of sqrt
                t1 = (an - bn).Abs;
                t2 = (an + bn).Abs;
                if (t1 > t2)
                {
                    bn = -bn;
                    //count++;
                }

                ////takes care of special edge cases
                //if (t1 == t2)
                //{
                //    t1 = (bn / an).CofI;
                //    if (t1 <= 0) bn = -bn;
                //}

                a = an;
                b = bn;
            }

            return a;

        }


        public static Cmplx AGM(Cmplx a, Cmplx b, int level, out double error)
        {
            Cmplx an, bn;
            double t1, t2;

            //count = 0;

            for (int i = 0; i < level; i++)
            {
                //computes the aretmentic and geometric means
                an = (a + b) / 2.0;
                bn = Cmplx.Sqrt(a * b);

                //makes shure we choose the right brance of sqrt
                t1 = (an - bn).Abs;
                t2 = (an + bn).Abs;
                if (t1 > t2)
                {
                    bn = -bn;
                    //count++;
                }

                ////takes care of special edge cases
                //if (t1 == t2)
                //{
                //    t1 = (bn / an).CofI;
                //    if (t1 <= 0) bn = -bn;
                //}

                a = an;
                b = bn;
            }

            error = (a - b).Abs / a.Abs;

            return a;

        }

    }
}
