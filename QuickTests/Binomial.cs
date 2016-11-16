using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;

namespace QuickTests
{
    public static class Binomial
    {
        public static void Run()
        {
            for (int n = 1; n <= 92; n++)
            {





                for (int k = 0; k <= n; k++)
                {
                    long x1 = LongBinomial(n, k);
                    double t = VMath.Binomial((double)n, k);
                    long x2 = (long)Math.Floor(t + 0.5);

                    Console.WriteLine("({0} : {1}) = {2}", n, k, x1);

                    if (x1 < 0)
                    {
                        continue;
                    }

                    if (x1 != x2)
                    {
                        Console.WriteLine("Gamma Function yeilds {0}", t);                      
                        //Console.ReadKey(true);
                    }


                }
            }
        }

        //Maximum N = 33
        public static int IntBinomial(int n, int k)
        {
            //takes advantage of symatry
            k = Math.Min(k, n - k);

            //takes care of the simple cases
            if (k < 0 || k > n) return 0;
            if (n <= 1 || k == 0) return 1;

            int c = 1;
            int m = n;

            for (int i = 1; i <= k; i++)
            {
                if (c / i > Int32.MaxValue / m) return -1;
                c = ((c / i) * m) + ((c % i) * m) / i;
                m = m - 1;
            }

            return c;
        }

        //Maximum N = 66
        public static long LongBinomial(long n, long k)
        {
            //takes advantage of symatry
            k = Math.Min(k, n - k);

            //takes care of the simple cases
            if (k < 0 || k > n) return 0;
            if (n <= 1 || k == 0) return 1;

            long c = 1;
            long m = n;

            for (long i = 1; i <= k; i++)
            {
                if (c / i > Int64.MaxValue / m) return -1;
                c = ((c / i) * m) + ((c % i) * m) / i;
                m = m - 1;
            }

            return c;
        }
    }
}
