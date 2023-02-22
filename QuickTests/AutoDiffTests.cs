using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.RandGen;

namespace QuickTests
{
    public class AutoDiffTestCase
    {
        public string name;

        public double min_x;
        public double max_x;

        public VFunc<Dual> fx;
        public VFunc dx;

        public AutoDiffTestCase(String name, VFunc<Dual> fx, VFunc dx, double min_x, double max_x)
        {
            this.name = name;
            this.min_x = min_x;
            this.max_x = max_x;
            this.fx = fx;
            this.dx = dx;
        }
    }




    public static class AutoDiffTests
    {

        public const int Num_Probes = 1000;

        public static void RunTests()
        {
            VRandom rng = new RandMT();
            StatRunner stats = new StatRunner();

            for (int i = 0; i < cases.Length; i++)
            {
                var test = cases[i];

                Console.Clear();
                Console.WriteLine();

                Console.WriteLine(test.name);
                Console.WriteLine();

                stats.Reset();

                for (int n = 0; n < Num_Probes; n++)
                {
                    double x = rng.RandDouble(test.min_x, test.max_x);
                    Dual d = new Dual(x, 1.0);

                    Dual dx = test.fx(d);
                    double dxp = test.dx(x);

                    double error = VMath.Error(dx.Eps, dxp);
                    stats.Add(error);
                }

                double max = stats.Max[0];
                double avg = stats.Mean[0];

                Console.Write("Max Error: " + max);
                if (max < VMath.TOL) Console.WriteLine();
                else Console.WriteLine("\t !!!");

                Console.Write("Avg Error: " + avg);
                if (avg < VMath.TOL) Console.WriteLine();
                else Console.WriteLine("\t !!!");

                Console.WriteLine();
                Console.WriteLine("Press Key For Next Test...");
                Console.ReadKey(true);
            }

        }


