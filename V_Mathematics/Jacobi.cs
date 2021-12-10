/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2021 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Geometry.Planer;
using Vulpine.Core.Data.Extentions;

namespace Vulpine.Core.Calc
{
    public static class Jacobi
    {
        //used to limit infinite sumations
        private const int TC = 10;
        private const int NC = 8;


        #region Elliptic Intergrals...

        /// <summary>
        /// Computes the Complete Elliptic Intergral of the First Kind, using the
        /// eleptic paramater (m = k^2). In order to use the eleptic modulous, one
        /// should compute K(k^2). The real valued function is undefined for (m > 1).
        /// </summary>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at (m)</returns>
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
        /// Computes the Complete Elliptic Intergral of the First Kind, using the
        /// eleptic paramater (m = k^2). In order to use the eleptic modulous, one
        /// should compute K(k^2). This varient uses the principle branch of the
        /// complex square root to compute the intergral.
        /// </summary>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at (m)</returns>
        public static Cmplx K(Cmplx m)
        {
            //computes the eleptic entergral
            Cmplx c = Cmplx.Sqrt(1.0 - m);
            Cmplx d = VMath.AGM(1.0, c);

            return Math.PI / (2.0 * d);
        }

        /// <summary>
        /// Computes the Incomplete Elliptic Intergral of the First Kind, using the
        /// eleptic paramater (m = k^2). In order to use the eliptic modulous one should
        /// use F(phi, k^2). The real valued function is undefined for (m > 1).
        /// Unlike the complex varient, this version runs relativly fast!
        /// </summary>
        /// <param name="phi">The Amplitude of the Eleptic Intergral</param>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at (phi, m)</returns>
        public static double F(double phi, double m)
        {
            //the eleptic intergral is undefined for (m > 1)
            if (m > 1.0) return Double.NaN;

            //uses the Legerande normal form
            VFunc intgnd = delegate(double t)
            {
                double t2 = t * t;
                double r = (1.0 - t2) * (1.0 - (m * t2));
                return 1.0 / Math.Sqrt(r);
            };

            double x = Math.Sin(phi);
            return Integrator.Kronrod(intgnd, 0.0, x);
        }

