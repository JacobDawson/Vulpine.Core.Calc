using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc
{
    public class ArgumentShapeException : ArgumentException
    {
        private const String MSG = 
            "The paramater {0} is of invalid dimention for the current opperation.";

        public ArgumentShapeException() : base() { }

        public ArgumentShapeException(string param)
            : base(String.Format(MSG, param), param) { }
    }
}
