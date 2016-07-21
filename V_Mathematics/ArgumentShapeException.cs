using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// This exeption is thrown when an argument to a funciton dose not have the correct
    /// "shape". For vectors, this implies the length of the vector, for matrices it
    /// inplices the width and the height of the matrix. This can be generalised to
    /// higher dimentional data structors, and can be used for any data structors used
    /// as arguments, not just vectors and matricies.
    /// </summary>
    public class ArgumentShapeException : ArgumentException
    {
        //default message used to explain the problem
        private const String MSG = 
            "The paramater {0} is of invalid shape for the current opperation.";

        /// <summary>
        /// Creates a new exception using the default message.
        /// </summary>
        public ArgumentShapeException() : base() { }

        /// <summary>
        /// Creates a new exception using the default message and custom paramater lable.
        /// </summary>
        /// <param name="param">Custom paramater label</param>
        public ArgumentShapeException(string param)
            : base(String.Format(MSG, param), param) { }

        /// <summary>
        /// Creates a new exception using a custom message and paramater lable.
        /// </summary>
        /// <param name="param">Custom paramater label</param>
        /// <param name="msg">Custome message</param>
        public ArgumentShapeException(string param, string msg)
            : base(msg, param) { }
    }
}
