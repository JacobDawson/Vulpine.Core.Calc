using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Numbers;

namespace Vulpine_Core_Calc_Tests.Unit.Algorythims
{
    [TestFixture]
    public class RootFinderTests
    {

        private int max;
        private double tol;
        private double exp;

        private bool loging;


        public RootFinderTests()
        {
            max = 256;
            tol = 1.0e-12;
            exp = 1.0e-11;

            loging = false;
        }


        public RootFinder GetFinder()
        {
            var rf = new RootFinder(max, tol);

            //rf.StepEvent += delegate(Object o, NumericStepEventArgs args)
            //{
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return rf;
        }


        public void LogResults(int f, Result<Double> res)
        {
            if (loging) Console.WriteLine
                ("Bisection:{0} error:{1} ittr:{2}", f, res.Error, res.NumSteps);
        }


        public VFunc GetFunciton(int index)
        {
            switch (index)
            {
                case 1: return x => (x * x) - (2.0 * x) - 3.0;
                case 2: return delegate(double x)
                    {
                        double t = (x * x) - (2.0 * x);
                        return -4.0 * (t + 2.0) * (t - 8.0);
                    };
                case 3: return x => 2.0 / (1.0 + Math.Exp(-4.0 * (x - 2.0))) - 1.0;
                case 4: return delegate(double x)
                    {
                        double t = x - 10.0;
                        t = (x * t) + 32.0;
                        return (x * t) - 30.0;
                    };
                case 5: return x => (x * x) - 7.0;
                case 6: return x => 2.0 * Math.Exp(-(x * x) / 8.0) - 1.0;
                case 7: return x => Math.Pow(x, x) - 0.75;
                case 8: return x => Math.Log(Math.Sin(x) + 1.0);


                //the derivitives
                case 11: return x => 2.0 * (x - 1.0);
                case 12: return x => -16.0 * (x - 3.0) * (x - 1.0) * (x + 1.0);
                case 13: return delegate(double x)
                    {
                        double t = (Math.Exp(4.0 * x) + 2980.957987041728);
                        return (8.0 * Math.Exp(4.0 * x + 8.0)) / (t * t);
                    };
                case 14: return x => (x - 4.0) * (3.0 * x - 8.0);
                case 15: return x => 2.0 * x;
                case 16: return x => -0.5 * Math.Exp((-x * x) / 8.0) * x;
                case 17: return x => Math.Pow(x, x) * (Math.Log(x) + 1.0);
                case 18: return x => Math.Cos(x) / (Math.Sin(x) + 1.0);


                //invertable funcitons
                case 21: return x => x * x;
                case 22: return x => Math.Exp(x);
                case 23: return x => Math.Log(x);
                case 24: return x => Math.Cos(x);
                case 25: return x => Math.Pow(x, x);


                //derivitives of invertable funcitons
                case 31: return x => x + x;
                case 32: return x => Math.Exp(x);
                case 33: return x => 1.0 / x;
                case 34: return x => -Math.Sin(x);
                case 35: return x => Math.Pow(x, x) * (Math.Log(x) + 1.0);
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException(); 
        }

        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 5.0, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 3.0, 2.35482004503095)]
        [TestCase(7, 0.1, 0.4, 0.153516789663953)]
        [TestCase(7, 0.4, 1.0, 0.636262939294531)]
        [TestCase(8, 2.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void Bisection_IdealConditions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Bisection(f, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));    
        }

