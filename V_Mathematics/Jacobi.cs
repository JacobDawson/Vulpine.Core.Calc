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
        private const int NC = 6;


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
        public static double F2(double phi, double m)
        {
            //the eleptic intergral is undefined for (m > 1)
            if (m > 1.0) return Double.NaN;

            int rx = (int)Math.Round(phi / Math.PI);
            double k2 = 2.0 * K(m);

            //uses the Legerande normal form
            VFunc intgnd = delegate(double t)
            {
                double t2 = t * t;
                double r = (1.0 - t2) * (1.0 - (m * t2));
                return 1.0 / Math.Sqrt(r);
            };

            double x = Math.Sin(phi);
            double intg = Integrator.Kronrod(intgnd, 0.0, x);

            if (rx % 2 != 0) intg = -intg;
            return intg + (k2 * rx);
        }


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
        public static Cmplx F3(Cmplx phi, Cmplx m)
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

            Cmplx intg = Integrator.Kronrod(intgnd, 0.0, x);


            return intg;
        }

        //Wolfram
        public static Cmplx F2(Cmplx phi, Cmplx m)
        {

            int rx = (int)Math.Round(phi.CofR / Math.PI);
            Cmplx k2 = 2.0 * K(m);

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

            Cmplx intg = Integrator.Kronrod(intgnd, 0.0, x);

            ////atempts to fix the branch cuts
            //if (phi.CofR < Math.PI) return intg;
            //else return intg.Conj();

            ////atempts to fix the branch cuts
            //if (Math.Abs(phi.CofR) < Math.PI / 2.0) return intg;
            //else return intg.Conj();

            //return intg;

            if (rx % 2 != 0) intg = -intg;
            return intg + (k2 * rx);
        }


        public static Cmplx F(Cmplx phi, Cmplx m)
        {
            //takes care of the periodic nature of the real axis
            if (Math.Abs(phi.CofR) > Math.PI)
            {
                //double r = phi.CofR.Mod(VMath.TAU);
                //if (r > Math.PI) r = r - Math.PI;

                double r = Math.Round(phi.CofR / VMath.TAU);
                r = phi.CofR - (VMath.TAU * r);


                phi = new Cmplx(r, phi.CofI);
            }

            //I Want: -pi < tau.R < pi

            //double rabs = Math.Abs(phi.CofR);

            //if (rabs > Math.PI)
            //{
            //    double r = r + (Math.PI / 2.0);
            //    r = Math.Floor(r / VMath.TAU);
            //    r = phi.CofR - (r * VMath.TAU);
            //    r = r - (Math.PI / 2.0); 
            //}


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

            Cmplx intg = Integrator.Kronrod(intgnd, 0.0, x);

            ////atempts to fix the branch cuts
            //if (phi.CofR < Math.PI) return intg;
            //else return intg.Conj();

            //atempts to fix the branch cuts
            if (Math.Abs(phi.CofR) < Math.PI / 2.0) return intg;
            else return -intg; // intg.Conj();

            //return intg;
        }

        public static double E(double m)
        {
            //the function is undefined for m > 1
            if (m > 1.0) return Double.NaN;

            double a = 1.0;
            double b = Math.Sqrt(1.0 - m);

            double ap = 0.0;
            double bp = 0.0;
            //double cp = 0.0;

            //stores values in an array for backwards loop
            double[] cn = new double[11];
            cn[0] = Math.Abs(1.0 - m);

            //computes the AGM in the forward loop
            for (int i = 1; i <= 10; i++)
            {
                ap = (a + b) * 0.5;
                bp = Math.Sqrt(a * b);
                //cp = cn[i - 1] / (4.0 * ap);

                a = ap;
                b = bp;

                cn[i] = Math.Abs((a * a) - (b * b));
                //cn[i] = cp * cp;
            }

            double s = 0.0;
            double t = 0.0;

            //computes the theta values in the backwards loop
            for (int i = 10; i > 0; i--)
            {
                t = Math.Pow(2.0, i - 1);
                s = s + (t * cn[i]);
            }

            t = (1.0 - s) * Math.PI;
            return t / (2.0 * a);
        }

        public static Cmplx E(Cmplx m)
        {
            Cmplx a = 1.0;
            Cmplx b = Cmplx.Sqrt(1.0 - m);

            Cmplx ap = 0.0;
            Cmplx bp = 0.0;
            Cmplx cp = 0.0;

            //stores values in an array for backwards loop
            Cmplx[] cn = new Cmplx[11];
            cn[0] = (1.0 - (b * b)).Abs;
            //cn[0] = (1.0 - m).Abs;

            double t1, t2;

            //computes the AGM in the forward loop
            for (int i = 1; i <= 10; i++)
            {
                ap = (a + b) * 0.5;
                bp = Cmplx.Sqrt(a * b);
                cp = (ap * ap) - (bp * bp);
                //cp = ap.Dist(bp);

                //makes shure we choose the right branch of sqrt
                t1 = (ap - bp).Abs;
                t2 = (ap + bp).Abs;
                if (t1 > t2) bp = -bp;

                a = ap;
                b = bp;

                cn[i] = cp.Abs;
                //cn[i] = a.Dist(b);
            }

            Cmplx s = 0.0;
            Cmplx t = 0.0;

            //computes the theta values in the backwards loop
            for (int i = 10; i > 0; i--)
            {
                t = Math.Pow(2.0, i - 1);
                s = s + (t * cn[i]);
            }

            t = (1.0 - s) * Math.PI;
            return t / (2.0 * a);
        }

        public static double E(double phi, double m)
        {
            return Double.NaN;
        }

        public static Cmplx E(Cmplx phi, Cmplx m)
        {
            return Cmplx.NaN;
        }


        #endregion ////////////////////////////////////////////////////////////////////////

        #region Elliptic Functions...

        //NOTE: Need to determin the bounds for (m) in the Real case.

        public static double SN(double u, double m)
        {
            //Uses the Landen Transformation to compute SN
            double t = AM(u, m);
            return Math.Sin(t);
        }

        public static double CN(double u, double m)
        {
            //Uses the Landen Transformation to compute CN
            double t = AM(u, m);
            return Math.Cos(t);
        }

        public static double DN(double u, double m)
        {
            //Uses SN to comptue DN in the real case
            double t = SN(u, m);
            t = m * t * t;
            return Math.Sqrt(1.0 - t);
        }

        public static double SC(double u, double m)
        {
            //Uses the Landen Transformation to compute CN
            double t = AM(u, m);
            return Math.Tan(t); 
        }

        ////represents the constant value K(1/2)
        //private const double KH = 1.85407467730137191843;


        public static Cmplx SN(Cmplx u, Cmplx m)
        {  
            //computes the quarter periods
            Cmplx kp = K(1.0 - m);
            Cmplx km = K(m);

            //restricts the domain to the tilable parallelagram
            Cmplx up = GetTile(u, km, kp);            
            Cmplx qm = Nome(km, kp);

            //up = u;

            //computes the corisponding theta functions
            Cmplx ts = ThetaS(up, m, km, qm, NC);
            Cmplx tn = ThetaN(up, m, km, qm, NC);

            //returns the ratio of the theta funcitons
            return ts / tn;
        }

        


        public static Cmplx CN(Cmplx u, Cmplx m)
        {
            //computes the quarter periods
            Cmplx kp = K(1.0 - m);
            Cmplx km = K(m);

            //restricts the domain to the tilable parallelagram
            Cmplx up = GetTile(u, km, kp);
            Cmplx qm = Nome(km, kp);

            //up = u;

            //computes the corisponding theta functions
            Cmplx tc = ThetaC(up, m, km, qm, NC);
            Cmplx tn = ThetaN(up, m, km, qm, NC);

            //returns the ratio of the theta funcitons
            return tc / tn;
        }

        public static Cmplx DN(Cmplx u, Cmplx m)
        {
            //computes the quarter periods
            Cmplx kp = K(1.0 - m);
            Cmplx km = K(m);

            //restricts the domain to the tilable parallelagram
            Cmplx up = GetTile(u, km, kp);
            Cmplx qm = Nome(km, kp);

            //up = u;

            //computes the corisponding theta functions
            Cmplx td = ThetaD(up, m, km, qm, NC);
            Cmplx tn = ThetaN(up, m, km, qm, NC);

            //returns the ratio of the theta funcitons
            return td / tn;
        }

        public static Cmplx SC(Cmplx u, Cmplx m)
        {
            //computes the quarter periods
            Cmplx kp = K(1.0 - m);
            Cmplx km = K(m);

            //restricts the domain to the tilable parallelagram
            Cmplx up = GetTile(u, km, kp);
            Cmplx qm = Nome(km, kp);

            //up = u;

            //computes the corisponding theta functions
            Cmplx ts = ThetaS(up, m, km, qm, NC);
            Cmplx tc = ThetaC(up, m, km, qm, NC);

            //returns the ratio of the theta funcitons
            return ts / tc;
        }


        //Things To Do:
        //1) Implement CN and DN for the Complx Case
        //2) Considering adding SC, CD, and/or SD
        //3) Write test cases that cover all four Theta Functions (S, C, D, N)
        //4) Add Lemniscate Functions and their Inverses
        //5) Add Eleptic Intergrals of the Second (and Third) Kind
        //5) Write Test Cases for ALL functional relations
        //6) Clean-up Code and Optimise



        #endregion ////////////////////////////////////////////////////////////////////////      

        #region Lemniscate Functions...

        public static double SL(double x)
        {
            ////calculates the inner variables
            //double xp = x / VMath.GA;
            //double sx = Math.Sin(x);
            //double cx = Math.Cos(x);

            ////computes the Lemniscate Sum
            //return LS(xp, sx, cx, 10);

            return SN(x, -1.0);
        }

        public static double CL(double x)
        {
            ////calculates the inner variables
            //double xp = x / VMath.GA;
            //double sx = Math.Sin(x);
            //double cx = Math.Cos(x);

            ////computes the Lemniscate Sum
            //return LS(xp, cx, sx, 10);

            return CN(x, -1.0) / DN(x, -1.0);
        }

        public static Cmplx SL(Cmplx x)
        {
            ////calculates the inner variables
            //Cmplx xp = x / VMath.GA;
            //Cmplx sx = Cmplx.Sin(x);
            //Cmplx cx = Cmplx.Cos(x);

            ////computes the Lemniscate Sum
            //return LS(xp, sx, cx, 10);

            return SN(x, -1.0);
        }

        public static Cmplx CL(Cmplx x)
        {
            ////calculates the inner variables
            //Cmplx xp = x / VMath.GA;
            //Cmplx sx = Cmplx.Sin(x);
            //Cmplx cx = Cmplx.Cos(x);

            ////computes the Lemniscate Sum
            //return LS(xp, cx, sx, 10);

            return CN(x, -1.0) / DN(x, -1.0);
        }

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Lemniscate Inverse Functions...

        public static double ArcSL(double x)
        {
            return Double.NaN;
        }

        public static double ArcCL(double x)
        {
            return Double.NaN;
        }

        public static Cmplx ArcSL(Cmplx x)
        {
            return Cmplx.NaN;
        }

        public static Cmplx ArcCL(Cmplx x)
        {
            return Cmplx.NaN;
        }

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Computes the inverse of the eleptic intergral of the first kind, also 
        /// known as the Jacobi Amplitude, as it is used in computing the Jacobi 
        /// Eleptic Functions. It effectivly uses a Landen Transformation to compute
        /// this value.
        /// </summary>
        /// <param name="x">Value of the Function</param>
        /// <param name="m">A Free Paramater</param>
        /// <returns>The Jacobi Amplitude</returns>
        private static double AM(double x, double m)
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
        /// This function maps all points in the plane to a single repeating
        /// Paralelagram, based on the quarter periods of the Jacobi Eleptic
        /// funcitons. This way, we only need to worry about computing values
        /// in this critical region.
        /// </summary>
        /// <param name="u">Input to the Jacobi Eleptic Function</param>
        /// <param name="km">The Quarter Period K(m)</param>
        /// <param name="kp">The Quarter Period K(1 - m)</param>
        /// <returns>The Point Maped to the Tilable Paralelagram</returns>
        private static Cmplx GetTile(Cmplx u, Cmplx km, Cmplx kp)
        {
            Cmplx ki = kp.MultI();

            //creates a change of basis for the tilable parallelagram
            Vector p1 = km * 4.0;
            Vector p2 = ki * 4.0; //2.0;
            Matrix t = new Matrix(2, 2,
                p1[0], p2[0],
                p1[1], p2[1]);

            //converts the tialable parallelagram to the unit square
            Vector x = t.InvAx(u);

            //grabs the fractional cordinate values
            x[0] = x[0].Frac();
            x[1] = x[1].Frac();

            //center aorund the origin
            if (x[0] > 0.5) x[0] -= 1.0;
            if (x[1] > 0.5) x[1] -= 1.0;

            //converts back to our original cordinates
            return (Cmplx)(t * x);
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

        private static Cmplx ThetaC(Cmplx z, Cmplx m, Cmplx km, Cmplx qm, int c)
        {
            //takes the quartic roots of the required terms
            Cmplx qm4 = Cmplx.Pow(qm, 0.25);
            Cmplx m4 = Cmplx.Pow(m, 0.25);

            //computes the scaling value based purly on (m)
            Cmplx v = m4 * Cmplx.Sqrt(km);
            v = VMath.RTP * qm4 / v;

            Cmplx s = 0.0;

            //computes the infinate series turncated at (k=c)
            for (int k = 0; k < c; k++)
            {
                Cmplx qx = Cmplx.Pow(qm, k * (k + 1));
                Cmplx cos = z * Math.PI * (k + k + 1);
                cos = Cmplx.Cos(cos / (2.0 * km));

                s += qx * cos;
            }

            //returns the full expression
            return v * s;
        }

        private static Cmplx ThetaD(Cmplx z, Cmplx m, Cmplx km, Cmplx qm, int c)
        {
            //computes the scaling value based purly on (m)
            Cmplx v = VMath.RTP / (2.0 * Cmplx.Sqrt(km));
            Cmplx s = 0.0;

            //computes the infinate series turncated at (k=c)
            for (int k = 1; k <= c; k++)
            {
                Cmplx qx = Cmplx.Pow(qm, k * k);
                Cmplx cos = z * Math.PI * k;
                cos = Cmplx.Cos(cos / km);

                s += qx * cos;
            }

            //returns the full expression
            return v * (1.0 + (2.0 * s));
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


        private static double LS(double x, double sx, double cx, int c)
        {
            double s = 0.0;

            //computes the inner sum up to (n=c)
            for (int n = 0; n < c; n++)
            {
                double ch = ((2 * n) - 1);
                ch = Math.Cosh(ch * Math.PI);

                double q = (ch * ch) - (cx * cx);
                s += ch / q;
            }

            //scales s by 4 pi sx / lc
            s = s * sx * 4.79256093894236882976;

            //computes tan(2 arctan(x))
            double p = 1.0 - s * s;
            return 2.0 * s / p;
        }


        private static Cmplx LS(Cmplx x, Cmplx sx, Cmplx cx, int c)
        {
            Cmplx s = 0.0;

            //computes the inner sum up to (n=c)
            for (int n = 0; n < c; n++)
            {
                Cmplx ch = ((2 * n) - 1);
                ch = Cmplx.Cosh(ch * Math.PI);

                Cmplx q = (ch * ch) - (cx * cx);
                s += ch / q;
            }

            //scales s by 4 pi sx / lc
            s = s * sx * 4.79256093894236882976;

            //computes tan(2 arctan(x))
            Cmplx p = 1.0 - s * s;
            return 2.0 * s / p;
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
