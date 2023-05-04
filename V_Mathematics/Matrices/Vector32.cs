using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Matrices
{
    public class Vector32 : Vector<Vector32>
    {
        //stores the vector as an array of 32-bit floats
        private float[] vector;

        public Vector32(int length)
        {
            vector = new float[length];
        }

        public override int Length
        {
            get { return vector.Length; }
        }

        public override double GetElement(int index)
        {
            return vector[index];
        }

        public override void SetElement(int index, double value)
        {
            //must cast the value to at 32-bit float
            vector[index] = (float)value;
        }

        protected override Vector32 CreateNew()
        {
            //creates a vector of the same type and length
            return new Vector32(vector.Length);
        }

        #region Opperator Overlodes...

        //refferences the Add(v) function
        public static Vector32 operator +(Vector32 v, Vector32 w)
        { return v.Add(w); }

        //references the Sub(v) function
        public static Vector32 operator -(Vector32 v, Vector32 w)
        { return v.Sub(w); }

        //references the Mult(v) function
        public static Double operator *(Vector32 v, Vector32 w)
        { return v.Mult(w); }

        //references the Mult(s) function
        public static Vector32 operator *(Vector32 v, Double s)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static Vector32 operator *(Double s, Vector32 v)
        { return v.Mult(s); }

        //references the Mult(s) function
        public static Vector32 operator /(Vector32 v, Double s)
        { return v.Mult(1.0 / s); }

        ////references the Outer(v) function
        //public static Matrix operator %(Vector32 v, Vector32 w)
        //{ return v.Outer(w); }

        //refrences the Mult(-1) function
        public static Vector32 operator -(Vector32 v)
        { return v.Mult(-1.0); }

        ////refrences the copy constructor
        //public static Vector32 operator +(Vector32 v)
        //{ return new Vector(v); }

        #endregion /////////////////////////////////////////////////////////////
    }
}
