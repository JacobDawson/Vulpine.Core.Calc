/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
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
    /// Contains extention methods for the IFormattable interface.
    /// </summary>
    /// <remarks>Last Update: 2016-11-14</remarks>
    public static class FormatExtentions
    {
        /// <summary>
        /// Provides a short-hand method for formating objects that implement the
        /// IFormatable interface, when the system-default format provider is required,
        /// or when the format provider is not used by the underlying implementation.
        /// </summary>
        /// <param name="obj">Object to be formated</param>
        /// <param name="format">A format defining string</param>
        /// <returns>A formated representation of the object as a string</returns>
        public static string ToString(this IFormattable obj, string format)
        {
            return obj.ToString(format, null);
        }


    }
}
