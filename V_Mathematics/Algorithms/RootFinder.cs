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
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// Often when working with funcitons, one is required to find the roots of
    /// a function, or the values for which the funciton maps to zero. By finding 
    /// the roots of funcitons it's possable to invert a funciton or even compute 
    /// the intersection of two funcitons. This class provides a set of methods 
    /// that can find the root of a funciton numericaly. Some methods require 
    /// the root to be bracketed, while others require an initial guess. Not all
    /// methods garentee a root will be found.
    /// </summary>
    /// <remarks>Last Update: 2017-02-02</remarks>
    public sealed class RootFinder : Algorithm
    {
        #region Class Definitions...

        //Degree of Seperation for Finite Diffrences
        private const double H0 = 1.0e-06;

        /// <summary>
        /// Creates a new Root Finder with default stoping criteria.
        /// </summary>
        public RootFinder()
        {
            base.max = 256;
            base.tol = VMath.TOL;
        }

        /// <summary>
        /// Creates a new Root Finder with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public RootFinder(int max, double tol)
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Bisection...

        /// <summary>
        /// Uses bisection to locate a paticular root of a given function.
        /// The desired root must be contained within the given search 
        /// bracket. Note that if the bracket contains more than one root, 
        /// only one will be found.
        /// </summary>
        /// <param name="f">The fucnction in question</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A root of the function</returns>
        public Result<Double> Bisection(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //preformes first evaluation
            double y1 = f(low);
            double y2 = f(high);
            double next = 0.0;
            double curr = low;

            //initialises the cotroler
            Initialise();

            //checks that we contain atleast one zero
            if (y1 * y2 > 0.0) return Finish(low);

            while (true)
            {
                //evaluates the midpoint
                next = (low + high) / 2.0;
                if (Step(curr, next)) break;

                double test = f(next);

                if (y1 * test > 0.0)
                {
                    //selects the upper bracket
                    low = next;
                    y1 = test;
                }
                else
                {
                    //selects the upper bracket
                    high = next;
                    y2 = test;
                }

                //updates values
                curr = next;
            }

            //returns the best answer so far
            return Finish(next);
        }

        /// <summary>
        /// Uses bisection to compute the inverse of an arbitary function.
        /// The solution of the inverse funciton must be contained within
        /// the search bracket. 
        /// </summary>
        /// <param name="f">The function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A soultion to the inverse function</returns>
        public Result<Double> Bisection(VFunc f, double y, double low, double high)
        {
            //finds the root of the modified fucntion
            return Bisection(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Uses bisection to locate the point of intersection between 
        /// two arbitrary functions. The given search bracket must contain 
        /// the desired point of intersection.
        /// </summary>
        /// <param name="f1">The first function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        public Result<Double> Bisection(VFunc f1, VFunc f2, double low, double high)
        {
            //finds the root of the functions' diffrence
            return Bisection(x => f1(x) - f2(x), low, high);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region False Position...

        /// <summary>
        /// Uses false position to locate a paticular root of a given 
        /// function. The desired root must be contained within the given 
        /// search bracket. Note that if the bracket contains more than 
        /// one root, only one will be returned.
        /// </summary>
        /// <param name="f">The fucnction in question</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A root of the function</returns>
        public Result<Double> FalsePos(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //preformes first evaluation
            double y1 = f(low);
            double y2 = f(high);
            double next = 0.0;
            double curr = low;

            //initialises the cotroler
            Initialise();

            //checks that we contain atleast one zero
            if (y1 * y2 > 0.0) return Finish(low);
         
            while (true)
            {
                //evaluates the false position
                next = (0.5 * low * y2) - (high * y1);
                next = next / ((0.5 * y2) - y1);

                //determins if a root is found
                if (Step(curr, next)) break;

                double test = f(next);

                if (y1 * test > 0.0)
                {
                    //selects the upper bracket
                    low = next;
                    y1 = test;
                }
                else
                {
                    //selects the upper bracket
                    high = next;
                    y2 = test;
                }

                //updates values
                curr = next;
            }

            //returns the best answer so far
            return Finish(next);
        }

        /// <summary>
        /// Uses false postion to compute the inverse of an arbitary function.
        /// The solution of the inverse funciton must be contained within
        /// the search bracket. 
        /// </summary>
        /// <param name="f">The function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A soultion to the inverse function</returns>
        public Result<Double> FalsePos(VFunc f, double y, double low, double high)
        {
            //finds the root of the modified fucntion
            return FalsePos(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Uses false postion to locate the point of intersection between 
        /// two arbitrary functions. The given search bracket must contain 
        /// the desired point of intersection.
        /// </summary>
        /// <param name="f1">The first function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        public Result<Double> FalsePos(VFunc f1, VFunc f2, double low, double high)
        {
            //finds the root of the functions' diffrence
            return FalsePos(x => f1(x) - f2(x), low, high);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Ridders Method...

        /// <summary>
        /// Uses Ridders method to locate a paticular root of the given
        /// function. Ridders method is similar to false position, except it
        /// uses an exponenital generate succesive itterations, rather than
        /// a straight line. 
        /// </summary>
        /// <param name="f">The fucnction in question</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A root of the function</returns>
        public Result<Double> Ridders(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //preformes first evaluation
            double y1 = f(low);
            double y2 = f(high);
            double x1 = low;
            double x2 = high;

            //initialises the cotroler
            Initialise();

            //checks that we contain atleast one zero
            if (y1 * y2 > 0.0) return Finish(low);

            while (true)
            {
                //calculates x3 and y3
                double x3 = (x1 + x2) * 0.5;
                double y3 = f(x3);

                //invokes Ridder's funciton
                double x4 = VMath.Sign(y1 - y2) * y3;
                x4 = (x3 * x4) - (x1 * x4);
                x4 = x4 / Math.Sqrt((y3 * y3) - (y1 * y2));
                x4 = x3 + x4;

                //determins if a root is found
                if (Step(x2, x4)) return Finish(x4);

                double y4 = f(x4);
                Increment(1);

                //determins which of x1, x2, or x3 to use next
                if (y3 * y4 < 0.0)
                {
                    x1 = x3;
                    y1 = y3;
                }
                else if (y2 * y4 < 0.0)
                {
                    x1 = x2;
                    y1 = y2;
                }

                x2 = x4;
                y2 = y4;
            }
        }

        /// <summary>
        /// Uses Ridders method to compute the inverse of an arbitrary 
        /// funciton. Ridders method is similar to false position, except it
        /// uses an exponenital generate succesive itterations, rather than
        /// a straight line. 
        /// </summary>
        /// <param name="f">The function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A soultion to the inverse function</returns>
        public Result<Double> Ridders(VFunc f, double y, double low, double high)
        {
            //finds the root of the modified fucntion
            return Ridders(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Uses Ridders method to locate the point of intersection between 
        /// two arbitrary functions. Ridders method is similar to false position, 
        /// except it uses an exponenital generate succesive itterations, rather 
        /// than a straight line. 
        /// </summary>
        /// <param name="f1">The first function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        public Result<Double> Ridders(VFunc f1, VFunc f2, double low, double high)
        {
            //finds the root of the functions' diffrence
            return Ridders(x => f1(x) - f2(x), low, high);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Newtons Method...

        /// <summary>
        /// Uses Newton's method to locate the root of a deferenciable funciton.
        /// Note this method only works if the derivitve is known. If the drivitive
        /// can only be aproximated, consider using the secant method instead.
        /// </summary>
        /// <param name="f">The function in question</param>
        /// <param name="dx">The dirivitive of the funciton</param>
        /// <param name="guess">An initial guess</param>
        /// <returns>A potential root of the given funciton</returns>
        public Result<Double> Newton(VFunc f, VFunc dx, double guess)
        {
            //keeps a trailing refrence
            double curr = guess;
            double next = 0.0;

            //initialises the cotroler
            Initialise();

            while (true)
            {
                //applys newton's formula
                next = f(curr) / dx(curr);
                next = curr - next;

                //determins if a root is found
                if (Step(curr, next)) break;

                //updates values
                curr = next;
            }

            //returns the best answer so far
            return Finish(next);
        }

        /// <summary>
        /// Uses Newton's method to compute the inverse of an arbitary funciton, 
        /// given it's derivitive. Note this method only works if the exact 
        /// derivitive is known. If the drivitive can only be aproximated, consider 
        /// using the secant method instead.
        /// </summary>
        /// <param name="f">The funciton in question</param>
        /// <param name="dx">The derivitive of the funciton</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="guess">An intial guess</param>
        /// <returns>A potential soultion to the inverse function</returns>
        public Result<Double> Newton(VFunc f, VFunc dx, double y, double guess)
        {
            return Newton(x => f(x) - y, dx, guess);
        }

        /// <summary>
        /// Uses Newton's method to locate the root of a complex, deferenciable 
        /// funciton. Note this method only works if the exact derivitve is known. 
        /// If the drivitive can only be aproximated, consider using the secant 
        /// method instead.
        /// </summary>
        /// <param name="f">The complx function in question</param>
        /// <param name="dx">The dirivitive of the funciton</param>
        /// <param name="guess">An initial guess</param>
        /// <returns>A potential root of the given funciton</returns>
        public Result<Cmplx> Newton(VFunc<Cmplx> f, VFunc<Cmplx> dx, Cmplx guess)
        {
            //keeps a trailing refrence
            Cmplx curr = guess;
            Cmplx next = 0.0;

            //initialises the cotroler
            Initialise();

            while (true)
            {
                //applys newton's formula
                next = f(curr) / dx(curr);
                next = curr - next;

                //determins if a root is found
                if (Step(curr, next)) break;

                //updates values
                curr = next;
            }

            //returns the best answer so far
            return Finish(next);
        }

        /// <summary>
        /// Uses Newton's method to compute the inverse of a complex funciton, 
        /// given it's derivitive. Note this method only works if the exact 
        /// derivitive is known. If the drivitive can only be aproximated, consider 
        /// using the secant method instead.
        /// </summary>
        /// <param name="f">The complx function in question</param>
        /// <param name="dx">The derivitive of the funciton</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="guess">An intial guess</param>
        /// <returns>A potential soultion to the inverse function</returns>
        public Result<Cmplx> Newton(VFunc<Cmplx> f, VFunc<Cmplx> dx, Cmplx y, Cmplx guess)
        {
            return Newton(x => f(x) - y, dx, guess);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Secant Method...

        /// <summary>
        /// Uses the secant method to find the root of a generic function,
        /// given two initial guesses. The guesses need not bracket the
        /// desired root, nor is it garenteed to find a solution if the
        /// guesses are far from any given root.
        /// </summary>
        /// <param name="f">The function in question</param>
        /// <param name="x1">First initial guess</param>
        /// <param name="x2">Second initial guess</param>
        /// <returns>A potential root of the given funciton</returns>
        public Result<Double> Secant(VFunc f, double x1, double x2)
        {
            //calls upon the more specialised method
            return Secant(f, 0.0, x1, x2);
        }

        /// <summary>
        /// Uses the secant method to compute the inverse of an arbitary 
        /// function. Two intial guesses to the function's solution must
        /// be given. It is not garenteed to find a solution if the guesses
        /// are far from the actual result.
        /// </summary>
        /// <param name="f">The function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="x1">First initial guess</param>
        /// <param name="x2">Second initial guess</param>
        /// <returns>A potential soultion to the inverse function</returns>
        public Result<Double> Secant(VFunc f, double y, double x1, double x2)
        {
            //used in preforming the secant method
            double prev = x1;
            double curr = x2;
            double next = 0.0;

            //initialises the cotroler
            Initialise();

            double y1 = f(curr);
            double y2 = f(prev);

            while (true)
            {
                //preformes the secend formula
                next = prev * (y1 - y);
                next = next - (curr * (y2 - y));
                next = next / (y1 - y2);

                //determins if a root is found
                if (Step(curr, next)) break;

                //updates values
                prev = curr;
                curr = next;
                y2 = y1;
                y1 = f(curr);
            }

            //returns the best answer so far
            return Finish(next);
        }       

        /// <summary>
        /// Uses the secent method to find the zeros of a complex funciton. Two
        /// initial gesses for the zero must be given. A zero is not garenteed
        /// to be found if the initial guesses are far apart or are nowhere near 
        /// an actual zero. 
        /// </summary>
        /// <param name="f">The complex function in question</param>
        /// <param name="x1">First initial guess</param>
        /// <param name="x2">Second initial guess</param>
        /// <returns>A potential zero of the complex funciton</returns>
        public Result<Cmplx> Secant(VFunc<Cmplx> f, Cmplx x1, Cmplx x2)
        {
            //calls upon the more specialised method
            return Secant(f, 0.0, x1, x2);
        }

        /// <summary>
        /// Uses the secant method to find the inverse of a complex funciton.
        /// Two intial guesses to the function's solution must be given. It is 
        /// not garenteed to find a solution if the guesses are far apart or
        /// far from the actual result.
        /// </summary>
        /// <param name="f">The complex function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="x1">First initial guess</param>
        /// <param name="x2">Second initial guess</param>
        /// <returns>A potential soultion to the inverse function</returns>
        public Result<Cmplx> Secant(VFunc<Cmplx> f, Cmplx y, Cmplx x1, Cmplx x2)
        {
            //used in preforming the secant method
            Cmplx prev = x1;
            Cmplx curr = x2;
            Cmplx next = 0.0;

            //initialises the cotroler
            Initialise();

            Cmplx y1 = f(curr);
            Cmplx y2 = f(prev);

            while (true)
            {
                //preformes the secant formula
                next = prev * (y1 - y);
                next = next - (curr * (y2 - y));
                next = next / (y1 - y2);

                //determins if a root is found
                if (Step(curr, next)) break;

                //updates values
                prev = curr;
                curr = next;
                y2 = y1;
                y1 = f(curr);
            }

            //returns the best answer so far
            return Finish(next);
        }

        /// <summary>
        /// Uses the secant method to locate the point of intersection between 
        /// two arbitrary functions. Two initial guesses must be given for
        /// the point of intersection. It is not garenteed to find a solution 
        /// if the guesses are far from the actual intersection.
        /// </summary>
        /// <param name="f1">The first function</param>
        /// <param name="f2">The second function</param>
        /// <param name="x1">First initial guess</param>
        /// <param name="x2">Second initial guess</param>
        /// <returns>A potential point of intersection between the functions</returns>
        public Result<Double> Secant(VFunc f1, VFunc f2, double x1, double x2)
        {
            //finds the root of the functions' diffrence
            return Secant(x => f1(x) - f2(x), 0.0, x1, x2);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Brents Method...

        /// <summary>
        /// Utilises the Brent-Dekker method to locate a paticular root of the 
        /// given function. This method uses a combination of bisection and
        /// inverse quadratic interpolation. It remains bounded, like bisection,
        /// with a convergence rate similar to the secent method.
        /// </summary>
        /// <param name="f">The fucnction in question</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A root of the function</returns>
        public Result<Double> Brent(VFunc f, double low, double high)
        {
            //preformes first evaluation
            double x1 = low;
            double x2 = high;
            double y1 = f(x1);
            double y2 = f(x2);

            //initialises the cotroler
            Initialise();

            //checks that we contain atleast one zero
            if (y1 * y2 > 0.0) return Finish(x1);

            //used in the main loop
            double x3 = x1;
            double y3 = y1;
            double x4 = 0.0;
            bool mflag = true;
            double s = 0.0;

            while (true)
            {
                //makes shure y1 is less in magnitude than y2
                if (Math.Abs(y1) < Math.Abs(y2))
                {
                    Swap(ref y1, ref y2);
                    Swap(ref x1, ref x2);
                }

                //determins what type of interpolation to use
                bool nequ = Math.Abs(y1 - y3) > VMath.TOL;
                nequ = nequ & Math.Abs(y2 - y3) > VMath.TOL;

                if (nequ)
                {
                    //uses inverse quadratic interpolation           
                    s = (y2 * y3 * x1) / ((y1 - y2) * (y1 - y3));
                    s += (y1 * y3 * x2) / ((y2 - y1) * (y2 - y3));
                    s += (y1 * y2 * x3) / ((y3 - y1) * (y3 - y2));
                }
                else
                {
                    //preformes the secend formula
                    s = (x2 * y2) - (x1 * y2);
                    s = s / (y2 - y1);
                    s = x2 - s;
                }

                //calculates intermediat values
                double t = ((3.0 * x1) + x2) * 0.25;
                double sb = Math.Abs(s - x2);
                double dx = mflag ? (x2 - x3) : (x3 - x4);

                //determins weatther to accpet s or use bisection
                bool test = s < t && s < x2;
                test |= s > x2 && s > t;
                test |= sb >= Math.Abs(dx) * 0.5;

                s = test ? (x1 + x2) * 0.5 : s;
                mflag = test;

                //determins if a root is found
                if (Step(x2, s)) return Finish(s);

                //calculates the next values
                double fs = f(s);
                x4 = x3;
                x3 = x2;
                y3 = y2;

                //determins wheater to search right or left
                if (y1 * fs < 0.0)
                {
                    x2 = s;
                    y2 = fs;
                }
                else
                {
                    x1 = s;
                    y1 = fs;
                }
            }
        }

        /// <summary>
        /// Utilises the Brent-Dekker method to compute the inverse of an arbitrary 
        /// funciton. This method uses a combination of bisection and inverse 
        /// quadratic interpolation. It remains bounded, like bisection, with a 
        /// convergence rate similar to the secent method.
        /// </summary>
        /// <param name="f">The function to invert</param>
        /// <param name="y">Input to the inverse function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A soultion to the inverse function</returns>
        public Result<Double> Brent(VFunc f, double y, double low, double high)
        {
            //finds the root of the modified fucntion
            return Brent(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Utilises the Brent-Dekker method to locate the point of intersection 
        /// between two arbitrary functions. This method uses a combination of 
        /// bisection and inverse quadratic interpolation. It remains bounded, 
        /// like bisection, with a convergence rate similar to the secent method.
        /// </summary>
        /// <param name="f1">The first function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        public Result<Double> Brent(VFunc f1, VFunc f2, double low, double high)
        {
            //finds the root of the functions' diffrence
            return Brent(x => f1(x) - f2(x), low, high);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Broydens Method...

        /// <summary>
        /// Uses Broyden's "Bad" method to solve a system of non-liniar equations,
        /// with n equations and n unknowns, presented as a vector-vlaued funciton.
        /// Despite the name, the method dose its job quite well, and is ironicly
        /// more numericaly stable than Broyden's "Good" method. It is superior to
        /// Newton's method, in that the Jacobian of the system need only be
        /// computed once at the start.
        /// </summary>
        /// <param name="f">System of equations as a vector-valued funciton</param>
        /// <param name="x0">Starting paramaters to serve as an inital guess</param>
        /// <returns>The vector of paramaters that solves the system</returns>
        public Result<Vector> Broyden(VFunc<Vector> f, Vector x0)
        {
            //uses the aproximate Jacobian to initiate the method
            return Broyden(f, x0, Jacobian(f, x0));
        }


        /// <summary>
        /// Uses Broyden's "Bad" method to solve a system of non-liniar equations,
        /// with n equations and n unknowns, presented as a vector-vlaued funciton.
        /// Despite the name, the method dose its job quite well, and is ironicly
        /// more numericaly stable than Broyden's "Good" method. It is superior to
        /// Newton's method, in that the Jacobian of the system need only be
        /// computed once at the start.
        /// </summary>
        /// <param name="f">System of equations as a vector-valued funciton</param>
        /// <param name="x0">Starting paramaters to serve as an inital guess</param>
        /// <param name="j0">The Jacobian matrix at the starting position</param>
        /// <returns>The vector of paramaters that solves the system</returns>
        public Result<Vector> Broyden(VFunc<Vector> f, Vector x0, Matrix j0)
        {
            //our starting aproximate inverse jacobian
            Matrix bn = Matrix.Invert(j0);

            //vectors used during our computaiton
            Vector f1, f2, x1, x2, df, dx, c1;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            f1 = f(x1);

            while (true)
            {
                //computes the next step
                x2 = x1 - bn * f1;
                if (Step(x1, x2)) break;

                //computes the divergence in input and output
                f2 = f(x2);
                dx = x2 - x1;
                df = f2 - f1;

                //updates our sudo-jacobian inverse
                c1 = (dx - (bn * df)) / (df * df);
                bn += c1.Outer(df);

                //updates the refrences
                x1 = new Vector(x2);
                f1 = new Vector(f2);
            }

            //returns our solution
            return Finish(x2);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Swaps two floating point values in place.
        /// </summary>
        /// <param name="a">First value to swap</param>
        /// <param name="b">Second value to swap</param>
        private static void Swap(ref double a, ref double b)
        {
            double temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Computes the aproximate Jacobian matrix for a vector-valued 
        /// function at a given point, using finite diffrence aproximations 
        /// for the partial derivitives.
        /// </summary>
        /// <param name="f">Vector-valued funciton to evaluate</param>
        /// <param name="x">Position to evaluate</param>
        /// <returns>The aproximate Jacobian a the given point</returns>
        private static Matrix Jacobian(VFunc<Vector> f, Vector x)
        {
            int len = x.Length;
            Matrix jacob = new Matrix(len, len);

            Vector dx;

            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + H0;
                b[i] = b[i] - H0;

                dx = (f(a) - f(b)) / (2.0 * H0);

                jacob.SetRow(i, dx);
            }

            return jacob;
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}
