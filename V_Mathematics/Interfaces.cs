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
    /// This interface treats a class of objects as a Metric Space. It exposes
    /// a notion of 'distance' which can be measured between any two object of
    /// the same class. This distance mesurement also induces a Norm for all
    /// objects of the same class. In this way a Norm can be thought of as the
    /// distance from some fixed null point. Distances are always a positive
    /// real number, and a distance of zero indicated that two objects are the same.
    /// </summary>
    /// <typeparam name="T">Type of Space</typeparam>
    /// <remarks>Last Update: 2014-10-08</remarks>
    public interface Metrizable<in T>
    {
        /// <summary>
        /// Determins the distance between this object and another
        /// object of the same type. Distances are always positive, 
        /// and are zero if, and only if, the objects are identical.
        /// </summary>
        /// <param name="other">The second object</param>
        /// <returns>The distance from the second object</returns>
        double Dist(T other);

        /// <summary>
        /// Determins the distance between this object and a null or
        /// zero-point object, inplicit in the same metric space.
        /// </summary>
        /// <returns>The magnitude of the object</returns>
        double Norm();
    }

    /// <summary>
    /// This interface asbtracts the notion of a Vector Space over a given Feild.
    /// It defines an adition operation, which must be both associative and
    /// comunitive, as well as invertable. It also includes scalar multiplication
    /// by elements of the associated feild, which must distribute with addition.
    /// Subtraction is defined simply as the inverse of adition.
    /// </summary>
    /// <typeparam name="V">Type of Vector Space</typeparam>
    /// <typeparam name="F">Feild of the Vector Space</typeparam>
    /// <remarks>Last Update: 2014-10-08</remarks>
    public interface Euclidean<V, in F> : Metrizable<V> where V : Euclidean<V, F>
    {
        /// <summary>
        /// Adds the opperand to the curent vector and returns the result.
        /// Adition must be both asociative and comunitive.
        /// </summary>
        /// <param name="opp">The opperand to add</param>
        /// <returns>The sum of the two vectors</returns>
        V Add(V other);

        /// <summary>
        /// Subtracts the opperand from the curent vector and returns the result.
        /// It is defined as being the inverse of adition.
        /// </summary>
        /// <param name="opp">The opperand to subtract</param>
        /// <returns>The diffrence of the two vectors</returns>
        V Sub(V other);

        /// <summary>
        /// Multiplys the current vector by some scalar value in the associated
        /// feild of the Vector Space. It must distribute with vector adition.
        /// </summary>
        /// <param name="scalar">The scalar multiple</param>
        /// <returns>The scaled vector</returns>
        V Mult(F scalar);
    }

    /// <summary>
    /// This interface abstracts the notion of an algbera over a feild. Algebras include
    /// the common mathmatic operations of addition, subtraction, and multiplication,
    /// as well as scalar multiplication by some feild. An Algebra can be thought of as
    /// a Vector Space with an aditional multiplication operator. Alternativly, they can
    /// aslo be thought of as a Ring with the adition of scalar multiplicaiton. In this
    /// library, it is the former definition that is implied.
    /// </summary>
    /// <typeparam name="A">Type of Algebra</typeparam>
    /// <typeparam name="F">Feild of the given Algebra</typeparam>
    /// <remarks>Last Update: 2016-10-26</remarks>
    public interface Algebraic<A, in F> : Euclidean<A, F> where A : Algebraic<A, F>
    {
        /// <summary>
        /// Multiplys the current object by the opperand and returns the result.
        /// Multiplication must be asociative, but not nessarly comunitive. It
        /// must also distribute over adition.
        /// </summary>
        /// <param name="opp">The opperand used to mutiply</param>
        /// <returns>The product of the two objects</returns>
        A Mult(A opp);
    }
}
