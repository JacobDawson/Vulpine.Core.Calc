using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.RandGen;

namespace QuickTests
{
    public static class LambertTests
    {
        public const double LW = -0.3678794411714423216;

        public const int Max_Ittr = 1000;

        public const int Num_Tests = 1000;

        public const double SD = 50.0;

        public const double MaxVal = 0.02 * Math.E;


        public static void RunTests()
        {
            int count = 0;
            double error = 1.0;
            double acc = 1.0;
            double x = 0.0;
            Cmplx result = 0.0;
            Cmplx z = 0.0;
            Cmplx z0 = 0.0;

            VRandom rng = new RandMT();
            StatRunner csr = new StatRunner();
            StatRunner esr = new StatRunner();
            StatRunner asr = new StatRunner();

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Newton's Method...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                z = RandCmplx(rng, SD);
                result = Lambert1(z, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0, z);

                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Newton's Method With Log Transform...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                z = RandCmplx(rng, SD);
                result = Lambert2(z, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0, z);

                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Halye's Method...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                z = RandCmplx(rng, SD);
                result = Lambert3(z, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0, z);

                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Complex Function With Real Arguments...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                x = rng.RandDouble(LW, MaxVal);
                result = Lambert4(x, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0.CofR, x);
                
                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Real Function With Newtons Method...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                x = rng.RandDouble(LW, MaxVal);
                result = Lambert6(x, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0.CofR, x);

                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);

            //////////////////////////////////////////////////////////////////

            Console.WriteLine("Running Tests For Real Function With LOG Newtons Method...");
            Console.WriteLine();

            csr.Reset();
            esr.Reset();
            asr.Reset();

            for (int i = 0; i < Num_Tests; i++)
            {
                x = rng.RandDouble(LW, MaxVal);
                result = Lambert7(x, out count, out error);
                z0 = LambertInv(result);
                acc = VMath.Error(z0.CofR, x);

                csr.Add(count);
                esr.Add(error);
                asr.Add(acc);
            }

            Console.WriteLine("   Max Itterations: " + csr.Max[0]);
            Console.WriteLine("   Avg Itterations: " + csr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Percission Error: " + esr.Max[0]);
            Console.WriteLine("   Avg Percission Error: " + esr.Mean[0]);
            Console.WriteLine();
            Console.WriteLine("   Max Accuracy Error: " + asr.Max[0]);
            Console.WriteLine("   Avg Accuracy Error: " + asr.Mean[0]);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey(true);
            
        }


        public static Cmplx RandCmplx(VRandom rng, double sd)
        {
            double r = rng.RandGauss(0.0, sd);
            double i = rng.RandGauss(0.0, sd);

            return new Cmplx(r, i);
        }


        public static Cmplx LambertInv(Cmplx z)
        {
            //computes the inverse Lambert W funciton
            return z * Cmplx.Exp(z);
        }


        

        public static Cmplx LambertAprox(Cmplx z)
        {
            Cmplx z0, y, lnB, lnC;

            //atempts to find an aproximate value to W(z)
            y = 2.0 * Math.E * z + 2.0;
            y = Cmplx.Sqrt(y);

            lnB = Cmplx.Log(1.0 + 0.8842 * y) * 2.0;
            lnC = Cmplx.Log(1.0 + 0.5106 * y) * 0.9294;

            z0 = lnB - Cmplx.Log(1.0 + lnC) - 1.213;
            z0 = z0 / (1.0 + (1.0 / (lnB + 4.688)));

            return z0;
        }


        public static Cmplx Lambert1(Cmplx z, out int count, out double e)
        {
            count = 0;
            e = 1.0;

            //atempts to find an aproximate value to W(z)
            Cmplx z0 = LambertAprox(z);

            Cmplx wn, e1, e2;

            //uses newton's method to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                e1 = Cmplx.Exp(z0);
                e2 = z0 * e1;

                wn = (e2 - z) / (e1 + e2);
                wn = z0 - wn;
                
                e = VMath.Error(z0, wn);
                z0 = wn;
                count++;
            }

            return z0;
        }


        public static Cmplx Lambert2(Cmplx z, out int count, out double e)
        {
            count = 0;
            e = 1.0;

            //atempts to find an aproximate value to W(z)
            Cmplx z0 = LambertAprox(z);
        
            Cmplx lz = Cmplx.Log(z);
            Cmplx wn, fz, dz;

            //uses newton's method with LOG to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                fz = z0 + Cmplx.Log(z0) + lz;
                dz = (1.0 / z0) + 1.0;
                wn = z0 - (fz / dz);

                e = VMath.Error(z0, wn);
                z0 = wn;
                count++;
            }

