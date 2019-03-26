/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
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
