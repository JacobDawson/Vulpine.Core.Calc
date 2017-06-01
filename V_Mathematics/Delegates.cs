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
