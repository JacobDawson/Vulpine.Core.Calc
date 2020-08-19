using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickTests
{
    public static class BayerMatrixTest
    {

        public static void Run()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    byte data = GetElement(i, k);
                    Console.Write("{0:00} ", data);
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    int index = i * 8 + k;
                    byte data = byer[index];
                    Console.Write("{0:00} ", data);
                }

                Console.WriteLine();
            }

            Console.ReadKey(true);

            Console.Clear();

            for (int i = 0; i < 16; i++)
            {
                for (int k = 0; k < 16; k++)
                {
                    byte data = GetElement16(i, k);
                    Console.Write("{0:000} ", data);
                }

                Console.WriteLine();
            }

            Console.ReadKey(true);
        }


        private static byte GetElement(int i, int k)
        {
            bool b0 = (i & 0x01) != 0;
            bool b1 = (k & 0x01) != 0;
            bool b2 = (i & 0x02) != 0;
            bool b3 = (k & 0x02) != 0;
            bool b4 = (i & 0x04) != 0;
            bool b5 = (k & 0x04) != 0;

            b0 = b1 ^ b0;
            b2 = b3 ^ b2;
            b4 = b5 ^ b4;

            byte data = 0;

            if (b5) data |= 0x01;
            if (b4) data |= 0x02;
            if (b3) data |= 0x04;
            if (b2) data |= 0x08;
            if (b1) data |= 0x10;
            if (b0) data |= 0x20;

            return data;

        }

        private static readonly byte[] byer = 
        {
            00, 48, 12, 60, 03, 51, 15, 63,
            32, 16, 44, 28, 35, 19, 47, 31,
            08, 56, 04, 52, 11, 59, 07, 55,
            40, 24, 36, 20, 43, 27, 39, 23,
            02, 50, 14, 62, 01, 49, 13, 61,
            34, 18, 46, 30, 33, 17, 45, 29,
            10, 58, 06, 54, 09, 57, 05, 53,
            42, 26, 38, 22, 41, 25, 37, 21,
        };


        private static byte GetElement16(int i, int k)
        {
            bool b0 = (i & 0x01) != 0;
            bool b1 = (k & 0x01) != 0;
            bool b2 = (i & 0x02) != 0;
            bool b3 = (k & 0x02) != 0;
            bool b4 = (i & 0x04) != 0;
            bool b5 = (k & 0x04) != 0;
            bool b6 = (i & 0x08) != 0;
            bool b7 = (k & 0x08) != 0;

            b0 = b1 ^ b0;
            b2 = b3 ^ b2;
            b4 = b5 ^ b4;
            b6 = b7 ^ b6;

            byte data = 0;

            if (b7) data |= 0x01;
            if (b6) data |= 0x02;
            if (b5) data |= 0x04;
            if (b4) data |= 0x08;
            if (b3) data |= 0x10;
            if (b2) data |= 0x20;
            if (b1) data |= 0x40;
            if (b0) data |= 0x80;

            return data;

        }

    }
}
