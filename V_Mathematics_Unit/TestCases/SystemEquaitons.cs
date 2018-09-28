using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Calc_Tests.TestCases
{
    //public delegate Matrix Jacobian(Vector x);

    public class SystemEquaitons
    {
        private VFunc<Vector> system;

        private Vector[] start;
        private Vector[] targ;

        public SystemEquaitons(VFunc<Vector> system, Vector[] targ, Vector[] start)
        {
            this.system = system;

            this.targ = new Vector[targ.Length];
            this.start = new Vector[start.Length];

            for (int i = 0; i < targ.Length; i++)
                this.targ[i] = new Vector(targ[i]);

            for (int i = 0; i < start.Length; i++)
                this.start[i] = new Vector(start[i]);
        }


        /// <summary>
        /// Evaluates the system of equaitons
        /// </summary>
        /// <param name="x">Paramaters to the system of equations</param>
        /// <returns>The result of the system of equations as a vector</returns>
        public Vector Evaluate(Vector x)
        {
            return system(x);
        }

        ///// <summary>
        ///// Computes the gradient of the objective funciton
        ///// </summary>
        ///// <param name="x">Paramaters to the objective funciton</param>
        ///// <returns>The gradient at the given input paramaters</returns>
        //public Vector Gradient(Vector x)
        //{
        //    return grad(x);
        //}

        /// <summary>
        /// Obtains the nth solution
        /// </summary>
        /// <param name="n">One Based Index</param>
        /// <returns>The desired solution</returns>
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
        /// Represents how many solutions are contained in the problem.
        /// </summary>
        public int TotalSolutions
        {
            get { return targ.Length; }
        }

        
    }
}
