using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Functions;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Exceptions;

namespace MathConsole
{
    public static class RootFinding
    {
        private static Polynomial f;
        private static Polynomial dx;
        private static RootFinder rf;
        private static double y;
        private static double x1;
        private static double x2;

        public static void Run()
        {
            PromptSettings();
        }

        /**
         *  Prompts the user to input the settings to use when preforming
         *  itterative numbrical analisis, or offers the default settings.
         */
        private static char PromptSettings()
        {
            char cont = 'S';

            while (cont == '4' || cont == 'S')
            {
                Console.Clear();
                Console.WriteLine("Please indicate the settings to use when " +
                "preforming itteritive numerical analisis. Or just enter 'D' " +
                "to use the default settings.");
                Console.WriteLine();

                double e = 0.0;
                int max = 0;
                string temp = null;
                bool sucess = false;

                Console.Write("Error Tollerence: ");
                temp = Console.ReadLine();
                sucess = Double.TryParse(temp, out e);

                if (temp.Length > 0 && Char.ToUpper(temp[0]) == 'D')
                {
                    e = VMath.TOL;
                }
                else if (sucess == false)
                {
                    Console.WriteLine("Unable to determin the desired Error " +
                    "Tollerence. Resorting to the default value. ");
                    Console.WriteLine();
                    e = VMath.TOL;
                }
                else if (e <= 0.0)
                {
                    Console.WriteLine("Invalid Error Tollerence given. Now " +
                    "resorting to the default value. ");
                    Console.WriteLine();
                    e = VMath.TOL;
                }

                Console.Write("Maximum Itterations: ");
                temp = Console.ReadLine();
                sucess = Int32.TryParse(temp, out max);

                if (temp.Length > 0 && Char.ToUpper(temp[0]) == 'D')
                {
                    max = 1024;
                }
                else if (sucess == false)
                {
                    Console.WriteLine("Unable to determin the desired Maximum " +
                    "Number of Itterations. Resorting to the default value.");
                    Console.WriteLine();
                    max = 1024;
                }
                else if (max < 5)
                {
                    Console.WriteLine("The Algroythim must run for atleast 5 " +
                    "Itterations. Now using the minimun number of itterations.");
                    Console.WriteLine();
                    max = 5;
                }

                rf = new RootFinder(max, e);
                cont = InputEquation();
            }

            return cont;
        }

        /**
         *  Prompts the user to input his equation for which he would like
         *  to find the roots in the form of a polynomial. It prints out
         *  the polynomial and its diritivitive before proceding to ask
         *  the user for which values to evaluate.
         */
        private static char InputEquation()
        {
            char cont = 'C';

            while (cont == '3' || cont == 'C')
            {
                f = ConsoleHelp.GetPoly("f(x)");
                dx = f.Deriv();

                Console.Clear();
                Console.WriteLine("f(x) = " + f.Print());
                Console.WriteLine("f'(x) = " + dx.Print());
                ConsoleHelp.Wait();

                //goes on to the next segement
                cont = InputNumerics();
            }

            return cont;
        }

        /**
         *  Asks the user to input numerical values for the target and
         *  the upper and lower x values that bracket the target. All
         *  before proceding to evaluate the inputed data.
         */
        private static char InputNumerics()
        {
            char cont = 'B';

            while (cont == '2' || cont == 'B')
            {
                bool got_values = false;
                while (true)
                {
                    Console.Clear();

                    Console.Write("Input the target (Y) value: ");
                    got_values = Double.TryParse(Console.ReadLine(), out y);
                    if (got_values == false)
                    {
                        Console.WriteLine("Input is not a number. Defaulting " +
                        "to the standard value of zero. ");
                        Console.WriteLine(); y = 0.0;
                    }

                    Console.Write("Input the lower bound on (X): ");
                    got_values = Double.TryParse(Console.ReadLine(), out x1);
                    if (got_values == false)
                    {
                        Console.WriteLine("Please input valid numerical values.");
                        ConsoleHelp.Wait(); continue;
                    }

                    Console.Write("Input the upper bound on (X): ");
                    got_values = Double.TryParse(Console.ReadLine(), out x2);
                    if (got_values == false)
                    {
                        Console.WriteLine("Please input valid numerical values.");
                        ConsoleHelp.Wait(); continue;
                    }

                    break;
                }

                //goes on to the next segement
                cont = Evaluation();
            }

            return cont;
        }

