using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace QuickTests
{
    public static class RandomMatrix
    {
        public static void Run()
        {
            Matrix m;

            m = BuildMatrix(
                -6.45567457039478043157e-1,
                 1.31830097128621787084e-1,
                 -7.13028979106255178522e-1,

                 0.97768455948746219567,
                 0.37143057540630950987,
                 0.75797076429625629749
            );
            Console.WriteLine(m.ToString());


            m = BuildMatrix(
                 8.02588406319545200063e-1,
                 3.10312885468724652949e-1,
                 5.22664738782391391503e-1,

                 0.53910324194620455986,
                 0.94412067310368527184,
                 0.44688251130497709818
            );
            Console.WriteLine(m.ToString());

            m = BuildMatrix(
                2.62496652915703176934e-1,
                2.18851668376025498830e+0,
                1.03058541471196374317e+0,

                0.33731825385264247537,
                0.87492225561395360946,
                0.20319933606291789480
            );
            Console.WriteLine(m.ToString());

            m = BuildMatrix(
                -1.24079285426373045476e-1,
                 1.51044290698699401609e+0,
                 2.32893122163021909499e-1,

                 0.09308201118623588626,
                 0.28578288549242551420,
                 0.41049457225912553697
            );
            Console.WriteLine(m.ToString());

            m = BuildMatrix(
                1.11520093123900054799e+0,
                -1.10198293434280092384e+0,
                1.37136802382375438958e-1,

                0.27474209552728661630,
                0.06351401498734184128,
                0.27219306751789223509
            );
            Console.WriteLine(m.ToString());


            Console.ReadKey(true);
        }


        public static Matrix BuildMatrix(double x, double y, double z, double rx, double ry, double rz)
        {
            Matrix trans = new Matrix(4, 4,
                1.0, 0.0, 0.0, x,
                0.0, 1.0, 0.0, y,
                0.0, 0.0, 1.0, z,
                0.0, 0.0, 0.0, 1.0
            );

            double sinx = Math.Sin(rx * VMath.TAU);
            double cosx = Math.Cos(rx * VMath.TAU);

            Matrix rotx = new Matrix(4, 4,
                1.0, 0.0, 0.0, 0.0,
                0.0, cosx, sinx, 0.0,
                0.0, -sinx, cosx, 0.0,
                0.0, 0.0, 0.0, 1.0
            );

            double siny = Math.Sin(ry * VMath.TAU);
            double cosy = Math.Cos(ry * VMath.TAU);

            Matrix roty = new Matrix(4, 4,
                cosy, 0.0, -siny, 0.0,
                0.0, 1.0, 0.0, 0.0,
                siny, 0.0, cosy, 0.0,
                0.0, 0.0, 0.0, 1.0
            );

            double sinz = Math.Sin(rz * VMath.TAU);
            double cosz = Math.Cos(rz * VMath.TAU);

            Matrix rotz = new Matrix(4, 4,
                cosz, sinz, 0.0, 0.0,
                -sinz, cosz, 0.0, 0.0,
                0.0, 0.0, 1.0, 0.0,
                0.0, 0.0, 0.0, 1.0
            );

            Matrix rtrans = new Matrix(4, 4,
                1.0, 0.0, 0.0, -x,
                0.0, 1.0, 0.0, -y,
                0.0, 0.0, 1.0, -z,
                0.0, 0.0, 0.0, 1.0
            );


            return (rtrans * rotz * roty * rotx * trans);

        }

    }
}
