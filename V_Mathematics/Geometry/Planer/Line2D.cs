using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Geometry.Planer
{
    public class Line2D : Curve2D
    {
        private Point2D a;
        private Point2D b;


        public Line2D(Point2D a, Point2D b)
        {
            this.a = a;
            this.b = b;
        }


        public Point2D Start
        {
            get { return a; }
        }

        public Point2D End
        {
            get { return b; }
        }

        public double Length()
        {
            return a.Dist(b);
        }

        public double Slope()
        {
            //computes the change in x and y
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            //uses the rise over run
            return dy / dx;
        }

        public double Angle()
        {
            //computes the change in x and y
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            //finds the angle with the X-axis
            return Math.Atan2(dy, dx);
        }

        public Point2D Sample(double t)
        {
            //uses straight linear interploation
            return (a * (1.0 - t)) + (b * t);
        }

        public Point2D Deriv(double t)
        {
            //takes the diffrence of the endpoints
            return (b - a);
        }
    }
}