        public static readonly AutoDiffTestCase[] cases =
        {
            new AutoDiffTestCase ( 
                "f(x) = x^x", 
                x => Dual.Pow(x, x), 
                x => Math.Pow(x, x) * (Math.Log(x) + 1.0), 
                0.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = x^x^x",
                x => Dual.Pow(x, Dual.Pow(x, x)),
                x => { 
                    //x^(x^x + x - 1) (x log^2(x) + x log(x) + 1)
                    double ln = Math.Log(x);
                    double t = Math.Pow(x, x);
                    t = Math.Pow(x, (t + x - 1.0)); 
                    return t * (x * ln * ln + x * ln + 1.0);
                },
                0.0, 2.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(sqrt((x^2 + 1)/(x + 3)))",
                x => Dual.Log(Dual.Sqrt((x * x + 1.0) / (x + 3.0))),
                x => {
                    //(x (x + 6) - 1)/(x (x (2 x + 6) + 2) + 6)
                    double q = 2.0 * x + 6.0;
                    q = x * q + 2.0;
                    q = x * q + 6.0;
                    double p = x + 6.0;
                    p = x * p - 1.0;
                    return p / q;
                },
                -3.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = x^2 cos(1 / x^3)",
                x => x * x * Dual.Cos(1.0 / (x * x * x)),
                x => {
                    //2 x cos(1/x^3) + (3 sin(1/x^3))/x^2
                    double x2 = x * x;
                    double x3 = x2 * x;
                    double cos = Math.Cos(1.0 / x3);
                    double sin = Math.Sin(1.0 / x3);
                    return 2.0 * x * cos + (3.0 * sin) / x2;
                },
                -3.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = (3 x - 1) / sqrt(x)",
                x => (3.0 * x - 1.0) / Dual.Sqrt(x),
                x => {
                    //(3 x + 1)/(2 x^(3/2))
                    double p = 3.0 * x + 1.0;
                    double q = 2.0 * Math.Pow(x, 3.0 / 2.0);
                    return p / q;
                },
                0.0, 4.0 ),

            new AutoDiffTestCase (
                "f(x) = (sqrt(x) - 1) / (sqrt(x) + 1)",
                x => { 
                    Dual s = Dual.Sqrt(x);
                    return (s - 1.0) / (s + 1.0);
                },
                x => {
                    //1/((sqrt(x) + 1)^2 sqrt(x))
                    double s = Math.Sqrt(x);
                    double t = s + 1.0;
                    return 1.0 / (t * t * s);
                },
                0.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = (2 x - sqrt(x) x) / x^2",
                x => (2.0 * x - Dual.Sqrt(x) * x) / (x * x),
                x => (Math.Sqrt(x) - 4.0) / (2.0 * x * x),
                0.0, 30.0 ),

            new AutoDiffTestCase (
                "f(x) = sin(x) cos(x)",
                x => Dual.Sin(x) * Dual.Cos(x),
                x => Math.Cos(2.0 * x),
                -VMath.TAU, VMath.TAU ),

            new AutoDiffTestCase (
                "f(x) = sec(sqrt(1 + x^2))",
                x => 1.0 / Dual.Cos(Dual.Sqrt(1.0 + x * x)),
                x => {
                    //(x sin(sqrt(1 + x^2)))/(sqrt(1 + x^2) cos^2(sqrt(1 + x^2)))
                    double s = Math.Sqrt(1.0 + x * x);
                    double p = x * Math.Sin(s);
                    double q = Math.Cos(s);
                    return p / (s * q * q);
                },
                -6.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = tan(sin(x^3))^2",
                x => {
                    Dual t = Dual.Tan(Dual.Sin(x * x * x));
                    return t * t;
                },
                x => {
                    //(6 x^2 cos(x^3) sin(sin(x^3)))/(cos^3(sin(x^3)))
                    double x2 = x * x;
                    double x3 = x * x2;
                    double sin = Math.Sin(x3);
                    double p = 6.0 * x2 * Math.Cos(x3) * Math.Sin(sin);
                    double q = Math.Cos(sin);
                    return p / (q * q * q);
                },
                -2.0, 2.0 ),

            new AutoDiffTestCase (
                "f(x) = (x^3 + 4)^2 / (x^2 + 1)^4",
                x => {
                    Dual x2 = x * x;
                    Dual p = (x * x2 + 4.0);
                    Dual q = (x2 + 1.0);
                    Dual q2 = q * q;
                    return (p * p) / (q2 * q2);
                },
                x => {
                    //-(2 x (x^3 + 4) (x^3 - 3 x + 16))/(x^2 + 1)^5
                    double x2 = x * x;
                    double x3 = x * x2;
                    double p = -(2.0 * x * (x3 + 4.0) * (x3 - 3.0 * x + 16.0));
                    double q = x2 + 1.0;
                    double q2 = q * q;
                    return p / (q2 * q2 * q);
                },
                -5.0, 5.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x) + x^3", 
                x => Dual.Log(x) + Dual.Pow(x, 3.0),
                x => 3.0 * x * x + (1.0 / x),
                0.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = x^2 e^x",
                x => x * x * Dual.Exp(x),
                x => x * Math.Exp(x) * (x + 2.0),
                -1.0, 1.0 ),

            new AutoDiffTestCase (
                "f(x) = e^(x cos(2x))",
                x => Dual.Exp(x * Dual.Cos(x + x)),
                x => {
                    //e^(x cos(2 x)) (cos(2 x) - 2 x sin(2 x))
                    double x2 = 2.0 * x;
                    double cos = Math.Cos(x2);
                    double p = 2.0 * x * Math.Sin(x2);
                    return Math.Exp(x * cos) * (cos - p);
                },
                -3.5, 3.5 ),

            new AutoDiffTestCase (
                "f(x) = sqrt(1 + 2 e^(3 x))",
                x => Dual.Sqrt(1.0 + 2.0 * Dual.Exp(x + x + x)),
                x => {
                    //(3 e^(3 x))/sqrt(1 + 2 e^(3 x))
                    double t = Math.Exp(3.0 * x);
                    return (3.0 * t) / Math.Sqrt(1.0 + 2.0 * t);
                },
                -2.0, 2.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x^4 sin(x)^2)",
                x => {
                    Dual x2 = x * x;
                    Dual sin = Dual.Sin(x);
                    return Dual.Log(x2 * x2 * sin * sin);
                },
                x => (4.0 / x) + (2.0 / Math.Tan(x)),
                -3.1, 3.1 ),

