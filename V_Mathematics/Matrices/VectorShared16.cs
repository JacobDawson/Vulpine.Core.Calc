using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Matrices
{
    public class VectorShared : Vector<VectorShared>
    {
        private int[] vector;

        private double exponent;

        private const double BIAS = 14.0;

        public override int Length
        {
            get { throw new NotImplementedException(); }
        }

        public override double GetElement(int index)
        {
            double m = vector[index];
            return m * exponent;
        }

        public override void SetElement(int index, double value)
        {
            //double a = Math.Abs(value);
            //double s = Math.Sign(value);

            //double e = Math.Log(a, 2.0);
            //e = Math.Floor(e) - BIAS;

            double m = value / exponent;
            vector[index] = (int)m;
        }

        protected override VectorShared CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
