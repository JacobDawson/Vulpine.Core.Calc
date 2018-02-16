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
    /// <summary>
    /// Optimization works by finding the point at wicth a real valued function,
    /// known as the objective funciton, is minimal. This is sometimes refered to 
    /// as the ArgMin of a given funciton, in mathmatical contexts. The minimal
    /// value of the function can be found simply by pluging the resulting point
    /// back into the objective function. Note that in many cases, the objective
    /// funciton may be multi-dimentional, taking a vector as input. Tipicaly the
    /// dirivitive of the objective funciton, or gradient, is required in this 
    /// case. However, an aproximation to the gradient can be given using finite 
    /// diffrences. The maximum function input can likewise be computed by negating 
    /// the objective function. Optimization can be used to solve all kinds of
    /// problems, including the inversion of multi-dimentional funcitons.
    /// </summary>
    public class Optimizer : Algorithm
    {
        //THINGS TO IMPLEMENT:

        //Rank One Quasi-Newton (SR1)
        //either BFGS or DFP
        //[Nonlinear] Conjugate Gradient ***
        //[Inverse] Column Updating Method
        //Quasi-Newton [Inverse] Least Squares ?
        //Broyden's Method ***
        //Simplex (Nelder-Meed) Method



        #region Class Definitions...

        //Factors Used in Backtracking Line Search
        private const double C0 = 0.3;
        private const double C1 = 0.1;

        //Degree of Seperation for Finite Diffrences
        private const double H0 = 1.0e-06;

        //stores the step-size for multivariable optimization
        private double step;

        /// <summary>
        /// Creates a new Optimizer with default stoping criteria.
        /// </summary>
        public Optimizer()
        {
            base.max = 4096;
            base.tol = VMath.TOL;        
            this.step = 1.0;
        }

        /// <summary>
        /// Creates a new Optimizer with the given maximum number of
        /// itterations and the minimal relitive error allowed.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        public Optimizer(int max, double tol)
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);
            this.step = 1.0;
        }

        /// <summary>
        /// Creates a new Optimizer with the given maximum number of
        /// itterations and the minimal relitive error allowed, and 
        /// the maximum step size to be used in gradient based methods.
        /// </summary>
        /// <param name="max">Maximum number of itterations</param>
        /// <param name="tol">Minimial accepted error</param>
        /// <param name="step">Maximum step size during search</param>
        public Optimizer(int max, double tol, double step) 
        {
            base.max = (max > 2) ? max : 2;
            base.tol = Math.Abs(tol);
            this.step = Math.Abs(step);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Ternary Search...

        /// <summary>
        /// Finds the value that minimises a one-dimentinal objective function,
        /// by means of Ternary Search. This is similar to how Bisection works
        /// in root finding, except here the search space is split into thirds
        /// at each itteration, hence the name. Convergence is only garenteed
        /// if the objective funciton is unimodal on the search inverval.
        /// </summary>
        /// <param name="f">Objective Funciton</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>The mimimum argument</returns>
        public Result<Double> MinTernary(VFunc f, double low, double high)
        {
            //makes shure a valid bracket is given
            if (high < low) Swap(ref high, ref low);

            //used in computing the minimum
            double c, m1, m2, f1, f2;

            Initialise();

            while (true)
            {
                //selects two points inside the bracket
                c = (high - low) / 3.0;
                m1 = low + c;
                m2 = high - c;

                //computes the error and determins if we should stop
                double error = Math.Abs(m2) + Math.Abs(m1);
                error = Math.Abs(high - low) / (error + 1.0);
                if (Step(error)) break;

                //evaluates the funciton at those points
                f1 = f(m1);
                f2 = f(m2);

                Increment(1);

                //updates the bounds acordingly
                if (f1 > f2) low = m1;
                else high = m2;
            }

            //takes the midpoint to be our best value
            double best = (low + high) / 2.0;
            return Finish(best);
        }

        /// <summary>
        /// Finds the value that maximises a one-dimentinal objective function,
        /// by means of Ternary Search. This is similar to how Bisection works
        /// in root finding, except here the search space is split into thirds
        /// at each itteration, hence the name. Convergence is only garenteed
        /// if the objective funciton is unimodal on the search inverval.
        /// </summary>
        /// <param name="f">Objective Funciton</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>The maximum argument</returns>
        public Result<Double> MaxTernary(VFunc f, double low, double high)
        {
            //uses the negative function to compute the maximum
            return MinTernary(x => -f(x), low, high);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Golden Section Search...

        /// <summary>
        /// Finds the value that minimises a one-dimentinal objective function,
        /// by means of Golden Section Search. This is similar to Ternary Search, 
        /// but uses an improved subdividing ruteen to allow the reuse of previously 
        /// computed values, thus saving on function evaluations. It is named after 
        /// the Golden Ratio, which is used in computing the subdivisions.
        /// </summary>
        /// <param name="f">Objective Funciton</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>The mimimum argument</returns>
        public Result<Double> MinGolden(VFunc f, double low, double high)
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

            while (true)
            {
                //computes the error and determins if we should stop
                double error = Math.Abs(m2) + Math.Abs(m1);
                error = Math.Abs(b - a) / (error + 1.0);
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
            }

            //takes the midpoint to be our best value
            double best = (a + b) / 2.0;
            return Finish(best);
        }

        /// <summary>
        /// Finds the value that maximises a one-dimentinal objective function,
        /// by means of Golden Section Search. This is similar to Ternary Search, 
        /// but uses an improved subdividing ruteen to allow the reuse of previously 
        /// computed values, thus saving on function evaluations. It is named after 
        /// the Golden Ratio, which is used in computing the subdivisions.
        /// </summary>
        /// <param name="f">Objective Funciton</param>
        /// <param name="low">Lower end of the search bracket</param>
        /// <param name="high">Upper end of the search bracket</param>
        /// <returns>The maximum argument</returns>
        public Result<Double> MaxGolden(VFunc f, double low, double high)
        {
            //uses the negative function to compute the maximum
            return MinGolden(x => -f(x), low, high);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Gradient Descent Methods...

        /// <summary>
        /// Uses Gradient Dessent, with Backtracking Line Search, to find the 
        /// Minimum of the given objective function. It simply uses the inverse
        /// gradient as a dessent direction, and tries multiple step sizes till
        /// it finds one that is maximal, and satisfies the armijo conditions
        /// for convergence. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientBt(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return GradientBt(f, g, x0);
        }

        /// <summary>
        /// Uses Gradient Dessent, with Backtracking Line Search, to find the 
        /// Minimum of the given objective function. It simply uses the inverse
        /// gradient as a dessent direction, and tries multiple step sizes till
        /// it finds one that is maximal, and satisfies the armijo conditions
        /// for convergence. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientBt(MFunc f, VFunc<Vector> g, Vector x0)
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

                //computes the gradient and it's dot product
                Vector grad = g(xn);
                double dot = grad * grad;

                //uses the armijo conditions to check alpha
                d2 = f_xn - (C1 * an * dot);
                d1 = f(xn - (an * grad));

                while (d1 > d2)
                {
                    //computes the new step-size
                    an = C0 * an;

                    //updates the armijo conditions
                    d2 = f_xn - (C1 * an * dot);
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

        /// <summary>
        /// Uses Gradient Decent, with Exact Line Search, to find the Mimimum
        /// of the given objective function. It uses the inverse gradient as
        /// a desent direction, and then tries to find the optimal step-size
        /// by means of a one-dimentional optimization function. This results
        /// in more effort per step, but fewer steps overall.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientEx(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return GradientEx(f, g, x0);
        }

        /// <summary>
        /// Uses Gradient Decent, with Exact Line Search, to find the Mimimum
        /// of the given objective function. It uses the inverse gradient as
        /// a desent direction, and then tries to find the optimal step-size
        /// by means of a one-dimentional optimization function. This results
        /// in more effort per step, but fewer steps overall.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientEx(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            //we create a second optimizer and initilise our own
            RootFinder rf = new RootFinder(MaxSteps, Tolerance);
            this.Initialise();

            while (true)
            {
                //aproximates the gradient using the previous step-size
                Vector grad = g(xn);

                ////determins the optimal step-size using golden search
                //VFunc af = a => f(xn - (a * grad));
                //var an = op.GoldenMin(af, 0.0, step);

                //initilises the values used to find the optimal step-size
                VFunc af = a => -grad * g(xn - (a * grad));
                double f1 = af(0.0);
                double f2 = af(step);
                double an = step;

                //finds the optimal step-size with root-finding methods
                if (f1 * f2 < 0)
                {
                    var res = rf.Brent(af, 0.0, step);
                    Increment(res.Count);

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


        #endregion /////////////////////////////////////////////////////////////////////////

        #region Rank One Quasi-Newton Methods...


        //NOTE: Try using Wolfe conditions on BackTrack

        public Result<Vector> RankOneBt(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return RankOneBt(f, g, x0);
        }

        public Result<Vector> RankOneBt(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //stores our aproximate inverse hessian
            Matrix bk = Matrix.Ident(x0.Length);

            //Matrix bk = Matrix.Invert(Hessian(g, x0, 1.0e-06));

            //vectors used during our computaiton
            Vector x1, x2, g1, g2, rn, cn;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            g1 = g(x1);


            while (true)
            {
                //computes our dessent direction
                Vector dess = bk.Mult(g1);

                double f_x1 = f(x1);
                double dot = dess * dess;  //dess * g1; ??
                double an = step;

                //uses the armijo conditions to check alpha
                double d2 = f_x1 - (C1 * an * dot);
                double d1 = f(x1 - (an * dess));

                while (d1 > d2)
                {
                    //computes the new step-size
                    an = C0 * an;

                    //updates the armijo conditions
                    d2 = f_x1 - (C1 * an * dot);
                    d1 = f(x1 - (an * dess));

                    Increment(1);
                }

                //travels the path of steepest decent
                x2 = x1 - (an * dess);
                if (Step(x1, x2)) break;

                //computes the intermediat values
                g2 = g(x2);
                rn = g2 - g1;
                cn = (x2 - x1) - (bk * rn);

                double t1 = Math.Abs(cn * rn);
                double t2 = rn.Norm() * cn.Norm();

                if (t1 > VMath.TOL * t2)
                {
                    //updates our aproximate inverse hessian
                    bk += cn.Outer(cn) / (cn * rn);

                    //Console.WriteLine(cn * rn);
                }

                //copies the curent vector and gradient
                x1 = new Vector(x2);
                g1 = new Vector(g2);
            }


            //returns our local minima point
            return Finish(x2);
        }


        //This uses Wolfe Conditons:
        public Result<Vector> RankOneBt2(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //stores our aproximate inverse hessian
            Matrix bk = Matrix.Ident(x0.Length);

            //vectors used during our computaiton
            Vector x1, x2, g1, g2, rn, cn;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            g1 = g(x1);


            while (true)
            {
                //computes our dessent direction
                Vector dess = bk.Mult(g1);

                double an = 1.0;  //step;
                double C2 = 0.8;

                double f_x1 = f(x1);
                double dot = dess * g1;  //dess * g1; ??

                ////uses the armijo conditions to check alpha
                //double d2 = f_x1 - (C1 * an * dot);
                //double d1 = f(x1 - (an * dess));

                ////uses the wolfe conditions to also check alpha
                //double w2 = dess * g(x1 - an * dess);
                //double w1 = C2 * dess * g1;


                bool found = false;
                double b1 = 0;
                double b2 = -1;


                while (!found)
                {
                    //uses the armijo conditions to check alpha
                    double d2 = f_x1 - (C1 * an * dot);
                    double d1 = f(x1 - (an * dess));

                    //uses the wolfe conditions to also check alpha
                    double w2 = dess * g(x1 - an * dess);
                    double w1 = -C2 * dess * g1;

                    found = Increment(1);

                    if (d1 > d2)
                    {
                        b2 = an;
                        an = (b1 + b2) * 0.5;
                    }
                    //else if (w1 > w2)   //w2 < w1
                    //{
                    //    b1 = an;
                    //    an = (b2 < 0) ? b1 * 2.0 : (b1 + b2) * 0.5;

                    //    //Console.WriteLine(w2);
                    //}
                    else
                    {
                        found = true;
                    }
                }


                //travels the path of steepest decent
                x2 = x1 - (an * dess);
                if (Step(x1, x2)) break;

                //computes the intermediat values
                g2 = g(x2);
                rn = g2 - g1;
                cn = (x2 - x1) - (bk * rn);

                double t1 = Math.Abs(cn * rn);
                double t2 = rn.Norm() * cn.Norm();

                if (t1 > VMath.TOL * t2)
                {
                    //updates our aproximate inverse hessian
                    bk += cn.Outer(cn) / (cn * rn);

                    //Console.WriteLine(cn * rn);
                }

                //copies the curent vector and gradient
                x1 = new Vector(x2);
                g1 = new Vector(g2);
            }


            //returns our local minima point
            return Finish(x2);
        }





        public Result<Vector> RankOneEx(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return RankOneEx(f, g, x0);
        }


        public Result<Vector> RankOneEx(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //we create a second optimizer and initilise our own
            RootFinder rf = new RootFinder(MaxSteps, Tolerance);
            this.Initialise();

            //stores our aproximate inverse hessian
            Matrix bk = Matrix.Ident(x0.Length);

            //Matrix bk = Matrix.Invert(Hessian(g, x0, 1.0e-06));

            //vectors used during our computaiton
            Vector x1, x2, g1, g2, rn, cn;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            g1 = g(x1);


            while (true)
            {
                //computes our dessent direction
                Vector dess = bk.Mult(g1);

                //initilises the values used to find the optimal step-size
                VFunc af = a => -dess * g(x1 - (a * dess)); //???
                double f1 = af(0.0);
                double f2 = af(step);
                double an = step;

                //finds the optimal step-size with root-finding methods
                if (f1 * f2 < 0)
                {
                    var res = rf.Brent(af, 0.0, step);
                    Increment(res.Count);

                    an = res.Value;
                }

                //travels the path of steepest decent
                x2 = x1 - (an * dess);
                if (Step(x1, x2)) break;

                //computes the intermediat values
                g2 = g(x2);
                rn = g2 - g1;
                cn = (x2 - x1) - (bk * rn);

                double t1 = Math.Abs(cn * rn);
                double t2 = rn.Norm() * cn.Norm();

                if (t1 > VMath.TOL * t2)
                {
                    //updates our aproximate inverse hessian
                    bk += cn.Outer(cn) / (cn * rn);

                    //Console.WriteLine(cn * rn);
                }

                //copies the curent vector and gradient
                x1 = new Vector(x2);
                g1 = new Vector(g2);
            }


            //returns our local minima point
            return Finish(x2);
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
        /// Helper method used to aproximate the gradient of a given function
        /// at a given point. It uses fininte diffrences to comptue the partial
        /// derivitives that comprise the gradient. Note that sutch aproximations
        /// can be numericaly unstable, due to cancelation errors.
        /// </summary>
        /// <param name="f">Function to be evaluated</param>
        /// <param name="x">Point of intrest</param>
        /// <param name="h">Degree of seperation</param>
        /// <returns>The gradient of the funciton at the point of intrest</returns>
        private static Vector Grad(MFunc f, Vector x, double h)
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


        private static Matrix Hessian(VFunc<Vector> g, Vector x, double h)
        {
            int len = x.Length;
            Matrix hess = new Matrix(len, len);

            Vector dx;

            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + h;
                b[i] = b[i] - h;

                dx = (g(a) - g(b)) / (2.0 * h);

                hess.SetRow(i, dx);
            }

            return hess;
        }


        private static Matrix Hessian(MFunc f, Vector x, double h)
        {
            int len = x.Length;
            Matrix hess = new Matrix(len, len);

            Vector dx;
            //double ddx = 0.0;

            //VFunc<Vector> g = gx => Grad(f, x, h);

            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + h;
                b[i] = b[i] - h;

                //dx = (g(a) - g(b)) / (2.0 * h);


                //creates a vector to store the gradient
                Vector grad = new Vector(x.Length);

                for (int j = 0; j < x.Length; j++)
                {
                    //creates copies of the input vectur
                    Vector c = new Vector(a);
                    Vector d = new Vector(a);

                    //permutes the input along a given axis
                    c[j] = c[j] + h;
                    d[j] = d[j] - h;

                    //aproximates the partial derivitive
                    grad[j] = (f(c) - f(d)) / (2.0 * h);
                }

                dx = grad;

                for (int j = 0; j < x.Length; j++)
                {
                    //creates copies of the input vectur
                    Vector c = new Vector(b);
                    Vector d = new Vector(b);

                    //permutes the input along a given axis
                    c[j] = c[j] + h;
                    d[j] = d[j] - h;

                    //aproximates the partial derivitive
                    grad[j] = (f(c) - f(d)) / (2.0 * h);
                }

                dx = (dx - grad) / (2.0 * h);


                hess.SetRow(i, dx);
            }


            throw new NotImplementedException();
        }


        private static Matrix Hessian2(MFunc f, Vector x, double h)
        {
            int len = x.Length;
            Matrix hess = new Matrix(len, len);

            double ddx = 0.0;



            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + h;
                b[i] = b[i] - h;

                for (int j = i; j < len; j++)
                {
                    Vector a1 = new Vector(a);
                    Vector a2 = new Vector(a);

                    Vector b1 = new Vector(b);
                    Vector b2 = new Vector(b);

                    a1[j] = a1[j] + h;
                    a2[j] = a2[j] - h;

                    b1[j] = b1[j] + h;
                    b2[j] = b2[j] - h;

                    double dx1 = (f(a1) - f(b1)) / (2.0 * h);
                    double dx2 = (f(a2) - f(b2)) / (2.0 * h);

                    ddx = (dx1 - dx2) / (2.0 * h);

                    hess[i, j] = ddx;
                    hess[j, i] = ddx;
                }
            }

            return hess;
        }


        private static Matrix Hessian3(MFunc f, Vector x, double h)
        {
            int len = x.Length;
            Matrix hess = new Matrix(len, len);

            double ddx = 0.0;



            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);
                Vector b = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + h;
                b[i] = b[i] - h;

                double f_a = f(a);
                double f_b = f(b);

                for (int j = i; j < len; j++)
                {                   
                    Vector c = new Vector(x);
                    Vector d = new Vector(x);

                    c[j] = c[j] + h;
                    d[j] = d[j] - h;

                    double dx1 = (f(c) - f_a) / (h * VMath.R2);
                    double dx2 = (f_b - f(d)) / (h * VMath.R2);

                    ddx = (dx1 - dx2) / (h * VMath.R2);

                    hess[i, j] = ddx;
                    hess[j, i] = ddx;
                }
            }

            return hess;
        }



        private static Matrix Hessian4(MFunc f, Vector x, double h)
        {
            int len = x.Length;
            Matrix hess = new Matrix(len, len);

            double f_x = f(x);
            double ddx = 0.0;



            for (int i = 0; i < len; i++)
            {
                //creates copies of the input vectur
                Vector a = new Vector(x);

                //permutes the input along a given axis
                a[i] = a[i] + h;

                double f_a = f(a);

                for (int j = i; j < len; j++)
                {
                    

                    //Vector b1 = new Vector(b);
                    //Vector b2 = new Vector(b);

                    //a1[j] = a1[j] + h;
                    //a2[j] = a2[j] - h;

                    //b1[j] = b1[j] + h;
                    //b2[j] = b2[j] - h;

                    //double dx1 = (f(a1) - f(b1)) / (2.0 * h);
                    //double dx2 = (f(a2) - f(b2)) / (2.0 * h);


                    Vector b = new Vector(x);
                    Vector c = new Vector(a);


                    b[j] = b[j] + h;
                    c[j] = c[j] + h;

                    double dx1 = (f_x - f_a) / h;
                    double dx2 = (f_x - f(b)) / h;


                    ddx = (dx1 - dx2) / h;

                    hess[i, j] = ddx;
                    hess[j, i] = ddx;
                }
            }

            return hess;
        }
        


        private double ExactSearch(RootFinder rf, VFunc<Vector> g, Vector gn, Vector xn)
        {
            //initilises the values used to find the optimal step-size
            VFunc af = a => -gn * g(xn - (a * gn));
            double f1 = af(0.0);
            double f2 = af(step);
            double an = step;

            //finds the optimal step-size with root-finding methods
            if (f1 * f2 < 0)
            {
                var res = rf.Brent(af, 0.0, step);
                Increment(res.Count);

                an = res.Value;
            }

            return an;
        }
    }
}
