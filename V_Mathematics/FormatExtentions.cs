/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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
