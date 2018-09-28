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
        #region Class Definitions...

        private VFunc<Vector> system;

        private Vector[] start;
        private Vector[] targ;

        private SystemEquaitons(VFunc<Vector> system, Vector[] targ, Vector[] start)
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

        #endregion ///////////////////////////////////////////////////////////////////

        #region Test Cases...

        /// <summary>
        /// Obtains one of the provided Systems of Equations for use in 
        /// testing equation solving algorythims.
        /// </summary>
        /// <param name="index">Index of the desired problem</param>
        /// <returns>A solved optimisaiton problem for testing</returns>
        public static SystemEquaitons GetCase(int index)
        {
            switch (index)
            {
                /**
                 *  Odd Loop and Straight Line
                 *  
                 *  f(x, y) = e^(x^2) + 8 * x * sin(y) = 0
                 *  g(x, y) = x + y - 1 = 0
                 *  
                 *  x1 = -0.140285010811189634041537597
                 *  y1 = 1.140285010811189634041537597
                 *  
                 *  x2 = -1.41971365341419697611490804
                 *  y2 = 2.41971365341419697611490804
                 *  
                 *  [ 0, 1] -> [x1, y1]
                 *  [-2, 2] -> [x2, y2]
                 */
                case 01: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double a = Math.Exp(x[0] * x[0]) + 8.0 * x[0] * Math.Sin(x[1]);
                        double b = x[0] + x[1] - 1.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.140285010811190, 1.14028501081119),
                        new Vector(-1.41971365341420, 2.41971365341420),
                    },
                    new Vector[]
                    {
                        new Vector(0.0, 1.0),
                        new Vector(-2.0, 2.0),
                    });

                /**
                 *  Oval and Parabola
                 *  
                 *  f(x, y) = x^2 - 2 x - y + 1/2 = 0
                 *  g(x, y) = x^2 + 4 y^2 - 4 = 0
                 *  
                 *  x1 = -0.22221455505972182402612858
                 *  y1 =  0.99380841859983379015533279
                 *  
                 *  x2 = 1.9006767263670657709625783
                 *  y2 = 0.31121856541929426976921900
                 *  
                 *  [0, 1] -> [x1, y1]
                 *  [2, 0] -> [x2, y2]
                 */
                case 02: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];

                        double a = xx - 2.0 * x[0] - x[1] + 0.5;
                        double b = xx + 4.0 * yy - 4.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.222214555059722, 0.993808418599834),
                        new Vector(1.90067672636707, 0.311218565419294),
                    },
                    new Vector[]
                    {
                        new Vector(0.0, 1.0),
                        new Vector(2.0, 0.0),
                    });

                /**
                 *  Line and Parabola
                 *  
                 *  f(x, y) = e^(x^2) - e^(sqrt(3) y) = 0
                 *  g(x, y) = x - y + 1 = 0
                 *  
                 *  x1 =  2.4414779760851338648477077
                 *  y1 =  3.4414779760851338648477077
                 *  
                 *  x2 = -0.70942716851625657132026137
                 *  y2 =  0.29057283148374342867973863
                 *  
                 *  [2, 3] -> [x1, y1]
                 *  [0, 1] -> [x2, y2]
                 */
                case 03: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double y3 = x[1] * 1.73205080756888;

                        double a = Math.Exp(xx) - Math.Exp(y3);
                        double b = x[0] - x[1] + 1.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(2.44147797608513, 3.44147797608513),
                        new Vector(-0.709427168516257, 0.290572831483743),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 3.0),
                        new Vector(0.0, 1.0),
                    });

                /**
                 *  L-Shaped Curves
                 *  
                 *  f(x, y) = ln(y^2) - x^2 + x y = 0
                 *  g(x, y) = ln(x^2) - y^2 + x y = 0
                 *  
                 *  s1 = [1, 1]
                 *  s2 = [-1, -1]
                 *  
                 *  [1.2, 1.2] -> [1, 1]
                 *  [0.5, 0.5] -> [1, 1]
                 */
                case 04: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];
                        double xy = x[0] * x[1];

                        double a = Math.Log(yy) - xx + xy;
                        double b = Math.Log(xx) - yy + xy;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 1.0),
                        new Vector(-1.0, -1.0),
                    },
                    new Vector[]
                    {
                        new Vector(1.2, 1.2),
                        new Vector(0.8, 0.8),
                    });

                /**
                 *  Circle and Hyperabola
                 *  
                 *  f(x, y) = x^2 + y^2 - 1 = 0
                 *  g(x, y) = x^2 - y^2 - 1/2 = 0
                 *  
                 *  x* = +/- sqrt(3) / 2
                 *  y* = +/- 0.5
                 *  
                 *  [0.8,  0.6] -> [+x*, +y*]
                 *  [0.8, -0.4] -> [+x*, -y*]
                 */
                case 05: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];

                        double a = xx + yy - 1.0;
                        double b = xx - yy - 0.5;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(0.866025403784439, 0.5),
                        new Vector(0.866025403784439, -0.5),
                        new Vector(-0.866025403784439, 0.5),
                        new Vector(-0.866025403784439, -0.5),
                    },
                    new Vector[]
                    {
                        new Vector(0.8, 0.6),
                        new Vector(0.8, -0.4),
                    });

                /**
                 *  Circle and Square Root
                 *  
                 *  f(x, y) = x^2 + y^2 - 42 = 0
                 *  g(x, y) = 2 sqrt(x) - 4 y + 5 = 0
                 *  
                 *  x* = 5.9900472901682776939427494
                 *  y* = 2.4737286556022415149390256
                 *  
                 *  [5.5, 2.5] -> [x*, y*]
                 *  [6.0, 2.0] -> [x*. y*]
                 */
                case 06: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double a = x[0] * x[0] + x[1] * x[1] - 42.0;
                        double b = 2.0 * Math.Sqrt(x[0]) - 4.0 * x[1] + 5.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(5.99004729016828, 2.47372865560224),
                    },
                    new Vector[]
                    {
                        new Vector(5.5, 2.5),
                        new Vector(6.0, 2.0),
                    });

                /**
                 *  Two Parabolas
                 *  
                 *  f(x, y) = y - (x - 4)^2 - 2 = 0
                 *  g(x, y) = y + (x - 3)^2 - 5 = 0
                 *  
                 *  x1 = 2.3819660112501051517954132
                 *  y1 = 4.6180339887498948482045868
                 *  
                 *  x2 = 4.6180339887498948482045868
                 *  y2 = 2.3819660112501051517954132
                 *  
                 *  [2, 4] -> [x1, y1]
                 *  [4, 2] -> [x2, y2]
                 */
                case 07: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xm4 = x[0] - 4.0;
                        double xm3 = x[0] - 3.0;

                        double a = x[1] - xm4 * xm4 - 2.0;
                        double b = x[1] + xm3 * xm3 - 5.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(2.38196601125011, 4.61803398874989),
                        new Vector(4.61803398874989, 2.38196601125011),
                    },
                    new Vector[]
                    {
                        new Vector(2.0, 4.0),
                        new Vector(4.0, 2.0),
                    });

                /**
                 *  Elipis and Line
                 *  
                 *  f(x, y) = 6x^2 + 3y^2 - 10 = 0
                 *  g(x, y) = 2 x + y - 1 = 0
                 *  
                 *  x1 = -0.37377344785321419106751103
                 *  y1 =  1.7475468957064283821350221
                 *  
                 *  x2 =  1.0404401145198808577341777
                 *  y2 = -1.0808802290397617154683554
                 */
                case 08: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xx = x[0] * x[0];
                        double yy = x[1] * x[1];

                        double a = 6.0 * xx + 3.0 * yy - 10.0;
                        double b = 2.0 * x[0] + x[1] - 1.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-0.373773447853214, 1.74754689570643),
                        new Vector(1.04044011451988, -1.08088022903976),
                    },
                    new Vector[]
                    {
                        new Vector(-1.0, 1.0),
                        new Vector(1.0, -1.0),
                    });

                /**
                 *  Circle and Sin Wave
                 *  
                 *  f(x, y) = x^2 + y^2 - 2 = 0
                 *  g(x, y) = sin(x) - y = 0
                 *  
                 *  x1 = -1.09858687486696308488609311233345
                 *  y1 = -0.89056548235940494071517569851848
                 *  
                 *  x2 = 1.09858687486696308488609311233345
                 *  y2 = 0.89056548235940494071517569851848
                 */
                case 09: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double a = x[0] * x[0] + x[1] * x[1] - 2.0;
                        double b = Math.Sin(x[0]) - x[1];

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.09858687486696, 0.890565482359405),
                        new Vector(-1.09858687486696, -0.890565482359405),
                    },
                    new Vector[]
                    {
                        new Vector(1.0, 1.0),
                        new Vector(-1.0, 1.0),
                    });

                /**
                 *  Two Circles
                 *  
                 *  f(x, y) = 4x^2 + 4y^2 - 100 = 0
                 *  g(x, y) = (x - 5)^2 + (y - 5)^2 - 50 = 0
                 *  
                 *  x1 = -2.0571891388307382381270197
                 *  y1 =  4.5571891388307382381270197
                 *  
                 *  x2 =  4.5571891388307382381270197
                 *  y2 = -2.0571891388307382381270197
                 */
                case 10: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xm5 = x[0] - 5.0;
                        double ym5 = x[1] - 5.0;

                        double a = 4.0 * x[0] * x[0] + 4.0 * x[1] * x[1] - 100.0;
                        double b = xm5 * xm5 + ym5 * ym5 - 50.0;

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(-2.05718913883074, 4.55718913883074),
                        new Vector(4.55718913883074, -2.05718913883074),
                    },
                    new Vector[]
                    {
                        new Vector(-1.5, 5.0),
                        new Vector(-2.5, 4.0),
                    });

                /**
                 *  Weird Exponential
                 *  
                 *  f(x, y) = (x - 3)^2 + (y - 3)^2 - 4 = 0
                 *  g(x, y) = x^y - y^x = 0
                 *  
                 *  x1 = 1.58578643762690495119831128
                 *  y1 = 1.5857864376269049511983113
                 *  
                 *  x2 = 4.62725712730000798747495473
                 *  y2 = 1.8372299274356406108806350
                 *  
                 *  x3 = 1.83722992743564061088063503
                 *  y3 = 4.62725712730000798747495473
                 *  
                 *  x4 = 4.41421356237309504880168872
                 *  y4 = 4.4142135623730950488016887
                 */
                case 11: return new SystemEquaitons(
                    delegate(Vector x)
                    {
                        double xm3 = x[0] - 3.0;
                        double ym3 = x[1] - 3.0;

                        double a = xm3 * xm3 + ym3 * ym3 - 4.0;
                        double b = Math.Pow(x[0], x[1]) - Math.Pow(x[1], x[0]);

                        return new Vector(a, b);
                    },
                    new Vector[]
                    {
                        new Vector(1.58578643762690, 1.58578643762690),
                        new Vector(4.62725712730001, 1.83722992743564),
                        new Vector(1.83722992743564, 4.62725712730001),
                        new Vector(4.41421356237310, 4.41421356237310),
                    },
                    new Vector[]
                    {
                        new Vector(4.5, 4.5),
                        new Vector(4.5, 1.5),
                    });

            }

            //Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }

        #endregion ///////////////////////////////////////////////////////////////////

    }
}