        /**
         *  This method asks the user which method of evaluation he would
         *  like to use and then preforemes it. It reports the sucess or
         *  failur of the operation allong with any pertenent information.
         *  It is also responcible for asking the user what he would like
         *  to do next once the operation is complete.
         */
        private static char Evaluation()
        {
            char cont = 'A';

            while (cont == '1' || cont == 'A')
            {
                Console.Clear();
                Console.WriteLine("Select a method of numerical evaluation: ");
                Console.WriteLine("\tA) Bisection");
                Console.WriteLine("\tB) False Position");
                Console.WriteLine("\tC) Secant Method");
                Console.CursorVisible = false;
                char choice = '0';

                while (true)
                {
                    choice = Console.ReadKey(false).KeyChar;
                    choice = Char.ToUpper(choice);
                    if (choice >= '1' && choice <= '3') break;
                    if (choice >= 'A' && choice <= 'C') break;
                }

                Console.Clear();
                Console.CursorVisible = true;
                Console.WriteLine("f(x) = " + f.Print());
                Console.WriteLine("f({0}) = {1}", x1, f.Evaluate(x1));
                Console.WriteLine("f({0}) = {1}", x2, f.Evaluate(x2));
                Console.WriteLine("y = " + y);
                Console.WriteLine();

                Console.WriteLine("Error Tolerance: " + rf.Tolerance);
                Console.WriteLine("Maximum Itterations: " + rf.MaxIters);

                Result<Double> value = default(Result<Double>);
                string method = null;
                bool sucess = true;

                try
                {
                    switch (choice)
                    {
                        case '1':
                        case 'A':
                            value = rf.Bisection(f.Evaluate, y, x1, x2);
                            method = "Bisection"; break;
                        case '2':
                        case 'B':
                            value = rf.FalsePos(f.Evaluate, y, x1, x2);
                            method = "False Position"; break;
                        case '3':
                        case 'C':
                            value = rf.Secant(f.Evaluate, y, x1, x2);
                            method = "Secant Method"; break;
                        default:
                            value = new Result<Double>();
                            method = "Error"; break;
                    }
                }
                catch (MathematicsExcp me)
                {
                    Console.Write("Exception generated when trying to " +
                    "find the root of the function. ");
                    Console.WriteLine(me.Message);
                    Console.WriteLine(me.StackTrace);
                    sucess = false;
                }

                if (sucess)
                {
                    Console.WriteLine("{0} resulted in {1} with an error of {2}",
                        method, value.Value.ToString("0.###"), value.Error);
                }

                Console.WriteLine();
                ConsoleHelp.Wait();

                Console.Clear();
                Console.WriteLine("What would you like to do now? ");
                Console.WriteLine("\tA) Another Method");
                Console.WriteLine("\tB) Another Evaluation");
                Console.WriteLine("\tC) Another Equation");
                Console.WriteLine("\tS) Change The Settings");
                Console.WriteLine("\tQ) Quit The Program");
                Console.CursorVisible = false;

                while (true)
                {
                    cont = Console.ReadKey(false).KeyChar;
                    cont = Char.ToUpper(cont);
                    if (cont >= '1' && cont <= '5') break;
                    if (cont >= 'A' && cont <= 'C') break;
                    if (cont == 'S' || cont == 'Q') break;
                }

                Console.Clear();
                Console.CursorVisible = true;
            }

            return cont;
        }
    }
}
