using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Functions;

namespace MathConsole
{
    public static class ConsoleHelp
    {
        private static Random rand;

        static ConsoleHelp()
        {
            rand = new Random();
        }

        /// <summary>
        /// Waits for the user to read the output, prompting him to
        /// press any key to continue.
        /// </summary>
        public static void Wait()
        {
            Console.WriteLine();
            Console.Write("Press Any Key to Continue... ");
            Console.ReadKey(false);
        }

        /// <summary>
        /// Assk the user if they want to continue.
        /// </summary>
        /// <returns>True if the user wants to continue</returns>
        public static bool Continue()
        {
            Console.WriteLine();
            Console.Write("Would you like to continue? (Y/N)");
            char c = Console.ReadKey(false).KeyChar;

            if (c == 'y' | c == 'Y') return true;
            else return false;
        }

        /// <summary>
        /// Prompts the user to input a polynomial.
        /// </summary>
        /// <param name="name">Name of the polynomial</param>
        /// <returns>The user's polynomial</returns>
        public static Polynomial GetPoly(string name)
        {
            Console.Clear();

            //asks the user for the degree of the polynomial
            Console.Write("Enter the degreee of polynomial "
            + name + ": ");
            int deg = 0;

            //defaults to degree 5 if the user input is invalid
            bool test = Int32.TryParse(Console.ReadLine(), out deg);
            if (test == false || deg < 0 || deg > 20)
            {
                Console.WriteLine("Expected an interger value between "
                + "0 and 20. Will now default to the value 3.");
                deg = 3;
            }

            Console.WriteLine("Input the values for the coeffecents, "
            + "or type 'R' for random. You may also type 'A' to "
            + "automaticaly fill in all remaning coffecents. ");
            Console.WriteLine();

            double[] coeffs = new double[deg + 1];
            int index = deg;

            while (index >= 0)
            {
                //informs the user which coffencent is being set
                Console.Write("X^" + index.ToString("00") + " = ");
                string temp = Console.ReadLine();
                double first = 0.0;
              
                //determins if any input has been given at all       
                if (temp.Length == 0)
                {
                    Console.WriteLine("No input given. " +
                    "Assining a random value");
                    Console.WriteLine();

                    first = rand.NextDouble();
                    coeffs[index] = first * 10.0 - 5.0;
                    index--; continue;
                }

                //determins if the user requested random input
                char c = Char.ToUpper(temp[0]);
                if (c == 'A')
                {
                    break;
                }
                if (c == 'R')
                {
                    first = rand.NextDouble();
                    coeffs[index] = first * 10.0 - 5.0;
                    index--; continue;
                }

                //parses the players input or assines a random value
                test = Double.TryParse(temp, out first);
                if (test) coeffs[index] = first;
                else
                {
                    Console.WriteLine("Invalid Input Given. "
                    + "Now assigning a random value. ");
                    Console.WriteLine();

                    first = rand.NextDouble();
                    coeffs[index] = first * 10.0 - 5.0;
                }

                index--;
            }

            //takes care of any remaning coeffecents
            while (index >= 0)
            {
                double first = rand.NextDouble();
                coeffs[index] = first * 10.0 - 5.0;
                index--;
            }

            //returns the generated polynomial
            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Converts a polynomial into a string format sutable for printing.
        /// </summary>
        /// <param name="f">Polynomial to print</param>
        /// <returns>The polynomial as a string</returns>
        public static string Print(this Polynomial f)
        {
            //takes care of the first argument
            StringBuilder sb = new StringBuilder();
            sb.Append(f[0].ToString("0.###"));

            if (f.Degree >= 1)
            {
                double abs = Math.Abs(f[1]);
                string val = abs.ToString("0.###");
                char sign = (f[1] < 0) ? '-' : '+';
                sb.AppendFormat(" {0} {1}x", sign, val);
            }

            for (int i = 2; i <= f.Degree; i++)
            {
                double abs = Math.Abs(f[i]);
                string val = abs.ToString("0.###");
                char sign = (f[i] < 0) ? '-' : '+'; 
                sb.AppendFormat(" {0} {1}x^{2}", sign, val, i);
            }

            return sb.ToString();
        }
    }
}
