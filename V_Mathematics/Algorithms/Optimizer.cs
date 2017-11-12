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

            //tracks the values as they change
            double last = low;
            double curr = high;

            while (true)
            {
                //checks the midpoint for termination
                curr = (low + high) / 2.0;
                if (Step(last, curr)) break;

                //selects two points inside the bracket
                c = (high - low) / 3.0;
                m1 = low + c;
                m2 = high - c;

                //evaluates the funciton at those points
                f1 = f(m1);
                f2 = f(m2);

                Increment(1);

                //updates the bounds acordingly
                if (f1 > f2) low = m1;
                else high = m2;

                last = curr;
            }

            //returns the minimum argument found
            return Finish(curr);
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

            //tracks the values as they change
            double last = a;
            double curr = b;

            while (true)
            {
                //checks the midpoint for termination
                curr = (a + b) / 2.0;
                if (Step(last, curr)) break;

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
            }

            //returns the minimum argument found
            return Finish(curr);

        }


        //NOTE: alternate error forumla ??
        //error = |b - a| / (|m2| + |m1|)
        //error = 2 * |b - a| / |m2 + m1|



        #endregion /////////////////////////////////////////////////////////////////////////


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
