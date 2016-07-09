using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// This interface represents a class of function-like mathmatical objects,
    /// which can be evaluated on some arbitary feild or ring, efectivly mapping
    /// objects from one set to other objects of the same set. Conceptualy it 
    /// differs from the funciton delegate in that represents a class of functions, 
    /// rather than a single funciton. Programaticly it differs in that it is an 
    /// interface, and not a delegate.
    /// </summary>
    /// <typeparam name="F">The chosen feild</typeparam>
    /// <remarks>Last Update: 2013-09-16</remarks>
    public interface Function<F>
    {
        /// <summary>
        /// Evaluates the current function at the given value.
        /// </summary>
        /// <param name="x">The input paramater</param>
        /// <returns>The evaluation of the funciton</returns>
        F Evaluate(F x);
    }

    /// <summary>
    /// A dirivable function is one where the derivitive can be evaluated directly,
    /// rather than aproximated by some numerical method. By definition, all derivable
    /// objects are also evaluable objects.
    /// </summary>
    /// <typeparam name="F">The chosen feild</typeparam>
    /// <remarks>Last Update: 2013-09-16</remarks>
    public interface Differentiable<F> : Function<F>
    {
        /// <summary>
        /// Evaluates the dirivitive of the current function, by using an
        /// exact method, rather than numerical aproximation.
        /// </summary>
        /// <param name="x">The input paramater</param>
        /// <returns>The evaluation of the derivitive</returns>
        F EvalDx(F x);
    }
}
