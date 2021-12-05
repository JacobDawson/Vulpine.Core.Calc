using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Data.Extentions;

namespace Vulpine.Core.Calc
{
    public static class Jacobi
    {

        /// <summary>
        /// Computes the Complete Elliptic Intergral of the first kind, using the
        /// eleptic paramater (m = k^2). In order to use the eleptic modulous, one
        /// should compute K(k^2). The real valued function is undefined for (m > 1).
        /// </summary>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at m</returns>
        public static double K(double m)
        {
            //the function is undefined for m > 1
            if (m > 1.0) return Double.NaN;

            //computes the eleptic entergral
            double c = Math.Sqrt(1.0 - m);
            double d = VMath.AGM(1.0, c);

            return Math.PI / (2.0 * d);
        }

        /// <summary>
        /// Computes the Complete Elliptic Intergral of the first kind, using the
        /// eleptic paramater (m = k^2). In order to use the eleptic modulous, one
        /// should compute K(k^2). This varient uses the principle branch of the
        /// complex square root to compute the intergral.
        /// </summary>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at m</returns>
        public static Cmplx K(Cmplx m)
        {
            //computes the eleptic entergral
            Cmplx c = Cmplx.Sqrt(1.0 - m);
            Cmplx d = VMath.AGM(1.0, c);

            return Math.PI / (2.0 * d);
        }



        //This method works increadably well !!!
        public static double F(double phi, double m)
        {
            double x = Math.Sin(phi);


            //the eleptic intergral is undefined for (m > 1)
            if (m > 1.0) return Double.NaN;


            //The following is unessary because c is always > 1 for 0 < m < 1

            ////checkes when the radical is negative
            //if (m > 0) //0 < m < 1
            //{
            //    double c = 1.0 / Math.Sqrt(m)


            //    if (x > +c) return Double.NaN;
            //    if (x < -c) return Double.NaN; 
            //}

            

            ////-1 < x < +1
            ////


            //////constructs the eleptic intergrand
            ////VFunc intgnd = delegate(double t)
            ////{
            ////    t = Math.Sin(t);
            ////    t = 1.0 - (t * t * m);
            ////    t = 1.0 / Math.Sqrt(t);
            ////    return t;
            ////};

            ////double cc = Integrator.Kronrod(intgnd, 0.0, phi);


            //uses the Legerande normal form
            VFunc intgnd = delegate(double t)
            {
                double t2 = t * t;
                double r = (1.0 - t2) * (1.0 - (m * t2));
                return 1.0 / Math.Sqrt(r);
            };

            double y = Integrator.Kronrod(intgnd, 0.0, x);

            return y; // Double.NaN;
        }




        public static Cmplx F(Cmplx phi, Cmplx m)
        {
            Cmplx x = Cmplx.Sin(phi);

            Cmplx c = x * x;
            c = (1.0 - x) * (1.0 - (m * x));
            double branch = 0.0;

            //if (c.CofR < 0)
            //{
            //    if (c.CofI > 0) branch = 0.5 * Math.PI;
            //    else branch = -0.5 * Math.PI;
            //}

            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
                Cmplx t2 = t * t;
                Cmplx r = (1.0 - t2) * (1.0 - (m * t2));
                //return 1.0 / Cmplx.Sqrt(r);
                return 1.0 / Sqrt2(r, branch);
            };

            return Integrator.Kronrod(intgnd, 0.0, x);
        }


        public static Cmplx F2(Cmplx phi, double m)
        {
            if (m > 1) return Cmplx.NaN;

            Cmplx x = Cmplx.Sin(phi);

            Cmplx c = 1.0 - (x * x);
            double branch = 0.0;

            if (c.CofR < 0.0)
            {
                if (c.CofI > 0) branch = 0.5 * Math.PI;
                else branch = -0.5 * Math.PI;
            }



            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
                Cmplx t2 = t * t;
                Cmplx r = (1.0 - t2) * (1.0 - (m * t2));
                //return 1.0 / Cmplx.Sqrt(r);
                return 1.0 / Sqrt2(r, branch);
            };

            return Integrator.Kronrod(intgnd, 0.0, x);
        }

        //this is the best method yet, but it still has problems
        public static Cmplx F3(Cmplx phi, Cmplx m)
        {
            Cmplx x = Cmplx.Sin(phi);

            //Cmplx c = x * x;
            //c = (1.0 - x) * (1.0 - (m * x));
            //double branch = 0.0;

            //if (c.CofR < 0)
            //{
            //    if (c.CofI > 0) branch = 0.5 * Math.PI;
            //    else branch = -0.5 * Math.PI;
            //}

            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
                //Cmplx t2 = t * t;
                //Cmplx r = (1.0 - t2) * (1.0 - (m * t2));
                ////return 1.0 / Cmplx.Sqrt(r);
                //return 1.0 / Sqrt2(r, branch);

                Cmplx t2 = t * t;
                Cmplx a = 1.0 - t2;
                Cmplx b = 1.0 - (m * t2);

                //splits the radical to avoid branch cuts
                a = Cmplx.Sqrt(a);
                b = Cmplx.Sqrt(b);

                return 1.0 / (a * b);
            };

            return Integrator.Kronrod(intgnd, 0.0, x);
        }




        private static double RadNorm3(double rad, double n)
        {
            double sec = Math.Floor((rad - n) / VMath.TAU);
            return rad - sec * VMath.TAU;
        }


        private static Cmplx Sqrt2(Cmplx z, double n)
        {
            //calculates the result storing temporary values
            double rad = Math.Sqrt(z.Abs);
            double arg = RadNorm3(z.Arg, n - Math.PI) / 2.0;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

    }
}
