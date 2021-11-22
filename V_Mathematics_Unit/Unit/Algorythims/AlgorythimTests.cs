using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Calc_Tests.AddOns;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Calc_Tests.Unit.Algorythims
{
    //uses a mock algryrythim implementation to test the protected methods
    //defined within the Algorythim class, from which it ihnerits
    public class TestableAlgorythim : Algorithm
    {
        public TestableAlgorythim(int max, double toll) 
            : base(max, toll) { }

        public int Count 
        { 
            get { return count; }
            set { count = value; }
        }

        public double Error 
        { 
            get { return error; }
            set { error = value; }
        }

        public void Call_Initialise()
        {
            base.Initialise();
        }

        public void Call_Increment(int steps)
        {
            base.Increment(steps);
        }

        public bool Call_Step(double last, double curr)
        {
            return base.Step(last, curr);
        }

        public bool Call_Step<T>(T last, T curr) where T : Metrizable<T>
        {
            return base.Step(last, curr);
        }
    }

    [TestFixture]
    public class AlgorythimTests
    {
        /// <summary>
        /// the tollarance used in desk calculations, the reason this is set 
        /// so high is due to the number of significant digits used.
        /// </summary>
        public const Double VTOL = 1.0e-10;

        //stores a rerenece to the tolarance for this instance
        private double tol;

        public AlgorythimTests() { tol = VTOL; }
        public AlgorythimTests(double tol) { this.tol = tol; }


        public Vector GetVector(int index)
        {
            switch (index)
            { 
                case 1: return new Vector( 1.22068,  1.16320,  3.43796);
                case 2: return new Vector( 5.95685, -7.80061, -2.79537);
                case 3: return new Vector( 4.28042,  4.06813,  4.95314);
                case 4: return new Vector(-3.35010,  5.01761,  1.34789);
                case 5: return new Vector(-6.51196,  1.22116, -1.85849);
                case 6: return new Vector( 3.73989, -4.62514, -6.48089);             
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException(); 
        }


        [Test]
        public void Initialise_NewInstance_CountIsZero()
        {
            var alg = new TestableAlgorythim(100, 0.001);
            alg.Call_Initialise();
            Assert.That(alg.Count, Is.EqualTo(0));
        }

        [Test]
        public void Initialise_NewInstance_ErrorIsInf()
        {
            var alg = new TestableAlgorythim(100, 0.001);
            alg.Call_Initialise();
            Assert.That(alg.Error, Is.EqualTo(Double.PositiveInfinity));
        }

        [TestCase(1)]
        [TestCase(15)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(200)]
        public void Increment_NewInstance_CountIncreases(int steps)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            int prev = alg.Count;
            alg.Call_Increment(steps);
            int after = alg.Count;

            Assert.That(after, Is.GreaterThan(prev));
        }

        [TestCase(1)]
        [TestCase(15)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(200)]
        public void Increment_NewInstance_ExpectedDiffrence(int steps)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            int prev = alg.Count;
            alg.Call_Increment(steps);
            int diff = alg.Count - prev;

            Assert.That(diff, Is.GreaterThanOrEqualTo(steps));
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(150)]
        public void Step_NewInstance_CountIncrements(int count)
        {
            var alg = new TestableAlgorythim(100, 0.001);
            alg.Count = count;

            alg.Call_Step(0.5, 1.0);
            int after = alg.Count;

            Assert.That(after, Is.EqualTo(count + 1));
        }

        [TestCase(2.70611, 2.70705, 0.000253570898693135)]
        [TestCase(-3.29341, -3.29939, 0.00139089498742837)]
        [TestCase(7.22920, 7.22919, 0.00000121518642777982)]
        [TestCase(-3.05114, -3.03836, 0.00316465099693941)]
        [TestCase(1.91267, 1.91471, 0.000699898103070227)]
        public void Step_RelativeError_ExptectedError(double last, double curr, double exp)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            alg.Call_Step(last, curr);
            double error = alg.Error;

            Assert.That(error, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(2.01882, 2.01883)]
        [TestCase(4.24047, 4.24042)]
        [TestCase(-6.59381, -6.59342)]
        [TestCase(2.30509,  2.30520)]
        [TestCase(-1.22068, -1.22068)]
        public void Step_RelativeError_TollerenceMet(double last, double curr)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            bool stop = alg.Call_Step(last, curr);

            Assert.That(stop, Is.True, "The step funciton did not stop, even though tollerence was met.");
        }

        //[Test]
        //public void Step_NewInstance_HandlerCalled()
        //{
        //    var alg = new TestableAlgorythim(100, 0.001);

        //    bool called = false;
        //    alg.StepEvent += delegate(Object o, StepEventArgs args)
        //    { called = true; };

        //    alg.Call_Step(0.5, 1.0);

        //    Assert.That(called, Is.True, "The StepEvent handler was never called.");           
        //}

        //[Test]
        //public void Step_NewInstance_HaltUpponRequest()
        //{
        //    var alg = new TestableAlgorythim(100, 0.001);

        //    alg.StepEvent += delegate(Object o, StepEventArgs args)
        //    { args.Halt = true; };

        //    bool stop = alg.Call_Step(0.5, 1.0);

        //    Assert.That(stop, Is.True, "The step function did not stop after the event handler halted.");
        //}

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(150)]
        public void Step_VectorsGiven_CountIncrements(int count)
        {
            var alg = new TestableAlgorythim(100, 0.001);
            alg.Count = count;

            Vector v1 = GetVector(1);
            Vector v2 = GetVector(2);
            alg.Call_Step(v1, v2);
            int after = alg.Count;

            Assert.That(after, Is.EqualTo(count + 1));
        }

        [TestCase(1, 2, 1.0620956442377968803)]
        [TestCase(2, 3, 1.6391582022830615345)]
        [TestCase(3, 4, 1.1824931431518093376)]
        [TestCase(4, 5, 0.7473396214036743215)]
        [TestCase(5, 6, 1.2937749468992645691)]
        public void Step_RelativeVectors_ExpectedError(int n1, int n2, double exp)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            Vector v1 = GetVector(n1);
            Vector v2 = GetVector(n2);
            alg.Call_Step(v1, v2);

            Assert.That(alg.Error, Ist.WithinTolOf(exp, tol));
        }

        [TestCase(1, 0.99982018)]
        [TestCase(2, 0.99970478)]
        [TestCase(3, 1.00058400)]
        [TestCase(4, 1.00070900)]
        [TestCase(5, 1.00086580)]
        public void Step_RelativeVectors_TollerenceMet(int last, double delta)
        {
            var alg = new TestableAlgorythim(100, 0.001);

            Vector v1 = GetVector(last);
            Vector v2 = v1 * delta;
            bool stop = alg.Call_Step(v1, v2);

            Assert.That(stop, Is.True, "The step funciton did not stop, even though tollerence was met.");
        }

        //[Test]
        //public void Step_VectorsGiven_HandlerCalled()
        //{
        //    var alg = new TestableAlgorythim(100, 0.001);

        //    bool called = false;
        //    alg.StepEvent += delegate(Object o, StepEventArgs args)
        //    { called = true; };

        //    Vector v1 = GetVector(1);
        //    Vector v2 = GetVector(2);
        //    alg.Call_Step(v1, v2);

        //    Assert.That(called, Is.True, "The StepEvent handler was never called.");
        //}

        //[Test]
        //public void Step_VectorsGiven_HaltUpponRequest()
        //{
        //    var alg = new TestableAlgorythim(100, 0.001);

        //    alg.StepEvent += delegate(Object o, StepEventArgs args)
        //    { args.Halt = true; };

        //    Vector v1 = GetVector(1);
        //    Vector v2 = GetVector(2);
        //    bool stop = alg.Call_Step(v1, v2);

        //    Assert.That(stop, Is.True, "The step function did not stop after the event handler halted.");
        //}


    }
}
