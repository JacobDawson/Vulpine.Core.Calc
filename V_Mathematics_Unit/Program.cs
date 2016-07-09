using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using NUnit.Unofficial;

namespace CVL_Mathematics_Test
{
    class Program
    {

        static void Main(string[] args)
        {
            TestRunner runner = new TestRunner();

            Console.WriteLine("Vulpine Procuctions Core Library: Mathmatics");
            Console.WriteLine("Loading Unit Tests via reflection ...");
            Console.WriteLine();
            runner.LoadTests(Assembly.GetExecutingAssembly());

            Console.WriteLine("Running Tests ...");
            Console.WriteLine();
            runner.RunAllTests();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey(true);
        }
    }
}
