/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
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

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Algorithms
{
    public class Optimizer : Algorithm
    {
        //public Optimizer(int max = DMAX, double tol = DTOL)
        //{
        //    //sets the tolerance and max itterations
        //    base.max = (max > 0) ? max : 1;
        //    base.tol = (tol >= 0.0) ? tol : 0.0;

        //    //initialises the cotroler
        //    Initialise();
        //}

        #region Ternary Search...

        public Result<Double> TernaryMin(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //sets the initial bounds
            double a = low;
            double b = high;

            //used in computing the minimum
            double c, m1, m2, f1, f2;

            double curr = (a + b) / 2.0;
            double last = a;

            while (!Step(last, curr))
            {
                //selects two points inside the bracket
                c = (b - a) / 3.0;
                m1 = a + c;
                m2 = b - c;

                //evaluates the funciton at those points
                f1 = f(m1);
                f2 = f(m2);

                Increment(1);

                //updates the bounds acordingly
                if (f1 > f2) a = m1;
                else b = m2;

                last = curr;
                curr = (a + b) / 2.0;
            }

            //returns the minimum argument found
            return Finish(curr);
        }


        public static Result<Double> TernaryMin(VFunc f, double low, double high,
            double tol = VMath.TOL, int max = 1024)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //sets the initial bounds
            double a = low;
            double b = high;

            //used in computing the minimum
            double c, m1, m2, f1, f2;

            //tracks the status of the algorythim
            double last = (a + b) / 2.0;
            double curr = 0.0;
            double error = 1.0;
            int count = 0;

            while (error > tol && count < max)
            {
                //selects two points inside the bracket
                c = (b - a) / 3.0;
                m1 = a + c;
                m2 = b - c;

                //evaluates the funciton at those points
                f1 = f(m1);
                f2 = f(m2);

                //updates the bounds acordingly
                if (f1 > f2) a = m1;
                else b = m2;

                //computes the relitive error
                curr = (a + b) / 2.0;
                error = VMath.Error(last, curr);

                //updates the status
                last = curr;
                count += 2;
            }

            //returns the minimum argument found
            return new Result<Double>(curr, error, count);
        }


        #endregion /////////////////////////////////////////////////////////////////////////

        #region Golden Section Search...


        public Result<Double> GoldenMin(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //sets the initial bounds
            double a = low;
            double b = high;

            //computes the middle points
            double m1 = b - (b - a) / VMath.PHI;
            double m2 = a + (b - a) / VMath.PHI;

            //computes the intitial funciton vlaues
            double f1 = f(m1);
            double f2 = f(m2);

            double curr = (a + b) / 2.0;
            double last = a;

            while (!Step(last, curr))
            {
                if (f1 > f2)
                {
                    a = m1;

                    //reclaculates the middel values (m1' = m2)
                    m1 = b - (b - a) / VMath.PHI;
                    m2 = a + (b - a) / VMath.PHI;

                    //updates the function vlaues 
                    f1 = f2;
                    f2 = f(m2);
                }
                else
                {
                    b = m2;

                    //reclaculates the middel values (m2' = m1)
                    m2 = a + (b - a) / VMath.PHI;
                    m1 = b - (b - a) / VMath.PHI;

                    //updates the function vlaues
                    f2 = f1;
                    f1 = f(m1);
                }

                last = curr;
                curr = (a + b) / 2.0;
            }

            //returns the minimum argument found
            return Finish(curr);

        }


        public static Result<Double> GoldenMin(VFunc f, double low, double high,
            double tol = VMath.TOL, int max = 1024)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //sets the initial bounds
            double a = low;
            double b = high;

            //computes the middle points
            double m1 = b - (b - a) / VMath.PHI;
            double m2 = a + (b - a) / VMath.PHI;

            //computes the intitial funciton vlaues
            double f1 = f(m1);
            double f2 = f(m2);

            //tracks the status of the algorythim
            double last = (a + b) / 2.0;
            double curr = 0.0;
            double error = 1.0;
            int count = 0;

            while (error > tol && count < max)
            {
                if (f1 > f2)
                {
                    a = m1;

                    //reclaculates the middel values (m1' = m2)
                    m1 = b - (b - a) / VMath.PHI;
                    m2 = a + (b - a) / VMath.PHI;

                    //updates the function vlaues 
                    f1 = f2;
                    f2 = f(m2);
                }
                else
                {
                    b = m2;

                    //reclaculates the middel values (m2' = m1)
                    m2 = a + (b - a) / VMath.PHI;
                    m1 = b - (b - a) / VMath.PHI;

                    //updates the function vlaues
                    f2 = f1;
                    f1 = f(m1);
                }

                //computes the relitive error
                curr = (a + b) / 2.0;
                error = VMath.Error(last, curr);

                //updates the status
                last = curr;
                count++;

            }

            //returns the minimum argument found
            return new Result<Double>(curr, error, count);

        }

        //NOTE: alternate error forumla ??
        //error = |b - a| / (|m2| + |m1|)
        //error = 2 * |b - a| / |m2 + m1|



        public static IEnumerable<Double> GoldenMinIttr(VFunc f, 
            double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //sets the initial bounds
            double a = low;
            double b = high;

            //computes the middle points
            double m1 = b - (b - a) / VMath.PHI;
            double m2 = a + (b - a) / VMath.PHI;

            //computes the intitial funciton vlaues
            double f1 = f(m1);
            double f2 = f(m2);

            while (true)
            {
                //computes the next value
                double curr = (a + b) / 2.0;
                yield return curr;

                if (f1 > f2)
                {
                    a = m1;

                    //reclaculates the middel values (m1' = m2)
                    m1 = b - (b - a) / VMath.PHI;
                    m2 = a + (b - a) / VMath.PHI;

                    //updates the function vlaues 
                    f1 = f2;
                    f2 = f(m2);
                }
                else
                {
                    b = m2;

                    //reclaculates the middel values (m2' = m1)
                    m2 = a + (b - a) / VMath.PHI;
                    m1 = b - (b - a) / VMath.PHI;

                    //updates the function vlaues
                    f2 = f1;
                    f1 = f(m1);
                }
            }
        }


        #endregion /////////////////////////////////////////////////////////////////////////


        public static Result<Vector> GradinetMin(MFunc f, Vector x,
            double tol = VMath.TOL, int max = 1024)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x);

            while (true)
            {
                //determins the optimal step-size using golden search
                VFunc af = a => f(xn - Gradient(f, xn, a));
                double an = GoldenMin(af, 0.0, 1.0, tol, max);

                //travels the path of steepest decent
                xn = xn - Gradient(f, xn, an);
            }


            //return default(Result<Vector>);

        }


        public Result<Vector> GradinetMin(MFunc f, Vector x)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            Optimizer op = new Optimizer();
            this.Initialise();

            while (true)
            {
                //determins the optimal step-size using golden search
                VFunc af = a => f(xn - Gradient(f, xn, a));
                double an = op.GoldenMin(af, 0.0, 1.0);

                //travels the path of steepest decent
                curr = xn - Gradient(f, xn, an);

                //determins if we should stop
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }


            return Finish(curr);

        }


        public static IEnumerable<Vector> GradinetMinIttr(MFunc f, Vector x)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x);

            while (true)
            {
                //determins the optimal step-size using golden search
                VFunc af = a => f(xn - Gradient(f, xn, a));
                var golden = GoldenMinIttr(af, 0.0, 1.0);
                double an = golden.Limit().Last();

                //NOTE: The choice of limit in computing 'an' is totaly arbitrary!!

                //travels the path of steepest decent
                xn = xn - Gradient(f, xn, an);

                yield return xn;
            }


            //return default(Result<Vector>);

        }


        public static Result<Vector> GradientMin(MFunc f, Vector x, double step,
            double tol = VMath.TOL, int max = 1024)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x);

            //initialises the algorythim
            double an = step;
            double d1 = 1.0;
            double d2 = 0.0;

            while (true)
            {
                //evaluates the funciton at xn
                double f_xn = f(xn);

                //aproximates the gradient at our curent point
                Vector agn = Gradient(f, xn, an);

                //uses the armijo conditions to check alpha
                d1 = f(xn - agn);
                d2 = f_xn - (0.5 * (agn * agn) / an);

                while (d1 > d2)
                {
                    //computes the new step-size and gradient
                    an = 0.5 * an;
                    agn = Gradient(f, xn, an);

                    //updates the armijo conditions
                    d1 = f(xn - agn);
                    d2 = f_xn - (0.5 * (agn * agn) / an);
                }

                //travels along the steepest decent
                xn = xn - agn;                               
            }
        }

        //IDEA: should I reset alpha to the step size at the begining
        //of each itteration of the outer loop??


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
        /// Helper method to aproximate the gradient of a multi-dimentional
        /// funciton at a given point. This method actualy calculates the
        /// gradient times the step size (h), in order to get the actual 
        /// gradient one must divide through by (h).
        /// </summary>
        /// <param name="f">Function to be evaluated</param>
        /// <param name="x">Point of intrest</param>
        /// <param name="h">Step size</param>
        /// <returns>The gradient of the funciton at the point of intrest</returns>
        private static Vector Gradient(MFunc f, Vector x, double h)
        {
            //obtains the step size
            double step = h / 2.0;

            //creates a vector to store the gradient
            Vector grad = new Vector(x.Length);

            for (int i = 0; i < x.Length; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + step;
                b[i] = b[i] - step;

                //aproximates the partial derivitive
                grad[i] = f(a) + f(b);
            }

            return grad;
        }

    }
}
