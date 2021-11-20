/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2020 Benjamin Jacob Dawson
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

namespace Vulpine.Core.Calc.Geometry.Planer
{
    /// <summary>
    /// This class represents arbitrary transfomations of 2D objects. It suports
    /// both affine and projective transformations, basiclay anything that can
    /// be represented by an augmented matrix. It also offers factory methods for
    /// creating several core transformations, from which more complex transformations
    /// can be built by combining them together. It is more usefull than using
    /// matrices to represent the transfomations directly, as it asbtracts some
    /// of the more fuzzy details.
    /// </summary>
    public class Trans2D
    {
        //IDEA: Add inverse transformations, and consider overloading the
        //subtration (-) and division (/) operators.

        #region Class Definitions...

        //stores the internal transformation matrix
        private Matrix trans;

        /// <summary>
        /// Private constructor for creating 2D transfomations. It dose
        /// not copy the matrix, nor dose it check for valid dimentions.
        /// </summary>
        /// <param name="trans">Tranfomation matrix</param>
        private Trans2D(Matrix trans)
        {
            this.trans = trans;
        }

        /// <summary>
        /// Private constructor for creating 2D transformations from an
        /// array of floating point vlaues. It dose not check the size
        /// of the array.
        /// </summary>
        /// <param name="trans">Tranfomation matrix</param>
        private Trans2D(params double[] trans)
        {
            this.trans = new Matrix(3, 3, trans);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Core Opperations...

        /// <summary>
        /// Applies the curent transformation to the given point.
        /// </summary>
        /// <param name="p">Point to transform</param>
        /// <returns>The transformed point</returns>
        public Point2D Transform(Point2D p)
        {
            //calls the method below
            return Transform(p.X, p.Y);
        }

        /// <summary>
        /// Applies the curent transformation to the given cordinents.
        /// </summary>
        /// <param name="x">X cordinate of point to transform</param>
        /// <param name="y">Y cordinate of point to transform</param>
        /// <returns>The transformed point</returns>
        public Point2D Transform(double x, double y)
        {
            //extends the point vector and applies the transform
            Vector v = new Vector(x, y, 1.0);
            v = trans.Mult(v);

            //determins if this is a point at infinity
            if (v[2].IsZero())
            {
                double inf = Double.PositiveInfinity;
                return new Point2D(inf, inf);
            }

            //projects down from homogenious space
            double xh = v[0] / v[2];
            double yh = v[1] / v[2];

            //returns the transfomred point
            return new Point2D(xh, yh);
        }

        public Point2D InvertPoint(double x, double y)
        {
            //extends the point vector and applies the inverse transform
            Vector v = new Vector(x, y, 1.0);
            v = trans.InvAx(v);

            //determins if this is a point at infinity
            if (v[2].IsZero())
            {
                double inf = Double.PositiveInfinity;
                return new Point2D(inf, inf);
            }

            //projects down from homogenious space
            double xh = v[0] / v[2];
            double yh = v[1] / v[2];

            //returns the transfomred point
            return new Point2D(xh, yh);
        }

        /// <summary>
        /// Constructs a new transformation that is equivlant to preforming
        /// the current transformation and THEN the second trasnformation. 
        /// </summary>
        /// <param name="other">Transformation to append</param>
        /// <returns>The agrigate transformation</returns>
        public Trans2D Append(Trans2D other)
        {
            Matrix tnew = other.trans * this.trans;
            return new Trans2D(tnew);
        }

        /// <summary>
        /// Constructs a new transformation that is equilvant to preforming
        /// the second transformation BEFORE applying the curent transformation.
        /// </summary>
        /// <param name="other">Transformation to prepend</param>
        /// <returns>The agrigate transformation</returns>
        public Trans2D Prepend(Trans2D other)
        {
            Matrix tnew = this.trans * other.trans;
            return new Trans2D(tnew);
        }

        /// <summary>
        /// Constructs a new transformation that is the inverse of the
        /// current transformation. This is an expensive operation, so
        /// it should be used sparingly.
        /// </summary>
        /// <returns>The inverse transformation</returns>
        public Trans2D Invert()
        {
            Matrix inv = Matrix.Invert(trans);
            return new Trans2D(inv);
        }

        /// <summary>
        /// Obtains a copy of the matrix representing this transformation.
        /// </summary>
        /// <returns>The transfomarion in matrix form</returns>
        public Matrix GetMatrix()
        {
            //returns a copy of the transformation matrix
            return new Matrix(trans);
        }
       
        #endregion //////////////////////////////////////////////////////////

        #region Single Transformations...

        /// <summary>
        /// Represents the identity transformation, which dose nothing to
        /// any given point. It is a simple pass-through function.
        /// </summary>
        public static Trans2D Identity
        {
            get
            {
                Matrix ident = Matrix.Ident(3);
                return new Trans2D(ident);
            }
        }

        /// <summary>
        /// Reflects objects across the vertical Y-Axis, 
        /// like a mirror hanging on a wall.
        /// </summary>
        public static Trans2D ReflectVert
        {
            get
            {
                Matrix mirror = Matrix.Ident(3);
                mirror[0, 0] = -1.0;
                return new Trans2D(mirror);
            }
        }

        /// <summary>
        /// Reflects objects scross the horizontal X-Axis, 
        /// like the surface of a pond or lake.
        /// </summary>
        public static Trans2D ReflectHorz
        {
            get
            {
                Matrix mirror = Matrix.Ident(3);
                mirror[1, 1] = -1.0;
                return new Trans2D(mirror);
            }
        }
        
        /// <summary>
        /// Rotates objects 90 degrees to the left, about the origin.
        /// </summary>
        public static Trans2D RotateLeft
        {
            get
            {
                return new Trans2D(
                    +0.0, +1.0, +0.0,
                    -1.0, +0.0, +0.0,
                    +0.0, +0.0, +1.0
                );
            }
        }

        /// <summary>
        /// Rotates objects 90 degrees to the right, about the origin.
        /// </summary>
        public static Trans2D RotateRight
        {
            get
            {
                return new Trans2D(
                    +0.0, -1.0, +0.0,
                    +1.0, +0.0, +0.0,
                    +0.0, +0.0, +1.0
                );
            }
        }

        /// <summary>
        /// Inverts objects by rotating them 180 degrees about the orign,
        /// or equevently, by reflecting across both the x and y asies 
        /// simultaniously.
        /// </summary>
        public static Trans2D RotateHalf
        {
            get
            {
                return new Trans2D(
                    -1.0, +0.0, +0.0,
                    +0.0, -1.0, +0.0,
                    +0.0, +0.0, +1.0
                );
            }
        }

        #endregion //////////////////////////////////////////////////////////

        #region Affine Transformations...

        /// <summary>
        /// Moves objects by the amount indicated by the translation
        /// vector, given as a 2D point.
        /// </summary>
        /// <param name="p">Translation vector</param>
        /// <returns>The translating transformation</returns>
        public static Trans2D Translate(Point2D p)
        {
            //calls the method below
            return Translate(p.X, p.Y);
        }

        /// <summary>
        /// Moves objects by the amount indicated by the translation
        /// vector, given by the amount to move in X and Y.
        /// </summary>
        /// <param name="x">Amount to move in X</param>
        /// <param name="y">Amount to move in Y</param>
        /// <returns>The translating transformation</returns>
        public static Trans2D Translate(double x, double y)
        {
            //generates the translation matrix
            Matrix m = Matrix.Ident(3);
            m[0, 2] = x;
            m[1, 2] = y;

            //returns the new transform
            return new Trans2D(m);
        }

        /// <summary>
        /// Scales objects by a given amount, centred on the origin.
        /// </summary>
        /// <param name="s">Scaling amount</param>
        /// <returns>The scaling transformation</returns>
        public static Trans2D Scale(double s)
        {
            //calls the method below
            return Scale(s, s);
        }

        /// <summary>
        /// Scales objects by a unique amount, in the X and Y directions,
        /// indicated by a 2D point.
        /// </summary>
        /// <param name="p">Scaling amount in X and Y</param>
        /// <returns>The scaling transformation</returns>
        public static Trans2D Scale(Point2D p)
        {
            //calls the method below
            return Scale(p.X, p.Y);
        }

        /// <summary>
        /// Scales objects by a unitue amount, in the X and Y directions,
        /// given by a pair of floating point values.
        /// </summary>
        /// <param name="x">Scaling amount in X</param>
        /// <param name="y">Scaling amount in Y</param>
        /// <returns>The scaling transformation</returns>
        public static Trans2D Scale(double x, double y)
        {
            //generates the scaling matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = x;
            m[1, 1] = y;

            //returns the new transform
            return new Trans2D(m);
        }

        /// <summary>
        /// Rotates objects counter-clockwise around the origin, assuming the 
        /// y-axis points up. In systems where the y-axis points down, it rotates 
        /// clockwise. The angle of rotation is given in radiens.
        /// </summary>
        /// <param name="theta">Angle of rotation</param>
        /// <returns>The rotating transformation</returns>
        public static Trans2D Rotate(double theta)
        {
            //extracts the sin and cos
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            //generates the rotation matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = cos;
            m[0, 1] = -sin;
            m[1, 0] = sin;
            m[1, 1] = cos;

            //returns the new transform
            return new Trans2D(m);
        }

        /// <summary>
        /// Reflects objects across an arbitrary line that passes through 
        /// the origin. The angle given mesures the displacment from the x-axis, 
        /// mesured in radians.
        /// </summary>
        /// <param name="theta">Angle of reflection</param>
        /// <returns>The reflecting transformation</returns>
        public static Trans2D Reflect(double theta)
        {
            //extracts the sin and cos
            double x = Math.Cos(theta);
            double y = Math.Sin(theta);

            //intermediate calculations
            double xx = x * x;
            double yy = y * y;
            double xy = 2.0 * x * y;

            //generates the reflection matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = xx - yy;
            m[0, 1] = xy;
            m[1, 0] = xy;
            m[1, 1] = yy - xx;

            //returns the new transform
            return new Trans2D(m);
        }
        
        /// <summary>
        /// Creates a shearing transformation, causing points to be moved x
        /// units along the the x axis, and y units along the y axis, for each
        /// unit moved along the respective axis.
        /// </summary>
        /// <param name="p">Point containing the amount of sheer in X and Y</param>
        /// <returns>The shearing transform</returns>
        public static Trans2D Shear(Point2D p)
        {
            //calls the method below
            return Shear(p.X, p.Y);
        }

        /// <summary>
        /// Creates a shearing transformation, causing points to be moved x
        /// units along the the x axis, and y units along the y axis, for each
        /// unit moved along the respective axis.
        /// </summary>
        /// <param name="x">Shear in X</param>
        /// <param name="y">Shear in Y</param>
        /// <returns>The shearing transform</returns>
        public static Trans2D Shear(double x, double y)
        {
            //generates the shearing matrix
            Matrix m = Matrix.Ident(3);
            m[0, 1] = x;
            m[1, 0] = y;

            //returns the new transform
            return new Trans2D(m);
        }

        /// <summary>
        /// Creates a shearing transformation, causing points to be moved x
        /// units along the the x axis for each unit along the y axis.
        /// </summary>
        /// <param name="amount">Shearing factor</param>
        /// <returns>The shearing transform</returns>
        public static Trans2D ShearHorz(double amount)
        {
            //calls the base method
            return Shear(amount, 0.0);
        }

        /// <summary>
        /// Appends a shearing transformation, causing points to be moved y
        /// units along the the y axis for each unit along the x axis.
        /// </summary>
        /// <param name="amount">Shearing factor</param>
        /// <returns>The shearing transform</returns>
        public static Trans2D ShearVert(double amount)
        {
            //calls the base method
            return Shear(0.0, amount);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Other Transformations...

        /// <summary>
        /// Creates a 2D transform from an arbitrary matrix. Note that only 3x3
        /// matricies are allowed. This is because we use homodinous cordinates
        /// to encode both affine and projective transformations.
        /// </summary>
        /// <param name="m">Transformation matrix</param>
        /// <returns>The corisponding transformation</returns>
        public static Trans2D FromMatrix(Matrix m)
        {
            //we only accept matrices that are 3x3
            if (m.NumRows != 3 || m.NumColumns != 3) throw new
                ArgumentShapeException("m");

            //makes a copy of the input matrix
            Matrix copy = new Matrix(m);
            return new Trans2D(copy);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Class Conversions...

        public static implicit operator Matrix(Trans2D t)
        {
            return t.GetMatrix();
        }

        public static explicit operator Trans2D(Matrix m)
        {
            return Trans2D.FromMatrix(m);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Operator Overloads...

        public static Trans2D operator +(Trans2D a, Trans2D b)
        {
            return a.Append(b);        
        }

        public static Point2D operator *(Trans2D t, Point2D p)
        {
            return t.Transform(p);
        }

        #endregion //////////////////////////////////////////////////////////

    }
}
