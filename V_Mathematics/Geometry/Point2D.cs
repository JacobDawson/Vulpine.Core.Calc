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

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Geometry
{
    /// <summary>
    /// This structor represents a unique point in the cartisian cordinate plane. 
    /// Mathmaticly it is identical to a vector of length two, but it is implemented
    /// as a value type and is therefore immutable. It also includes methods pertanent
    /// to two dimentional points, sutch as the calculation of slope. It can also
    /// convert to and from polar cordinates by a simple formula.
    /// </summary>
    /// <remarks>Last Update: 2013-09-16</remarks>
    public struct Point2D : Euclidean<Point2D, Double>, IComparable<Point2D>
    {
        #region Class Deffinitions...

        //stores the x and y values
        private double x;
        private double y;

        /// <summary>
        /// Constructs a new point in cartisian cordnats.
        /// </summary>
        /// <param name="x">The position along the x axis</param>
        /// <param name="y">The position along the y axis</param>
        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructs a new point from a vector of values.
        /// </summary>
        /// <param name="point">Vector to convert</param>
        public Point2D(Vector point)
        {
            x = point.GetElement(0);
            y = point.GetElement(1);
        }

        /// <summary>
        /// Generates a string representation of the current point.
        /// </summary>
        /// <returns>The point as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("0.000");
        }

        /// <summary>
        /// Generates a string representation of the current point, using
        /// a custom format. A seperate format can be provided for both
        /// the X and Y values, by seperating them by a virtical pipe.
        /// </summary>
        /// <param name="format">A custom format string</param>
        /// <returns>The point as a string</returns>
        public string ToString(string format)
        {
            //splits the format string around the comma
            string[] f = format.Split('|');
            string xs = (f.Length < 2) ? format : f[0];
            string ys = (f.Length < 2) ? format : f[1];

            //formats the diffrent parts seperatly
            xs = x.ToString(xs);
            ys = y.ToString(ys);

            //builds and returns the output
            return String.Format("({0}, {1})", xs, ys);
        }

        /// <summary>
        /// Compares the curent point to another given point. This is
        /// used to list the points in order of their indpendent value.
        /// </summary>
        /// <param name="other">A point to compare</param>
        /// <returns>Negative if the current is less, positive if the
        /// current is more, and zero if both are equal</returns>
        public int CompareTo(Point2D other)
        {
            //compares first by the independent variable
            int test = x.CompareTo(other.x);

            //uses the dependent variable if it is nessary
            return (test == 0) ? y.CompareTo(other.y) : test;
        }

        /// <summary>
        /// Determins if this point is equal to another point.
        /// </summary>
        /// <param name="obj">Point to compare</param>
        /// <returns>True if the points are equal</returns>
        public override bool Equals(object obj)
        {
            //makes shure the other object is a point
            if (!(obj is Point2D)) return false;

            //converts the object and compares
            var temp = (Point2D)obj;
            return (CompareTo(temp) == 0);
        }

        /// <summary>
        /// Generates a sudo-unique value pertaning to the
        /// current cartisian point.
        /// </summary>
        /// <returns>The hash of the curent point</returns>
        public override int GetHashCode()
        {
            //uses exclusive-or for the hash
            int xh = x.GetHashCode();
            int yh = y.GetHashCode();
            return (xh ^ yh);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Constructs a new point in polar cordinats, where theta is
        /// mesured in radians from the positive x axis. Note that a
        /// negitive radius reflects the point about the origin.
        /// </summary>
        /// <param name="theta">Distance in radians from the x axis</param>
        /// <param name="r">Radius mesured from the origin</param>
        /// <returns>The cartisian equivlent of the polar cordinats</returns>
        public static Point2D FromPolar(double theta, double r = 1.0)
        {
            double x = Math.Cos(theta) * r;
            double y = Math.Sin(theta) * r;
            return new Point2D(x, y);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Property Deffinitions...

        /// <summary>
        /// The distance along the X axis. It is also considered the
        /// independent value for functional analisis. Read-Only
        /// </summary>
        public double X
        {
            get { return x; }
        }

        /// <summary>
        /// The distance along the Y axis. It is also considered the
        /// dependent value for functional analisis. Read-Only
        /// </summary>
        public double Y
        {
            get { return y; }
        }


        /// <summary>
        /// The radius, or distance from the origin in polar
        /// cordinats. It is also the magnitude of the vector.
        /// Read-Only
        /// </summary>
        public double Radius
        {
            get
            {
                double rad = (x * x) + (y * y);
                return Math.Sqrt(rad);
            }
        }

        /// <summary>
        /// The polar cordinat angle, in radians, mesured from
        /// the positive X axis. Output ranges from just above
        /// negative PI to PI. Read-Only
        /// </summary>
        public double Theta
        {
            get { return Math.Atan2(y, x); }
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Point Operations...

        /// <summary>
        /// Sums the curent point with another point and generates
        /// a new point. It overloads the (+) opperator.
        /// </summary>
        /// <param name="b">The point to add</param>
        /// <returns>The sum of the two points</returns>
        public Point2D Add(Point2D b)
        {
            //adds the two parts seperatly
            double xr = x + b.x;
            double yr = y + b.y;

            //returns the resultant
            return new Point2D(xr, yr);
        }

        /// <summary>
        /// Subtracts a given point from the current and generates 
        /// a new point. It overloads the (-) opperator.
        /// </summary>
        /// <param name="b">The point to subtract</param>
        /// <returns>The difrance of the two points</returns>
        public Point2D Sub(Point2D b)
        {
            //subtracts the two parts seperatly
            double xr = x - b.x;
            double yr = y - b.y;

            //returns the resultant
            return new Point2D(xr, yr);
        }

        /// <summary>
        /// Computes the dot procuct of the current point and a second
        /// point, resulting in a single scalar value. It overloads
        /// the (*) opperator.
        /// </summary>
        /// <param name="b">The second point</param>
        /// <returns>The dot product of the two points</returns>
        public double Mult(Point2D b)
        {
            //returns the sum of the products
            return (x * b.x) + (y * b.y);
        }

        /// <summary>
        /// Multiplies the current point by a scalar value. It is the
        /// equivlent to increasing the distance from the origin by
        /// the same ammount. Overloads the (*) and (/) opperators.
        /// </summary>
        /// <param name="s">The scalar multiple</param>
        /// <returns>The product of this point and a scalar</returns>
        public Point2D Mult(double s)
        {
            //subtracts the two parts seperatly
            double xr = x * s;
            double yr = y * s;

            //returns the resultant
            return new Point2D(xr, yr);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Metric Implementation ...

        /// <summary>
        /// Obtains the Norm of the current point. It is the same
        /// as the radius in polar coridnates.
        /// </summary>
        /// <returns>The magnitude of the point</returns>
        public double Norm()
        {
            //computes the sum of the squares
            double rad = (y * y) + (x * x);
            return Math.Sqrt(rad);
        }

        /// <summary>
        /// Computes the distance between this point and another point.
        /// </summary>
        /// <param name="other">The other point</param>
        /// <returns>The distance from the other point</returns>
        public double Dist(Point2D other)
        {
            //calls upon the static method
            return Point2D.Dist(this, other);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Special Methods...

        /// <summary>
        /// Inverts the X and Y axis, making the independent variable
        /// dependent, and the dependent variable independent.
        /// </summary>
        /// <param name="p">The point to invert</param>
        /// <returns>The inverted point</returns>
        public static Point2D Invert(Point2D p)
        {
            //swaps the x and y cordinats
            return new Point2D(p.y, p.x);
        }

        /// <summary>
        /// Normalises the point to have a radius of one, placing it
        /// on the unit circle while maintaining direction. Overloads
        /// the (~) opperator.
        /// </summary>
        /// <param name="p">The point to normalise</param>
        /// <returns>The normalised point</returns>
        public static Point2D Unit(Point2D p)
        {
            //computes the magnitude of the vector
            double div = (p.x * p.x) + (p.y * p.y);
            div = 1.0 / Math.Sqrt(div);

            //preforms the normalizing divide
            double xr = p.x * div;
            double yr = p.y * div;

            //returns the normalised vector
            return new Point2D(xr, yr);
        }

        /// <summary>
        /// Computes the distance between two points.
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns>The distance between the points</returns>
        public static double Dist(Point2D a, Point2D b)
        {
            //subtracts the two parts seperatly
            double xr = b.x - a.x;
            double yr = b.y - a.y;

            //computes the sum of the squares
            return Math.Sqrt((xr * xr) + (yr * yr));
        }

        /// <summary>
        /// Computes the slope of the line connecting the two points.
        /// The slope is calculated as the diifence in Y (rise) over 
        /// the diffrence in X (run).
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns>Slope of the line connecting the points</returns>
        public static double Slope(Point2D a, Point2D b)
        {
            //subtracts the two parts seperatly
            double xr = b.x - a.x;
            double yr = b.y - a.y;

            //computes the rise over the run
            return (yr / xr);
        }

        /// <summary>
        /// Computes the point that lies halfway between two given points. 
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns>The midpoint between the two points</returns>
        public static Point2D MidPoint(Point2D a, Point2D b)
        {
            //computes the two parts seperatly
            double xr = (b.x + a.x) / 2.0;
            double yr = (b.y + a.y) / 2.0;

            //returns the midpoint
            return new Point2D(xr, yr);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Class Conversions...

        //creates a vector containing the listed cordinates
        public static implicit operator Vector(Point2D p)
        { return new Vector(p.x, p.y); }

        //converts the vector by calling the constructor
        public static explicit operator Point2D(Vector v)
        { return new Point2D(v); }

        #endregion //////////////////////////////////////////////////////////////

        #region Opperator Overlodes...

        //refferences the Add(p) function
        public static Point2D operator +(Point2D a, Point2D b)
        { return a.Add(b); }

        //references the Sub(p) function
        public static Point2D operator -(Point2D a, Point2D b)
        { return a.Sub(b); }

        //references the Mult(p) function
        public static Double operator *(Point2D a, Point2D b)
        { return a.Mult(b); }

        //references the Mult(s) function
        public static Point2D operator *(Point2D a, Double s)
        { return a.Mult(s); }

        //references the Mult(s) function
        public static Point2D operator *(Double s, Point2D a)
        { return a.Mult(s); }

        //references the Mult(s) function
        public static Point2D operator /(Point2D v, Double s)
        { return v.Mult(1.0 / s); }

        //refrences the Mult(-1.0) function
        public static Point2D operator -(Point2D a)
        { return a.Mult(-1.0); }

        //refrences the Unit(p) function
        public static Point2D operator ~(Point2D a)
        { return Point2D.Unit(a); }

        #endregion ////////////////////////////////////////////////////////////
    }
}
