using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Vulpine.Core.Calc.RandGen;

namespace RandTests
{
    class Program
    {
        public const int SEED = 516152248;


        public static void Main(string[] args)
        {
            VRandom rng = SelectRGN();
            SelectTests(rng);

            //ShiftTest();

            //TestFunction();
        }

        public static void ShiftTest()
        {
            int x = (-1 >> 31);
            int y = (int)(UInt32.MaxValue >> 31);

            Console.WriteLine("Int(-1) >> 31 = " + x);
            Console.WriteLine("UInt(-1) >> 31 = " + y);

            Console.ReadKey(true);
        }

        public static VRandom SelectRGN()
        {
            Console.Clear();

            Console.WriteLine("Please select an PRGN to test:");
            Console.WriteLine("   1) LCG");
            Console.WriteLine("   2) Mersenne Twister");
            Console.WriteLine("   3) System.Random");
            Console.WriteLine("   4) XOR-Shift Register");
            Console.WriteLine("   5) Rule 30");
            Console.WriteLine("   6) Prime Numbers");

            bool pass = false;
            int select = -1;

            while (!pass)
            {
                string s = Console.ReadLine();
                pass = Int32.TryParse(s, out select);

                if (select < 1 | select > 6) pass = false;
            }

            switch (select)
            {
                case 1:
                    return new RandLCG(SEED);
                case 2:
                    return new RandMT(SEED);
                case 3:
                    return new RandSystem(SEED);
                case 4:
                    return new RandXOR(SEED);
                case 5:
                    return new RandR30(SEED);
                case 6:
                    return new RandFile("primes3.rng");
                default:
                    throw new Exception();
            }            
        }

        public static void SelectTests(VRandom rng)
        {
            Console.Clear();

            Console.WriteLine("Please choose which test to run:");
            Console.WriteLine("   1) List Double Values");
            Console.WriteLine("   2) List Interger Values");
            Console.WriteLine("   3) Generate File For DieHard");
            Console.WriteLine("   4) Generate Image From Bits");
            Console.WriteLine("   5) Test Uniform Distribution");
            Console.WriteLine("   6) Roll Dice");

            bool pass = false;
            int select = -1;

            while (!pass)
            {
                string s = Console.ReadLine();
                pass = Int32.TryParse(s, out select);

                if (select < 1 | select > 6) pass = false;
            }

            switch (select)
            {
                case 1:
                    ListDoubles(rng, 100000);
                    break;
                case 2:
                    ListIntergers(rng, 100000);
                    break;
                case 3:
                    GenerateFile(rng, 10);
                    break;
                case 4:
                    GenerateImage(rng);
                    break;
                case 5:
                    TestUniform(rng, 20, 1000000);
                    break;
                case 6:
                    RollDice(rng, 1000000);
                    break;
                default:
                    throw new Exception();
            } 


        }

        public static void ListDoubles(VRandom rng, int run)
        {
            Console.Clear();

            for (int i = 0; i < run; i++)
            {
                double x = rng.NextDouble();
                long bytes = BitConverter.DoubleToInt64Bits(x);
                bool test = (x >= 0.0) && (x < 1.0);

                string s1 = x.ToString("0.000000000000000");
                string s2 = bytes.ToString("X");
                string s3 = test ? "pass" : "fail";
 
                Console.WriteLine("{0}  -  {1}  -  {2}  -  {3}", i, s1, s2, s3);
                if (!test) break;
            }

            Console.WriteLine("End of Test");

            Console.ReadKey(true);
        }

        public static void ListIntergers(VRandom rng, int run)
        {
            Console.Clear();

            for (int i = 0; i < run; i++)
            {
                int x1 = rng.NextInt();
                int x2 = rng.NextInt();
                int x3 = rng.NextInt();
                int x4 = rng.NextInt();


                Console.WriteLine("{0:X8} - {1:X8} - {2:X8} - {3:X8}", x1, x2, x3, x4);

            }

            Console.WriteLine("End of Test");

            Console.ReadKey(true);
        }

