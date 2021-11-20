using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Geometry.Planer
{
    public interface Curve2D
    {
        //Implementations
        //--Line
        //--Arc
        //--Polyline
        //--Polygon
        //--BezierCurve
        //--BezierSpline
        //--ClosedSpline
        //--ParametricEquation
        //----CartisianCordinates
        //----PolarCordinates

        Point2D Sample(double t);

        Point2D Deriv(double t);
    }
}
