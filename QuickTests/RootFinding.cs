using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Algorithms;

namespace QuickTests
{
    public static class RootFinding
    {
        public static void Run()
        {

            var sw = new StreamWriter("root_tests.txt");
            var rf = new RootFinder(20, 0.0);

            //rf.StepEvent += delegate(Object o, StepEventArgs args)
            //{
            //    double loginv = -Math.Log10(args.Error);
            //    sw.WriteLine("{0}, {1:g8}, {2:g8}", args.Step, args.Error, loginv);
            //};

            #region Function_1

            sw.WriteLine();
            Console.WriteLine("f(x) = cos(x) - x^3 :: x = [0 .. 1.5]");
            sw.WriteLine("f(x) = cos(x) - x^3 :: x = [0 .. 1.5]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => Math.Cos(x) - (x * x * x), 0.0, 1.5);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Position: ");
            rf.FalsePos(x => Math.Cos(x) - (x * x * x), 0.0, 1.5);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => Math.Cos(x) - (x * x * x), 0.0, 1.5);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => Math.Cos(x) - (x * x * x), 0.0, 1.5);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => Math.Cos(x) - (x * x * x), 0.0, 1.5);
            sw.Flush();

            #endregion

            #region Function2:

            sw.WriteLine();
            Console.WriteLine("f(x) = sin(x) - log(x) :: x = [1 .. 3]");
            sw.WriteLine("f(x) = sin(x) - log(x) :: x = [1 .. 3]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => Math.Sin(x) - Math.Log(x), 1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => Math.Sin(x) - Math.Log(x), 1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => Math.Sin(x) - Math.Log(x), 1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => Math.Sin(x) - Math.Log(x), 1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => Math.Sin(x) - Math.Log(x), 1.0, 3.0);
            sw.Flush();

            #endregion

            #region Funciton3:

            sw.WriteLine();
            Console.WriteLine("f(x) = x^x - 3 :: x = [1.5 .. 2]");
            sw.WriteLine("f(x) = x^x - 3 :: x = [1.5 .. 2]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => Math.Pow(x, x) - 3.0, 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => Math.Pow(x, x) - 3.0, 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => Math.Pow(x, x) - 3.0, 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => Math.Pow(x, x) - 3.0, 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => Math.Pow(x, x) - 3.0, 1.0, 2.0);
            sw.Flush();

            #endregion

            #region Function4:

            sw.WriteLine();
            Console.WriteLine("f(x) = cos(x) - log(x) :: x = [1 .. 2]");
            sw.WriteLine("f(x) = cos(x) - log(x) :: x = [1 .. 2]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => Math.Cos(x) - Math.Log(x), 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => Math.Cos(x) - Math.Log(x), 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => Math.Cos(x) - Math.Log(x), 1.0, 2.0); ;
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => Math.Cos(x) - Math.Log(x), 1.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => Math.Cos(x) - Math.Log(x), 1.0, 2.0);
            sw.Flush();

            #endregion

            #region Function5:

            sw.WriteLine();
            Console.WriteLine("f(x) = 2x^2 + 7x - 16 :: x = [-1 .. 3]");
            sw.WriteLine("f(x) = 2x^2 + 7x - 16 :: x = [-1 .. 3]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => (2.0 * x * x) + (7.0 * x) - 16, -1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => (2.0 * x * x) + (7.0 * x) - 16, -1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => (2.0 * x * x) + (7.0 * x) - 16, -1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => (2.0 * x * x) + (7.0 * x) - 16, -1.0, 3.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => (2.0 * x * x) + (7.0 * x) - 16, -1.0, 3.0);
            sw.Flush();

            #endregion

            #region Function6:

            sw.WriteLine();
            Console.WriteLine("f(x) = 2^(2x - 4) - 3 :: x = [1 .. 4]");
            sw.WriteLine("f(x) = 2^(2x - 4) - 3 :: x = [1 .. 4]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => Math.Pow(2.0, (2.0 * x) - 4.0) - 3.0, 1.0, 4.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => Math.Pow(2.0, (2.0 * x) - 4.0) - 3.0, 1.0, 4.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => Math.Pow(2.0, (2.0 * x) - 4.0) - 3.0, 1.0, 4.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => Math.Pow(2.0, (2.0 * x) - 4.0) - 3.0, 1.0, 4.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => Math.Pow(2.0, (2.0 * x) - 4.0) - 3.0, 1.0, 4.0);
            sw.Flush();

            #endregion

            #region Function7:

            sw.WriteLine();
            Console.WriteLine("f(x) = x! - 50 :: x = [4 .. 5]");
            sw.WriteLine("f(x) = x! - 50 :: x = [4 .. 5]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => VMath.Gamma(x + 1.0) - 50.0, 4.0, 5.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => VMath.Gamma(x + 1.0) - 50.0, 4.0, 5.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => VMath.Gamma(x + 1.0) - 50.0, 4.0, 5.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => VMath.Gamma(x + 1.0) - 50.0, 4.0, 5.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => VMath.Gamma(x + 1.0) - 50.0, 4.0, 5.0);
            sw.Flush();

            #endregion

            #region Function8:

            sw.WriteLine();
            Console.WriteLine("f(x) = erf(x) - 4/5 :: x = [-2 .. 2]");
            sw.WriteLine("f(x) = erf(x) - 4/5 :: x = [-2 .. 2]");

            sw.WriteLine();
            sw.WriteLine("Bisection: ");
            rf.Bisection(x => VMath.Erf(x) - 0.8, -2.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("False Postion: ");
            rf.FalsePos(x => VMath.Erf(x) - 0.8, -2.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Ridders Method: ");
            rf.Ridders(x => VMath.Erf(x) - 0.8, -2.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Brents Method: ");
            rf.Brent(x => VMath.Erf(x) - 0.8, -2.0, 2.0);
            sw.Flush();

            sw.WriteLine();
            sw.WriteLine("Secant Method: ");
            rf.Secant(x => VMath.Erf(x) - 0.8, -2.0, 2.0);
            sw.Flush();

            #endregion

            Console.WriteLine("Done!");


        }

    }
}
