using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc.Exceptions;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Numbers
{
    /// <summary>
    /// Complex numbers are numbers that contain both a real part and an imaginary
    /// part. They are based on the imaginary unit (I) who's square is negative one.
    /// Any multiple of (I) is said to be imaginary, froming a line of numbers
    /// orthognal to the real number line. In this way, complex numbers extend the
    /// real numbers into a two-dimentional feild that is algebraicly complete.
    /// </summary>
    /// <remarks>Last Update: 2013-09-18</remarks>
    public struct Cmplx : Algebraic<Cmplx>, Euclidean<Cmplx, Double>
    {
        #region Class Definitions...

        //stores the real and imaginary components
        private double real;
        private double imag;

        /// <summary>
        /// Converts a real number to a complex number.
        /// </summary>
        /// <param name="num">Value to convert</param>
        public Cmplx(double num)
        {
            //initialises values
            real = num;
            imag = 0.0;
        }

        /// <summary>
        /// Constructs a complex number of the form (a + b * I) given
        /// it's real and imaginary components.
        /// </summary>
        /// <param name="r">The real compoent</param>
        /// <param name="i">The imaginary component</param>
        public Cmplx(double r, double i)
        {
            //initialises values
            real = r;
            imag = i;
        }

        /// <summary>
        /// Constructs a complex number of the form (a + b * I) given
        /// a vector containing the corisponding coffecents.
        /// </summary>
        /// <param name="v">Vector to convert</param>
        public Cmplx(Vector v)
        {
            real = v.GetElement(0);
            imag = v.GetElement(1);
        }

        /// <summary>
        /// Generates a string representation of the complex number.
        /// </summary>
        /// <returns>The number as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("0.000");
        }

        /// <summary>
        /// Generates a string representation of the current complex 
        /// number, using a custom format. A seperate format can be 
        /// provided for both the real and imaginary coffecents, by 
        /// seperating them by a virtical pipe.
        /// </summary>
        /// <param name="format">A custom format string</param>
        /// <returns>The number as a string</returns>
        public string ToString(string format)
        {
            //splits the format string around the comma
            string[] f = format.Split('|');
            string xs = (f.Length < 2) ? format : f[0];
            string ys = (f.Length < 2) ? format : f[1];

            //formats the diffrent parts seperatly
            xs = real.ToString(xs);
            ys = imag.ToString(ys);

            //builds and returns the output
            return String.Format("({0}, {1})", xs, ys);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Constant Constructors...

        /// <summary>
        /// The imaginary constant, and the basis of the imaginary numbers. It
        /// is defined as the square root of negative one. Read-Only
        /// </summary>
        public static Cmplx I
        {
            get { return new Cmplx(0.0, 1.0); }
        }

        /// <summary>
        /// Represnents the value 2 * PI * I, which apeares frequently in many
        /// formulas invlovling complex numbers, usualy represented by the
        /// greek letter omega. Read-Only
        /// </summary>
        public static Cmplx W
        {
            get { return new Cmplx(0.0, VMath.TAU); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The coffecent of the real compoent (1) making up the
        /// complex number. Read-Only
        /// </summary>
        public double CofR
        {
            get 
            {
                if (!Double.IsNaN(imag)) return real;
                else return Double.NaN;
            }
        }

        /// <summary>
        /// The coffecent of the imaginary component (I) makeing up
        /// the complex number. Read-Only
        /// </summary>
        public double CofI
        {
            get 
            {
                if (!Double.IsNaN(real)) return imag;
                else return Double.NaN;
            }
        }

        /// <summary>
        /// The absolute value of the complex number. This is equivlent
        /// to the magnitude or radius in polar cordinats. Read-Only
        /// </summary>
        public double Abs
        {
            get
            {
                //uses the distace fourmual
                double z = (real * real) + (imag * imag);
                return Math.Sqrt(z);
            }
        }

        /// <summary>
        /// The priniciple argument of the complex number, ranging from
        /// just above negative PI to PI. It is equivlent to the
        /// anglular mesure in polar cordinats. Read-Only.
        /// </summary>
        public double Arg
        {
            get { return Math.Atan2(imag, real); }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Urinary Operations...

        /// <summary>
        /// Generates the real compoent of the complex number. This is
        /// diffrent from the coffecent as the result is another complex 
        /// number. Use CofR for the coffecent.
        /// </summary>
        /// <returns>An exclusivly real number</returns>
        public Cmplx Real()
        {
            //returns the exclusivly real component
            return new Cmplx(real, 0.0);
        }

        /// <summary>
        /// Generates the imaginary compoent of the complex number. This is
        /// diffrent from the coffecent as the result is another complex
        /// number. Use CofI for the coffecent.
        /// </summary>
        /// <returns>An exclusivly imaginary number</returns>
        public Cmplx Imag()
        {
            //returns the exclusivly imaginary component
            return new Cmplx(0.0, imag);
        }

        /// <summary>
        /// Computes the negative of the current complex number. It
        /// overloads the uinary (-) operatoer.
        /// </summary>
        /// <returns>The negative complex number</returns>
        public Cmplx Neg()
        {
            //negates the complex number
            return new Cmplx(-real, -imag);
        }

        /// <summary>
        /// Computes the conjgate of the curent complex number, defined
        /// by the negation of the imaginary coefecent. It overloads
        /// the (~) opperator.
        /// </summary>
        /// <returns>The complex conjigate</returns>
        public Cmplx Conj()
        {
            //produces the complex conjagate
            return new Cmplx(real, -imag);
        }

        /// <summary>
        /// Computes the multiplicitve inverse of the complex number. 
        /// </summary>
        /// <returns>The inverse of the complex number</returns>
        public Cmplx Inv()
        {
            //uses the conjgate to invert the number
            double temp = (real * real) + (imag * imag);
            double re = real / temp;
            double ie = imag / temp;

            //returns the complex inverse
            return new Cmplx(re, -ie);
        }

        /// <summary>
        /// Computes the multiplication of the complex number by the
        /// imaginary constant (I), saving on opperations. 
        /// </summary>
        /// <returns>The complex number multiplyed by I</returns>
        public Cmplx MultI()
        {
            //computes the multily in one opperation
            return new Cmplx(-imag, real);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Binary Operations...

        /// <summary>
        /// Adds the opperand to the curent complex number and returns 
        /// the result. It overloads the (+) opperator.
        /// </summary>
        /// <param name="z">The number to add</param>
        /// <returns>The sum of the complex numbers</returns>
        public Cmplx Add(Cmplx z)
        {
            //dose peicewise addition of coffecents
            double a = real + z.real;
            double b = imag + z.imag;

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Subtracts the opperand from the curent complex number and returns 
        /// the result. It overloads the (-) opperator.
        /// </summary>
        /// <param name="z">The number to subtract</param>
        /// <returns>The diffrence of the complex numbers</returns>
        public Cmplx Sub(Cmplx z)
        {
            //dose peicewise addition of coffecents
            double a = real - z.real;
            double b = imag - z.imag;

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Multiplys the current complex number by the opperand and returns 
        /// the result. It overloads the (*) opperator.
        /// </summary>
        /// <param name="z">The number used to mutiply</param>
        /// <returns>The product of the complex numbers</returns>
        public Cmplx Mult(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly
            double a = (real * z.real) - (imag * z.imag);
            double b = (real * z.imag) + (imag * z.real);

            return new Cmplx(a, b);
        }

        /// <summary>
        /// Divides the current complex number by the opperand and returns 
        /// the result. It overloads the (/) opperator.
        /// </summary>
        /// <param name="z">The number used to devide</param>
        /// <returns>The quotient of the two complex numbers</returns>
        public Cmplx Div(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly
            double temp = (z.real * z.real) + (z.imag * z.imag);
            double a = (real * z.real + imag * z.imag) / temp;
            double b = (imag * z.real - real * z.imag) / temp;

            return new Cmplx(a, b);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Conputes the euclidian distance between two complex numbers
        /// within the complex feild. This is the same as the absolute
        /// value of the number's diffrence.
        /// </summary>
        /// <param name="z">The other number</param>
        /// <returns>The distance between the numbers in the 
        /// complex feild</returns>
        public double Dist(Cmplx z)
        {
            //subtracts the two parts seperatly
            double xr = z.real - real;
            double yr = z.imag - imag;

            //computes the sum of the squares
            return Math.Sqrt((xr * xr) + (yr * yr));
        }

        /// <summary>
        /// Computes the magnitude of the current complex number. This
        /// is exactly the same as the absolute value.
        /// </summary>
        /// <returns>The magnitude of the complex number</returns>
        public double Mag()
        {
            //simply returns the asbolute value
            return this.Abs;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Transindental Functions...

        /// <summary>
        /// Raises a complex number to an arbitrary, real power. In general, this
        /// function may have more that one solution. A branch index can be
        /// spesified to select from the diffrent possable solutions.
        /// </summary>
        /// <param name="z">Base of the exponintial</param>
        /// <param name="exp">Power of the exponential</param>
        /// <param name="n">Branch selector index</param>
        /// <returns>A complex number raised to a given power</returns>
        public static Cmplx Pow(Cmplx z, double exp, int n = 0)
        {
            //calculates the result storing temporary values
            double rad = Math.Pow(z.Abs, exp);
            double arg = (z.Arg + (VMath.TAU * n)) * exp;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This is the most general form of the expnential, as it raises any
        /// complex number to any complex power, and is in general multi-valued.
        /// Diffrent, potential values can be selected by changing the branch
        /// factor. Consider using one of the other functions if either the
        /// base or the exponent are known to be real.
        /// </summary>
        /// <param name="z">Base of the exponential</param>
        /// <param name="exp">Power of the exponential</param>
        /// <param name="n">Branch selector index</param>
        /// <returns>A complex number raised to a complex power</returns>
        public static Cmplx Pow(Cmplx z, Cmplx exp, int n = 0)
        {
            //allows for computation of zero, avoiding ln(0)
            if (z.Abs == 0) return new Cmplx(0.0, 0.0);

            //uses the definition of the general exponential
            return Exp(exp * Log(z, n));
        }

        /// <summary>
        /// This method extends the domain of the exponintial function
        /// (defined by euler's constant) into the complex plain. It is a
        /// single-valued, continous function for all complex numbers.
        /// </summary>
        /// <param name="z">Power of the exponential</param>
        /// <returns>Euler's constant raised to a complex power</returns>
        public static Cmplx Exp(Cmplx z)
        {
            //calculates the real and imaginary parts seperatly 
            double power = Math.Exp(z.real);
            double outR = power * Math.Cos(z.imag);
            double outI = power * Math.Sin(z.imag);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This metnod generalises the complex exponential function, allowing
        /// for any positive real base. It is also uniquly defined for all
        /// complex numbers under these restrictions.
        /// </summary>
        /// <param name="z">Power of the exponential</param>
        /// <param name="bass">Base of the exponential</param>
        /// <returns>A given base raised to a complex power</returns>
        /// <exception cref="ArgBoundsException">If the base of the exponential
        /// is strictly less than zero</exception>
        public static Cmplx Exp(Cmplx z, double bass)
        {
            //checks that the base is a positive real number
            ArgBoundsException.Atleast("bass", bass, 0.0);

            //allows for the computation when the bass is zero
            if (bass == 0.0) return new Cmplx(0.0, 0.0);

            //calculates the result storing temporary values
            double temp1 = Math.Pow(bass, z.real);
            double temp2 = z.imag * Math.Log(bass);
            double outR = temp1 * Math.Cos(temp2);
            double outI = temp1 * Math.Sin(temp2);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// The complex logarythim is the inverse of the complex exponential.
        /// In general, there are infinatly many solutions, similar to the
        /// inverse trigometric functions on the real numbers. A branch index
        /// can be spesified to select from the diffrent possable solutions. 
        /// </summary>
        /// <param name="z">Argument of the logarythim</param>
        /// <param name="n">Branch selector index</param>
        /// <returns>The natural log of a complex number</returns>
        public static Cmplx Log(Cmplx z, int n = 0)
        {
            //calculates the resulting components seperatly
            double outI = z.Arg + (VMath.TAU * n);
            double outR = Math.Log(z.Abs);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This method generalises the complex logarythim, allowing for
        /// any positive real base (except for base-1). Like the natural
        /// logarythim, it also has infinatly many solutions. A branch index
        /// can be spesified to select from the diffrent possable solutions. 
        /// </summary>
        /// <param name="z">Argument of the complex logarythim</param>
        /// <param name="bass">Base of the complex logarythim</param>
        /// <param name="n">Branch selector index</param>
        /// <returns>The log of a complex number in the given base</returns>
        /// <exception cref="RangeException">If the base of the lograythim
        /// is less than or equal to zero</exception>
        public static Cmplx Log(Cmplx z, double bass, int n = 0)
        {
            //checks that the base is a positive real number
            ArgBoundsException.Atleast("bass", bass, 0.0);

            //computes the radius and the log of the bass
            double outR = Math.Log(z.Abs);
            double outI = Math.Log(bass);

            //calculates the log with the change of base formula
            outR = outR / outI;
            outI = (z.Arg + (VMath.TAU * n)) / outI;
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// This method calculates the square root of a complex number. All
        /// numbers in the complex plain have exactly two square roots. This
        /// method returns the principle value, with a positive imaginary
        /// component.
        /// </summary>
        /// <param name="z">Argument of the radical</param>
        /// <returns>The square root of a complex number</returns>
        public static Cmplx Sqrt(Cmplx z)
        {
            //calculates the result storing temporary values
            double rad = Math.Sqrt(z.Abs);
            double arg = z.Arg / 2.0;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Trigimetric Functions...

        /// <summary>
        /// Computes the sine of a complex number. The sign function extends
        /// naturaly to the coplex numbers, due to the complex exponential
        /// and Euler's identity.
        /// </summary>
        /// <param name="arg">Argument of the complex sine</param>
        /// <returns>The sine of the complex number</returns>
        public static Cmplx Sin(Cmplx arg)
        {
            //uses the hyperboloc definition to compute the sine
            double outR = Math.Sin(arg.real) * Math.Cosh(arg.imag);
            double outI = Math.Cos(arg.real) * Math.Sinh(arg.imag);
            return new Cmplx(outR, outI);
        }

        /// <summary>
        /// Computes the cosign of a complex number. The cosign function 
        /// extends naturaly to the coplex numbers, due to the complex 
        /// exponential and Euler's identity.
        /// </summary>
        /// <param name="arg">Argument of the complex cosine</param>
        /// <returns>The cosine of the complex number</returns>
        public static Cmplx Cos(Cmplx arg)
        {
            //uses the hyperboloc definition to compute the cosine
            double outR = Math.Cos(arg.real) * Math.Cosh(arg.imag);
            double outI = Math.Sin(arg.real) * Math.Sinh(arg.imag);
            return new Cmplx(outR, -outI);
        }

        /// <summary>
        /// Computes the tangent of a complex number. Like the tangent
        /// function on the real numbers, it is defined as the ratio of
        /// the sign over the cosign.
        /// </summary>
        /// <param name="arg">Argument of the complex tangent</param>
        /// <returns>The tangent of the complex number</returns>
        public static Cmplx Tan(Cmplx arg)
        {
            //computes the raito of the sign over the cosine
            return Cmplx.Sin(arg) / Cmplx.Cos(arg);
        }

        /// <summary>
        /// Computes the principle arcsine of a complex number. That is,
        /// it finds a complex value who's sign is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arcsine</param>
        /// <returns>The principle arcsine of the given number</returns>
        public static Cmplx Asin(Cmplx arg)
        {
            //uses the logarthim definition of arcsign
            Cmplx temp = 1.0 - (arg * arg);
            temp = arg.MultI() + Cmplx.Sqrt(temp);
            return -Cmplx.Log(temp).MultI();
        }

        /// <summary>
        /// Computes the principle arcosine of a complex number. That is,
        /// it finds a complex value who's cosign is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arcosine</param>
        /// <returns>The principle arcosine of the given number</returns>
        public static Cmplx Acos(Cmplx arg)
        {
            //uses the logarthim definition of arcosine
            Cmplx temp = 1.0 - (arg * arg);
            temp = arg + Cmplx.Sqrt(temp).MultI();
            return -Cmplx.Log(temp).MultI();
        }

        /// <summary>
        /// Computes the principle arctangent of a complex number. That is,
        /// it finds a complex value who's tangent is the given number. Like
        /// it's real counterpart, it is multi-valued.
        /// </summary>
        /// <param name="arg">Argument of the complex arctangent</param>
        /// <returns>The principle arctangent of the given number</returns>
        public static Cmplx Atan(Cmplx arg)
        {
            //uses the logarithm definition of arctangent
            Cmplx temp1 = Cmplx.Log(1.0 - arg.MultI());
            Cmplx temp2 = Cmplx.Log(1.0 + arg.MultI());
            return (temp1 - temp2) * new Cmplx(0.0, 0.5);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Value Checking Functions...

        /// <summary>
        /// Determins if a complex number exists at one of the points of
        /// infinity. This set of points can be viewed as the outter edge 
        /// of the extended complex plane, or as a single point on the 
        /// Riemann Sphere.
        /// </summary>
        /// <param name="z">Number to test</param>
        /// <returns>True if the number is at one of the points of infinity,
        /// and false if otherwise</returns>
        public static bool IsInfinity(Cmplx z)
        {
            if (Double.IsInfinity(z.real)) return true;
            else if (Double.IsInfinity(z.imag)) return true;
            else return false;
        }

        /// <summary>
        /// Determins if a complex number is a valid number. If either the
        /// real or imaginary component is invalid, then the number as a whole
        /// is invalid. It returns true for invlalid numbers.
        /// </summary>
        /// <param name="z">Number to test</param>
        /// <returns>True if the number is invalid, and false otherwise</returns>
        public static bool IsNaN(Cmplx z)
        {
            if (Double.IsNaN(z.real)) return true;
            else if (Double.IsNaN(z.imag)) return true;
            else return false;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        //creates a new complex number through the constructor
        public static implicit operator Cmplx(Double n)
        { return new Cmplx(n); }

        /// <summary>
        /// Converts the complex number to a floating point number if it lies 
        /// on the real number line. Otherwise it reutrns NaN.
        /// </summary>
        public static explicit operator Double(Cmplx n)
        {
            bool is_real = Math.Abs(n.imag) < VMath.TOL;
            return (is_real) ? n.real : Double.NaN;
        }

        //creates a vector containing the real and imagnary components
        public static implicit operator Vector(Cmplx n)
        { return new Vector(n.real, n.imag); }

        //converts the vector by calling the constructor
        public static explicit operator Cmplx(Vector v)
        { return new Cmplx(v); }

        #endregion //////////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the Add(z) function
        public static Cmplx operator +(Cmplx a, Cmplx b)
        { return a.Add(b); }

        //references the Sub(z) function
        public static Cmplx operator -(Cmplx a, Cmplx b)
        { return a.Sub(b); }

        //references the Mult(z) function
        public static Cmplx operator *(Cmplx a, Cmplx b)
        { return a.Mult(b); }

        //references the Div(z) function
        public static Cmplx operator /(Cmplx a, Cmplx b)
        { return a.Div(b); }

        //refrences the Neg() function
        public static Cmplx operator -(Cmplx n)
        { return n.Neg(); }

        //refrences the Conj() function
        public static Cmplx operator ~(Cmplx n)
        { return n.Conj(); }

        #endregion //////////////////////////////////////////////////////////////

        Cmplx Euclidean<Cmplx, Double>.Mult(double scalar)
        {
            return this.Mult((Cmplx)scalar);
        }
    }
}
