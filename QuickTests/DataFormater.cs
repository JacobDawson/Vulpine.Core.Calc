using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace QuickTests
{
    public static class DataFormater
    {
        public const string file_in = @"H:\Programing\Download\randbyte02.txt";

        public static void GenFile()
        {
            StreamReader sr = new StreamReader(file_in);
            StreamWriter sw = new StreamWriter("output.txt");

            StringBuilder output = new StringBuilder(50);


            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] bits = line.Split(' ');

                //Console.WriteLine(line);
                //Console.WriteLine(bits.Length);

                output.Clear();

                for (int i = 0; i < bits.Length - 1; i++)
                {
                    if (i % 4 == 0)
                    {
                        output.Append("0x");
                    }

                    //Console.WriteLine(bits[i]);
                    output.Append(bits[i]);

                    if (i % 4 == 3)
                    {
                        output.Append(", ");
                    }
                }

                string outs = output.ToString();

                Console.WriteLine(outs);
                sw.WriteLine(outs);
            }

            sw.Flush();
            sw.Close();
            sr.Close();

            Console.WriteLine("EOF");
            Console.WriteLine();
        }
    }
}
