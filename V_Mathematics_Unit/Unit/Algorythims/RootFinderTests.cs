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


        public RootFinderTests()
        {
            max = 256;
            tol = 1.0e-12;
            exp = 1.0e-11;
        }


        public RootFinder GetFinder()
        {
            var rf = new RootFinder(max, tol);

            //rf.StepEvent += delegate(Object o, NumericStepEventArgs args) {
            //    Console.WriteLine("Step{0}: {1}", args.Step, args.Error);
            //};

            return rf;
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


            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException(); 
        }

        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 2.5, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 2.5, 2.35482004503095)]
        [TestCase(7, 0.1, 0.3, 0.153516789663953)]
        [TestCase(7, 0.5, 0.8, 0.636262939294531)]
        [TestCase(8, 3.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void Bisection_IdealConditions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.Bisection(f, low, high);

            //Console.WriteLine("Bisection:{0} error:{1} ittr:{2}", findex, res.Error, res.NumSteps);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));    
        }

        [TestCase(1, -2.0, 0.0, -1.0)]
        [TestCase(1, 0.0, 4.0, 3.0)]
        [TestCase(2, -3.0, 3.0, -2.0)]
        [TestCase(2, 1.0, 5.0, 4.0)]
        [TestCase(3, 0.0, 3.0, 2.0)]
        [TestCase(4, 0.0, 2.5, 1.64069591402822)]
        [TestCase(6, -5.0, 0.0, -2.35482004503095)]
        [TestCase(6, -2.0, 2.5, 2.35482004503095)]
        [TestCase(7, 0.1, 0.3, 0.153516789663953)]
        [TestCase(7, 0.5, 0.8, 0.636262939294531)]
        [TestCase(8, 3.0, 4.0, Math.PI)]
        [TestCase(8, 4.0, 8.0, Math.PI * 2.0)]
        public void FalsePos_IdealConitions_FindsRoot(int findex, double low, double high, double root)
        {
            RootFinder rf = GetFinder();
            VFunc f = GetFunciton(findex);

            var res = rf.FalsePos(f, low, high);

            //Console.WriteLine("FalsePos:{0} error:{1} ittr:{2}", findex, res.Error, res.NumSteps);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
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

            //Console.WriteLine("Secant:{0} error:{1} ittr:{2}", findex, res.Error, res.NumSteps);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
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

            //Console.WriteLine("Newton:{0} error:{1} ittr:{2}", findex, res.Error, res.NumSteps);
            Assert.That(res.Value, Ist.WithinTolOf(root, exp));  
        }


    }
}
