/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
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

using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Calc.Geometry
{
    /// <summary>
    /// This structor represents a bounded area within the two dimentional cartisian
    /// cordinate plane, in the shape of a rectangle. It provides convient methods
    /// for contanment, transformation, and expantion, and can also compute the
    /// union and intersection with other rectangles.
    /// </summary>
    /// <remarks>Last Update: 2016-02-18</remarks>
    public struct Rectangle
    {
        #region Class Definitions...

        //stores the corenes of the rectangle
        private Point2D p0, p1;

        /// <summary>
        /// Private constructor that dose not check the order of the points.
        /// </summary>
        /// <param name="x0">X cordinate of the upper-left</param>
        /// <param name="y0">Y cordinate of the upper-left</param>
        /// <param name="x1">X cordinate of the lower-right</param>
        /// <param name="y1">Y cordinate of the lower-right</param>
        private Rectangle(double x0, double y0, double x1, double y1)
        {
            this.p0 = new Point2D(x0, y0);
            this.p1 = new Point2D(x1, y1);
        }

        /// <summary>
        /// Private constructor that dose not check the order of the points.
        /// </summary>
        /// <param name="p0">Upper-left corner</param>
        /// <param name="p1">Lower-right corner</param>
        private Rectangle(Point2D p0, Point2D p1)
        {
            this.p0 = p0;
            this.p1 = p1;
        }

        #endregion //////////////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Constructs a rectangle, given it's upper-left corner and it's dimentions.
        /// </summary>
        /// <param name="x">X cordinate of the upper-left corner</param>
        /// <param name="y">Y cordinate of the upper-left corner</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <returns>The generated rectangle</returns>
        /// <exception cref="ArgBoundsException">If either the width or the height
        /// of the rectangle is negative</exception>
        public static Rectangle FromArea(double x, double y, double width, double height)
        {
            //checks that the width and height are valid
            ArgBoundsException.Atleast("width", width, 0.0);
            ArgBoundsException.Atleast("height", height, 0.0);

            //generates the new rectangle
            return new Rectangle(x, y, x + width, y + height);
        }

        /// <summary>
        /// Constructs a rectangle, given it's upper-left corner and it's dimentions.
        /// </summary>
        /// <param name="loc">Upper-left corner of the rectangle</param>
        /// <param name="area">Dimentions of the rectangle</param>
        /// <returns>The generated rectangle</returns>
        /// <exception cref="ArgBoundsException">If either the width or the height
        /// of the rectangle is negative</exception>
        public static Rectangle FromArea(Point2D loc, Point2D area)
        {
            //checks that the width and height are valid
            ArgBoundsException.Atleast("area.X", area.X, 0.0);
            ArgBoundsException.Atleast("area.Y", area.Y, 0.0);

            //generates the new rectangle
            return new Rectangle(loc, loc + area);
        }

        /// <summary>
        /// Creates a rectangle that spans the area between the two points given.
        /// The arangment of the points dose not matter, as the area between them
        /// always exists and is unique.
        /// </summary>
        /// <param name="x0">X cordinate of the first point</param>
        /// <param name="y0">Y cordinate of the first point</param>
        /// <param name="x1">X cordinate of the second point</param>
        /// <param name="y1">Y cordinate of the second point</param>
        /// <returns>The generated rectangle</returns>
        public static Rectangle FromPoints(double x0, double y0, double x1, double y1)
        {
            //the arangment of the points dosent matter
            double x2 = Math.Min(x0, x1);
            double y2 = Math.Min(y0, y1);
            double x3 = Math.Max(x0, x1);
            double y3 = Math.Max(y0, y1);

            //generates the new rectangle
            return new Rectangle(x2, y2, x3, y3);
        }

        /// <summary>
        /// Creates a rectangle that spans the area between the two points given.
        /// The arangment of the points dose not matter, as the area between them
        /// always exists and is unique.
        /// </summary>
        /// <param name="p0">The first point</param>
        /// <param name="p1">The second point</param>
        /// <returns>The generated rectangle</returns>
        public static Rectangle FromPoints(Point2D p0, Point2D p1)
        {
            //the arangment of the points dosent matter
            Point2D p2 = Min(p0, p1);
            Point2D p3 = Max(p0, p1);

            //generates the new rectangle
            return new Rectangle(p2, p3);
        }

        #endregion //////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public double Width
        {
            get { return (p1.X - p0.X); }
        }

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public double Height
        {
            get { return (p1.Y - p0.Y); }
        }

        /// <summary>
        /// The upper-left corner of the rectangle, or the lower-left 
        /// corner if the Y axis is inverted.
        /// </summary>
        public Point2D UpperLeft
        {
            get { return p0; }
        }

        /// <summary>
        /// The lower-right corner of the rectangle, or the upper-right 
        /// corner if the Y axis is inverted.
        /// </summary>
        public Point2D LowerRight
        {
            get { return p1; }
        }

        /// <summary>
        /// The center of the rectangle.
        /// </summary>
        public Point2D Center
        {
            get { return (p0 + p1) / 2.0; }
        }

        /// <summary>
        /// The area covered by the rectangle.
        /// </summary>
        public double Area
        {
            get
            {
                double w = p1.X - p0.X;
                double h = p1.Y - p0.Y;
                return w * h;
            }
        }

        /// <summary>
        /// The length of the perimiter of the rectangel.
        /// </summary>
        public double Perim
        {
            get
            {
                double w = p1.X - p0.X;
                double h = p1.Y - p0.Y;
                return w + w + h + h;
            }
        }

        #endregion //////////////////////////////////////////////////////////////////////

        #region Rectangle Operations...

        /// <summary>
        /// Determins if a given point is contained within the rectangle.
        /// </summary>
        /// <param name="x">X cordinate to test</param>
        /// <param name="y">Y cordinate to test</param>
        /// <returns>True if the point is inside the rectangle</returns>
        public bool Contains(double x, double y)
        {
            //checks agenst the bounds
            if (x < p0.X || x > p1.X) return false;
            if (y < p0.Y || y > p1.Y) return false;

            //we are in the rectangle
            return true;
        }

        /// <summary>
        /// Determins if a given point is contained within the rectangle.
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <returns>True if the point is inside the rectangle</returns>
        public bool Contains(Point2D point)
        {
            //calls the method above
            return Contains(point.X, point.Y);
        }

        /// <summary>
        /// Transforms points in the range of (0, 0) to (1, 1) to lie
        /// within the bounds of the rectangle.
        /// </summary>
        /// <param name="x">X cordinate to transform</param>
        /// <param name="y">Y cordinate to transform</param>
        /// <returns>The transformed point</returns>
        public Point2D Transform(double x, double y)
        {
            //scales the point apropriatly
            double xn = (x * (p1.X - p0.X)) + p0.X;
            double yn = (y * (p1.Y - p0.Y)) + p0.Y;

            return new Point2D(xn, yn);
        }

        /// <summary>
        /// Transforms points in the range of (0, 0) to (1, 1) to lie
        /// within the bounds of the rectangle.
        /// </summary>
        /// <param name="point">Point to transform</param>
        /// <returns>The transformed point</returns>
        public Point2D Transform(Point2D point)
        {
            //calls the method above
            return Transform(point.X, point.Y);
        }

        /// <summary>
        /// Generates an expanded version of the current rectangle, 
        /// encompasing the given point.
        /// </summary>
        /// <param name="x">X cordinate to include</param>
        /// <param name="y">Y cordinate to include</param>
        /// <returns>The expanded rectangle</returns>
        public Rectangle Expand(double x, double y)
        {
            double x2 = Math.Min(x, p0.X);
            double y2 = Math.Min(y, p0.Y);
            double x3 = Math.Max(x, p1.X);
            double y3 = Math.Max(y, p1.Y);

            return new Rectangle(x2, y2, x3, y3);
        }

        /// <summary>
        /// Generates an expanded version of the current rectangle, 
        /// encompasing the given point.
        /// </summary>
        /// <param name="point">Point to expand</param>
        /// <returns>The expanded rectangle</returns>
        public Rectangle Expand(Point2D point)
        {
            Point2D p2 = Min(p0, point);
            Point2D p3 = Max(p1, point);

            return new Rectangle(p2, p3);
        }

        #endregion //////////////////////////////////////////////////////////////////////

        #region Set-Like Operations...

        /// <summary>
        /// Generates a new rectangle that is the smallest possable
        /// rectangle that contains both the current rectangle and
        /// the given rectangle.
        /// </summary>
        /// <param name="other">The other rectangle</param>
        /// <returns>The union of the two rectangles</returns>
        public Rectangle Union(Rectangle other)
        {
            Point2D p2 = Min(p0, other.p0);
            Point2D p3 = Max(p1, other.p1);

            return new Rectangle(p2, p3);
        }

        /// <summary>
        /// Genereates a new rectangle that is the largest possable
        /// rectangle that is contaned by both the curent rectangle
        /// and the given rectangle.
        /// </summary>
        /// <param name="other">The other rectangle</param>
        /// <returns>The intersection of the two rectangles</returns>
        public Rectangle Intersection(Rectangle other)
        {
            Point2D p2 = Max(p0, other.p0);
            Point2D p3 = Min(p1, other.p1);

            if (p2.X > p3.X || p2.Y > p3.Y)
            {
                //return the zero rectangle if we fail to intersect
                return new Rectangle(0.0, 0.0, 0.0, 0.0);
            }
            else
            {
                //return the generated intersection
                return new Rectangle(p2, p3);
            }
        }

        #endregion //////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Computes the component-wise minimum of two points.
        /// </summary>
        /// <param name="p0">First point</param>
        /// <param name="p1">Second point</param>
        /// <returns>Minimum of the two points</returns>
        private static Point2D Min(Point2D p0, Point2D p1)
        {
            double xn = Math.Min(p0.X, p1.X);
            double yn = Math.Min(p0.Y, p1.Y);

            return new Point2D(xn, yn);
        }

        /// <summary>
        /// Computes the component-wise maximum of two points.
        /// </summary>
        /// <param name="p0">First point</param>
        /// <param name="p1">Second point</param>
        /// <returns>Maximum of the two points</returns>
        private static Point2D Max(Point2D p0, Point2D p1)
        {
            double xn = Math.Max(p0.X, p1.X);
            double yn = Math.Max(p0.Y, p1.Y);

            return new Point2D(xn, yn);
        }

        #endregion //////////////////////////////////////////////////////////////////////
    }
}
