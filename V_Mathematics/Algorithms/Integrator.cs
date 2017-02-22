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

            ////adjusts for intfinite intervals
            //f = CheckBounds(f, ref a, ref b);

            while (true)
            {
                //preforms the trapizoid rule
                trap = SingleTrap(f, a, b, n);

                //determins if tollerence is met
                if (Step(last, trap)) break;

                //trailing refrence
                last = trap;
                //n = n + n;
                n = n << 1;
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
                rombl = prev[level - 1];
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



        public Result<Double> Gauss(VFunc f, double a, double b)
        {
            //checks that we have a correct bracket
            if (a > b)
            {
                var res = Gauss(f, b, a);
                return Finish(-res.Value);
            }

            //start with 4 subdivisions
            int n = 4;

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

            //adjusts for intfinite intervals
            f = CheckBounds(f, ref a, ref b);

            while (true)
            {
                //preforms the trapizoid rule
                curr = SingleLobatto5(f, a, b, n);

                //determins if tollerence is met
                if (Step(last, curr)) break;

                //trailing refrence
                last = curr;
                //n = n + n;
                n = n << 1;
            }

            //returns the best answer
            return Finish(curr);
        }




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
        private static VFunc CheckBounds(VFunc f, ref double a, ref double b)
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


        //private double SingleTrap2(VFunc f, double a, double b, int n)
        //{
        //    //notes that we preform n + 1 evaluations
        //    //Increment(n);

        //    //calculates the step size
        //    double h = (b - a) / n;
        //    double curr = a + h;
        //    double trap = f(curr);

        //    //preforms trapazoid rule
        //    for (int i = 2; i < (n - 1); i++)
        //    {
        //        curr = curr + h;
        //        trap = trap + (2.0 * f(curr));
        //    }

        //    trap = trap + f(b - h);

        //    //finishes rule and returns
        //    trap = trap * (h / 2.0);
        //    return trap;
        //}




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


        private double SingleGauss(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double mid = (b1 - a1) / 2.0;
                double avg = (b1 + a1) / 2.0;

                double x1 = (mid * 0.57735026918962576451) + avg;
                double x2 = (mid * -0.57735026918962576451) + avg;


                double temp = f(x1) + f(x2);
                gauss += temp * mid;


                curr = curr + h;
            }

            return gauss;
        }


        private double SingleGauss3(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double mid = (b1 - a1) / 2.0;
                double avg = (b1 + a1) / 2.0;

                double x1 = avg;
                double x2 = (mid * 0.77459666924148337704) + avg;
                double x3 = (mid * -0.77459666924148337704) + avg;


                double temp = f(x1) * (8.0 / 9.0);
                temp += f(x2) * (5.0 / 9.0);
                temp += f(x3) * (5.0 / 9.0);
                gauss += temp * mid;


                curr = curr + h;
            }

            return gauss;
        }


        private double SingleGauss4(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double mid = (b1 - a1) / 2.0;
                double avg = (b1 + a1) / 2.0;

                double x1 = (mid * 0.33998104358485626480) + avg;
                double x2 = (mid * -0.33998104358485626480) + avg;
                double x3 = (mid * 0.86113631159405257522) + avg;
                double x4 = (mid * -0.86113631159405257522) + avg;


                double temp = f(x1) * 0.65214515486254614263;
                temp += f(x2) * 0.65214515486254614263;
                temp += f(x3) * 0.34785484513745385737;
                temp += f(x4) * 0.34785484513745385737;
                gauss += temp * mid;


                curr = curr + h;
            }

            return gauss;
        }


        private double SingleGauss5(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double mid = (b1 - a1) / 2.0;
                double avg = (b1 + a1) / 2.0;

                double x1 = avg;
                double x2 = (mid * 0.53846931010568309104) + avg;
                double x3 = (mid * -0.53846931010568309104) + avg;
                double x4 = (mid * 0.90617984593866399280) + avg;
                double x5 = (mid * -0.90617984593866399280) + avg;


                double temp = f(x1) * (128.0 / 225.0);
                temp += f(x2) * 0.47862867049936646804;
                temp += f(x3) * 0.47862867049936646804;
                temp += f(x4) * 0.23692688505618908751;
                temp += f(x5) * 0.23692688505618908751;
                gauss += temp * mid;


                curr = curr + h;
            }

            return gauss;
        }


        private double SingleGauss7(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double dif = (b1 - a1) / 2.0;
                double mid = (b1 + a1) / 2.0;


                double temp = f(mid) * GW[0];
                temp += f((dif * GKN[2]) + mid) * GW[1];
                temp += f((dif * -GKN[2]) + mid) * GW[1];
                temp += f((dif * GKN[4]) + mid) * GW[2];
                temp += f((dif * -GKN[4]) + mid) * GW[2];
                temp += f((dif * GKN[6]) + mid) * GW[3];
                temp += f((dif * -GKN[6]) + mid) * GW[3];


                gauss += temp * dif;

                curr = curr + h;
            }

            return gauss;
        }


        private double SingleLobatto5(VFunc f, double a, double b, int n)
        {
            //calculates the step size
            double h = (b - a) / n;
            double curr = a;
            double gauss = 0.0;

            //preforms trapazoid rule
            for (int i = 0; i < n; i++)
            {
                double a1 = (curr);
                double b1 = (curr + h);

                double dif = (b1 - a1) / 2.0;
                double mid = (b1 + a1) / 2.0;


                double temp = f(mid) * (64.0 / 90.0);
                temp += f((dif * 0.65465367070797714380) + mid) * (49.0 / 90.0);
                temp += f((dif * -0.65465367070797714380) + mid) * (49.0 / 90.0);
                temp += f((dif * 1.0) + mid) * (9.0 / 90.0);
                temp += f((dif * -1.0) + mid) * (9.0 / 90.0);


                gauss += temp * dif;

                curr = curr + h;
            }

            return gauss;
        }



        #endregion //////////////////////////////////////////////////////////////////


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

        private static readonly double[] GW =
        {
            4.1795918367346938776e-01,
            3.8183005050511894495e-01,
            2.7970539148927666790e-01,
            1.2948496616886969327e-01,
        };

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



        private static double KronrodRec
            (VFunc f, double a, double b, double tol, int depth)
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

            gauss = gauss * dif;
            kron = kron * dif;

            ////computes the recomended error estimate
            //double err = 200.0 * Math.Abs(gauss - kron);
            //err = Math.Sqrt(err * err * err);

            ////computes the apropriately scaled error
            //double err = Math.Abs((gauss - kron) / kron);
            //err = Math.Sqrt(err * err * err);


            double err = (gauss - kron) / kron;


            if (depth < 64 && Math.Abs(err) > 1.0e-10)
            {
                tol = tol / 2.0;

                x1 = KronrodRec(f, a, mid, tol, depth + 1);
                x2 = KronrodRec(f, mid, b, tol, depth + 1);

                return x1 + x2;
            }
            else
            {
                if (depth < 16)
                {
                    Console.WriteLine("Depth: " + depth);
                }


                return kron;

                
            }

        }


        public static double Kronrod(VFunc f, double a, double b, double tol)
        {
            f = CheckBounds(f, ref a, ref b);

            //intitiates the recursive procedure
            return KronrodRec(f, a, b, tol, 0);
        }



        /**
         * NOTES:
         * 
         * Consider using both the 3-point and the 5-point gausuian quadrature rules
         * as seperate methods of intergration.
         * 
         * Try adding Gauss-Loboto quadrature as a static recursive method, utlising
         * the 3-point and the 5-point rules, to see how that works. I could also
         * use Adaptive Simpisons method, should I need another recursive method.
         * 
         * Gauss-Kronrod quadrature is probably the best intergration formula I could
         * get. However, it is a static recursive method, which means that it cannot
         * be compared to the other dynamic methods.
         * 
         * At this point in time, I don't think I want to bother with Clenshaw-Kurtius,
         * or Chebychev quadrature. They don't exactly work the way I intend to use
         * them, and I doubt they would add mutch to my toolbox.
         * 
         */



    }
}