            return z0;
        }


        public static Cmplx Lambert3(Cmplx z, out int count, out double e)
        {
            count = 0;
            e = 1.0;

            //atempts to find an aproximate value to W(z)
            Cmplx z0 = LambertAprox(z);

            Cmplx lz = Cmplx.Log(z);
            Cmplx wn, e1, e2;

            //uses haleys's method to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                e1 = Cmplx.Exp(z0);
                e2 = z0 * e1;

                wn = 2.0 * z0 + 2.0;
                wn = (z0 + 2.0) * (e2 - z) / wn;
                wn = e1 * (z0 + 1.0) - wn;
                wn = (e2 - z) / wn;
                wn = z0 - wn;

                e = VMath.Error(z0, wn);
                z0 = wn;
                count++;
            }

            return z0;
        }


        public static double Lambert4(double x, out int count, out double e)
        {
            count = 0;
            e = 1.0;

            //makes shure we stay within the defined range
            if (x < LW) return Double.NaN; 

            //atempts to find an aproximate value to W(z)
            double z0 = LambertAprox(x).CofR;

            double wn, e1, e2;

            //uses newton's method to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                e1 = Math.Exp(z0);
                e2 = z0 * e1;

                wn = (e2 - x) / (e1 + e2);
                wn = z0 - wn;

                e = VMath.Error(z0, wn);
                z0 = wn;
                count++;
            }

            return z0;
        }


        //public double Lambert5(double x, out int count, out double e)
        //{
        //    count = 0;
        //    e = 1.0;
        //    double x0 = 0.0;

        //    if (x > Math.E) //x > e
        //    {
        //        x0 = Math.Log(x);
        //        x0 = x0 - Math.Log(x0);
        //    }
        //    else if (x > 0) //0 > x >= e
        //    {
        //        x0 = x / Math.E;
        //    }
        //    else if (x > LW) //LW > x >= 0
        //    {
        //        double ez = x * Math.E;
        //        double sr = Math.Sqrt(1.0 + ez);

        //        x0 = 1.0 + ez + sr;
        //        x0 = ez / x0;
        //        x0 = x0 * Math.Log(1.0 + sr);
        //    }
        //    else //x <= LW
        //    {
        //        return Double.NaN;
        //    }

        //    return Double.NaN;

        //}


        public static double Lambert6(double x, out int count, out double e)
        {
            count = 0;
            e = 1.0;
            double x0 = 0.0;

            if (x >= 0.0) //x > 0
            {
                double ln = Math.Log(1.0 + x);

                x0 = Math.Log(1.0 + ln);
                x0 = x0 / (2.0 + ln);
                x0 = ln * (1.0 - x0);
            }
            else if (x >= LW) //LW > x > 0
            {
                //x0 = 2.0 * Math.E * x + 2.0;
                //x0 = 1.0 / Math.Sqrt(x0);
                //x0 = x0 - 0.1251300743172211;
                //x0 = 1.0 + (1.0 / x0);
                //x0 = Math.E * x / x0;

                double ex = x * Math.E;
                double sr = Math.Sqrt(1.0 + ex);

                x0 = 1.0 + ex + sr;
                x0 = ex / x0;
                x0 = x0 * Math.Log(1.0 + sr);
            }
            else //x < LW
            {
                //the function is undefined for this range
                return Double.NaN;
            }

            double wn, e1, e2;

            //uses newton's method to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                e1 = Math.Exp(x0);
                e2 = x0 * e1;

                wn = (e2 - x) / (e1 + e2);
                wn = x0 - wn;

                e = VMath.Error(x0, wn);
                x0 = wn;
                count++;
            }

            return x0;
        }


        public static double Lambert7(double x, out int count, out double e)
        {
            count = 0;
            e = 1.0;
            double x0 = 0.0;

            if (x >= 0.0) //x > 0
            {
                double ln = Math.Log(1.0 + x);

                x0 = Math.Log(1.0 + ln);
                x0 = x0 / (2.0 + ln);
                x0 = ln * (1.0 - x0);
            }
            else if (x >= LW) //LW > x > 0
            {
                //x0 = 2.0 * Math.E * x + 2.0;
                //x0 = 1.0 / Math.Sqrt(x0);
                //x0 = x0 - 0.1251300743172211;
                //x0 = 1.0 + (1.0 / x0);
                //x0 = Math.E * x / x0;

                double ex = x * Math.E;
                double sr = Math.Sqrt(1.0 + ex);

                x0 = 1.0 + ex + sr;
                x0 = ex / x0;
                x0 = x0 * Math.Log(1.0 + sr);
            }
            else //x < LW
            {
                //the function is undefined for this range
                return Double.NaN;
            }

            double lz = Math.Log(x);
            double wn, fz, dz;

            //uses newton's method with LOG to fine-tune the aproximation
            while (count < Max_Ittr && e > VMath.TOL)
            {
                fz = x0 + Math.Log(x0) + lz;
                dz = (1.0 / x0) + 1.0;
                wn = x0 - (fz / dz);

                e = VMath.Error(x0, wn);
                x0 = wn;
                count++;
            }

            return x0;
        }

    }
}