        [TestCase(21, 7.0, 0.0, 10.0, 2.64575131106459)]
        [TestCase(21, 18.0, 0.0, 10.0, 4.24264068711929)]
        [TestCase(22, 22.0, 0.0, 5.0, 3.09104245335831)]
        [TestCase(22, 47.0, 0.0, 5.0, 3.85014760171006)]
        [TestCase(23, 4.0, 1.0, 100.0, 54.5981500331442)]
        [TestCase(23, 4.5, 1.0, 100.0, 90.0171313005218)]
        [TestCase(24, 0.25, 0.0, Math.PI, 1.31811607165282)]
        [TestCase(24, 0.75, 0.0, Math.PI, 0.722734247813416)]
        [TestCase(25, 3.0, 1.0, 3.0, 1.82545502292483)]
        [TestCase(25, 5.0, 1.0, 3.0, 2.12937248276016)]
        public void Bisection_InverseFunction_FindsSolution(int findex, double y, double low, double high, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Bisection(f, y, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp));   
        }

        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 5.0, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 3.0, 2.35482004503095)]
        [TestCase(7, 0.1, 0.4, 0.153516789663953)]
        [TestCase(7, 0.4, 1.0, 0.636262939294531)]
        [TestCase(8, 2.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void FalsePos_IdealConitions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.FalsePos(f, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
        }

        [TestCase(21, 7.0, 0.0, 10.0, 2.64575131106459)]
        [TestCase(21, 18.0, 0.0, 10.0, 4.24264068711929)]
        [TestCase(22, 22.0, 0.0, 5.0, 3.09104245335831)]
        [TestCase(22, 47.0, 0.0, 5.0, 3.85014760171006)]
        [TestCase(23, 4.0, 1.0, 100.0, 54.5981500331442)]
        [TestCase(23, 4.5, 1.0, 100.0, 90.0171313005218)]
        [TestCase(24, 0.25, 0.0, Math.PI, 1.31811607165282)]
        [TestCase(24, 0.75, 0.0, Math.PI, 0.722734247813416)]
        [TestCase(25, 3.0, 1.0, 3.0, 1.82545502292483)]
        [TestCase(25, 5.0, 1.0, 3.0, 2.12937248276016)]
        public void FalsePos_InverseFunction_FindsSolution(int findex, double y, double low, double high, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.FalsePos(f, y, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp));
        }

        [TestCase(1, -2.0, -1.5, -1.0)]
        [TestCase(1, 2.0, 4.0, 3.0)]
        [TestCase(2, -2.4, -1.8, -2.0)]
        [TestCase(2, 4.5, 4.4, 4.0)]
        [TestCase(3, 1.7, 2.2, 2.0)]
        [TestCase(4, 1.5, 1.8, 1.64069591402822)]
        [TestCase(6, -2.5, -2.6, -2.35482004503095)]
        [TestCase(6, 2.3, 2.4, 2.35482004503095)]
        [TestCase(7, 0.1, 0.2, 0.153516789663953)]
        [TestCase(7, 0.5, 0.6, 0.636262939294531)]
        [TestCase(8, 3.0, 4.0, Math.PI)]
        [TestCase(8, 5.0, 6.0, Math.PI * 2.0)]
        public void Secant_NearRoot_FindsRoot(int findex, double x1, double x2, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Secant(f, x1, x2);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
        }

        [TestCase(21, 7.0, 0.0, 10.0, 2.64575131106459)]
        [TestCase(21, 18.0, 0.0, 10.0, 4.24264068711929)]
        [TestCase(22, 22.0, 0.0, 5.0, 3.09104245335831)]
        [TestCase(22, 47.0, 0.0, 5.0, 3.85014760171006)]
        [TestCase(23, 4.0, 1.0, 100.0, 54.5981500331442)]
        [TestCase(23, 4.5, 1.0, 100.0, 90.0171313005218)]
        [TestCase(24, 0.25, 0.0, Math.PI, 1.31811607165282)]
        [TestCase(24, 0.75, 0.0, Math.PI, 0.722734247813416)]
        [TestCase(25, 3.0, 1.0, 3.0, 1.82545502292483)]
        [TestCase(25, 5.0, 1.0, 3.0, 2.12937248276016)]
        public void Secant_InverseFunction_FindsSolution(int findex, double y, double x1, double x2, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Secant(f, y, x1, x2);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp));
        }

        [TestCase(1, 11, -0.9, -1.0)]
        [TestCase(1, 11, 2.8, 3.0)]
        [TestCase(2, 12, -1.7, -2.0)]
        [TestCase(2, 12, 3.6, 4.0)]
        [TestCase(3, 13, 1.5, 2.0)]
        [TestCase(4, 14, 1.5, 1.64069591402822)]
        [TestCase(6, 16, 2.5, 2.35482004503095)]
        [TestCase(6, 16, -2.5, -2.35482004503095)]
        [TestCase(7, 17, 0.1, 0.153516789663953)]
        [TestCase(7, 17, 0.5, 0.636262939294531)]
        [TestCase(8, 18, 3.0, Math.PI)]
        [TestCase(8, 18, 6.5, Math.PI * 2.0)]
        public void Newton_NearRoot_FindsRoot(int findex, int dxindex, double g, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);
            VFunc dx = GetFunciton(dxindex);

            var res = rf.Newton(f, dx, g);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
        }


        [TestCase(21, 31, 7.0, 5.0, 2.64575131106459)]
        [TestCase(21, 31, 18.0, 5.0, 4.24264068711929)]
        [TestCase(22, 32, 22.0, 2.5, 3.09104245335831)]
        [TestCase(22, 32, 47.0, 2.5, 3.85014760171006)]
        [TestCase(23, 33, 4.0, 50.0, 54.5981500331442)]
        [TestCase(23, 33, 4.5, 50.0, 90.0171313005218)]
        [TestCase(24, 34, 0.25, 1.5, 1.31811607165282)]
        [TestCase(24, 34, 0.75, 1.5, 0.722734247813416)]
        [TestCase(25, 35, 3.0, 2.0, 1.82545502292483)]
        [TestCase(25, 35, 5.0, 2.0, 2.12937248276016)]
        public void Newton_InverseFunction_FindsSolution(int findex, int dxindex, double y, double g, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);
            VFunc dx = GetFunciton(dxindex);

            var res = rf.Newton(f, dx, y, g);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp)); 
        }


        /*******************************************************************************/


        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 5.0, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 3.0, 2.35482004503095)]
        [TestCase(7, 0.1, 0.4, 0.153516789663953)]
        [TestCase(7, 0.4, 1.0, 0.636262939294531)]
        [TestCase(8, 2.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void Ridder_IdealConitions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Ridders(f, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));
        }

        [TestCase(21, 7.0, 0.0, 10.0, 2.64575131106459)]
        [TestCase(21, 18.0, 0.0, 10.0, 4.24264068711929)]
        [TestCase(22, 22.0, 0.0, 5.0, 3.09104245335831)]
        [TestCase(22, 47.0, 0.0, 5.0, 3.85014760171006)]
        [TestCase(23, 4.0, 1.0, 100.0, 54.5981500331442)]
        [TestCase(23, 4.5, 1.0, 100.0, 90.0171313005218)]
        [TestCase(24, 0.25, 0.0, Math.PI, 1.31811607165282)]
        [TestCase(24, 0.75, 0.0, Math.PI, 0.722734247813416)]
        [TestCase(25, 3.0, 1.0, 3.0, 1.82545502292483)]
        [TestCase(25, 5.0, 1.0, 3.0, 2.12937248276016)]
        public void Ridder_InverseFunction_FindsSolution(int findex, double y, double low, double high, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Ridders(f, y, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp));
        }


        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 5.0, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 3.0, 2.35482004503095)]
        [TestCase(7, 0.1, 0.4, 0.153516789663953)]
        [TestCase(7, 0.4, 1.0, 0.636262939294531)]
        [TestCase(8, 2.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void Brent_IdealConitions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Brent(f, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));
        }

        [TestCase(21, 7.0, 0.0, 10.0, 2.64575131106459)]
        [TestCase(21, 18.0, 0.0, 10.0, 4.24264068711929)]
        [TestCase(22, 22.0, 0.0, 5.0, 3.09104245335831)]
        [TestCase(22, 47.0, 0.0, 5.0, 3.85014760171006)]
        [TestCase(23, 4.0, 1.0, 100.0, 54.5981500331442)]
        [TestCase(23, 4.5, 1.0, 100.0, 90.0171313005218)]
        [TestCase(24, 0.25, 0.0, Math.PI, 1.31811607165282)]
        [TestCase(24, 0.75, 0.0, Math.PI, 0.722734247813416)]
        [TestCase(25, 3.0, 1.0, 3.0, 1.82545502292483)]
        [TestCase(25, 5.0, 1.0, 3.0, 2.12937248276016)]
        public void Brent_InverseFunction_FindsSolution(int findex, double y, double low, double high, double t)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Brent(f, y, low, high);

            LogResults(findex, res);
            Assert.That(res.Value, Ist.WithinTolOf(t, exp));
        }



    }
}
