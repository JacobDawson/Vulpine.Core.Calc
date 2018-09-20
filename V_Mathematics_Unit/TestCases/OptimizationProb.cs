using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Calc_Tests.TestCases
{
    /// <summary>
    /// This class contains all the information nessary to describe an optimisation problem,
    /// that has already been solved, so that it may be used as a test case.
    /// </summary>
    public class OptimizationProb
    {
        private MFunc obj;
        private VFunc<Vector> grad;

        private Vector[] targ;
        private Vector[] start;

        /// <summary>
        /// Creates a solved optimisation problem for use in testing.
        /// </summary>
        /// <param name="obj">The objective function to solve</param>
        /// <param name="grad">The gradient of the objective funciton</param>
        /// <param name="targ">A list of target minima to be found</param>
        /// <param name="start">A list of starting points to be tested</param>
        public OptimizationProb(MFunc obj, VFunc<Vector> grad, Vector[] targ, Vector[] start)
        {
            this.obj = obj;
            this.grad = grad;

            this.targ = new Vector[targ.Length];
            this.start = new Vector[start.Length];

            for (int i = 0; i < targ.Length; i++)
                this.targ[i] = new Vector(targ[i]);

            for (int i = 0; i < start.Length; i++)
                this.start[i] = new Vector(start[i]);
        }

        /// <summary>
        /// Computes the objective funciton
        /// </summary>
        /// <param name="x">Paramaters to the objective function</param>
        /// <returns>The result of the objective funciton</returns>
        public double Objective(Vector x)
        {
            return obj(x);
        }

        /// <summary>
        /// Computes the gradient of the objective funciton
        /// </summary>
        /// <param name="x">Paramaters to the objective funciton</param>
        /// <returns>The gradient at the given input paramaters</returns>
        public Vector Gradient(Vector x)
        {
            return grad(x);
        }

        /// <summary>
        /// Obtains the nth targed minima
        /// </summary>
        /// <param name="n">One Based Index</param>
        /// <returns>The desired minima</returns>
        public Vector GetTarget(int n)
        {
            return targ[n - 1];
        }

        /// <summary>
        /// Obtains the nth starting point
        /// </summary>
        /// <param name="n">One Based Index</param>
        /// <returns>The desired starting point</returns>
        public Vector GetStart(int n)
        {
            return start[n - 1];
        }

        /// <summary>
        /// Represents how many minima are contained in the problem.
        /// </summary>
        public int TotalMinima
        {
            get { return targ.Length; }
        }

    }
}
