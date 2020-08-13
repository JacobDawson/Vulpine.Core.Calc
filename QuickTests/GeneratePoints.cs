using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.RandGen;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Data;


namespace QuickTests
{
    /// <summary>
    /// The purppous of this class is to generate a set of points to be used
    /// in testing the effectivness of Vector Trees, such as the KD-Tree
    /// </summary>
    public static class GeneratePoints
    {
        public static void Run()
        {            
            Console.Write("Enter the number of dimentions: ");
            int dim = Int32.Parse(Console.ReadLine());

            Console.Write("Ender the number of points to generate: ");
            int count = Int32.Parse(Console.ReadLine());


            //NOTE: change this line to change the class under test
            TreeVector<Vector> tree = new TreeKD<Vector>(dim);



            Console.WriteLine();
            List<Vector> points = new List<Vector>(count);
            VRandom rng = new RandMT();

            //generates the set of points
            for (int i = 0; i < count; i++)
            {
                Vector v = new Vector(dim);

                for (int j = 0; j < dim; j++)
                v[j] = rng.RandDouble(-100.0, 100.0);

                points.Add(v);
                tree.Add(v, v);
                Console.WriteLine(v.ToString("0.00"));
            }

            Console.WriteLine("Building Tree Structur... ");
            tree.Build();

            Console.WriteLine("Press Any Key To Continue... ");
            Console.ReadKey(true);

            Console.Clear();

            Console.Write("Enter the number of samples: ");
            int samp = Int32.Parse(Console.ReadLine());

            int pass_count = 0;

            for (int i = 0; i < samp; i++)
            {
                //obtains a random point
                Vector probe = new Vector(dim);

                for (int j = 0; j < dim; j++)
                probe[j] = rng.RandDouble(-100.0, 100.0);

                Vector n1 = new Vector(dim);
                Vector n2 = new Vector(dim);
                double maxdist = Double.PositiveInfinity;

                //finds the nearest vector by exhaustive search
                for (int k = 0; k < count; k++)
                {
                    Vector test = points[k];
                    double dist = test.Dist(probe);

                    if (dist < maxdist)
                    {
                        maxdist = dist;
                        n1 = test;
                    }             
                }

                var pair = tree.GetNearest(probe);
                n2 = pair.Location;

                double comp = n1.Dist(n2);
                bool pass = (comp < VMath.TOL);
                if (pass) pass_count++;

                double d1 = probe.Dist(n1);
                double d2 = probe.Dist(n2);

                string vs = probe.ToString("0.00");
                string n1s = d1.ToString("0.00000");
                string n2s = d2.ToString("0.00000");
                string ps = pass ? "PASS" : "FAIL";

                if (!pass) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine("{0}, {1}, {2}, {3}", vs, n1s, n2s, ps);
            }

            Console.WriteLine();

            if (pass_count < samp) Console.ForegroundColor = ConsoleColor.Red;
            else Console.ForegroundColor = ConsoleColor.Gray;

            string final = (pass_count == samp) ? "PASS" : "FAIL";
            Console.WriteLine("{0} / {1} {2}", pass_count, samp, final);
            
        }
    }
}
