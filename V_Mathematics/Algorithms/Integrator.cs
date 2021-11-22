/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
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
using Vulpine.Core.Calc.Geometry.Planer;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// This class provides methods for intergrating arbitrary functions, as well as
    /// various other methods related to intergration. A definite intergral can be
    /// visualised as the area beneth the function's curve, bordered by the endpoints
    /// of intergration. Indefinite intergrals can be computed from definite intergrals
    /// via the fundemental therum of calclus. All of the intergration methods in this 
    /// class were designed to work with proper definate intergrals. They are ideal for 
    /// functions which are continious on the closed interval of intergration. Improper 
    /// intergrals are not garenteed to converge.
    /// </summary>
    /// <remarks>Last Update: 2017-02-22</remarks>
    public sealed class Integrator : Algorithm
    {
        //Things To Add:

        //Complex Intergration
        //Inverse Intergration
        //Computing the Average Value
        //Computing the Arc Length
        //Multi-varient Intergration

        //Tanh-Sinh Quadrature "qthsh.pdf" !!!


        //NOTE: Consider making the itterator count report the number
        //of function invocations for each method. 


        #region Class Definitions...

        //used in 3-point Gausian intergration
        private const double G3 = 0.77459666924148337704;
        
        //stores the weights for 7-point Gausian quadrature
        private static readonly double[] GW =
        {
            4.1795918367346938776e-01,
            3.8183005050511894495e-01,
            2.7970539148927666790e-01,
            1.2948496616886969327e-01,
        };

        //stores the weights for 15-point Kronrod quadrature
        private static readonly double[] KW =
        {
            2.0948214108472782801e-01,
            2.0443294007529889241e-01,
            1.9035057806478540991e-01,
            1.6900472663926790283e-01,
            1.4065325971552591875e-01,
            1.0479001032225018384e-01,
            6.3092092629978553291e-02,
            2.2935322010529224964e-02,
        };

        //stores the nodal points for Gauss-Kronrod quadrature
        private static readonly double[] GKN =
        {
            0.0,
            2.0778495500789846760e-01,
            4.0584515137739716691e-01,
            5.8608723546769113029e-01,
            7.4153118559939443986e-01,
            8.6486442335976907279e-01,
            9.4910791234275852453e-01,
            9.9145537112081263921e-01,
        };

        /// <summary>
        /// Creates a new Integrator with default stoping criteria.
        /// </summary>
        public Integrator()
        {
            base.max = 32;
            base.tol = 1.0e-10;
        }

        /// <summary>
        /// Creates a new Integrator with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Integrator(int max, double tol)
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Trapezoid Rule...

        /// <summary>
        /// Uses repeated aplications of the trapizoid rule to aproximate the
        /// anti-drivitive of the given function at the given value. That is,
        /// it aproximates the function who's dirivitive is the given funciton.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="x">Input to the anti-drivitive</param>
        /// <returns>The anti-drivitive at the given value</returns>
        public Result<Double> Trapezoid(VFunc f, double x)
        {
            return Trapezoid(f, 0.0, x);
        }

        /// <summary>
        /// Uses repeated aplications of the trapizoid rule to aproximate the 
        /// given definate intergral to the requested acuracy. Geometricly, this 
        /// can be thought of as finding the area under the curve, bracketed by
        /// the two endpoints.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <returns>The results of intergration</returns>
        public Result<Double> Trapezoid(VFunc f, double a, double b)
        {
            //checks that we have a correct bracket
            if (a > b)
            {
                var res = Trapezoid(f, b, a);
                return Finish(-res.Value);
            }

            //start with 4 subdivisions
            long n = 4;

            //used in main loop
            double trap = 0.0;
            double last = 0.0;
            Initialise();

            //checks to see if we have zero length
            if (VMath.IsZero(b - a))
            {
                base.error = Math.Abs(b - a);
                return Finish(0.0);
            }

            while (true)
            {
                //preforms the trapizoid rule
                trap = SingleTrap(f, a, b, n);

                //determins if tollerence is met
                if (Step(last, trap)) break;

                //trailing refrence
                last = trap;
                n = n << 1;
            }

            //returns the best answer
            return Finish(trap);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Romberg Intergration...

        /// <summary>
        /// Uses Romberg intergration to aproximate the anti-drivitive of the
        /// function at the given value. That is, it aproximates the function 
        /// who's dirivitive is the given funciton.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="x">Input to the anti-drivitive</param>
        /// <returns>The anti-drivitive at the given value</returns>
        public Result<Double> Romberg(VFunc f, double x)
        {
            return Romberg(f, 0.0, x);
        }

        /// <summary>
        /// Uses Romberg intergration to aproximate the given definate intergral 
        /// to the requested acuracy. Geometricly, this can be thought of as finding 
        /// the area under the curve, bracketed by the two endpoints.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <returns>The results of intergration</returns>
        public Result<Double> Romberg(VFunc f, double a, double b)
        {
            //checks that we have a correct bracket
            if (a > b)
            {
                var res = Romberg(f, b, a);
                return Finish(-res.Value);
            }

            Initialise();

            //checks to see if we have zero length
            if (VMath.IsZero(b - a))
            {
                base.error = Math.Abs(b - a);
                return Finish(0.0);
            }

            //sets up the array for romberg intergration
            int array_size = base.MaxSteps + 2;
            double[] prev = new double[array_size];
            double[] curr = new double[array_size];

            //performs the first level of intergration
            prev[0] = SingleTrap(f, a, b, 1);

            //used in main loop
            double rombn = 0.0;
            double rombl = 0.0;
            long trap = 2;
            int level = 1;

            while (true)
            {
                if (level % 2 == 0)
                {
                    //prefroms romberg intergration
                    prev[0] = SingleTrap(f, a, b, trap);
                    RombergLevel(curr, prev, level);

                    //checks for validation
                    rombn = prev[level];
                    rombl = curr[level - 1];
                }
                else
                {
                    //prefroms romberg intergration
                    curr[0] = SingleTrap(f, a, b, trap);
                    RombergLevel(prev, curr, level);

                    //checks for validation
                    rombn = curr[level];
                    rombl = prev[level - 1];
                }

                //determins if we should continue
                if (Step(rombl, rombn)) break;

                //updates counters
                level = level + 1;
                trap = trap << 1;
            }

            //returns the best answer
            return Finish(rombn);
        }


        #endregion //////////////////////////////////////////////////////////////////

        #region Gausian Quadrature...

        /// <summary>
        /// Uses 3-Point Gausian quadrature to aproximate the anti-drivitive of the
        /// function at the given value. That is, it aproximates the function 
        /// who's dirivitive is the given funciton.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="x">Input to the anti-drivitive</param>
        /// <returns>The anti-drivitive at the given value</returns>
        public Result<Double> Gauss(VFunc f, double x)
        {
            return Gauss(f, 0.0, x);
        }

        /// <summary>
        /// Uses 3-Point Gausian quadrature to aproximate the given definate intergral 
        /// to the requested acuracy. Geometricly, this can be thought of as finding 
        /// the area under the curve, bracketed by the two endpoints.
        /// </summary>
        /// <param name="f">The integrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <returns>The results of intergration</returns>
        public Result<Double> Gauss(VFunc f, double a, double b)
        {
            //checks that we have a correct bracket
            if (a > b)
            {
                var res = Gauss(f, b, a);
                return Finish(-res.Value);
            }

            //start with 4 subdivisions
            long n = 4;

            //used in main loop
            double curr = 0.0;
            double last = 0.0;
            Initialise();

            //checks to see if we have zero length
            if (VMath.IsZero(b - a))
            {
                base.error = Math.Abs(b - a);
                return Finish(0.0);
            }

            while (true)
            {
                //preforms the trapizoid rule
                curr = SingleGauss3(f, a, b, n);

                //determins if tollerence is met
                if (Step(last, curr)) break;

                //trailing refrence
                last = curr;
                n = n << 1;
            }

            //returns the best answer
            return Finish(curr);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Gauss-Kronrod Quadrature...

        /// <summary>
        /// Uses Gauss-Kronrod quadrature to aproximate the anti-drivitive of the
        /// function at the given value. This is achieved by sub-dividing the 
        /// intergral only where greater accuracy is required.
        /// </summary>
        /// <param name="f">The function to intergrate</param>
        /// <param name="x">Input to the anti-drivitive</param>
        /// <returns>The anti-drivitive at the given value</returns>
        public static double Kronrod(VFunc f, double x)
        {
            return Kronrod(f, 0.0, x);
        }

        /// <summary>
        /// Uses Gauss-Kronrod quadrature to aproximate the given definate intergral 
        /// to near machine acuracy. This is achieved by sub-dividing the intergral
        /// only where greater accuracy is required.
        /// </summary>
        /// <param name="f">The function to intergrate</param>
        /// <param name="a">Lower bound of intergraiton</param>
        /// <param name="b">Upper bound of intergration</param>
        /// <returns>The results of intergration</returns>
        public static double Kronrod(VFunc f, double a, double b)
        {
            //takes care of special cases
            if (VMath.IsZero(a - b)) return 0.0;
            if (a > b) return -KronrodRec(f, b, a, 0);

            //intitiates the recursive procedure
            return KronrodRec(f, a, b, 0);
        }


        #endregion //////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Preforms a single level of the trapizoid rule, with the desired number 
        /// of sub-divisions. Basicly it splits the intgral into equal parts, 
        /// computes the area of a trapizoid under each part, and sums them together.
        /// </summary>
        /// <param name="f">The intergrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <param name="n">Number of sub-divisions</param>
        /// <returns>A single level of the trapizoid rule</returns>
        private static double SingleTrap(VFunc f, double a, double b, long n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double trap = f(a);

            //preforms trapazoid rule
            for (long i = 1; i < n; i++)
            {
                curr = curr + h;
                trap = trap + (2.0 * f(curr));
            }

            trap = trap + f(b);

            //finishes rule and returns
            return trap * (h / 2.0);
        }

        /// <summary>
        /// Computes the next level of values for Romberg intergration, using
        /// the exisiting values of the previous level. It overwrites the
        /// values of the curent level as it goes.
        /// </summary>
        /// <param name="prev">The previous level</param>
        /// <param name="curr">The curent level</param>
        /// <param name="level">Index of the curent level</param>
        private static void RombergLevel(double[] prev, double[] curr, int level)
        {
            //used in loop
            double a1 = 0.0;
            double a2 = 0.0;
            double factor = 1.0;

            for (int i = 1; i <= level; i++)
            {
                //obtains the values to combine
                a1 = curr[i - 1];
                a2 = prev[i - 1];
                factor = factor * 4.0;

                //combines the values
                curr[i] = ((factor * a1) - a2) / (factor - 1.0);
            }

        }

        /// <summary>
        /// Preforms a single level of 3-Point Gausian quadrature, with the
        /// desired number of sub-divisions. It preforms Gausian intergration
        /// on each sub-interval, and sums them to get the final result.
        /// </summary>
        /// <param name="f">The intergrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <param name="n">Number of sub-divisions</param>
        /// <returns>A single level of Gausian intergration</returns>
        private static double SingleGauss3(VFunc f, double a, double b, long n)
        {
            //calculates the step size
            double h = (b - a) / (2 * n);
            double curr = a;
            double gauss = 0.0;

            //preforms gausian intergration
            for (long i = 0; i < n; i++)
            {
                double mid = curr + h;

                double temp = f(mid) * 8.0;
                temp += f((h * G3) + mid) * 5.0;
                temp += f((h * -G3) + mid) * 5.0;

                gauss += (temp / 9.0) * h;
                curr += h + h;
            }

            return gauss;
        }

        /// <summary>
        /// Recursivly preforms Gauss-Kronrod quadrature on the given function,
        /// subdividing the interval of intergration when and only when greater 
        /// precision is needed. 
        /// </summary>
        /// <param name="f">The function to intergrate</param>
        /// <param name="a">Lower bound of intergraiton</param>
        /// <param name="b">Upper bound of intergration</param>
        /// <param name="depth">Curent number of subdivisions</param>
        /// <returns>The intergral over the sub-interval</returns>
        private static double KronrodRec(VFunc f, double a, double b, int depth)
        {
            //used to evaluate the intergrand
            double dif = (b - a) / 2.0;
            double mid = (b + a) / 2.0;
            double x1 = f(mid);
            double x2 = 0.0;

            //samples the middle point
            double gauss = x1 * GW[0];
            double kron = x1 * KW[0];

            for (int i = 1; i < 8; i++)
            {
                //calculates the kronrod value for all points
                x1 = f((dif * GKN[i]) + mid);
                x2 = f((dif * -GKN[i]) + mid);
                kron += (x1 + x2) * KW[i];

                //calculates the gausian value for even points
                if (i % 2 == 0) gauss += (x1 + x2) * GW[i / 2];
            }

            //computes the reletive error in the estiments
            double err = (gauss - kron) / kron;

            if (depth < 64 && Math.Abs(err) > 1.0e-08)
            {
                x1 = KronrodRec(f, a, mid, depth + 1);
                x2 = KronrodRec(f, mid, b, depth + 1);

                return x1 + x2;
            }
            else
            {
                return kron * dif;
            }
        }

        #endregion //////////////////////////////////////////////////////////////////

        public Result<Cmplx> Trapezoid(VFunc<Cmplx> f, Cmplx a, Cmplx b)
        {
            //Line2D line = new Line2D((Point2D)a, (Point2D)b);

            //throw new NotImplementedException();

            Line2D line = new Line2D(a, b);
            return Trapezoid(f, line);
        }

        public Result<Cmplx> Trapezoid(VFunc<Cmplx> f, Curve2D c)
        {
            //splits the intergrand into two parts
            VFunc freal = RealIntergrand(f, c);
            VFunc fimag = ImagIntergrand(f, c);

            //intergrates over f(c(t)) * c'(t)
            var res_r = Trapezoid(freal, 0.0, 1.0);
            var res_i = Trapezoid(fimag, 0.0, 1.0);

            //returns the combined result
            return Combine(res_r, res_i);
        }

        public Result<Cmplx> Romberg(VFunc<Cmplx> f, Curve2D c)
        {
            //splits the intergrand into two parts
            VFunc freal = RealIntergrand(f, c);
            VFunc fimag = ImagIntergrand(f, c);

            //uses an internal intergrator to preform the intergration
            var quad = new Integrator(base.MaxSteps, base.Tolerance);
            quad.StepEvent += delegate(Object sender, StepEventArgs args)
            { args.Halt = this.Step(args.Error); };

            Initialise();

            //NOTE: If We Halt the first mehtod, the second method will still run

            //intergrates over f(c(t)) * c'(t)
            var res_r = quad.Romberg(freal, 0.0, 1.0);
            var res_i = quad.Romberg(fimag, 0.0, 1.0);

            //returns the combined result
            return Combine(res_r, res_i);

            //NOTE: Need to call Finish!!
        }

        private static VFunc RealIntergrand(VFunc<Cmplx> f, Curve2D c)
        {
            return delegate(double t)
            {
                Cmplx z = (Cmplx)c.Sample(t);
                z = f(z) * (Cmplx)c.Deriv(t);
                return z.CofR;
            };
        }

        private static VFunc ImagIntergrand(VFunc<Cmplx> f, Curve2D c)
        {
            return delegate(double t)
            {
                Cmplx z = (Cmplx)c.Sample(t);
                z = f(z) * (Cmplx)c.Deriv(t);
                return z.CofI;
            };
        }

        private static Result<Cmplx> Combine(Result<Double> res_r, Result<Double> res_i)
        {
            Cmplx res = new Cmplx(res_r, res_i);

            //NOTE: Use the euclidan norm to combine error values, the itteration
            //counts can simply be sumed.

            throw new NotImplementedException();
        }


        public IEnumerable<Double> Romberg2(VFunc f, double a, double b)
        {
            //checks that we have a correct bracket
            if (a > b) yield break; // return Romberg2(f, a, b);

            Initialise();

            //checks to see if we have zero length
            if (VMath.IsZero(b - a))
            {
                base.error = Math.Abs(b - a);
                yield return 0.0;
                yield break;
                //return Finish(0.0);
            }

            //sets up the array for romberg intergration
            int array_size = base.MaxSteps + 2;
            double[] prev = new double[array_size];
            double[] curr = new double[array_size];

            //performs the first level of intergration
            prev[0] = SingleTrap(f, a, b, 1);

            //used in main loop
            double rombn = 0.0;
            double rombl = 0.0;
            long trap = 2;
            int level = 1;

            while (true)
            {
                if (level % 2 == 0)
                {
                    //prefroms romberg intergration
                    prev[0] = SingleTrap(f, a, b, trap);
                    RombergLevel(curr, prev, level);

                    //checks for validation
                    rombn = prev[level];
                    rombl = curr[level - 1];
                }
                else
                {
                    //prefroms romberg intergration
                    curr[0] = SingleTrap(f, a, b, trap);
                    RombergLevel(prev, curr, level);

                    //checks for validation
                    rombn = curr[level];
                    rombl = prev[level - 1];
                }

                ////determins if we should continue
                //if (Step(rombl, rombn)) break;

                yield return rombn;

                //updates counters
                level = level + 1;
                trap = trap << 1;
            }

            ////returns the best answer
            //return Finish(rombn);
        }


        public IEnumerable<Cmplx> Romberg2(VFunc<Cmplx> f, Curve2D c)
        {
            //splits the intergrand into two parts
            VFunc freal = RealIntergrand(f, c);
            VFunc fimag = ImagIntergrand(f, c);

            var ittr1 = Romberg2(freal, 0.0, 1.0).GetEnumerator();
            var ittr2 = Romberg2(fimag, 0.0, 1.0).GetEnumerator();

            do
            {
                double r = ittr1.Current;
                double i = ittr2.Current;
                yield return new Cmplx(r, i);

            } while (ittr1.MoveNext() && ittr2.MoveNext());

            ittr1.Dispose();
            ittr2.Dispose();
        }


    }
}