        public static void GenerateFile(VRandom rng, int mb)
        {
            Console.Clear();

            Console.WriteLine("Output File Name:");
            string file = Console.ReadLine();


            Stream output = File.Create(file, 2048);
            BinaryWriter bw = new BinaryWriter(output);


            int size = mb * 1024;
            int bytes = size * 1024;

            Console.Clear();

            for (int i = 0; i < size; i++)
            {
                //writes to the file in 1024 byte chunks
                for (int j = 0; j < 256; j++)
                bw.Write(rng.Get32bits());

                bw.Flush();
                
                int written = i * 1024;
                double t = written / (double)bytes;
                int per = (int)(t * 100.0);

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Written {0} bytes out of {1} : {2}% finished.   ",
                    i * 1024, bytes, per); 
            }


            bw.Close();
            bw.Dispose();

            Console.ReadKey(true);
        }

        public static void GenerateImage(VRandom rng)
        {
            Console.Clear();

            Console.WriteLine("Output File Name:");
            string file = Console.ReadLine();

            Bitmap bmp = new Bitmap(1024, 1024);

            Console.WriteLine("Drawing Started.");

            for (int x = 0; x < 1024; x += 32)
            {
                for (int y = 0; y < 1024; y++)
                DrawInt(rng, bmp, x, y);
            }


            bmp.Save(file);

            Console.WriteLine("Drawing Fiinished.");
            Console.ReadKey(true);
        }

        public static void GenerateImage2(VRandom rng)
        {
            Console.Clear();

            Console.WriteLine("Output File Name:");
            string file = Console.ReadLine();

            Bitmap bmp = new Bitmap(1024, 1024);

            Console.WriteLine("Drawing Started.");

            for (int x = 0; x < 1024; x += 128)
            {
                for (int y = 0; y < 1024; y++)
                {
                    DrawInt(rng, bmp, x, y);
                    DrawInt(rng, bmp, x + 32, y);
                    DrawInt(rng, bmp, x + 64, y);
                    DrawInt(rng, bmp, x + 96, y);
                }
            }


            bmp.Save(file);

            Console.WriteLine("Drawing Fiinished.");
            Console.ReadKey(true);
        }

        private static void DrawInt(VRandom rng, Bitmap bmp, int x, int y)
        {
            byte[] data = BitConverter.GetBytes(rng.Get32bits());

            for (int i = 0; i < 32; i++)
            {
                int j = i / 8;
                int k = i % 8;

                bool bit = ((data[j] >> k) % 2) == 0;
                Color c = bit ? Color.White : Color.Black;

                bmp.SetPixel(x + i, y, c);               
            }
        }

        public static void TestUniform(VRandom rng, int size, int run)
        {
            Console.Clear();
            Console.WriteLine("Generating Data...");

            int[] counts = new int[size];

            for (int i = 0; i < size; i++)
                counts[i] = 0;

            for (int i = 0; i < run; i++)
            {
                int index = rng.RandInt(size);
                counts[index]++;
            }

            for (int i = 0; i < size; i++)
            {
                double per = counts[i] / (double)run;
                Console.WriteLine("{0:00}  -  {1:0000000}  -  {2}", i, counts[i], per);
            }

            Console.WriteLine();
            Console.WriteLine("Press Any Key...");
            Console.ReadKey(true);
        }

        public static void RollDice(VRandom rng, int rolls)
        {
            Console.Clear();
            Console.WriteLine();

            int[] count = new int[18 - 2];

            for (int i = 0; i < count.Length; i++)
                count[i] = 0;

            for (int i = 0; i < rolls; i++)
            {
                int x = rng.RollDice(3, 6, 0);
                count[x - 3] += 1;
            }

            for (int i = 0; i < count.Length; i++)
            {
                int x = i + 3;
                double f = (double)count[i] / (double)rolls;
                int d = (int)(f * 100.0);
                string dd = new String('*', d);


                Console.WriteLine("{0:00}:  {1:0.000}   {2}", x, f, dd);
            }

            Console.WriteLine();
            Console.WriteLine("Press Any Key...");
            Console.ReadKey(true);
        }

        private static void TestFunction()
        {
            for (int i = 1; i < Int32.MaxValue; i++)
            {
                int x = (Int32.MaxValue / i) + 1;
                x = Int32.MaxValue / x;

                bool test = (x >= 0) && (x < i);

                Console.WriteLine("F({0}) = {1}  :  {2}", i, x, test);
                if (!test) break;
            }

            Console.WriteLine();
            Console.WriteLine("End of Run");
            Console.ReadKey(true);
        }


    }
}
