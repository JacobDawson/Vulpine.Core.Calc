/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;

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
    /// <remarks>Last Update: 2013-11-19</remarks>
    public class RootFinder : Algorithm
    {
        #region Class Definitions...

        ///// <summary>
        ///// Constructs a new root finder instance, set with a given error
        ///// tollarance and maximum number of itterations, to be used in all
        ///// root finding methods.
        ///// </summary>
        ///// <param name="max">Maximum number of itterations</param>
        ///// <param name="tol">Error tollerence in output</param>
        //public RootFinder(int max = DMAX, double tol = DTOL)
        //{
        //    //sets the tolerance and max itterations
        //    base.max = (max > 0) ? max : 1;
        //    base.tol = (tol > 0.0) ? tol : 0.0;

        //    //initialises the cotroler
        //    Initialise();
        //}

        //public override string ToString()
        //{
        //    return base.ToString();
        //}

        /// <summary>
        /// Creates a new Root Finder with default stoping criteria.
        /// </summary>
        public RootFinder() 
            : base() { }

        /// <summary>
        /// Creates a new Root Finder with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial relitave error</param>
        public RootFinder(int max, double tol) 
            : base(max, tol) { }

        /// <summary>
        /// Creates a new Root Finder with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial relitave error</param>
        public RootFinder(int max, double toll, bool rel) 
            : base(max, toll, rel) { }


        public event EventHandler<EventArgs> MyEvent
        {
            add
            {
                Console.WriteLine("add operation");
            }
            remove
            {
                Console.WriteLine("remove operation");
            }
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
        /// <exception cref="InvBracketExcp">If the given search bracket
        /// dose not contain a root of the function</exception>
        public Result<Double> Bisection(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            InvBracketExcp.CheckOrder(low, high);

            //preformes first evaluation
            double y1 = f(low);
            double y2 = f(high);
            double next = 0.0;
            double curr = low;

            //checks that the bracket contains atleast one zero
            if (y1 * y2 > 0.0) InvBracketExcp.Throw(low, high);

            //initialises the cotroler
            Initialise();

            while (true)
            {
                //evaluates the midpoint
                next = (low + high) / 2.0;
                if (Step(curr, next)) break;

                //selects next bracket 
                y1 = f(low);
                y2 = f(next);
                if (y1 * y2 > 0.0) low = next;
                else high = next;

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
        /// <exception cref="InvBracketExcp">If the given search bracket
        /// dose not contain a solution</exception>
        public Result<Double> Bisection(VFunc f, double y,
            double low, double high)
        {
            //finds the root of the modified fucntion
            return Bisection(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Uses bisection to locate the point of intersection between 
        /// two arbitrary functions. The given search bracket must contain 
        /// the desired point of intersection.
        /// </summary>
        /// <param name="f1">The frist function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        /// <exception cref="InvBracketExcp">If the given search bracket dose
        /// not contain a point of intersection</exception>
        public Result<Double> Bisection(VFunc f1, VFunc f2, 
            double low, double high)
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
        /// <exception cref="InvBracketExcp">If the given search bracket
        /// dose not contain a root of the function</exception>
        public Result<Double> FalsePos(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            InvBracketExcp.CheckOrder(low, high);

            //preformes first evaluation
            double y1 = f(low);
            double y2 = f(high);
            double next = 0.0;
            double curr = low;

            //checks that the bracket contains atleast one zero
            if (y1 * y2 > 0.0) InvBracketExcp.Throw(low, high);

            //initialises the cotroler
            Initialise();
         
            while (true)
            {
                //evaluates the false position
                y1 = f(low);
                y2 = f(high);
                next = y2 * (low - high);
                next = high - (next / (y1 - y2));

                //determins if a root is found
                if (Step(curr, next)) break;

                //selects next bracket          
                if (y1 * f(next) > 0) low = next;
                else high = next;

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
        /// <exception cref="InvBracketExcp">If the given search bracket
        /// dose not contain a solution</exception>
        public Result<Double> FalsePos(VFunc f, double y,
            double low, double high)
        {
            //finds the root of the modified fucntion
            return FalsePos(x => f(x) - y, low, high);
        }

        /// <summary>
        /// Uses false postion to locate the point of intersection between 
        /// two arbitrary functions. The given search bracket must contain 
        /// the desired point of intersection.
        /// </summary>
        /// <param name="f1">The frist function</param>
        /// <param name="f2">The second function</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>A point of intersection between the two functions</returns>
        /// <exception cref="InvBracketExcp">If the given search bracket dose
        /// not contain a point of intersection</exception>
        public Result<Double> FalsePos(VFunc f1, VFunc f2,
            double low, double high)
        {
            //finds the root of the functions' diffrence
            return FalsePos(x => f1(x) - f2(x), low, high);
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
        /// <returns>A potential root of the given funciton, based off 
        /// of the initial guesses</returns>
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

            while (true)
            {
                //preformes the secend formula
                double temp = f(curr);
                next = (temp - y) * (prev - curr);
                next = next / (f(prev) - temp);
                next = curr - next;

                //determins if a root is found
                if (Step(curr, next)) break;

                //updates values
                prev = curr;
                curr = next;
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
        /// <param name="f1">The frist function</param>
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

        public Result<Double> Newton(VFunc f, VFunc dx, double y, double guess)
        {
            return Newton(x => f(x) - y, dx, guess);
        }

        ///**
        // *  Preforms Newton's Method to locate a root to a diffrentiable
        // *  function.
        // *  Precondtions: A diffrentiable function to evaluate, and an
        // *  innital guess at the position of the root.
        // *  Postcondtions: The root found by using Newton's Method.
        // *  Throws: CalculusException if the maximum number of allowed
        // *  itterations are exausted.
        // */
        //public double Newtons
        //(Func f, Func dx, double guess, double target)
        //{
        //    double curr = guess;
        //    double next = 0.0;
        //    Initialise();

        //    //iterative loop
        //    while (true)
        //    {
        //        //newton's formula
        //        next = f.Evaluate(curr) - target;
        //        next = next / dx.Evaluate(curr);
        //        next = curr - next;

        //        //determins if a root is found
        //        if (Step(curr, next)) break;

        //        //updates values
        //        curr = next;
        //    }

        //    //returns the best answer
        //    return next;
        //}

        
    }
}
