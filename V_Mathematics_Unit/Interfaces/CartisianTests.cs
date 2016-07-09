using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace CVL_Mathematics_Test.Interfaces
{
    public abstract class CartisianTests<T> where T : Metrizable<T>
    {
        public const double DELTA = 0.000000000001;

        public abstract T NilPoint { get; }

        public abstract T GetSubject(int i);

        [Test]
        public void NilMagnitude()
        {
            Console.WriteLine("Testing that the nil-point has zero magnitude.");
            double mag = NilPoint.Mag();
            Assert.AreEqual(0.0, mag, DELTA, "The magnitude of the nil-point "
                + "object is something other than zero.");
        }

        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void DistAsMagnitude(int index)
        {
            T subject = GetSubject(index);
            Console.WriteLine("Testing that the distance between {0} and the "
                + "nil-point object is the sams as the magnitude.", subject);
            double mag = subject.Mag();
            double dist = subject.Dist(NilPoint);
            Assert.AreEqual(mag, dist, DELTA, "The magnitude of {0} varies from "
                + "it's distance to the nil-point object. ", subject);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void NonNilMagintude(int index)
        {
            T subject = GetSubject(index);
            Console.WriteLine("Testing that the magnitude of {0} is positive.");
            double mag = subject.Mag();
            Assert.GreaterOrEqual(mag, 0.0, "The magnitude of {0} is negative.",
                subject);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(1, 3)]
        public void TriangleInequality(int index1, int index2)
        {
            T s1 = GetSubject(index1);
            T s2 = GetSubject(index2);
            Console.WriteLine("Testing the triangle inequality for {0} and {1}", s1, s2);
            double d1 = s1.Mag();
            double d2 = s2.Mag();
            double d3 = s1.Dist(s2);
            Assert.GreaterOrEqual(d1 + d2, d3, "The triangle inequality fails for "
                + "{0} and {1}", s1, s2);

        }

        //public void AddComunitive(int index1, int index2)
        //{
        //    T s1 = GetSubject(index1);
        //    T s2 = GetSubject(index2);

        //    T r1 = s1.Add(s2);
        //    T r2 = s2.Add(s1);
        //}

        //public void ScaleDistributitive(int index1, int index2)
        //{
        //    T s1 = GetSubject(index1);
        //    T s2 = GetSubject(index2);

        //    T r1 = s1.Add(s2).Mult(2.0);
        //    T r2 = s1.Mult(2.0).Add(s2.Mult(2.0)); 

        //}
    }
}
