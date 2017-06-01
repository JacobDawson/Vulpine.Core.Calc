/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

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
