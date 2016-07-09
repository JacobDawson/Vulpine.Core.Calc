using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickTests
{
    public static class DivByZero
    {
        public static void Run()
        {
            InfinityComparison();
            DivByZeroTest();
            LogNearZero();
            MullByInfTest();
            NormaliseTest();
        }

        public static void LogNearZero()
        {
            Console.Clear();

            double test = Double.Epsilon;
            Console.WriteLine("e = {0}", test);

            test = 1.0 / Double.MaxValue;
            Console.WriteLine("E = {0}", test);

            test = Math.Log(Double.Epsilon);
            Console.WriteLine("Log(e) = {0}", test);

            test = Math.Log(1.0 / Double.MaxValue);
            Console.WriteLine("Log(E) = {0}", test);

            Console.ReadKey(true);
        }

        private static void InfinityComparison()
        {
            bool test = Double.PositiveInfinity > 1.0;
            string s = test ? "true" : "false";
            Console.WriteLine("Inf > 1 = {0}", test);

            test = Double.NegativeInfinity < -1.0;
            s = test ? "true" : "false";
            Console.WriteLine("-Inf < -1 = {0}", test);

            test = Double.NaN > 1.0;
            s = test ? "true" : "false";
            Console.WriteLine("NaN > 1 = {0}", test);

            test = Double.NaN< -1.0;
            s = test ? "true" : "false";
            Console.WriteLine("NaN < -1 = {0}", test);

            test = Double.PositiveInfinity < Double.PositiveInfinity;
            Console.WriteLine("Inf < Inf = {0}", test);

            test = Double.PositiveInfinity <= Double.PositiveInfinity;
            Console.WriteLine("Inf <= Inf = {0}", test);
        }

        private static void DivByZeroTest()
        {
            double a = 0.0 / 0.0;
            Console.WriteLine("0/0 = {0}", a.ToString());
 
            a = 1.0 / 0.0;
            Console.WriteLine("1/0 = {0}", a.ToString());

            a = -1.0 / 0.0;
            Console.WriteLine("-1/0 = {0}", a.ToString());

            a = 1.0 / Double.Epsilon;
            Console.WriteLine("1/e = {0}", a.ToString());

            a = -1.0 / Double.Epsilon;
            Console.WriteLine("-1/e = {0}", a.ToString());

            a = 0.0 / Double.Epsilon;
            Console.WriteLine("0/e = {0}", a.ToString());

            a = 0.0 / 1.0;
            Console.WriteLine("0/1 = {0}", a.ToString());

            a = 0.0 / -1.0;
            Console.WriteLine("0/-1 = {0}", a.ToString());

            a = 100000000000.0 / 0.000000000001;
            Console.WriteLine("1e12/1e-12 = {0}", a.ToString());

            Console.ReadKey(true);
        }

        private static void MullByInfTest()
        {
            double a = 0.0 * Double.PositiveInfinity;
            Console.WriteLine("0*Inf= {0}", a.ToString());

            a = 0.0 * Double.NegativeInfinity;
            Console.WriteLine("0*-Inf = {0}", a.ToString());

            a = 1.0 * Double.PositiveInfinity;
            Console.WriteLine("1.0*Inf= {0}", a.ToString());

            a = -1.0 * Double.PositiveInfinity;
            Console.WriteLine("-1.0*Inf= {0}", a.ToString());

            a = Double.PositiveInfinity * Double.PositiveInfinity;
            Console.WriteLine("Inf*Inf= {0}", a.ToString());

            Console.ReadKey(true);
        }

        private static void NormaliseTest(double a, double b, double c)
        {
            Console.Clear();
            double temp = (a * a) + (b * b) + (c * c);

            Console.WriteLine();
            Console.WriteLine("a = {0}", a.ToString());
            Console.WriteLine("b = {0}", b.ToString());
            Console.WriteLine("c = {0}", c.ToString());
            Console.WriteLine("t = {0}", temp.ToString());

            temp = 1.0 / Math.Sqrt(temp);
            a = a * temp;
            b = b * temp;
            c = c * temp;

            Console.WriteLine();
            Console.WriteLine("a' = {0}", a.ToString());
            Console.WriteLine("b' = {0}", b.ToString());
            Console.WriteLine("c' = {0}", c.ToString());
            Console.WriteLine("t' = {0}", temp.ToString());

            Console.ReadKey();
        }

        private static void NormaliseTest()
        {
            NormaliseTest(1.0, 0.0, 0.0);
            NormaliseTest(0.0, 0.0, 0.0);
            NormaliseTest(Double.MaxValue, 0.0, 0.0);
            NormaliseTest(Double.Epsilon, 0.0, 0.0);
            NormaliseTest(-Double.Epsilon, 0.0, 0.0);
            NormaliseTest(0.0, Double.Epsilon, -Double.Epsilon);

            double bad = Math.Sqrt(Double.MaxValue / 2.0);
            NormaliseTest(bad, bad, bad);

        }


       
        
    }
}
