using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using Vulpine.Core.Calc.Numbers;

namespace ComplexMaps
{
    class Program
    {
        //constant values used in conversion operations
        private const float ONE_S = 1.0f / 6.0f;
        private const float ONE_T = 1.0f / 3.0f;
        private const float TWO_T = 2.0f / 3.0f;

        public delegate Cmplx CFunc(Cmplx z);

        static void Main(string[] args)
        {
            Bitmap bmp;
            int width = 720;
            int height = 720;

            Console.WriteLine("Drawing Basic Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => x);
            bmp.Save("direct_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Conjagate Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => x.Conj());
            bmp.Save("conjagate_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Recprical Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => x.Inv());
            bmp.Save("recprical_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Real Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => x.Real());
            bmp.Save("real_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Imaginary Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => x.Imag());
            bmp.Save("imag_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Exponential Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Exp(x));
            bmp.Save("exp_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Logarythimic Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Log(x));
            bmp.Save("log_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Square Root Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Sqrt(x));
            bmp.Save("square_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Sine Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Sin(x));
            bmp.Save("sin_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Cosine Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Cos(x));
            bmp.Save("cos_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Tangent Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Tan(x));
            bmp.Save("tan_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Arcsine Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Asin(x));
            bmp.Save("asin_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Arcosine Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Acos(x));
            bmp.Save("acos_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Arctangent Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Atan(x));
            bmp.Save("atan_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Inverse Sine Map");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Sin(x.Inv()));
            bmp.Save("invsin_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Powers of I");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Pow(Cmplx.I, x));
            bmp.Save("powofi_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Powers of Negative 2");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Pow(-2.0, x));
            bmp.Save("powneg2_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Powers of 2");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => Cmplx.Pow(2.0, x));
            bmp.Save("pow2_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Inverse Log");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => 1.0 / Cmplx.Log(x));
            bmp.Save("invlog_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Log Base I");
            bmp = new Bitmap(width, height);
            Cmplx logi = Cmplx.Log(Cmplx.I);
            DrawFunction(bmp, x => Cmplx.Log(x) / logi);
            bmp.Save("logbasei_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing Rational Function");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => (x - 1.0) / (x + 1.0));
            bmp.Save("rational_map.png");
            bmp.Dispose();

            Console.WriteLine("Drawing 2nd Rational Function");
            bmp = new Bitmap(width, height);
            DrawFunction(bmp, x => ((x - 1.0) * (x + 1.0)) / ((x - Cmplx.I) * (x + Cmplx.I)));
            bmp.Save("rational2_map.png");
            bmp.Dispose();


            Console.WriteLine("All Drawings Finished");
        }

        public static void DrawFunction(Bitmap bmp, CFunc func)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            //double ex = Math.PI;
            double ex = 0.5;
            double duex = ex * 2.0;

            for (int x = 0; x < w; x++)
            {
                //computes the real value form the cordinats
                double r = (((double)x / w) * duex) - ex;

                for (int y = 0; y < h; y++)
                {
                    //computes the imaginary value from the cordinats
                    double i = (((double)(h-y) / h) * duex) - ex;

                    //passes the complex value throught the funciton
                    Cmplx z = new Cmplx(r, i);
                    z = func(z);

                    //collors the spot in the image apropratly
                    Color c = CmplxToColor(z);
                    bmp.SetPixel(x, y, c); 
                }
            }
        }

        public static Color CmplxToColor(Cmplx z)
        {
            //takes care of the extreem cases
            if (z.IsNaN()) return Color.White;
            if (z.IsInfinity()) return Color.White;

            double temp = (-1.0 / (z.Abs + 1.0)) + 1.0;

            //gets the HSL values from the polar cordinats
            float hue = (float)z.Arg;
            float lum = (float)(temp);
            float sat = 1.0f;

            //used for intermediat calculations
            float q = 0.0f;
            float p = 0.0f;

            //determins the p and q values
            if (lum < 0.5f) q = lum * (1.0f + sat);
            else q = lum + sat - (lum * sat);
            p = (2.0f * lum) - q;
            q = 2.0f * (lum - p);

            //determins the temporary color values
            float tg = hue / (float)(2.0 * Math.PI);
            float tr = tg + ONE_T;
            float tb = tg - ONE_T;

            //corects the temporary colors
            if (tr < 0.0f) tr = tr + 1.0f;
            if (tr > 1.0f) tr = tr - 1.0f;
            if (tg < 0.0f) tg = tg + 1.0f;
            if (tg > 1.0f) tg = tg - 1.0f;
            if (tb < 0.0f) tb = tb + 1.0f;
            if (tb > 1.0f) tb = tb - 1.0f;

            //computes the red component
            if (tr < ONE_S) tr = p + (q * 6.0f * tr);
            else if (tr < 0.5f) tr = p + q;
            else if (tr < TWO_T) tr = p + (q * 6.0f * (TWO_T - tr));
            else tr = p;

            //computes the green component
            if (tg < ONE_S) tg = p + (q * 6.0f * tg);
            else if (tg < 0.5f) tg = p + q;
            else if (tg < TWO_T) tg = p + (q * 6.0f * (TWO_T - tg));
            else tg = p;

            //computes the blue component
            if (tb < ONE_S) tb = p + (q * 6.0f * tb);
            else if (tb < 0.5f) tb = p + q;
            else if (tb < TWO_T) tb = p + (q * 6.0f * (TWO_T - tb));
            else tb = p;

            //caluclates each channel in the range 0-255
            int br = (int)(tr * 255.0f);
            int bg = (int)(tg * 255.0f);
            int bb = (int)(tb * 255.0f);

            //clamps the range just incase it exceeds the bounds
            br = Clamp(br);
            bg = Clamp(bg);
            bb = Clamp(bb);

            //returns the generated color
            return Color.FromArgb(br, bg, bb);
        }

        private static int Clamp(int x)
        {
            if (x < 0) return 0;
            if (x > 255) return 255;
            return x;
        }
    }
}
