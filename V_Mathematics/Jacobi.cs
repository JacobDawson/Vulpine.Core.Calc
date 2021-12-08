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
        public static Cmplx FP(double x, double m)
        {
            //the eleptic intergral is undefined for (m > 1)
            if (m > 1.0) return Double.NaN;

            //checkes when the radical is negative
            if (m > 0) //0 < m < 1
            {
                double c = 1.0 / Math.Sqrt(m);

                if (x > +c) return Double.NaN;
                if (x < -c) return Double.NaN; 
            }

            //uses the Legerande normal form
            VFunc intgnd = delegate(double t)
            {
                double t2 = t * t;
                double r = (1.0 - t2) * (1.0 - (m * t2));
                return 1.0 / Math.Sqrt(r);
            };

            //double x = Math.Sin(phi);
            return Integrator.Kronrod(intgnd, 0.0, x);
        }


        public static Cmplx FP(Cmplx x, Cmplx m)
        {
            //Cmplx x = Cmplx.Sin(phi);

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

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Elliptic Functions...

        public static double SN(double u, double m)
        {
            //To Implement Later...
            throw new NotImplementedException();
        }

        public static double CN(double u, double m)
        {
            //To Implement Later...
            throw new NotImplementedException();
        }

        public static Cmplx SN(Cmplx u, double m)
        {
            double mp = 1.0 - m;

            ////uses the hyperboloc definition to compute the sine
            //double outR = Math.Sin(arg.real) * Math.Cosh(arg.imag);
            //double outI = Math.Cos(arg.real) * Math.Sinh(arg.imag);

            double outR = SN(u.CofR, m) * (1.0 / CN(u.CofI, mp));
            double outI = CN(u.CofR, m) * (SN(u.CofI, mp) / CN(u.CofI, mp));




            return new Cmplx(outR, outI);
        }


        public static Cmplx CN(Cmplx u, double m)
        {
            double mp = 1.0 - m;

            ////uses the hyperboloc definition to compute the cosine
            //double outR = Math.Cos(arg.real) * Math.Cosh(arg.imag);
            //double outI = Math.Sin(arg.real) * Math.Sinh(arg.imag);

            double outR = CN(u.CofR, m) * (1.0 / CN(u.CofI, mp));
            double outI = SN(u.CofR, m) * (SN(u.CofI, mp) / CN(u.CofI, mp));


            return new Cmplx(outR, -outI);

            //formula for hyperbolic cosine
            //double outR = Math.Cosh(arg.real) * Math.Cos(arg.imag);
            //double outI = Math.Sinh(arg.real) * Math.Sin(arg.imag);
        }

        //represents the constant value K(1/2)
        private const double KH = 1.85407467730137191843;


        public static Cmplx SN(Cmplx u, Cmplx m)
        {  
            //computes the quarter periods
            Cmplx kp = K(1.0 - m).MultI();
            Cmplx k = K(m);

            //creates a change of basis for the tilable parallelagram
            Vector p1 = k * 4.0;
            Vector p2 = kp * 2.0;
            Matrix t = new Matrix(2, 2,
                p1[0], p2[0],
                p1[1], p2[1]);


            ////converts the unit parallelagram to the unit square
            //Cmplx up = (Cmplx)t.InvAx(u) * KH * 4.0;

            //converts the tialable parallelagram to the unit square
            Cmplx up = (Cmplx)t.InvAx(u);

            //double a = 4.0;

            ////scales the unit square by KH * 4.0
            //double sr = ((up.CofR.Frac() * 4.0) - 2.0) * KH;
            //double si = ((up.CofI.Frac() * 2.0) - 1.0) * KH;
            //up = new Cmplx(sr, si);


            //double sr = up.CofR.Frac() * 4.0 * KH;
            //double si = up.CofI.Frac() * 2.0 * KH;
            //up = new Cmplx(sr, si);


            ////we need to isolate the range (-0.5 to 0.5)
            //double x = 0.0;

            //x = x.Frac();
            //if (x > 0.5) x = x - 1.0;


            //grab the fractional cordinate values
            double sr = up.CofR.Frac();
            double si = up.CofI.Frac();

            //center aorund the origin
            if (sr > 0.5) sr = sr - 1.0;
            if (si > 0.5) si = si - 1.0;

            //scale to the domain of SN(u, 1/2)
            sr = sr * 4.0 * KH;
            si = si * 2.0 * KH;

            up = new Cmplx(sr, si);
           

            //NEXT: Try using the pade aproximate for SN(u, 1/2)


            //up = u;

            //computes the [11/12] Pade Axpoximate for SN(u, 1/2)
            Cmplx p = -1.1662025348979380E-07;
            Cmplx q = 1.2305284491651105E-08;

            p = (up * p) + 0.0;
            p = (up * p) - 7.0728792484709221E-06;
            p = (up * p) + 0.0;
            p = (up * p) - 6.2031120811464283E-05;
            p = (up * p) + 0.0;
            p = (up * p) - 3.9520756905764912E-03;
            p = (up * p) + 0.0;
            p = (up * p) + 1.5888158184827195E-02;
            p = (up * p) + 0.0;
            p = (up * p) + 1.0;
            p = (up * p) + 0.0;

            q = (up * q) + 0.0;
            q = (up * q) + 5.6919553612084142E-07;
            q = (up * q) + 0.0;
            q = (up * q) + 3.3455477445225117E-05;
            q = (up * q) + 0.0;
            q = (up * q) + 4.1314896788924044E-04;
            q = (up * q) + 0.0;
            q = (up * q) - 6.2300361443697050E-03;
            q = (up * q) + 0.0;
            q = (up * q) + 2.6588815818482719E-01;
            q = (up * q) + 0.0;
            q = (up * q) + 1.0;

            return p / q;

            //return Cmplx.NaN;
        }


        //Uses the [19/20] Pade Axpoximate for SN(u, 1/2)
        public static Cmplx SN2(Cmplx u, Cmplx m)
        {
            //computes the quarter periods
            Cmplx kp = K(1.0 - m).MultI();
            Cmplx k = K(m);

            //creates a change of basis for the tilable parallelagram
            Vector p1 = k * 4.0;
            Vector p2 = kp * 2.0;
            Matrix t = new Matrix(2, 2,
                p1[0], p2[0],
                p1[1], p2[1]);

            //converts the tialable parallelagram to the unit square
            Cmplx up = (Cmplx)t.InvAx(u);

            //grab the fractional cordinate values
            double sr = up.CofR.Frac();
            double si = up.CofI.Frac();

            //center aorund the origin
            if (sr > 0.5) sr = sr - 1.0;
            if (si > 0.5) si = si - 1.0;

            //scale to the domain of SN(u, 1/2)
            sr = sr * 4.0 * KH;
            si = si * 2.0 * KH;
            up = new Cmplx(sr, si);


            //NEXT: Try using the pade aproximate for SN(u, 1/2)


            //up = u;

            //computes the [19/20] Pade Axpoximate for SN(u, 1/2)
            Cmplx p = 1.953967432892824600E-013;
            Cmplx q = -4.210105787648752800E-014;

            //p = (up * p) +
            p = (up * p) + 0.0;
            p = (up * p) - 8.920747691067194400E-012;
            p = (up * p) + 0.0;
            p = (up * p) - 5.143546368273501900E-010;
            p = (up * p) + 0.0;
            p = (up * p) + 5.606262937604865600E-008;
            p = (up * p) + 0.0;
            p = (up * p) - 1.104889919071368800E-006;
            p = (up * p) + 0.0;
            p = (up * p) + 2.890730639366435900E-005;
            p = (up * p) + 0.0;
            p = (up * p) - 3.215880735386371300E-004;
            p = (up * p) + 0.0;
            p = (up * p) - 1.269841195140552500E-002;
            p = (up * p) + 0.0;
            p = (up * p) + 1.035285868715790900E-001;
            p = (up * p) + 0.0;
            p = (up * p) + 1.0;
            p = (up * p) + 0.0;


            q = (up * q) + 0.0;
            q = (up * q) + 2.234415094469537600E-012;
            q = (up * q) + 0.0;
            q = (up * q) - 5.614316818462869000E-011;
            q = (up * q) + 0.0;
            q = (up * q) + 2.206391833841200100E-009;
            q = (up * q) + 0.0;
            q = (up * q) - 1.528560602969263500E-007;
            q = (up * q) + 0.0;
            q = (up * q) - 2.897371464331276500E-006;
            q = (up * q) + 0.0;
            q = (up * q) + 1.961491963587991800E-004;
            q = (up * q) + 0.0;
            q = (up * q) - 2.580744729337389400E-003;
            q = (up * q) + 0.0;
            q = (up * q) + 6.933734766489230200E-003;
            q = (up * q) + 0.0;
            q = (up * q) + 3.535285868715791800E-001;
            q = (up * q) + 0.0;
            q = (up * q) + 1.0;



            return p / q;

            //return Cmplx.NaN;
        }

        //Uses the [15/16] Pade Axpoximate for SN(u, 1/2)
        public static Cmplx SN3(Cmplx u, Cmplx m)
        {
            //computes the quarter periods
            Cmplx kp = K(1.0 - m).MultI();
            Cmplx k = K(m);

            //creates a change of basis for the tilable parallelagram
            Vector p1 = k * 4.0;
            Vector p2 = kp * 2.0;
            Matrix t = new Matrix(2, 2,
                p1[0], p2[0],
                p1[1], p2[1]);

            //converts the tialable parallelagram to the unit square
            Cmplx up = (Cmplx)t.InvAx(u);

            //grab the fractional cordinate values
            double sr = up.CofR.Frac();
            double si = up.CofI.Frac();

            //center aorund the origin
            if (sr > 0.5) sr = sr - 1.0;
            if (si > 0.5) si = si - 1.0;

            //scale to the domain of SN(u, 1/2)
            sr = sr * 4.0 * KH;
            si = si * 2.0 * KH;
            up = new Cmplx(sr, si);


            //NEXT: Try using the pade aproximate for SN(u, 1/2)


            //up = u;

            //computes the [15/16] Pade Axpoximate for SN(u, 1/2)
            Cmplx p = -2.308261101796228600E-011;
            Cmplx q = 5.652045497424510300E-012;

            //p = (up * p) +
            p = (up * p) + 0.0;
            p = (up * p) + 1.159689553289719700E-009;
            p = (up * p) + 0.0;
            p = (up * p) + 9.191940149532765200E-008;
            p = (up * p) + 0.0;
            p = (up * p) - 6.339331952112939400E-006;
            p = (up * p) + 0.0;
            p = (up * p) + 6.270724516678946600E-005;
            p = (up * p) + 0.0;
            p = (up * p) - 4.131831955912198200E-003;
            p = (up * p) + 0.0;
            p = (up * p) - 1.498622082364003600E-002;
            p = (up * p) + 0.0;
            p = (up * p) + 1.0;
            p = (up * p) + 0.0;

            q = (up * q) + 0.0;
            q = (up * q) - 2.512157501281880000E-010;
            q = (up * q) + 0.0;
            q = (up * q) + 3.492286257032539200E-009;
            q = (up * q) + 0.0;
            q = (up * q) - 3.003670084253313300E-007;
            q = (up * q) + 0.0;
            q = (up * q) + 1.825587569376501300E-005;
            q = (up * q) + 0.0;
            q = (up * q) + 6.859131363364863900E-004;
            q = (up * q) + 0.0;
            q = (up * q) - 1.412838716182221100E-002;
            q = (up * q) + 0.0;
            q = (up * q) + 2.350137791763599600E-001;
            q = (up * q) + 0.0;
            q = (up * q) + 1.0;


            return p / q;

            //return Cmplx.NaN;
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
