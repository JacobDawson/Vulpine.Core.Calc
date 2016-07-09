using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
