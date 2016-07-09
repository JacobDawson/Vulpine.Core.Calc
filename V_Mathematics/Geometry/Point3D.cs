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
    /// This structor represents a unique point in cartisian cordinate space.
    /// Mathmaticly it is identical to a vector of length three, but it is implemented
    /// as a value type and is therefore immutable. It also include a method for
    /// calculating the cross product, unique to three-dimentional space. In adition,
    /// it provides methods to convert to and from spherical and cylindrical cordinates.
    /// </summary>
    /// <remarks>Last Update: 2013-09-16</remarks>
    public struct Point3D : Euclidian<Point3D, Double>, IComparable<Point3D>
    {
        #region Class Deffinitions...

        //stores the x, y, and z values
        private double x;
        private double y;
        private double z;

        /// <summary>
        /// Constructs a new point in cartisian cordinats.
        /// </summary>
        /// <param name="x">The position on the x axis</param>
        /// <param name="y">The position on the y axis</param>
        /// <param name="z">The position on the z axis</param>
        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructs a new point from a vector of values.
        /// </summary>
        /// <param name="point">Vector to convert</param>
        public Point3D(Vector point)
        {
            x = point.GetExtended(0);
            y = point.GetExtended(1);
            z = point.GetExtended(2);
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
            string xs = (f.Length < 3) ? format : f[0];
            string ys = (f.Length < 3) ? format : f[1];
            string zs = (f.Length < 3) ? format : f[2];

            //formats the diffrent parts seperatly
            xs = x.ToString(xs);
            ys = y.ToString(ys);

            //builds and returns the output
            return String.Format("({0}, {1}, {2})", xs, ys, zs);
        }

        /// <summary>
        /// Compares the curent point to another given point. This is
        /// used to list the points in order of there X cordinates.
        /// </summary>
        /// <param name="other">A point to compare</param>
        /// <returns>Negative if the current is less, positive if the
        /// current is more, and zero if both are equal</returns>
        public int CompareTo(Point3D other)
        {
            //compares first by the X cordinates
            int test = x.CompareTo(other.x);

            //compares second by the Y cordinates, if nessary
            if (test == 0) test = y.CompareTo(other.y);

            //finaly compares the Z cordinates, if nessary
            return (test == 0) ? z.CompareTo(other.z) : test;
        }

        /// <summary>
        /// Determins if this point is equal to another point.
        /// </summary>
        /// <param name="obj">Point to compare</param>
        /// <returns>True if the points are equal</returns>
        public override bool Equals(object obj)
        {
            //makes shure the other object is a point
            if (!(obj is Point3D)) return false;

            //converts the object and compares
            var temp = (Point3D)obj;
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
            int zh = z.GetHashCode();
            return (xh ^ yh ^ zh);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Constructs a new point from spherical cordinats, where the 
        /// inclination and azimuth are taken to have the same meaning
        /// as the class properties. 
        /// </summary>
        /// <param name="inc">The inclination, from XY</param>
        /// <param name="azm">The azimuth, from X in XY</param>
        /// <param name="r">The radius, or distance from the origin</param>
        /// <returns>The equivlent point in cartisian cordinats</returns>
        public static Point3D FromSphere
            (double inc, double azm, double r = 1.0)
        {
            //normalises the inclination to run from 0 to pi
            double a = inc + (Math.PI / 2);

            //obtains the cartisian cordinates
            double x = Math.Sin(a) * Math.Cos(azm) * r;
            double y = Math.Sin(a) * Math.Sin(azm) * r;
            double z = Math.Cos(a) * r;

            //returnst the ccartisian point
            return new Point3D(x, y, z);
        }

        /// <summary>
        /// Constructs a new point from cylindrical cordinats, where rho
        /// and the azimuth are taken to have the same meaning as the
        /// class properties.
        /// </summary>
        /// <param name="rho">The distance from the Z axis</param>
        /// <param name="azm">The azimuth, from X in XY</param>
        /// <param name="z">The Z-cordinate or height</param>
        /// <returns>The equivlent point in cartisian cordinats</returns>
        public static Point3D FromCylindrical
            (double rho, double azm, double z = 0.0)
        {
            //obtains the X and Y cordinates
            double x = rho * Math.Cos(azm);
            double y = rho * Math.Sin(azm);

            //returnst the ccartisian point
            return new Point3D(x, y, z);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The distance allong the X axis. Read-Only
        /// </summary>
        public double X
        {
            get { return x; }
        }

        /// <summary>
        /// The distance allong the Y axis. Read-Only
        /// </summary>
        public double Y
        {
            get { return y; }
        }

        /// <summary>
        /// The distance allong the Z axis. Read-Only
        /// </summary>
        public double Z
        {
            get { return z; }
        }

        /// <summary>
        /// The distance from the origin. Read-Only
        /// </summary>
        public double Radius
        {
            get
            {
                //sums the squares of X, Y and Z
                double rad = (x * x) + (y * y) + (z * z);
                return Math.Sqrt(rad);
            }
        }

        /// <summary>
        /// The angular distance, mesured in radians, from the positive
        /// X axis, releitive to the XY plane. Output ranges from just 
        /// above negative PI to PI. Read-Only.
        /// </summary>
        public double Azum 
        {
            get { return Math.Atan2(y, x); }
        }

        /// <summary>
        /// The angular distance, mesured in radians, between the positive 
        /// Z axis and the XY plane. Output ranges from negative half-PI to
        /// half-PI, inclusive. Read-Only.
        /// </summary>
        public double Incl
        {
            get
            {
                //must first compute the altitude
                double rad = (y * y) + (x * x) + (z * z);
                rad = Math.Sqrt(rad);

                //determins the latatiude on the sphere
                double lat = Math.Acos(z / rad);
                return lat - (Math.PI / 2);
            }
        }

        /// <summary>
        /// The shortest, euclidian distance from the Z axis. It is used
        /// allong with the Azimuth and Z-position to form a cylindrical
        /// cordinate system. Read-Only.
        /// </summary>
        public double Rho
        {
            get
            {
                //sums the squares of X and Y only
                double rho = (y * y) + (x * x);
                return Math.Sqrt(rho);
            }
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Point Operations...

        /// <summary>
        /// Sums the curent point with another point and generates
        /// a new point. It overloads the (+) opperator.
        /// </summary>
        /// <param name="b">The point to add</param>
        /// <returns>The sum of the two points</returns>
        public Point3D Add(Point3D b)
        {
            //adds the two parts seperatly
            double xr = x + b.x;
            double yr = y + b.y;
            double zr = z + b.z;

            //returns the resultant
            return new Point3D(xr, yr, zr);
        }

        /// <summary>
        /// Subtracts a given point from the current and generates 
        /// a new point. It overloads the (-) opperator.
        /// </summary>
        /// <param name="b">The point to subtract</param>
        /// <returns>The difrance of the two points</returns>
        public Point3D Sub(Point3D b)
        {
            //subtracts the two parts seperatly
            double xr = x - b.x;
            double yr = y - b.y;
            double zr = z - b.z;

            //returns the resultant
            return new Point3D(xr, yr, zr);
        }

        /// <summary>
        /// Computes the dot procuct of the current point and a second
        /// point, resulting in a single scalar value. It overloads
        /// the (*) opperator.
        /// </summary>
        /// <param name="b">The second point</param>
        /// <returns>The dot product of the two points</returns>
        public double Mult(Point3D b)
        {
            //returns the sum of the products
            return (x * b.x) + (y * b.y) + (z * b.z);
        }

        /// <summary>
        /// Multiplies the current point by a scalar value. It is the
        /// equivlent to increasing the distance from the origin by
        /// the same ammount. Overloads the (*) and (/) opperators.
        /// </summary>
        /// <param name="s">The scalar multiple</param>
        /// <returns>The product of this point and a scalar</returns>
        public Point3D Mult(double s)
        {
            //subtracts the two parts seperatly
            double xr = x * s;
            double yr = y * s;
            double zr = z * s;

            //returns the resultant
            return new Point3D(xr, yr, zr);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Metric Implementation ...

        /// <summary>
        /// Obtains the magnitude of the current point. It is the same
        /// as the radius in spherical coridnates.
        /// </summary>
        /// <returns>The magnitude of the point</returns>
        public double Mag()
        {
            //computes the sum of the squares
            double rad = (y * y) + (x * x) + (z * z);
            return Math.Sqrt(rad);
        }

        /// <summary>
        /// Computes the distance between this point and another point.
        /// </summary>
        /// <param name="other">The other point</param>
        /// <returns>The distance from the other point</returns>
        public double Dist(Point3D other)
        {
            //calls upon the static method
            return Point3D.Dist(this, other);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Special Methods...

        /// <summary>
        /// Normalises the point to have a radius of one, placing it
        /// on the unit circle while maintaining direction. Overloads
        /// the (~) opperator.
        /// </summary>
        /// <param name="p">The point to normalise</param>
        /// <returns>The normalised point</returns>
        public static Point3D Norm(Point3D p)
        {
            //computes the magnitude of the vector
            double div = (p.x * p.x) + (p.y * p.y) + (p.z * p.z);
            div = 1.0 / Math.Sqrt(div);

            //preforms the normalizing divide
            double xr = p.x * div;
            double yr = p.y * div;
            double zr = p.z * div;

            //returns the normalised vector
            return new Point3D(xr, yr, zr);
        }

        /// <summary>
        /// Computes the distance between two points.
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns>The distance between the points</returns>
        public static double Dist(Point3D a, Point3D b)
        {
            //subtracts the three parts seperatly
            double xr = b.x - a.x;
            double yr = b.y - a.y;
            double zr = b.z - a.z;

            //computes the sum of the squares
            return Math.Sqrt((xr * xr) + (yr * yr) + (zr * zr));
        }

        /// <summary>
        /// Computes the right-hand cross product between two vectors in 
        /// three dimentonal cartisian space. The result is a vector orthogonal
        /// to both vectors with a magntiude equal to the aria between them.
        /// It overloads the (%) opperator.
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The right-handed cross product</returns>
        public static Point3D Cross(Point3D a, Point3D b)
        {
            //computes each component using the XYZZY rule
            double xr = (a.y * b.z) - (a.z * b.y);
            double yr = (a.z * b.x) - (a.x * b.z);
            double zr = (a.x * b.y) - (a.y * b.x);

            //returns the cross product
            return new Point3D(xr, yr, zr);
        }

        /// <summary>
        /// Computes the point that lies halfway between two given points. 
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns>The midpoint between the two points</returns>
        public static Point3D MidPoint(Point3D a, Point3D b)
        {
            //computes the three parts seperatly
            double xr = (b.x + a.x) * 0.5;
            double yr = (b.y + a.y) * 0.5;
            double zr = (b.z + a.z) * 0.5;

            //returns the midpoint
            return new Point3D(xr, yr, zr);
        }

        #endregion ////////////////////////////////////////////////////////////

        #region Class Conversions...

        //creates a vector containing the listed cordinates
        public static implicit operator Vector(Point3D p)
        { return new Vector(p.x, p.y, p.z); }

        //converts the vector by calling the constructor
        public static explicit operator Point3D(Vector v)
        { return new Point3D(v); }

        #endregion //////////////////////////////////////////////////////////////

        #region Opperator Overlodes...

        //refferences the Add(p) function
        public static Point3D operator +(Point3D a, Point3D b)
        { return a.Add(b); }

        //references the Sub(p) function
        public static Point3D operator -(Point3D a, Point3D b)
        { return a.Sub(b); }

        //references the Mult(p) function
        public static Double operator *(Point3D a, Point3D b)
        { return a.Mult(b); }

        //references the Mult(s) function
        public static Point3D operator *(Point3D a, Double s)
        { return a.Mult(s); }

        //references the Mult(s) function
        public static Point3D operator *(Double s, Point3D a)
        { return a.Mult(s); }

        //references the Mult(s) function
        public static Point3D operator /(Point3D v, Double s)
        { return v.Mult(1.0 / s); }

        //refrences the Cross(a, b) function
        public static Point3D operator %(Point3D a, Point3D b)
        { return Cross(a, b); }

        //refrences the Mult(-1) function
        public static Point3D operator -(Point3D a)
        { return a.Mult(-1.0); }

        //refrences the Norm(p) function
        public static Point3D operator ~(Point3D a)
        { return Point3D.Norm(a); }

        #endregion ////////////////////////////////////////////////////////////
    }
}
