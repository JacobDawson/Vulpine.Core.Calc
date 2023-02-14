/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2023 Benjamin Jacob Dawson
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

namespace Vulpine.Core.Calc.Numbers
{
    /// <summary>
    /// Dual numbers are numbers of the form (a + be) where a and b are real values, and
    /// (e) is defined such that e^2 = 0. Thay are analogus to the complex numbers, except 
    /// that the "imaginary" component is defined diffrently. They can, in a sence, be
    /// though of as encoding infentessimal values, as (e) can be considered an infintessimal.
    /// There primary pourpus though is to aid in automatic diffenencation, as the (e) 
    /// component encodes information about the derivitive.
    /// </summary>
    public struct Dual
    {


    }
}
