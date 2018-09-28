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
        #region Class Definitions...

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
        private OptimizationProb(MFunc obj, VFunc<Vector> grad, Vector[] targ, Vector[] start)
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

        #endregion ///////////////////////////////////////////////////////////////////

        #region Test Cases...

        /// <summary>
        /// Obtains one of the provided Optimization Problems for use in 
        /// testing optimisation algorythims.
        /// </summary>
        /// <param name="index">Index of the desired problem</param>
        /// <returns>A solved optimisaiton problem for testing</returns>
        public static OptimizationProb GetCase(int index)
        {
            switch (index)
            {
                /**
                 *  Circular Contures around [7, 2]
                 * 
                 *  f(x, y) = (x - 7)^2 + (y - 2)^2
                 *  
                 *  d/dx = 2 (x - 7)
                 *  d/dy = 2 (y - 2)
                 *  
                 *  [5, 3] -> [7, 2]
                 *  [9, 4] -> [7, 2]
                 */
                case 01: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[0] - 7.0;
                        double b = x[1] - 2.0;
                        return (a * a) + (b * b);
                    },
                    delegate(Vector x)
                    {
                        double a = 2 * (x[0] - 7.0);
                        double b = 2 * (x[1] - 2.0);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(7.0, 2.0),
                    },
                    new Vector[]
                    {
                        new Vector(5.0, 3.0),
                        new Vector(9.0, 4.0),
                    });

                /**
                 *  Eleptical Contours around [3, 4]
                 * 
                 *  f(x, y) = y (-2 x + y - 2) + x (4 x - 16) + 28
                 *  
                 *  d/dx = 8 x - 2 y - 16
                 *  d/dy = -2 x + 2 y - 2
                 *  
                 *  [2, 2] -> [3, 4]
                 *  [4, 4] -> [3, 4]
                 */
                case 02: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[1] * (-2.0 * x[0] + x[1] - 2.0);
                        double b = x[0] * (4.0 * x[0] - 16.0);
                        return a + b + 28.0;
                    },
                    delegate(Vector x)
                    {
                        double a = 8.0 * x[0] - 2.0 * x[1] - 16.0;
                        double b = 2.0 * x[1] - 2.0 * x[0] - 2.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(3.0, 4.0),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 2.0),
                        new Vector(4.0, 4.0),
                    });

                /**
                 *  More Eliptical Contours
                 *  
                 *  f(x, y) = x^2 - 3 x y + 4 y^2 + x - y
                 *  
                 *  d/dx =  2 x - 3 y + 1
                 *  d/dy = -3 x + 8 y - 1
                 *  
                 *  [2,  2] -> [-5/7, -1/7]
                 *  [5, -1] -> [-5/7, -1/7]
                 */
                case 03: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = x[0] * x[0];
                        temp = temp - 3.0 * x[0] * x[1];
                        temp = temp + 4.0 * x[1] * x[1];
                        return temp + x[0] - x[1];
                    },
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[0] - 3.0 * x[1] + 1.0;
                        double b = 8.0 * x[1] - 3.0 * x[0] - 1.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-5.0/7.0, -1.0/7.0),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 2.0),
                        new Vector(5.0, -1.0),
                    });

                /**
                 *  A Gentler Version of Rosenbroch
                 *  
                 *  f(x, y) = 25 (y - x^2)^2 + (1 - x)^2
                 *  
                 *  d/dx = x (100 x^2 - 100 y + 2) - 2
                 *  d/dy = 50 (y - x^2)
                 *  
                 *  [-1, 1] -> [1, 1]
                 *  [ 0, 0] -> [1, 1]
                 */
                case 04: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[1] - x[0] * x[0];
                        double b = 1.0 - x[0];
                        return 25.0 * a * a + b * b;
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double a = x[0] * (100.0 * xx - 100.0 * x[1] + 2.0) - 2.0;
                        double b = 50.0 * (x[1] - xx);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 1.0),
                    },
                    new Vector[]
                    {
                        new Vector(-1.0, 1.0),
                        new Vector(0.0, 0.0),
                    });

                /**
                 *  Has one Maximum and one Minimum
                 *  
                 *  f(x, y) = x * e^(-x^2 - y^2)
                 *  
                 *  d/dx = (1 - 2 x^2) e^(-x^2 - y^2)
                 *  d/dy = -2 x y e^(-x^2 - y^2)
                 *  
                 *  [-1.2, 0.4]  -> [-1 / sqrt(2), 0]
                 *  [ 0.4, 0.2]  -> [-1 / sqrt(2), 0]
                 */
                case 05: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = -(x[0] * x[0]) - (x[1] * x[1]);
                        return x[0] * Math.Exp(temp);
                    },
                    delegate(Vector x)
                    {
                        double temp = Math.Exp(-(x[0] * x[0]) - (x[1] * x[1]));
                        double a = (1.0 - 2.0 * x[0] * x[0]) * temp;
                        double b = -2.0 * x[0] * x[1] * temp;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.707106781186548, 0.0),
                    },
                    new Vector[]
                    {
                        new Vector(-1.2, 0.4),
                        new Vector(0.4, 0.2),
                    });

                /**
                 *  Triangle Shaped Valley
                 *  
                 *  f(x, y) = e^(x + y - 1) + e^(x - y - 1) + e^(-x - 1)
                 *  
                 *  d/dx = e^(x + y - 1) + e^(x - y - 1) - e^(-x - 1)
                 *  d/dy = e^(x + y - 1) - e^(x - y - 1)
                 *  
                 *  [ 1, 2] -> [-log(2) / 2, 0]
                 *  [-2, 3] -> [-log(2) / 2, 0]
                 */
                case 06: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = x[0] + x[1] - 1.0;
                        double b = x[0] - x[1] - 1.0;
                        double c = -x[0] - 1.0;
                        return Math.Exp(a) + Math.Exp(b) + Math.Exp(c);
                    },
                    delegate(Vector x)
                    {
                        double a = Math.Exp(x[0] + x[1] - 1.0);
                        double b = Math.Exp(x[0] - x[1] - 1.0);
                        double c = Math.Exp(-x[0] - 1.0);

                        double g0 = a + b - c;
                        double g1 = a - b;
                        return new Vector(g0, g1);
                    },
                    new Vector[]
                    {
                        new Vector(-0.346573590279973, 0.0),
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 2.0),
                        new Vector(-2.0, 3.0),
                    });

                /**
                 *  Bannana Shaped Curve With Two Minima
                 *  
                 *  f(x, y) = 2 (2 y^2 - x)^2 + (x - 1)^2
                 *  
                 *  d/dx = 6 x - 8 y^2 - 2
                 *  d/dy = y (32 y^2 - 16 x)
                 *  
                 *  [-1, 1] ->  [1,  1 / sqrt(2)]
                 *  [2, -1] ->  [1, -1 / sqrt(2)]
                 */
                case 07: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[1] * x[1] - x[0];
                        double b = x[0] - 1.0;
                        return 2.0 * a * a + b * b;
                    },
                    delegate(Vector x)
                    {
                        double yy = x[1] * x[1];
                        double a = 6.0 * x[0] - 8.0 * yy - 2.0;
                        double b = x[1] * (32.0 * yy - 16.0 * x[0]);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 0.707106781186548),
                        new Vector(1.0, -0.707106781186548),
                    },
                    new Vector[]
                    {
                        new Vector(-1.0, 1.0),
                        new Vector(2.0, -1.0),
                    });

                /**
                 *  Polynomial With Many Local Minima
                 *  
                 *  f(x, y) = ((x^2/3 - 21/10) x^2 + 4) x^2 + y (x + y (4 y^2 - 4))
                 *  
                 *  d/dx = x ((2 x^2 - 42/5) x^2 + 8) + y
                 *  d/dy = x + y (16 y^2 - 8)
                 *  
                 *  x* = +/- 0.08984201310031806242249056062
                 *  y* = +/- 0.7126564030207396333972658142
                 *  
                 *  [0.8,  0.8] -> [x-, y+]
                 *  [0.8, -0.4] -> [x+, y-]
                 */
                case 08: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];
                        double temp = ((xx / 3.0 - 21.0 / 10.0) * xx + 4.0) * xx;
                        return temp + x[1] * (x[0] + x[1] * (4.0 * yy - 4.0));
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double a = x[0] * ((2.0 * xx - 42.0 / 5.0) * xx + 8.0) + x[1];
                        double b = x[0] + x[1] * (16 * x[1] * x[1] - 8.0);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-1.70360671496998, 0.796083568672625),
                        new Vector(-1.60710475292020, -0.568651454884131),
                        new Vector(-0.0898420131003181, 0.712656403020740),
                        new Vector(0.0898420131003181, -0.712656403020740),
                        new Vector(1.60710475292020, 0.568651454884131),
                        new Vector(1.70360671496998, -0.796083568672625),
                    },
                    new Vector[]
                    {
                        new Vector(0.8, 0.8),
                        new Vector(0.6, -0.4),
                    });

                /**
                 *   Slope With A Hole In It
                 *   
                 *   f(x, y) = x^2 + y^2 - ln(x^2 + y^2) - y
                 *   
                 *   d/dx = x (2 - 2/(x^2 + y^2))
                 *   d/dy = y (2 - 2/(x^2 + y^2)) - 1
                 *   
                 *   [1.0,  0.0] -> [0, 1/4 + sqrt(17)/4]
                 *   [0.0, -1.5] -> [0, 1/4 + sqrt(17)/4]
                 */
                case 09: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = x[0] * x[0] + x[1] * x[1];
                        return temp - Math.Log(temp) - x[1];
                    },
                    delegate(Vector x)
                    {
                        double temp = 2.0 - 2.0 / (x[0] * x[0] + x[1] * x[1]);
                        double a = x[0] * temp;
                        double b = x[1] * temp - 1.0;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(0.0, 1.28077640640442),
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 0.0),
                        new Vector(0.0, -1.5),
                    });

                /**
                 *  Two Humps and Two Valleys
                 *  
                 *  f(x, y) = x * y * e^(-x^2 - y^2)
                 *  
                 *  d/dx = y (2 x^2 - 1) (-e^(-x^2 - y^2))
                 *  d/dy = x (2 y^2 - 1) (-e^(-x^2 - y^2))
                 *  
                 *  [-1.5, 0.5] -> [-1/sqrt(2), 1/sqrt(2)]
                 *  [0.1, -0.1] -> [1/sqrt(2), -1/sqrt(2)]
                 */
                case 10: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = -(x[0] * x[0]) - (x[1] * x[1]);
                        return x[0] * x[1] * Math.Exp(temp);
                    },
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];
                        double temp = -Math.Exp(-xx - yy);

                        double a = x[1] * (2.0 * xx - 1.0) * temp;
                        double b = x[0] * (2.0 * yy - 1.0) * temp;
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.707106781186548, 0.707106781186548),
                        new Vector(0.707106781186548, -0.707106781186548),
                    },
                    new Vector[]
                    {
                        new Vector(-1.5, 0.5),
                        new Vector(0.1, -0.1),
                    });

                /**
                 *  A Wavy Valley
                 *  
                 *  f(x, y) = sin(x) + sin(2 y) + x^2 + y^2
                 *  
                 *  d/dx = 2 x + cos(x)
                 *  d/dy = 2 y + 2 cos(2 y)
                 *  
                 *  x* = -0.4501836112948735730365386968
                 *  y* = -0.5149332646611294138010592584
                 *
                 *  [-5, 5] -> [x*, y*]
                 *  [ 8, 0] -> [x*, y*]
                 */
                case 11: return new OptimizationProb(
                    delegate(Vector x)
                    {
                        double temp = (x[0] * x[0]) + (x[1] * x[1]);
                        return Math.Sin(x[0]) + Math.Sin(2.0 * x[1]) + temp;
                    },
                    delegate(Vector x)
                    {
                        double a = 2.0 * x[0] + Math.Cos(x[0]);
                        double b = 2.0 * x[1] + 2.0 * Math.Cos(2.0 * x[1]);
                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.450183611294874, -0.514933264661129),
                    },
                    new Vector[]
                    {
                        new Vector(-5.0, 5.0),
                        new Vector(8.0, 0.0),
                    });

            }

            //Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }

        #endregion ///////////////////////////////////////////////////////////////////

    }
}
