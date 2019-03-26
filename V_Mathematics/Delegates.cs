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

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// Represents a mathmatical function defined on the real numbers. That 
    /// is, any function which maps one real number to another.
    /// </summary>
    /// <param name="x">The input variable</param>
    /// <returns>Result of evaluating the function</returns>
    public delegate double VFunc(double x);

    /// <summary>
    /// Represents a mult-dimention function defined on the real numbers. That
    /// is, any funciton which maps an n-dimentional vector of numbers to a single
    /// real number. 
    /// </summary>
    /// <param name="x">The input variables</param>
    /// <returns>The result of evaluating the function</returns>
    public delegate double MFunc(Vector x);

    /// <summary>
    /// Represents a mathmatical function defined on an arbitrary ring or 
    /// feild, mapping items to the same ring or feild.
    /// </summary>
    /// <typeparam name="T">The feild of the function</typeparam>
    /// <param name="x">The input variable</param>
    /// <returns>The result of evaluating the function</returns>
    public delegate T VFunc<T>(T x);

    /// <summary>
    /// Defines an explicit metric, or a distance measure, between any 
    /// two objects of a given metric space.
    /// </summary>
    /// <typeparam name="T">The space of the metric</typeparam>
    /// <param name="a">The first object</param>
    /// <param name="b">The second object</param>
    /// <returns>The distance between the two objects</returns>
    public delegate double Metric<in T>(T a, T b);
}
