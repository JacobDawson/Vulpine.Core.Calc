using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;

namespace QuickTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //BayerMatrixTest.Run();
            //GeneratePoints.Run();
            
            //Distrobution.Run();
            //RootFinding.Run();
            //DivByZero.Run();
            //LU_Decomp.Run();
            //RandomMatrix.Run();
            //Binomial.Run();

            //AGM_Tests.TestComp4();
            //AGM_Tests.TestError();

            //PadeAprox.TryPade();

            //LambertTests.RunTests();
            //AutoDiffTests.RunTests();

            //MemStress.Run01();


            //DataFormater.GenFile();


            GradientTextFile();


            //JacobiTests.ListSamples();
            //JacobiTests.TestInversion6();         //6
            //JacobiTests.TestCompatablity2();      //2

            Console.ReadKey();
        }


        static void GradientTextFile()
        {
            StreamWriter sw = new StreamWriter("image.txt");
            int data = 0;
            int r, g, b;

            Console.WriteLine("Drawing First Gradient");

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    r = (j * 2) << 16;
                    g = (i * 2) << 8;
                    b = 0 << 0;

                    data = r | g | b;

                    sw.WriteLine(data);
                }

                sw.Flush();
            }

            Console.WriteLine("Drawing Second Gradient");

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    r = 255 << 16;
                    g = (i * 2) << 8;
                    b = (j * 2) << 0;

                    data = r | g | b;

                    sw.WriteLine(data);
                }

                sw.Flush();
            }

            Console.WriteLine("DONE!!");

            sw.Close();
        }
    }
}
