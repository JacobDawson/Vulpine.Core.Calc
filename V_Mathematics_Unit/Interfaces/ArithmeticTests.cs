using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;

namespace CVL_Mathematics_Test.Interfaces
{
    public static class AlgebraicTests
    {
        public static void AddAsso<T>(T a, T b) where T : Algebraic<T>
        {
            T c = a.Add(b);
            T d = b.Add(a);          
        }
    }
}
