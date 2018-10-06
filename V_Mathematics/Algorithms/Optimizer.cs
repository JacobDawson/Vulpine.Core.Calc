/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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

        //[Inverse] Column Updating Method
        //Quasi-Newton [Inverse] Least Squares ?
        //Simplex (Nelder-Meed) Method
        //Limited Memory BFGS Method


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
        public Result<Double> TernaryMin(VFunc f, double low, double high)
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
        public Result<Double> TernaryMax(VFunc f, double low, double high)
        {
            //uses the negative function to compute the maximum
            return TernaryMin(x => -f(x), low, high);
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
        public Result<Double> GoldenMax(VFunc f, double low, double high)
        {
            //uses the negative function to compute the maximum
            return GoldenMin(x => -f(x), low, high);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Gradient Descent Methods...

        /// <summary>
        /// Uses Gradient Dessent, with Backtracking Line Search, to find the 
        /// Minimum of the given objective function. It simply uses the inverse
        /// gradient as a dessent direction, and tries multiple step sizes till
        /// it finds one that is maximal, and satisfies the Armijo conditions
        /// for convergence. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientMin(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return GradientMin(f, g, x0);
        }

        /// <summary>
        /// Uses Gradient Dessent, with Backtracking Line Search, to find the 
        /// Minimum of the given objective function. It simply uses the inverse
        /// gradient as a dessent direction, and tries multiple step sizes till
        /// it finds one that is maximal, and satisfies the Armijo conditions
        /// for convergence. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> GradientMin(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //makes a copy of our starting point
            Vector xn = new Vector(x0);
            Vector curr = xn;

            Initialise();

            while (true)
            {
                //computes the gradient and uses backtracking
                Vector grad = g(xn);
                double an = Backtrack(f, grad, xn);

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

        #region Rank One Quasi-Newton Methods...

        /// <summary>
        /// Finds the minimum of the objective function, by using symetric rank-one
        /// updates to apriximate the Hessian. This allows the method to converge
        /// much faster than Gradient Dessent, because it chooses a "smart" search
        /// direction. However, because the new search direction is not garenteed to
        /// be a proper dessent direciton, the Hessian must be reset whenever this
        /// condition fails to hold.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> RankOneMin(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return RankOneMin(f, g, x0);
        }

        /// <summary>
        /// Finds the minimum of the objective function, by using symetric rank-one
        /// updates to apriximate the Hessian. This allows the method to converge
        /// much faster than Gradient Dessent, because it chooses a "smart" search
        /// direction. However, because the new search direction is not garenteed to
        /// be a proper dessent direciton, the Hessian must be reset whenever this
        /// condition fails to hold.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> RankOneMin(MFunc f, VFunc<Vector> g, Vector x0)
        {
            Initialise();

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

                //tests for positive-definitness
                if (dess * g1 < 0)
                {
                    dess = g1;
                    bk = Matrix.Ident(x0.Length);
                }

                //uses backtracking to refine our search
                double an = Backtrack(f, dess, x1);

                //travels the path of steepest decent
                x2 = x1 - (an * dess);
                if (Step(x1, x2)) break;

                //computes the intermediat values
                g2 = g(x2);
                rn = g2 - g1;
                cn = (x2 - x1) - (bk * rn);

                double t1 = Math.Abs(cn * rn);
                double t2 = rn.Norm() * cn.Norm();

                //updates our aproximate inverse hessian
                if (t1 > VMath.TOL * t2)
                    bk += cn.Outer(cn) / (cn * rn);

                //copies the curent vector and gradient
                x1 = new Vector(x2);
                g1 = new Vector(g2);
            }

            //returns our local minima point
            return Finish(x2);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region BFGS Quasi-Newton Methods...

        /// <summary>
        /// Finds the minimum of the objective function using the BFGS method. In
        /// this case, the Hessian matrix is updated in such a way that positive
        /// definitness is garenteed. Thus the search direction is always a dessent
        /// direciton, so it never needs to reset, unlike the symetric rank-one
        /// update mehtod. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> BFGS_Min(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return BFGS_Min(f, g, x0);
        }

        /// <summary>
        /// Finds the minimum of the objective function using the BFGS method. In
        /// this case, the Hessian matrix is updated in such a way that positive
        /// definitness is garenteed. Thus the search direction is always a dessent
        /// direciton, so it never needs to reset, unlike the symetric rank-one
        /// update mehtod. 
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> BFGS_Min(MFunc f, VFunc<Vector> g, Vector x0)
        {
            Initialise();

            //stores our aproximate inverse hessian
            Matrix bk = Matrix.Ident(x0.Length);

            //vectors used during our computaiton
            Vector x1, x2, g1, g2, rn, sn, btr;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            g1 = g(x1);

            while (true)
            {
                //computes our dessent direction
                Vector dess = bk.Mult(g1);

                //uses backtracking to refine our search
                double an = Backtrack(f, dess, x1);

                //travels the path of steepest decent
                x2 = x1 - (an * dess);
                if (Step(x1, x2)) break;

                //computes the intermediate vectors
                g2 = g(x2);
                rn = g2 - g1;
                sn = x2 - x1;
                btr = bk * rn;

                double dot = sn * rn;

                //updates our aproximate inverse hessian
                if (dot > VMath.TOL)
                {
                    Matrix u = sn.Outer(sn) * (dot + rn * btr) / (dot * dot);
                    Matrix v = (btr.Outer(sn) + sn.Outer(btr)) / dot;

                    bk += (u - v);
                }

                //copies the curent vector and gradient
                x1 = new Vector(x2);
                g1 = new Vector(g2);
            }

            //returns our local minima point
            return Finish(x2);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Nonlinear Conjugate Gradient Methods...

        /// <summary>
        /// Uses the Nonlinear Conjugate Gradient Method with Exact Line Search
        /// to minimise the objective funciton. Basicly it 'remembers' the past
        /// derecitons that were searched, inorder to choose a direction that
        /// is conjugate to the preivous directions. This helps to improve 
        /// search time over Gradient Desent, without the need to compute an
        /// expensive Hessian matrix.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> ConjugateMin(MFunc f, Vector x0)
        {
            VFunc<Vector> g = x => Grad(f, x, H0);
            return ConjugateMin(f, g, x0);
        }

        /// <summary>
        /// Uses the Nonlinear Conjugate Gradient Method with Exact Line Search
        /// to minimise the objective funciton. Basicly it 'remembers' the past
        /// derecitons that were searched, inorder to choose a direction that
        /// is conjugate to the preivous directions. This helps to improve 
        /// search time over Gradient Desent, without the need to compute an
        /// expensive Hessian matrix.
        /// </summary>
        /// <param name="f">Objective Function</param>
        /// <param name="g">Gradient of the Objective Function</param>
        /// <param name="x0">Starting Point</param>
        /// <returns>The point of minimum value</returns>
        public Result<Vector> ConjugateMin(MFunc f, VFunc<Vector> g, Vector x0)
        {
            //we create a second optimizer and initilise our own
            RootFinder rf = new RootFinder(MaxSteps, Tolerance);
            this.Initialise();

            //vectors used during our computaiton
            Vector x1, x2, d1, d2, s1, s2;

            //initialises the starting paramaters
            x1 = new Vector(x0);
            d1 = g(x0);
            s1 = new Vector(d1);

            //initialises the search direction paramaters
            double bpr = 0.0;
            double an = 1.0;

            while (true)
            {
                //chooses a search direction and step-size
                s2 = d1 + (bpr * s1);
                an = ExactSearch(rf, g, s2, x1);

                //updates our best-guess and checks for termination
                x2 = x1 - an * s2;
                if (Step(x1, x2)) break;

                d2 = g(x2);

                //updates the Polak Ribiere paramater
                bpr = (d2 * (d2 - d1)) / (d1 * d1);
                bpr = Math.Max(0.0, bpr);

                //copies the vectors to the next itteraiton
                x1 = new Vector(x2);
                d1 = new Vector(d2);
                s1 = new Vector(s2);
            }

            return Finish(x2);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

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

        /// <summary>
        /// Preforms an Exact Line Search in order to determin the ideal step size
        /// to take along a desent direciton. It is able to compute the step size
        /// exactly by solving a one dimentional equation.
        /// </summary>
        /// <param name="rf">Root Finder to use in solving the equation</param>
        /// <param name="g">Gradient of the equation</param>
        /// <param name="gn">Direction along which to travel</param>
        /// <param name="xn">Start point of the search</param>
        /// <returns>The ideal step size</returns>
        private double ExactSearch(RootFinder rf, VFunc<Vector> g, Vector gn, Vector xn)
        {
            //initilises the values used to find the optimal step-size
            VFunc af = a => -gn * g(xn - (a * gn));
            double f1 = af(0.0);
            double f2 = af(step);
            double an = step;

            Increment(1);

            //finds the optimal step-size with root-finding methods
            if (f1 * f2 < 0)
            {
                var res = rf.Brent(af, 0.0, step);
                Increment(res.Count);

                an = res.Value;
            }

            return an;
        }

        /// <summary>
        /// Uses Backtrackign Line Search to produce a step size that corisponds 
        /// to a large enough decress in the objective function in order to
        /// garentee convergence. It dose so by starting with the maximum step
        /// size and decreasing it till meets convergence condtions. It is not
        /// as precice as Exact Line Search, but is more effecient in most cases.
        /// </summary>
        /// <param name="f">Objective function</param>
        /// <param name="dess">Desent direciton</param>
        /// <param name="x1">Starting postion</param>
        /// <returns>An adaquate step size</returns>
        private double Backtrack(MFunc f, Vector dess, Vector x1)
        {
            //intiialises the armijo paramaters
            double an = step;
            double f_x1 = f(x1);
            double dot = dess * dess;

            while (true)
            {
                //uses the armijo conditions to check alpha
                double d2 = f_x1 - (C1 * an * dot);
                double d1 = f(x1 - (an * dess));

                //checks for termination
                if (Increment(1)) break;
                if (d1 <= d2) break;
                
                //computes the new step-size
                an = C0 * an;
            }

            //returns the stepsize
            return an;
        }

        #endregion /////////////////////////////////////////////////////////////////////////

    }
}