        /// <summary>
        /// Warning! This method may run slowly for certain inputs! It atempts to
        /// evaluate the Incomplete Elliptic Intergral of the Frist Kind for complex
        /// values. Either the Amplitude or the Elliptic Paramater may be complex. 
        /// It uses the Ellpitic Paramater (m = k^2). In order to use the Eleptic
        /// Modulous one should compute F(phi, k^2) instead.
        /// </summary>
        /// <param name="phi">The Amplitude of the Eleptic Intergral</param>
        /// <param name="m">The Eleptic Paramater (m = k^2)</param>
        /// <returns>The Eleptic Intergral evaluated at (phi, m)</returns>
        public static Cmplx F(Cmplx phi, Cmplx m)
        {
            Cmplx x = Cmplx.Sin(phi);

            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
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

        //defining this function dose not seem to be as usefull, as the only
        //valid values for "x" are (-1 < x < 1) which achieves the same result
        //as if we computed F(arcsin(x), m) as (phi = sin(x)).

        //public static double FP(double x, double m)
        //{
        //    //the eleptic intergral is undefined for (m > 1)
        //    if (m > 1.0) return Double.NaN;

        //    //checkes when the radical is negative
        //    if (m > 0) //0 < m < 1
        //    {
        //        double c = 1.0 / Math.Sqrt(m);

        //        if (x > +c) return Double.NaN;
        //        if (x < -c) return Double.NaN; 
        //    }

        //    //uses the Legerande normal form
        //    VFunc intgnd = delegate(double t)
        //    {
        //        double t2 = t * t;
        //        double r = (1.0 - t2) * (1.0 - (m * t2));
        //        return 1.0 / Math.Sqrt(r);
        //    };

        //    //double x = Math.Sin(phi);
        //    return Integrator.Kronrod(intgnd, 0.0, x);
        //}


        //public static Cmplx FP(Cmplx x, Cmplx m)
        //{
        //    //Cmplx x = Cmplx.Sin(phi);

        //    //uses the Legerande normal form
        //    VFunc<Cmplx> intgnd = delegate(Cmplx t)
        //    {
        //        Cmplx t2 = t * t;
        //        Cmplx a = 1.0 - t2;
        //        Cmplx b = 1.0 - (m * t2);

        //        //splits the radical to avoid branch cuts
        //        a = Cmplx.Sqrt(a);
        //        b = Cmplx.Sqrt(b);

        //        return 1.0 / (a * b);
        //    };

        //    return Integrator.Kronrod(intgnd, 0.0, x);
        //}

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Elliptic Functions...

        //NOTE: Need to determin the bounds for (m) in the Real case.

        public static double SN(double u, double m)
        {
            //Uses the theta function to compute SN
            double t = Theta(u, m);
            return Math.Sin(t);
        }

        public static double CN(double u, double m)
        {
            //Uses the theta function to compute CN
            double t = Theta(u, m);
            return Math.Cos(t);
        }

        public static double DN(double u, double m)
        {
            //Uses SN to comptue DN in the real case
            double t = SN(u, m);
            t = m * t * t;
            return Math.Sqrt(1.0 - t);
        }

        //Dose Not Work!!

        //public static Cmplx SN(Cmplx u, double m)
        //{
        //    double mp = 1.0 - m;

        //    ////uses the hyperboloc definition to compute the sine
        //    //double outR = Math.Sin(arg.real) * Math.Cosh(arg.imag);
        //    //double outI = Math.Cos(arg.real) * Math.Sinh(arg.imag);
        //    //return new Cmplx(outR, outI);

        //    double outR = SN(u.CofR, m) * (1.0 / CN(u.CofI, mp));
        //    double outI = CN(u.CofR, m) * (SN(u.CofI, mp) / CN(u.CofI, mp));


        //NOTE: I feel like this probably would work, but trying to find the
        //exact form of sin, cos, sinh, and cosh to use would be a headache!


        //    return new Cmplx(outR, outI);
        //}


        //public static Cmplx CN(Cmplx u, double m)
        //{
        //    double mp = 1.0 - m;

        //    ////uses the hyperboloc definition to compute the cosine
        //    //double outR = Math.Cos(arg.real) * Math.Cosh(arg.imag);
        //    //double outI = Math.Sin(arg.real) * Math.Sinh(arg.imag);
        //    //return new Cmplx(outR, -outI);

        //    double outR = CN(u.CofR, m) * (1.0 / CN(u.CofI, mp));
        //    double outI = SN(u.CofR, m) * (SN(u.CofI, mp) / CN(u.CofI, mp));


        //    return new Cmplx(outR, -outI);

        //    ////inverts the formula used for complex cosine
        //    //double outR = Math.Cosh(arg.real) * Math.Cos(arg.imag);
        //    //double outI = Math.Sinh(arg.real) * Math.Sin(arg.imag);
        //    //return new Cmplx(outR, outI);
        //}

        //represents the constant value K(1/2)
        private const double KH = 1.85407467730137191843;


        public static Cmplx SN(Cmplx u, Cmplx m)
        {  
            //computes the quarter periods
            Cmplx kp = K(1.0 - m);
            Cmplx ki = kp.MultI();
            Cmplx km = K(m);

            //creates a change of basis for the tilable parallelagram
            Vector p1 = km * 4.0;
            Vector p2 = ki * 2.0;
            Matrix t = new Matrix(2, 2,
                p1[0], p2[0],
                p1[1], p2[1]);


            //converts the tialable parallelagram to the unit square
            Vector x = t.InvAx(u);

            //grabs the fractional cordinate values
            x[0] = x[0].Frac();
            x[1] = x[1].Frac();

            ////center aorund the origin
            //if (x[0] > 0.5) x[0] -= 1.0;
            //if (x[1] > 0.5) x[1] -= 1.0;

            //converts back to our original cordinates
            Cmplx up = (Cmplx)(t * x);


            ////grab the fractional cordinate values
            //double sr = up.CofR.Frac();
            //double si = up.CofI.Frac();

            ////center aorund the origin
            //if (sr > 0.5) sr = sr - 1.0;
            //if (si > 0.5) si = si - 1.0;

            ////scale to the domain of SN(u, 1/2)
            //sr = sr * 4.0 * KH;
            //si = si * 2.0 * KH;

            //up = new Cmplx(sr, si);


            //up = u;

            Cmplx qm = Nome(km, kp);

            Cmplx ts = ThetaS(up, m, km, qm, NC);
            Cmplx tn = ThetaN(up, m, km, qm, NC);

            return ts / tn;


            //return Cmplx.NaN;
        }

        public static Cmplx CN(Cmplx u, Cmplx m)
        {
            throw new NotImplementedException();
        }

        public static Cmplx DN(Cmplx u, Cmplx m)
        {
            throw new NotImplementedException();
        }


        //Things To Do:
        //1) Implement CN and DN for the Complx Case
        //2) Considering adding SC, CD, and/or SD
        //3) Write test cases that cover all four Theta Functions (S, C, D, N)
        //4) Add Lemniscate Functions and their Inverses
        //5) Add Eleptic Intergrals of the Second (and Third) Kind
        //5) Write Test Cases for ALL functional relations



        #endregion ////////////////////////////////////////////////////////////////////////

        #region Neville Theta Functions...


        private static double Theta(double x, double m)
        {
            //used in the itterative method
            double b = Math.Sqrt(1.0 - m);
            double c = Math.Sqrt(m);          
            double a = 1.0;

            double ap = 0.0;
            double bp = 0.0;

            //stores values in an array for backwards loop
            double[] ca = new double[11];
            ca[0] = c / a;

            //computes the AGM in the forward loop
            for (int i = 1; i <= 10; i++)
            {                 
                ap = (a + b) * 0.5;
                bp = Math.Sqrt(a * b);
                c  = (a - b) * 0.5;

                a = ap;
                b = bp;

                ca[i] = c / a;
            }

            double t = 1024.0 * a * x;
            double s = 0.0;

            //computes the theta values in the backwards loop
            for (int i = 10; i > 0; i--)
            {
                s = ca[i] * Math.Sin(t);
                s = t + Math.Asin(s);

                t = 0.5 * s;
            }

            return t;
        }


        /// <summary>
        /// Computes the Neville Theta Function N(z, m) with aditional paramaters for
        /// k(m) and q(m) which should be computed before invoking this funciton. It is
        /// used in the computaiton of the Jacobi Eleptic Functions.
        /// </summary>
        /// <param name="z">Paramater (z) to the Nevile Theta Funciton</param>
        /// <param name="m">Paramater (m) to the Nevile Theta Funciton</param>
        /// <param name="km">Precomputed value k(m)</param>
        /// <param name="qm">Precomputed value q(m)</param>
        /// <param name="c">Number of itterations for the inner loop</param>
        /// <returns>The Nevel Theta Function evaluated at N(z, m)</returns>
        private static Cmplx ThetaN(Cmplx z, Cmplx m, Cmplx km, Cmplx qm, int c)
        {
            //computes the scaling value based purly on (m)
            Cmplx v = 1.0 - m;
            v = Cmplx.Pow(v, 0.25);
            v = 2.0 * v * Cmplx.Sqrt(km);
            v = VMath.RTP / v;

            Cmplx s = 0.0;

            //computes the infinate series turncated at (k=c)
            for (int k = 1; k <= c; k++)
            {
                Cmplx qx = Cmplx.Pow(qm, k * k);
                if (k % 2 != 0) qx = -qx;

                Cmplx cos = z * Math.PI * k;
                cos = Cmplx.Cos(cos / km);

                s += qx * cos;
            }

            //returns the full expression
            return v * (1.0 + (2.0 * s));
        }

        /// <summary>
        /// Computes the Neville Theta Function S(z, m) with aditional paramaters for
        /// k(m) and q(m) which should be computed before invoking this funciton. It is
        /// used in the computaiton of the Jacobi Eleptic Functions.
        /// </summary>
        /// <param name="z">Paramater (z) to the Nevile Theta Funciton</param>
        /// <param name="m">Paramater (m) to the Nevile Theta Funciton</param>
        /// <param name="km">Precomputed value k(m)</param>
        /// <param name="qm">Precomputed value q(m)</param>
        /// <param name="c">Number of itterations for the inner loop</param>
        /// <returns>The Nevel Theta Function evaluated at S(z, m)</returns>
        private static Cmplx ThetaS(Cmplx z, Cmplx m, Cmplx km, Cmplx qm, int c)
        {
            //takes the quartic roots of the required terms
            Cmplx qm4 = Cmplx.Pow(qm, 0.25);
            Cmplx mp4 = Cmplx.Pow(1.0 - m, 0.25);
            Cmplx m4 = Cmplx.Pow(m, 0.25);

            //computes the scaling value based purly on (m)
            Cmplx v = m4 * mp4 * Cmplx.Sqrt(km);
            v = VMath.RTP * qm4 / v;

            Cmplx s = 0.0;

            //computes the infinate series turncated at (k=c)
            for (int k = 0; k < c; k++)
            {
                Cmplx qx = Cmplx.Pow(qm, k * (k + 1));
                if (k % 2 != 0) qx = -qx;

                Cmplx sin = z * Math.PI * (k + k + 1);
                sin = Cmplx.Sin(sin / (2.0 * km));

                s += qx * sin;
            }

            //returns the full expression
            return v * s;
        }

        /// <summary>
        /// Computes the value q(m), given the precomputed values k(m) and k(1 - m).
        /// This is called the Nome, and is essential to computing the Nevil Theta
        /// Functions.
        /// </summary>
        /// <param name="km">Precomputed value k(m)</param>
        /// <param name="kp">Precomputed value k(1 - m) </param>
        /// <returns></returns>
        private static Cmplx Nome(Cmplx km, Cmplx kp)
        {
            Cmplx exp = Math.PI * kp / km;
            return Cmplx.Exp(-exp);
        }

        





        #endregion ////////////////////////////////////////////////////////////////////////


        //private static double RadNorm3(double rad, double n)
        //{
        //    double sec = Math.Floor((rad - n) / VMath.TAU);
        //    return rad - sec * VMath.TAU;
        //}


        //private static Cmplx Sqrt2(Cmplx z, double n)
        //{
        //    //calculates the result storing temporary values
        //    double rad = Math.Sqrt(z.Abs);
        //    double arg = RadNorm3(z.Arg, n - Math.PI) / 2.0;
        //    double outR = rad * Math.Cos(arg);
        //    double outI = rad * Math.Sin(arg);

        //    //returns the constructed number
        //    return new Cmplx(outR, outI);
        //}

    }
}
