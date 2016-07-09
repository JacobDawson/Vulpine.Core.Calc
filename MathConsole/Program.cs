using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Vulpine Productions Core Library -- Mathmatics Library");
                Console.WriteLine("Testing Suite For Various Numerical Methods");
                Console.WriteLine();

                Console.WriteLine("What test would you like to run?");
                Console.WriteLine("\tA) Polynomial Class");
                Console.WriteLine("\tB) Root Finding Methods");
                Console.WriteLine("\tQ) Quit Program");
                char sel = '\0';

                while (true)
                {
                    sel = Console.ReadKey(false).KeyChar;
                    sel = Char.ToUpper(sel);
                    if (sel >= '1' && sel <= '2') break;
                    if (sel >= 'A' && sel <= 'B') break;
                    if (sel == 'Q') break;
                }

                //this gets us out of the main loop
                if (sel == 'Q') break;

                switch (sel)
                {
                    case '1': case 'A':
                        PolynomialTests.Run(); break;
                    case '2': case 'B':
                        RootFinding.Run(); break;                 
                }

                Console.Clear();
            }
        }
    }
}
