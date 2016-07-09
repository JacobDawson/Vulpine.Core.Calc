using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// This interface describes an algebraic sturctor with the common opperators of 
    /// adition, subtraction, and multiplication, but not division. In abstract terms 
    /// it forms a Ring. Adition must be asociative, comunitive, and invertable; while
    /// multiplication need only be asociative. It is also required that multiplication
    /// distributs over adition. There also exist two implicit elements in every Ring,
    /// the aditive identity and the multiplicitive identity.
    /// </summary>
    /// <typeparam name="T">Type of Ring</typeparam>
    /// <remarks>Last Update: 2014-10-08</remarks>
    public interface Algebraic<T> where T : Algebraic<T>
    {
        /// <summary>
        /// Adds the opperand to the curent object and returns the result.
        /// Adition must be both asociative and comunitive.
        /// </summary>
        /// <param name="opp">The opperand to add</param>
        /// <returns>The sum of the two objects</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the addition</exception>
        T Add(T opp);

        /// <summary>
        /// Subtracts the opperand from the curent object and returns the result.
        /// It is defined as being the inverse of adition.
        /// </summary>
        /// <param name="opp">The opperand to subtract</param>
        /// <returns>The diffrence of the two objects</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the subtraction</exception>
        T Sub(T opp);

        /// <summary>
        /// Multiplys the current object by the opperand and returns the result.
        /// Multiplication must be asociative, but not nessarly comunitive. It
        /// must also distribute over adition.
        /// </summary>
        /// <param name="opp">The opperand used to mutiply</param>
        /// <returns>The product of the two objects</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the multiplication</exception>
        T Mult(T opp);
    }

    /// <summary>
    /// This interface treats a class of objects as a Metric Space. It exposes
    /// a notion of 'distance' which can be measured between any two object of
    /// the same class. It also provides a notion of magnitude, which can be
    /// thought of as the distance from some implictly defind zero-point object.
    /// Distances are always a positive real number, and a distance of zero indicates
    /// that the two objects are the same.
    /// </summary>
    /// <typeparam name="T">Type of Space</typeparam>
    /// <remarks>Last Update: 2014-10-08</remarks>
    public interface Metrizable<in T>
    {
        /// <summary>
        /// Determins the distance between this object and another
        /// metrizable object of the same type. Distances are always
        /// positive, and only zero if the objects are identical.
        /// </summary>
        /// <param name="other">The second object</param>
        /// <returns>The distance from the second object</returns>
        double Dist(T other);

        /// <summary>
        /// Determins the distance between this object and a null or
        /// zero-point object, inplicit in the same metric space.
        /// </summary>
        /// <returns>The magnitude of the object</returns>
        double Mag();
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
    public interface Spacial<V, in F> where V : Spacial<V, F>
    {
        /// <summary>
        /// Adds the opperand to the curent vector and returns the result.
        /// Adition must be both asociative and comunitive.
        /// </summary>
        /// <param name="opp">The opperand to add</param>
        /// <returns>The sum of the two vectors</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the addition</exception>
        V Add(V other);

        /// <summary>
        /// Subtracts the opperand from the curent vector and returns the result.
        /// It is defined as being the inverse of adition.
        /// </summary>
        /// <param name="opp">The opperand to subtract</param>
        /// <returns>The diffrence of the two vectors</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the subtraction</exception>
        V Sub(V other);

        /// <summary>
        /// Multiplys the current vector by some scalar value in the associated
        /// feild of the Vector Space. It must distribute with vector adition.
        /// </summary>
        /// <param name="scalar">The scalar multiple</param>
        /// <returns>The scaled vector</returns>
        /// <exception cref="MathematicsExcp">If the underlying class is for
        /// some reason unable to preform the multiplication</exception>
        V Mult(F scalar);
    }

    /// <summary>
    /// Represents a class of objects that are both Vector Spaces and define
    /// a Metric. It is mostly provided as a convience, as the two interfaces
    /// are often pared together.
    /// </summary>
    /// <typeparam name="V">Type of Vector Space</typeparam>
    /// <typeparam name="F">Feild of the Vector Space</typeparam>
    public interface Euclidian<V, F> : Spacial<V, F>, Metrizable<V>
        where V : Euclidian<V, F> { }

}