            new AutoDiffTestCase (
                "f(x) = ln(sqrt((5 x - 1) / (2 x + 3)))",
                x => Dual.Log(Dual.Sqrt((5.0 * x - 1.0) / (x + x + 3.0))),
                x => {
                    //17/(x (20 x + 26) - 6)
                    double q = 20.0 * x + 26.0;
                    q = x * q - 6.0;
                    return 17.0 / q;
                },
                0.2, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(sec x + tan x)",
                x => Dual.Log((1.0 / Dual.Cos(x)) + Dual.Tan(x)),
                x => 1.0 / Math.Cos(x),
                -Math.PI, Math.PI ),

            new AutoDiffTestCase (
                "f(x) = ln(x) / x^2",
                x => Dual.Log(x) / (x * x),
                x => (1.0 - 2.0 * Math.Log(x)) / (x * x * x),
                0.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = 4 x^5 - 3 x^4 + 2 x^3 - x + 100",
                x => {
                    Dual x5 = Dual.Pow(x, 5.0);
                    Dual x4 = Dual.Pow(x, 4.0);
                    Dual x3 = Dual.Pow(x, 3.0);
                    return 4.0 * x5 - 3.0 * x4 + 2.0 * x3 - x + 100.0;
                },
                x => {
                    //2 x^2 (2 x (5 x - 3) + 3) - 1
                    double p = 5.0 * x - 3.0;
                    p = 2.0 * x * p + 3.0;
                    return 2.0 * x * x * p - 1.0;
                },
                -3.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = (3 x^2 - 4) (3 x^2 + 4)",
                x => {
                    Dual t = 3.0 * x * x;
                    return (t - 4.0) * (t + 4.0);
                },
                x => 36.0 * x * x * x,
                -1.5, 1.5 ),

            new AutoDiffTestCase (
                "f(x) = (x^2 - 6 x + 9) / (x^2 - 5 x + 6)",
                x => {
                    Dual x2 = x * x;
                    Dual p = x2 - 6.0 * x + 9.0;
                    Dual q = x2 - 5.0 * x + 6.0;
                    return p / q;
                },
                x => {
                    //1/(x - 2)^2
                    double q = x - 2.0;
                    return 1.0 / (q * q);
                },
                0.0, 4.0 ),

            new AutoDiffTestCase (
                "f(x) = ((x + 1) (x^2 - 1)) / (x^2 + 2 x + 1)",
                x => {
                    Dual x2 = x * x;
                    Dual p = (x + 1.0) * (x2 - 1.0);
                    Dual q = (x2 + 2.0 * x + 1.0);
                    return p / q;
                },
                x => 1.0,
                -10.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = (x / (x + 1)) + ((x - 1) / x)",
                x => (x / (x + 1.0)) + ((x - 1.0) / x),
                x => {
                    //1/x^2 + 1/(x + 1)^2
                    double t = x + 1.0;
                    return (1.0 / (x * x)) + (1.0 / (t * t));
                },
                -3.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = sqrt((3 x^2 - 2 x + 1) (x^2 - 1))",
                x => {
                    Dual x2 = x * x;
                    Dual t = 3.0 * x2 - 2.0 * x + 1.0;
                    return Dual.Sqrt(t * (x2 - 1.0));
                },
                x => {
                    //((2 x - 1) (3 x^2 - 1))/sqrt((x - 1) (x + 1) (x (3 x - 2) + 1))
                    double p = 3.0 * x * x - 1.0;
                    p = (2.0 * x - 1.0) * p;
                    double q = 3.0 * x - 2.0;
                    q = x * q + 1.0;
                    q = (x - 1.0) * (x + 1.0) * q;
                    return p / Math.Sqrt(q);
                },
                1.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = ((3 x + 2) / (2 x - 1))^2",
                x => {
                    Dual p = 3.0 * x + 2.0;
                    Dual q = 2.0 * x - 1.0;
                    Dual t = p / q;
                    return t * t;
                },
                x => {
                    //-(14 (3 x + 2))/(2 x - 1)^3
                    double p = -14.0 * (3.0 * x + 2.0);
                    double q = 2.0 * x - 1.0;
                    return p / (q * q * q);
                },
                -10.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = (x + 1) sqrt(2 x - 1)",
                x => (x + 1.0) * Dual.Sqrt(x + x - 1.0),
                x => (3.0 * x) / Math.Sqrt(2.0 * x - 1.0),
                0.5, 5.0 ),

