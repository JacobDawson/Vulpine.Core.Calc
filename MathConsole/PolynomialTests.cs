using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Functions;
using Vulpine.Core.Calc.Numeric;
using Vulpine.Core.Calc.Exceptions;

namespace MathConsole
{
    public static class PolynomialTests
    {
        public static void Run()
        {
            while (true)
            {
                Polynomial f = ConsoleHelp.GetPoly("f(x)");
                Polynomial g = ConsoleHelp.GetPoly("g(x)");
                Polynomial temp = null;

                Console.Clear();
                Console.WriteLine("f(x) = " + f.Print());
                Console.WriteLine();
                Console.WriteLine("g(x) = " + g.Print());
                Console.WriteLine();

                temp = f.Deriv();
                Console.WriteLine("f'(x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Prim();
                Console.WriteLine("F[-1](x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Add(g);
                Console.WriteLine("f(x) + g(x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Sub(g);
                Console.WriteLine("f(x) - g(x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Mult(g);
                Console.WriteLine("f(x) * g(x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Div(g);
                Console.WriteLine("f(x) / g(x) = " + temp.Print());
                Console.WriteLine();

                temp = f.Mod(g);
                Console.WriteLine("f(x) % g(x) = " + temp.Print());
                Console.WriteLine();

                if (!ConsoleHelp.Continue()) break;
            }
        }
    }
}
