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

namespace Vulpine.Core.Calc.Algorithms
{
    public sealed class Integrator : Algorithm
    {
        //Things To Add:

        //Complex Intergration
        //Inversion of the Intergrand
        //Computing the Average Value
        //Computing the Arc Length
        //Multi-varient Intergration


        #region Class Definitions...

        /// <summary>
        /// Creates a new Integrator with default stoping criteria.
        /// </summary>
        public Integrator() : base() { }

        /// <summary>
        /// Creates a new Integrator with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Integrator(int max, double tol) : base(max, tol) { }

        /// <summary>
        /// Creates a new Integrator with the given maximum number of
        /// itterations and the minimal error allowed. Set the flag to false
        /// to use exact error instead of relative error.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="rel">Flag to use relitive error</param>
        public Integrator(int max, double toll, bool rel) 
            : base(max, toll, rel) { }

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

                //return Trapezoid(x => -f(x), b, a);
            }

            //start with 4 subdivisions
            int n = 4;

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
                n = n + n;
            }

            //returns the best answer
            return Finish(trap);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Romberg Intergration...

        /// <summary>
        /// Uses Romberg intergration to aproximate the antidrivitive of the
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
            //NEED TO CHECK THIS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //int array_size = (int)(2.0 + Math.Sqrt(0.25 + 2 * max));
            int array_size = base.MaxSteps + 2;
            double[] prev = new double[array_size];
            double[] curr = new double[array_size];

            //performs the first level of intergration
            prev[0] = SingleTrap(f, a, b, 1);

            //used in main loop
            double rombn = 0.0;
            double rombl = 0.0;
            int level = 1;
            int trap = 2;

            while (true)
            {
                //prefroms romberg intergration
                curr[0] = SingleTrap(f, a, b, trap);
                RombergLevel(prev, curr, level);

                //checks for validation
                rombn = curr[level];
                rombl = curr[level - 1];
                if (Step(rombl, rombn)) break;

                //copyes prev to curr
                for (int i = 0; i <= level; i++)
                    prev[i] = curr[i];

                //updates counters
                level = level + 1;
                trap = trap + trap;
            }

            //returns the best answer
            return Finish(rombn);
        }


        #endregion //////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Helper method, adjusts the bounds and the inergrand for open intervals,
        /// that is where the bounds are infinate. This makes it possable to compute
        /// infinate invervals with finite methods.
        /// </summary>
        /// <param name="f">The ingergrand</param>
        /// <param name="a">The lower bound</param>
        /// <param name="b">The upper bound</param>
        /// <returns>The modified intergrand</returns>
        private VFunc CheckBounds(VFunc f, ref double a, ref double b)
        {
            //stores the original bounds
            double x1 = a;
            double x2 = b;

            if (a.IsInfinity() && b.IsInfinity())
            {
                a = -1.0; 
                b = 1.0;

                //intergrates over the entire real line
                return delegate(double t)
                {
                    double t2 = t * t;
                    double tm = 1.0 - t2;
                    return f(t / tm) * (1.0 + t2) / (tm * tm);
                };
            }
            else if (a.IsInfinity())
            {
                a = 0.0;
                b = 1.0;

                //intergrates from negative infinity to (b)
                return t => f(x2 - (1.0 - t) / t) / (t * t);
            }
            else if (b.IsInfinity())
            {
                a = 0.0;
                b = 1.0;

                //intergrates from (a) to positive inifinty
                return delegate(double t)
                {
                    double tm = 1.0 - t;
                    return f(x1 + (t / tm)) / (tm * tm);
                };
            }

            return f;
        }


        private double SingleTrap(VFunc f, double a, double b, int n)
        {
            //notes that we preform n + 1 evaluations
            //Increment(n);

            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double trap = f(a);

            //preforms trapazoid rule
            for (int i = 1; i < n; i++)
            {
                curr = curr + h;
                trap = trap + (2.0 * f(curr));
            }

            trap = trap + f(b);

            //finishes rule and returns
            trap = trap * (h / 2.0);
            return trap;
        }


        private void RombergLevel(double[] prev, double[] curr, int level)
        {
            //used in loop
            double a1 = 0.0;
            double a2 = 0.0;
            double factor = 0.0;

            for (int i = 1; i <= level; i++)
            {
                //obtains the values to combine
                a1 = curr[i - 1];
                a2 = prev[i - 1];
                factor = Math.Pow(4.0, i);

                //combines the values
                curr[i] = ((factor * a1) - a2) / (factor - 1);
            }

        }



        #endregion //////////////////////////////////////////////////////////////////




    }
}