            new AutoDiffTestCase (
                "f(x) = x^(sin x)",
                x => Dual.Pow(x, Dual.Sin(x)),
                x => {
                    //x^(sin(x) - 1) (sin(x) + x log(x) cos(x))
                    double sin = Math.Sin(x);
                    double p = Math.Pow(x, sin - 1.0);
                    double q = x * Math.Log(x) * Math.Cos(x);
                    return p * (sin + q);
                },
                0.0, 15.0 ),

            new AutoDiffTestCase (
                "f(x) = x^ln(x)",
                x => Dual.Pow(x, Dual.Log(x)),
                x => {
                    //2 x^(log(x) - 1) log(x)
                    double ln = Math.Log(x);
                    return 2.0 * Math.Pow(x, ln - 1.0) * ln;
                },
                0.0, 5.0 ),


            new AutoDiffTestCase (
                "f(x) = ln(ln(ln(x)))",
                x => Dual.Log(Dual.Log(Dual.Log(x))),
                x => {
                    // 1/(x log(x) log(log(x)))
                    double ln = Math.Log(x);
                    double q = x * ln * Math.Log(ln);
                    return 1.0 / q;
                },
                Math.E, 30.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(ln(x + 1) + 1)",
                x => Dual.Log(Dual.Log(x + 1.0) + 1.0),
                x => {
                    // 1/((x + 1) (log(x + 1) + 1))
                    double t = x + 1.0;
                    double q = t * Math.Log(t) + t;
                    return 1.0 / q;
                },
                0.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x + ln(x + 1))",
                x => Dual.Log(x + Dual.Log(x + 1.0)),
                x => {
                    //(1/(x + 1) + 1)/(x + log(x + 1))
                    double t = x + 1.0;
                    double p = (1.0 / t) + 1.0;
                    double q = x + Math.Log(t);
                    return p / q;
                },
                0.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x + 1) / ln(x - 1)",
                x => Dual.Log(x + 1.0) / Dual.Log(x - 1.0),
                x => {
                    // 1/((x + 1) log(x - 1)) - log(x + 1)/((x - 1) log^2(x - 1))
                    double t = x + 1.0;
                    double s = x - 1.0;
                    double lt = Math.Log(t);
                    double ls = Math.Log(s);

                    double p = 1.0 / (t * ls);
                    double q = lt / (s * ls * ls);
                    return p - q;
                },
                1.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x sqrt(x^2 - 1))",
                x => Dual.Log(x * Dual.Sqrt(x * x - 1.0)),
                x => {
                    // (2 x^2 - 1)/((x - 1) x (x + 1))
                    double p = 2.0 * x * x - 1.0;
                    double q = x * (x - 1.0) * (x + 1.0);
                    return p / q;
                },
                1.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = ln(x + sqrt(x^2 - 1))",
                x => Dual.Log(x + Dual.Sqrt(x * x - 1.0)),
                x => 1.0 / Math.Sqrt(x * x - 1.0),
                1.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = ln ((2x + 1)^5 / sqrt(x^2 + 1))",
                x => {
                    Dual p = x + x + 1.0;
                    Dual P5 = Dual.Pow(p, 5.0);
                    Dual q = Dual.Sqrt(x * x + 1.0);
                    return Dual.Log(P5 / q);
                },
                x => {
                    //(x (8 x - 1) + 10)/(x (x (2 x + 1) + 2) + 1)
                    double p = 8.0 * x - 1.0;
                    p = x * p + 10.0;
                    double q = 2.0 * x + 1.0;
                    q = x * q + 2.0;
                    q = x * q + 1.0;
                    return p / q;
                },
                -0.5, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = ln ( e^(-x) + x e^(-x) )",
                x => {
                    Dual t = Dual.Exp(-x);
                    return Dual.Log(t + x * t);
                },
                x => -x / (x + 1.0),
                -1.0, 2.0 ),

            new AutoDiffTestCase (
                "f(x) = x^2 ln(2 x)",
                x => x * x * Dual.Log(x + x),
                x => {
                    //x (2 log(x) + 1 + 2 log(2))
                    double t = 2.0 * Math.Log(x);
                    t = t + 2.3862943611198906188;
                    return x * t;
                },
                0.0, 1.0 ),

            new AutoDiffTestCase (
                "f(x) = Sqrt((x - 1) / (x^4 + 1))",
                x => {
                    Dual x2 = x * x;
                    Dual x4 = x2 * x2;
                    return Dual.Sqrt((x - 1.0) / (x4 + 1.0));
                },
                x => {
                    //((4 - 3 x) x^3 + 1)/(2 sqrt(x - 1) (x^4 + 1)^(3/2))
                    double x2 = x * x;
                    double x4 = x2 * x2;
                    double p = 4.0 - 3.0 * x;
                    p = p * x2 * x + 1.0;
                    double q = Math.Pow(x4 + 1.0, 3.0 / 2.0);
                    q = 2.0 * Math.Sqrt(x - 1.0) * q;
                    return p / q;
                },
                1.0, 5.0 ),

            new AutoDiffTestCase (
                "f(x) = cos(x^(sin(2 x)))",
                x => Dual.Cos(Dual.Pow(x, Dual.Sin(x + x))),
                x => {
                    //-x^(sin(2 x) - 1) sin(x^sin(2 x)) (sin(2 x) + 2 x log(x) cos(2 x))
                    double x2 = 2.0 * x;
                    double t = Math.Sin(x2);
                    double p = -Math.Pow(x, t - 1.0) * Math.Sin(Math.Pow(x, t));
                    double q = x2 * Math.Log(x) * Math.Cos(x2);
                    return p * (t + q);
                },
                0.0, 8.5 ),

            new AutoDiffTestCase (
                "f(x) = (sqrt(x) + 1) / curt(x^4)",
                x => (Dual.Sqrt(x) + 1.0) / VMath.Curt(Dual.Pow(x, 4.0)),
                x => {
                    //-4/(3 x^(7/3)) - 5/(6 x^(11/6))
                    double t = Math.Pow(x, 7.0 / 3.0);
                    double s = Math.Pow(x, 11.0 / 6.0);
                    return -(4.0 / (3.0 * t)) - (5.0 / (6.0 * s));
                },
                0.0, 3.0 ),

            new AutoDiffTestCase (
                "f(x) = x atan(4 x)",
                x => {
                    Dual s = x + x;
                    return x * Dual.Atan(s + s);
                },
                x => {
                    //(4 x)/(16 x^2 + 1) + tan^(-1)(4 x)
                    double p = 4.0 * x;
                    double q = (p * p) + 1.0;
                    return (p / q) + Math.Atan(p);
                },
                -0.5, 0.5 ),

            new AutoDiffTestCase (
                "f(x) = ln | sec (5 x) + tan (5 x) |",
                x => {
                    Dual x5 = 5.0 * x;
                    Dual t = 1.0 / Dual.Cos(x5);
                    t = t + Dual.Tan(x5);
                    return Dual.Log(Dual.Abs(t));
                },
                x => 5.0 / Math.Cos(5.0 * x),
                -Math.PI, Math.PI ),

            new AutoDiffTestCase (
                "f(x) = sin( tan( sqrt(1 + x^3) ) )",
                x => Dual.Sin(Dual.Tan(Dual.Sqrt(1.0 + x * x * x))),
                x => {
                    //= (3 x^2 cos((sin(sqrt(1 + x^3)))/(cos(sqrt(1 + x^3)))))/(2 sqrt(1 + x^3) cos^2(sqrt(1 + x^3)))
                    double x2 = x * x;
                    double r = Math.Sqrt(1.0 + (x2 * x));
                    double sin = Math.Sin(r);
                    double cos = Math.Cos(r);

                    double p = 3.0 * x2 * Math.Cos(sin / cos);
                    double q = 2.0 * r * cos * cos;
                    return p / q;
                },
                0.5, 1.5 ),

            new AutoDiffTestCase (
                "f(x) = tan^2 (sin x)",
                x => {
                    Dual t = Dual.Tan(Dual.Sin(x));
                    return t * t;
                },
                x => {
                    //(2 cos(x) sin(sin(x)))/(cos^3(sin(x)))
                    double sin = Math.Sin(x);
                    double p = 2.0 * Math.Cos(x) * Math.Sin(sin);
                    double q = Math.Cos(sin);
                    return p / (q * q * q);
                },
                -VMath.TAU, VMath.TAU ),

            new AutoDiffTestCase (
                "f(x) = sqrt(x + 1) (2 - x)^5 / (x + 3)^7",
                x => {
                    Dual s = Dual.Pow(2.0 - x, 5.0);
                    Dual t = Dual.Pow(3.0 + x, 7.0);
                    return Dual.Sqrt(x + 1.0) * s / t;
                },
                x => {
                    //((x - 2)^4 (x (3 x - 55) - 52))/(2 sqrt(x + 1) (x + 3)^8)
                    double s = x - 2.0;
                    s = s * s;
                    s = s * s;
                    double t = x + 3.0;
                    t = t * t;
                    t = t * t;

                    double p = 3.0 * x - 55.0;
                    p = x * p - 52.0;
                    p = s * p;
                    double q = 2.0 * Math.Sqrt(x + 1.0);
                    q = q * t * t;

                    return p / q;
                },
                -1.0, 1.0 ),

            new AutoDiffTestCase (
                "f(x) = x sinh(x^2)",
                x => x * Dual.Sinh(x * x),
                x => {
                    //sinh(x^2) + 2 x^2 cosh(x^2)
                    double x2 = x * x;
                    return Math.Sinh(x2) + 2.0 * x2 * Math.Cos(x2);
                },
                -1.5, 1.5 ),

            new AutoDiffTestCase (
                "f(x) = ln(cosh 3x)",
                x => Dual.Log(Dual.Cosh(x + x + x)),
                x => 3.0 * Math.Tanh(3.0 * x),
                -1.0, 1.0 ),

            new AutoDiffTestCase (
                "f(x) = acosh (sinh x)",
                x => Dual.Acosh(Dual.Sinh(x)),
                x => {
                    //cosh(x)/sqrt(sinh^2(x) - 1)
                    double p = Math.Cosh(x);
                    double q = Math.Sinh(x);
                    q = q * q - 1.0;
                    return p / Math.Sqrt(q);
                },
                0.88138, 2.0 ),

            new AutoDiffTestCase (
                "f(x) = asin(e^x)",
                x => Dual.Asin(Dual.Exp(x)),
                x => Math.Exp(x) / Math.Sqrt(1.0 - Math.Exp(2.0 * x)),
                -3.0, 0.0 ),

            new AutoDiffTestCase (
                "f(x) = 1 / curt(x + sqrt(x))",
                x => 1.0 / VMath.Curt(x + Dual.Sqrt(x)),
                x => {
                    // -(2 sqrt(x) + 1)/(6 sqrt(x) (x + sqrt(x))^(4/3))
                    double sr = Math.Sqrt(x);
                    double p = 2.0 * sr + 1.0;
                    double q = Math.Pow(x + sr, 4.0 / 3.0);
                    q = 6.0 * sr * q;
                    return -(p / q);
                },
                0.0, 10.0 ),

            new AutoDiffTestCase (
                "f(x) = atan( asin ( sqrt(x) ) )",
                x => Dual.Atan(Dual.Asin(Dual.Sqrt(x))),
                x => {
                    //1/(2 sqrt(x - x^2) (sin^(-1)(sqrt(x))^2 + 1))
                    double r = Math.Sqrt(x - x * x);
                    double q = Math.Sqrt(x);
                    q = Math.Asin(q);
                    q = q * q + 1.0;
                    q = 2.0 * r * q;
                    return 1.0 / q;
                },
                0.0, 1.0 ),

            new AutoDiffTestCase (
                "f(x) = ln | (x^2 - 4) / (2x + 5) |",
                x => {
                    Dual p = x * x - 4.0;
                    Dual q = x + x + 5.0;
                    return Dual.Log(Dual.Abs(p / q));
                },
                x => {
                    //(x (2 x + 10) + 8)/(x (x (2 x + 5) - 8) - 20)
                    double p = 2.0 * x + 10.0;
                    p = x * p + 8.0;
                    double q = 2.0 * x + 5.0;
                    q = x * q - 8.0;
                    q = x * q - 20.0;
                    return p / q;
                },
                -6.0, 6.0 ),

            new AutoDiffTestCase (
                "f(x) = x atanh (sqrt(x))",
                x => x * Dual.Atanh(Dual.Sqrt(x)),
                x => {
                    // sqrt(x)/(2 - 2 x) + tanh^(-1)(sqrt(x))
                    double sr = Math.Sqrt(x);
                    return (sr / (2.0 - 2.0 * x)) + Math.Atan(sr);
                },
                0.0, 1.0 ),



        };
    }
}
