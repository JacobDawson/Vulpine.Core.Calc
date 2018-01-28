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
        #region Class Definitions...

        /// <summary>
        /// Creates a new Optimizer with default stoping criteria.
        /// </summary>
        public Optimizer() : base() { }

        /// <summary>
        /// Creates a new Optimizer with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Optimizer(int max, double tol) : base(max, tol) { }

        /// <summary>
        /// Creates a new Optimizer with the given maximum number of
        /// itterations and the minimal error allowed. Set the flag to false
        /// to use exact error instead of relative error.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="rel">Flag to use relitive error</param>
        public Optimizer(int max, double toll, bool rel) 
            : base(max, toll, rel) { }

        #endregion //////////////////////////////////////////////////////////////

        #region Ternary Search...

        public Result<Double> TernaryMin(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            ////sets the initial bounds
            //double a = low;
            //double b = high;

            //used in computing the minimum
            double c, m1, m2, f1, f2;

            Initialise();

            ////tracks the values as they change
            //double last = low;
            //double curr = high;

            while (true)
            {
                ////checks the midpoint for termination
                //curr = (low + high) / 2.0;
                //if (Step(last, curr)) break;

                //selects two points inside the bracket
                c = (high - low) / 3.0;
                m1 = low + c;
                m2 = high - c;

                //computes the error and determins if we should stop
                double error = Math.Abs(m2) + Math.Abs(m1);
                error = Math.Abs(high - low) / error;
                if (Step(error)) break;

                //evaluates the funciton at those points
                f1 = f(m1);
                f2 = f(m2);

                Increment(1);

                //updates the bounds acordingly
                if (f1 > f2) low = m1;
                else high = m2;

                //last = curr;
            }

            ////returns the minimum argument found
            //return Finish(curr);

            //takes the midpoint to be our best value
            double best = (low + high) / 2.0;
            return Finish(best);
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

            Initialise();

            //computes the middle points
            double m1 = b - (b - a) / VMath.PHI;
            double m2 = a + (b - a) / VMath.PHI;

            //computes the intitial funciton vlaues
            double f1 = f(m1);
            double f2 = f(m2);

            ////tracks the values as they change
            //double last = a;
            //double curr = b;

            while (true)
            {
                ////checks the midpoint for termination
                //curr = (a + b) / 2.0;
                //if (Step(last, curr)) break;

                //computes the error and determins if we should stop
                double error = Math.Abs(m2) + Math.Abs(m1);
                error = Math.Abs(b - a) / error;
                if (Step(error)) break;

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

                //last = curr;
            }

            ////returns the minimum argument found
            //return Finish(curr);

            //takes the midpoint to be our best value
            double best = (a + b) / 2.0;
            return Finish(best);
        }


        //NOTE: alternate error forumla ??
        //error = |b - a| / (|m2| + |m1|)
        //error = 2 * |b - a| / |m2 + m1|



        #endregion /////////////////////////////////////////////////////////////////////////


        #region Gradient Using Finite Diffrence


        public Result<Vector> ExGradinetMin(MFunc f, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            Optimizer op = new Optimizer(MaxSteps, Tolerance);
            this.Initialise();

            while (true)
            {
                //aproximates the gradient using finite diffrences
                Vector g = Gradient2(f, xn, 1.0e-08);

                //determins the optimal step-size using golden search
                VFunc af = a => f(xn - (a * g));
                var an = op.GoldenMin(af, 0.0, step);

                Increment(an.NumSteps);

                //travels the path of steepest decent
                curr = xn - (an * g);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }

        public Result<Vector> ExGradinetMin2(MFunc f, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            Optimizer op = new Optimizer(MaxSteps, Tolerance);
            RootFinder rf = new RootFinder(MaxSteps, Tolerance);
            this.Initialise();

            while (true)
            {
                //aproximates the gradient using finite diffrences
                Vector grad = Gradient2(f, xn, 1.0e-08);

                ////determins the optimal step-size using golden search
                //VFunc af = a => f(xn - (a * grad));
                //var an = op.GoldenMin(af, 0.0, step);

                //Increment(an.NumSteps);



                //VFunc af = a => -grad * g(xn - (a * grad));
                VFunc af = a => -grad * Gradient2(f, xn - (a * grad), 1.0e-08);


                double f1 = af(0.0);
                double f2 = af(step);
                double an = step;
                double am = 0.0;


                //while (f1 * f2 > 0.0 && an > Tolerance)
                //{
                //    an = an * 0.5;
                //    f2 = af(an);
                //}

                //while (f1 * f2 > 0.0 && an - am > Tolerance)
                //{
                //    am = (am + an) / 2.0;
                //    f1 = af(am);
                //}


                if (f1 * f2 < 0)
                {
                    var res = rf.Brent(af, am, an);
                    Increment(res.NumSteps);

                    an = res.Value;
                }
                //else
                //{
                //    VFunc af2 = a => f(xn - (a * grad));
                //    var res = op.GoldenMin(af, 0.0, step);
                //    Increment(res.NumSteps);

                //    an = res.Value;

                //}
                //else
                //{
                //    double f_xn = f(xn);
                //    double dot = grad * grad;

                //    //uses the armijo conditions to check alpha
                //    double d2 = f_xn - (0.5 * an * dot);
                //    double d1 = f(xn - (an * grad));

                //    while (d1 > d2)
                //    {
                //        //computes the new step-size
                //        an = 0.5 * an;

                //        //updates the armijo conditions
                //        d2 = f_xn - (0.5 * an * dot);
                //        d1 = f(xn - (an * grad));

                //        Increment(1);
                //    }
                //}


                //if (an.IsNaN())
                //{
                //    Console.WriteLine("NaN found!");
                //}


                //travels the path of steepest decent
                curr = xn - (an * grad);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }


        public Result<Vector> BtGradientMin(MFunc f, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //used in finding the minimum
            double f_xn, an, d1, d2;

            Initialise();

            while (true)
            {
                //evaluates the funciton at xn
                f_xn = f(xn);
                an = step;

                //aproximates the gradient using the previous step-size
                Vector g = Gradient2(f, xn, 1.0e-08);
                double dot = g * g;

                //uses the armijo conditions to check alpha
                d2 = f_xn - (0.5 * an * dot);
                d1 = f(xn - (an * g));

                while (d1 > d2)
                {
                    //computes the new step-size
                    an = 0.5 * an;

                    //updates the armijo conditions
                    d2 = f_xn - (0.5 * an * dot);
                    d1 = f(xn - (an * g));

                    Increment(1);
                }

                //travels along the steepest decent
                curr = xn - (an * g);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Gradient Given By User


        public Result<Vector> ExGradinetMin(MFunc f, VFunc<Vector> g, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            Optimizer op = new Optimizer(MaxSteps, Tolerance);
            this.Initialise();

            while (true)
            {
                //aproximates the gradient using the previous step-size
                Vector grad = g(xn);

                //determins the optimal step-size using golden search
                VFunc af = a => f(xn - (a * grad));
                var an = op.GoldenMin(af, 0.0, step);

                Increment(an.NumSteps);

                //travels the path of steepest decent
                curr = xn - (an * grad);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }



        public Result<Vector> ExGradinetMin2(MFunc f, VFunc<Vector> g, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            Optimizer op = new Optimizer(MaxSteps, Tolerance);
            this.Initialise();

            RootFinder rf = new RootFinder(MaxSteps, Tolerance);

            while (true)
            {
                //aproximates the gradient using the previous step-size
                Vector grad = g(xn);

                ////determins the optimal step-size using golden search
                //VFunc af = a => f(xn - (a * grad));
                //var an = op.GoldenMin(af, 0.0, step);

                


                VFunc af = a => -grad * g(xn - (a * grad));
                double f1 = af(0.0);
                double f2 = af(step);
                double an = step;



                if (f1 * f2 < 0)
                {
                    var res = rf.Ridders(af, 0.0, step);
                    Increment(res.NumSteps);

                    an = res.Value;
                }




                //travels the path of steepest decent
                curr = xn - (an * grad);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }


        public Result<Vector> BtGradientMin(MFunc f, VFunc<Vector> g, Vector x0, double step)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //used in finding the minimum
            double f_xn, an, d1, d2;

            Initialise();

            while (true)
            {
                //evaluates the funciton at xn
                f_xn = f(xn);
                an = step;

                //aproximates the gradient using the previous step-size
                Vector grad = g(xn);
                double dot = grad * grad;

                //uses the armijo conditions to check alpha
                d2 = f_xn - (0.5 * an * dot);
                d1 = f(xn - (an * grad));

                while (d1 > d2)
                {
                    //computes the new step-size
                    an = 0.5 * an;

                    //updates the armijo conditions
                    d2 = f_xn - (0.5 * an * dot);
                    d1 = f(xn - (an * grad));

                    Increment(1);
                }

                //travels along the steepest decent
                curr = xn - (an * grad);
                if (Step(xn, curr)) break;

                //maske a copy of the curent vector
                xn = new Vector(curr);
            }

            //returns our local minima point
            return Finish(curr);
        }

        #endregion /////////////////////////////////////////////////////////////////////////







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
                grad[i] = f(a) - f(b);
            }

            return grad;
        }


        private static Vector Gradient2(MFunc f, Vector x, double h)
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
                grad[i] = (f(a) - f(b)) / h;
            }

            return grad;
        }


        private static Vector Gradient3(MFunc f, Vector x, double tol)
        {
            //creates a vector to store the gradient
            Vector grad = new Vector(x.Length);

            //computes the step size based on the output
            double step = tol * Math.Abs(f(x));

            for (int i = 0; i < x.Length; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                ////NOTE: This fails if x[i] is zero
                ////computes the step-size based on tolerance
                //double step = tol * Math.Abs(x[i]);

                //permutes the input along a given axis
                a[i] = a[i] + step;
                b[i] = b[i] - step;

                //aproximates the partial derivitive
                grad[i] = (f(a) - f(b)) / (2.0 * step);
            }

            return grad;
        }

    }
}
