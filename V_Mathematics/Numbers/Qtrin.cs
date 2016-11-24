using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Calc.Numbers
{
    /// <summary>
    /// The Quaternions form a four dimentional algebra. They are an extention 
    /// of the Complex numbers, in the same way that the Complex numbers are an 
    /// extention of the Real numbers. They can be thought of as containing one 
    /// real component and three orthoginal imaginary components. Unlike the 
    /// Real and Complex numbers, Quaternions are non-comunitive. This means that 
    /// the product AB is not nessarly the same as the product BA. Quaternions 
    /// also have a polar form, which consists of there absolute value, an angle 
    /// theta, and a three-dimentional imaginary vector.
    /// </summary>
    /// <remarks>Last Update: 2016-11-24</remarks>
    public struct Qtrin : Algebraic<Qtrin, Double>, IFormattable
    {
        #region Class Definitions...

        //stores the coffecients of the quaternion
        private double real;
        private double icof;
        private double jcof;
        private double kcof;

        /// <summary>
        /// Converts a real number to a quaternion.
        /// </summary>
        /// <param name="num">Value to convert</param>
        public Qtrin(double value)
        {
            //initialises values
            icof = jcof = kcof = 0.0;
            real = value;
        }

        /// <summary>
        /// Constructs a new quaternion out of it's real component,
        /// and three orthoginal, imaginary compoennts.
        /// </summary>
        /// <param name="r">The real compoent</param>
        /// <param name="i">The first imaginary component</param>
        /// <param name="j">The second imaginary component</param>
        /// <param name="k">The third imaginary component</param>
        public Qtrin(double r, double i, double j, double k)
        {
            //initialises values
            real = r;
            icof = i;
            jcof = j;
            kcof = k;
        }

        /// <summary>
        /// Converts a complex number to a quaternion. It scales the
        /// standard (I) basis vector by default.
        /// </summary>
        /// <param name="z">Complex value to convert</param>
        public Qtrin(Cmplx z)
        {
            //initialises values
            jcof = kcof = 0.0;
            real = z.CofR;
            icof = z.CofI;
        }

        /// <summary>
        /// Constructs a quaternion from a complex number, given an imaginary
        /// unit vector to rotate the imaginary component. The complex number
        /// can be thought of as containing the essential real and imaginary
        /// parts, while the vector contains the directional informaiton.
        /// </summary>
        /// <param name="z">Complex value to convert</param>
        /// <param name="vec">Vector portion of the quaternion</param>
        public Qtrin(Cmplx z, Qtrin vec)
        {
            //normalises the vector, if not already   
            double vn = 1.0 / vec.ImAbs;
            vn = Double.IsInfinity(vn) ? 0.0 : vn;

            //copies the real part from the base
            real = z.CofR;

            //scales the imag part based on the vector
            icof = z.CofI * vec.icof * vn;
            jcof = z.CofI * vec.jcof * vn;
            kcof = z.CofI * vec.kcof * vn;
        }

        /// <summary>
        /// Generates a string representation of the quaternion.
        /// </summary>
        /// <returns>The quaternion as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("g5", null);
        }

        /// <summary>
        /// Generates a formated string representation of the current quaternion
        /// by passing the format infromation to both the real and imaginary 
        /// components. It handels the sign information independtly from the 
        /// format string.
        /// </summary>
        /// <param name="format">A numeric format string</param>
        /// <param name="provider">An object that suplies formating information</param>
        /// <returns>The number formated as a string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (IsInfinity()) return "Inf";
            if (IsNaN()) return "NaN";

            StringBuilder sb = new StringBuilder();
            double r = Math.Abs(real);
            double i = Math.Abs(icof);
            double j = Math.Abs(jcof);
            double k = Math.Abs(kcof);

            sb.Append(real < 0 ? "-" : "");
            sb.Append(r.ToString(format, provider));

            sb.Append(icof < 0 ? " - " : " + ");
            sb.Append(i.ToString(format, provider));
            sb.Append("i");

            sb.Append(jcof < 0 ? " - " : " + ");
            sb.Append(j.ToString(format, provider));
            sb.Append("j");

            sb.Append(kcof < 0 ? " - " : " + ");
            sb.Append(k.ToString(format, provider));
            sb.Append("k");

            return sb.ToString();
        }

        #endregion //////////////////////////////////////////////////////////

        #region Constant Constructors...
        
        /// <summary>
        /// Represents the unit quaternion (I), one of three imaginary
        /// basis vectors who's square is equivlent to negative one.
        /// </summary>
        public static Qtrin I
        {
            get { return new Qtrin(0.0, 1.0, 0.0, 0.0); }
        }

        /// <summary>
        /// Represents the unit quaternion (J), one of three imaginary
        /// basis vectors who's square is equivlent to negative one.
        /// </summary>
        public static Qtrin J
        {
            get { return new Qtrin(0.0, 0.0, 1.0, 0.0); }
        }

        /// <summary>
        /// Represents the unit quaternion (K), one of three imaginary
        /// basis vectors who's square is equivlent to negative one.
        /// </summary>
        public static Qtrin K
        {
            get { return new Qtrin(0.0, 0.0, 0.0, 1.0); }
        }

        /// <summary>
        /// Returns the quaternion equevlent of a NaN (Not a Number) value, borrowed
        /// from the conventions used by real valued floating-point numbers.
        /// </summary>
        public static Qtrin NaN
        {
            get 
            {
                double nan = Double.NaN;
                return new Qtrin(nan, nan, nan, nan); 
            }
        }

        #endregion //////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The coffecent of the real compoent (1).
        /// </summary>
        public double CofR
        {
            get { return real; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (I).
        /// </summary>
        public double CofI
        {
            get { return icof; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (J).
        /// </summary>
        public double CofJ
        {
            get { return jcof; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (K).
        /// </summary>
        public double CofK
        {
            get { return kcof; }
        }

        /// <summary>
        /// Obtains the absolute value. In quaternion space this is 
        /// equivlent to the radius, or the distance from the origin.
        /// </summary>
        public double Abs
        {
            get
            {
                //computes the sum of the squares
                double temp = (real * real) + (icof * icof);
                temp = temp + (jcof * jcof) + (kcof * kcof);

                //returns the distance generated
                return Math.Sqrt(temp);
            }
        }

        /// <summary>
        /// Returns the argument of the quaternion as part of it's polar form.
        /// Unlike the complex argument, which ranges from minius PI to PI, the
        /// quaternion argument ranges only from 0 to PI. It is equivlent to
        /// the argument of the degenerate complex form.
        /// </summary>
        public double Arg
        {
            get
            {
                //computes the angle with the real line
                return Math.Acos(real / this.Abs);
            }
        }

        /// <summary>
        /// Returns the abslute value of the pure imaginary part of the
        /// quaternion. This value is used frequently by other methods in
        /// the quaternion class.
        /// </summary>
        public double ImAbs
        {
            get
            {
                double t = (icof * icof) + (jcof * jcof);
                return Math.Sqrt(t + (kcof * kcof));
            }
        }

        #endregion //////////////////////////////////////////////////////////

        #region Uninary Operations...

        /// <summary>
        /// Generates the real compoent of the quaternion. This is
        /// diffrent from the coffecent as the result is another 
        /// quaternion. Use CofR for the coffecent.
        /// </summary>
        /// <returns>An exclusivly real number</returns>
        public Qtrin Real()
        {
            //returns the exclusivly real component
            return new Qtrin(real, 0.0, 0.0, 0.0);
        }

        /// <summary>
        /// Generates the imaginary part of the quaternion as a three
        /// dimentional vector in pure imaginary space. That is, a
        /// quaternion without any real component.
        /// </summary>
        /// <returns>The imaginary vector</returns>
        public Qtrin Imag()
        {
            //returns the exclusivly imaginary component
            return new Qtrin(0.0, icof, jcof, kcof);
        }

        /// <summary>
        /// Computes the negative of the current quaternion.
        /// </summary>
        /// <returns>The negative quaternion</returns>
        /// <remarks>It overloads the uinary (-) operatoer</remarks>
        public Qtrin Neg()
        {
            //negates each peice of the quaternion
            return new Qtrin(-real, -icof, -jcof, -kcof);
        }

        /// <summary>
        /// Computes the conjgate of the curent quaternion, defined
        /// by the negation of the imaginary coefecents.
        /// </summary>
        /// <returns>The quaternionic conjgate</returns>
        /// <remarks>It overloads the (~) opperator</remarks>
        public Qtrin Conj()
        {
            //negates each peice of vector portion
            return new Qtrin(real, -icof, -jcof, -kcof);
        }

        /// <summary>
        /// Computes the multiplicitve inverse of the quaternion. That 
        /// is, a number when multiplied by the original number, in either 
        /// order, yeilds the number one.
        /// </summary>
        /// <returns>The inverse of the quaternion</returns>
        public Qtrin Inv()
        {
            //computes the sum of the squares of the quaternion
            double norm = (real * real) + (icof * icof);
            norm = norm + (jcof * jcof) + (kcof * kcof);

            double a, b, c, d;

            //inverts each of the four parts of the quaternion
            a = real / norm;
            b = icof / norm;
            c = jcof / norm;
            d = kcof / norm;

            //returns the inverted quaternion
            return new Qtrin(a, -b, -c, -d);
        }

        /// <summary>
        /// Every quaternion can be decomposed into one of two complex numbers,
        /// which are conjagets. This method returns the number that has a
        /// positive imaginary coffecent.
        /// </summary>
        /// <returns>The degenerate from of the quaternion</returns>
        public Cmplx Decomp()
        {
            //calculates the magnitude of the vector portion
            double temp = (icof * icof) + (jcof * jcof) + (kcof * kcof);

            //builds the degenerate complex number
            return new Cmplx(real, Math.Sqrt(temp));
        }

        /// <summary>
        /// The versor of a quaternion is an imaginary unit vector, formed by
        /// the basis vectors (I), (J), and (K). It can be thought of as the
        /// direction the quaternion points in imaginary space. It is precicly
        /// the information left-over by the complex degenerate.
        /// </summary>
        /// <returns>The versor of the quaternion</returns>
        public Qtrin Versor()
        {
            //calculates the magnitude of the versor portion
            double temp = 1.0 / this.ImAbs;

            //we default to the (I) axis if we have no direction
            if (Double.IsInfinity(temp)) return Qtrin.I;

            //normalises the versor portion
            double ui = icof * temp;
            double uj = jcof * temp;
            double uk = kcof * temp;

            //builds the new unit quaternion
            return new Qtrin(0.0, ui, uj, uk);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Binary Operations...

        /// <summary>
        /// Adds the opperand to the curent quaternion.
        /// </summary>
        /// <param name="q">The number to add</param>
        /// <returns>The sum of the quaternions</returns>
        /// <remarks>It overloads the (+) opperator</remarks>
        public Qtrin Add(Qtrin q)
        {
            double a, b, c, d;

            //dose a peicewise adition of all compoents
            a = real + q.real;
            b = icof + q.icof;
            c = jcof + q.jcof;
            d = kcof + q.kcof;

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Subtracts the opperand from the curent quaternion.
        /// </summary>
        /// <param name="q">The number to subtract</param>
        /// <returns>The diffrence of the quaternions</returns>
        /// <remarks>It overloads the (-) opperator</remarks>
        public Qtrin Sub(Qtrin q)
        {
            double a, b, c, d;

            //dose a peicewise subtraction of all compoents
            a = real - q.real;
            b = icof - q.icof;
            c = jcof - q.jcof;
            d = kcof - q.kcof;

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Multiplys the current quaternion by the opperand. Note that
        /// quaternion multiplication is non-comunitive, meaning that the
        /// order in which two quaternions are multiplied is important.
        /// </summary>
        /// <param name="q">The number used to mutiply</param>
        /// <returns>The product of the quaternions</returns>
        /// <remarks>It overloads the (*) opperator</remarks>
        public Qtrin Mult(Qtrin q)
        {
            //computes the real portion of the procuct
            double a = real * q.real;
            a = a - (icof * q.icof);
            a = a - (jcof * q.jcof);
            a = a - (kcof * q.kcof);

            //computes the (I) coffecient of the product
            double b = real * q.icof;
            b = b + (icof * q.real);
            b = b + (jcof * q.kcof);
            b = b - (kcof * q.jcof);

            //computes the (J) coffecient of the product
            double c = real * q.jcof;
            c = c - (icof * q.kcof);
            c = c + (jcof * q.real);
            c = c + (kcof * q.icof);

            //computes the (K) coffecient of the product
            double d = real * q.kcof;
            d = d + (icof * q.jcof);
            d = d - (jcof * q.icof);
            d = d + (kcof * q.real);

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Multiplies the quaternion by a real scalar value. Note that unlike
        /// generic quaternion multiplicaiton, scalar multiplication is always
        /// communitive. It effectlivly scales the quaternion by the given value. 
        /// </summary>
        /// <param name="s">Scalor value</param>
        /// <returns>The scaled quaternion</returns>
        /// <remarks>It overloads the (*) and (/) opperators</remarks>
        public Qtrin Mult(double s)
        {
            double a, b, c, d;

            //scales each coffecent by the given abount
            a = real * s;
            b = icof * s;
            c = jcof * s;
            d = kcof * s;

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Divides the current quaternion by the opperand, by multiplying
        /// by the inverse opperand. This is known as dividing on the right.
        /// Reversing the multiplication results in division on the left, which
        /// may not be the same due to the non-comunitivity of multiplication.
        /// </summary>
        /// <param name="q">The number used to devide</param>
        /// <returns>The right-hand quotient of the two numbers</returns>
        /// <remarks>It overloads the (/) opperator</remarks>
        public Qtrin Div(Qtrin q)
        {
            //multiplies by the recprical on the right
            return Mult(q.Inv());
        }

        #endregion //////////////////////////////////////////////////////////

        #region Metric Implementation...

        /// <summary>
        /// Conputes the euclidian distance between two quaternions
        /// within the quaternion space. This is the same as the absolute
        /// value of the number's diffrence.
        /// </summary>
        /// <param name="q">The other number</param>
        /// <returns>The distance between the numbers in quaternion space</returns>
        public double Dist(Qtrin q)
        {
            double xr, yr, zr, wr;

            //subtracts the two parts seperatly
            xr = q.real - real;
            yr = q.icof - icof;
            zr = q.jcof - jcof;
            wr = q.kcof - kcof;

            //squares each sub-component
            xr = xr * xr;
            yr = yr * yr;
            zr = zr * zr;
            wr = wr * wr;

            //computes the sum of the squares
            return Math.Sqrt(xr + yr + zr + wr);
        }

        /// <summary>
        /// Computes the magnitude of the current quaternion. This
        /// is exactly the same as the absolute value.
        /// </summary>
        /// <returns>The magnitude of the quaternion</returns>
        public double Mag()
        {
            //simply returns the asbolute value
            return this.Abs;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Transindental Functions...

        /// <summary>
        /// This method extends the domain of the exponintial function
        /// (defined by euler's constant) for the entire quaternion feild. 
        /// It is a single-valued function for all quaternions.
        /// </summary>
        /// <param name="q">Power of the exponential</param>
        /// <returns>Euler's constant raised to a quaternion power</returns>
        public static Qtrin Exp(Qtrin q)
        {
            double exp, vn, real, imag, i, j, k;

            //computes the vector norm and exponential
            exp = Math.Exp(q.real);
            vn = q.ImAbs;

            //computes the real and imaginary components
            real = exp * Math.Cos(vn) / 1.0;
            imag = exp * Math.Sin(vn) / vn;

            //checks if the answer is real
            if (vn.IsZero()) return new Qtrin(real);

            //builds the rest from the unit vector
            i = q.icof * imag;
            j = q.jcof * imag;
            k = q.kcof * imag;

            return new Qtrin(real, i, j, k);
        }

        /// <summary>
        /// This metnod generalises the quaternion exponential function, 
        /// allowing for any positive real base. It is also uniquly 
        /// defined for all quaternions under these restrictions.
        /// </summary>
        /// <param name="q">Power of the exponential</param>
        /// <param name="bass">Base of the exponential</param>
        /// <returns>A given base raised to a quaternion power</returns>
        public static Qtrin Exp(Qtrin q, double bass)
        {
            double vn, temp1, temp2, real, imag, i, j, k;

            //takes care of the special casses
            if (bass.IsZero()) return new Qtrin();
            if (bass < 0.0) return Qtrin.NaN;

            //saves the norm of the imaginary vector
            vn = q.ImAbs;

            //calculates the result in the complex plain
            temp1 = Math.Pow(bass, q.CofR);
            temp2 = vn * Math.Log(bass);
            real = temp1 * Math.Cos(temp2) / 1.0;
            imag = temp1 * Math.Sin(temp2) / vn;

            //checks if the answer is real
            if (vn.IsZero()) return new Qtrin(real);

            //builds the rest from the unit vector
            i = q.icof * imag;
            j = q.jcof * imag;
            k = q.kcof * imag;

            return new Qtrin(real, i, j, k);
        }

        /// <summary>
        /// Computes the principle value of the quaternion logarythim.
        /// This is only one possable solution for the exponential
        /// function, as the real logarythim is multi-valued.
        /// </summary>
        /// <param name="q">Argument of the logarythim</param>
        /// <returns>The natural log of a quaternion</returns>
        public static Qtrin Log(Qtrin q)
        {
            double vn, qn, imag, real, i, j, k;

            //stores both of the vector norms
            vn = q.ImAbs;
            qn = q.Abs;

            //computes the real and imaginary components
            imag = Math.Acos(q.real / qn) / vn;
            real = Math.Log(qn);

            //checks if the answer is real
            if (vn.IsZero()) return new Qtrin(real);

            //builds the rest from the unit vector
            i = q.icof * imag;
            j = q.jcof * imag;
            k = q.kcof * imag;

            return new Qtrin(real, i, j, k);
        }

        /// <summary>
        /// This method generalises the quaternion logarythim, allowing 
        /// for any positive real base (except for base-1). Like the 
        /// natural logarythim, it also has infinatly many solutions. 
        /// </summary>
        /// <param name="q">Argument of the logarythim</param>
        /// <param name="bass">Base of the logarythim</param>
        /// <returns>The log of a quaternion in the given base</returns>
        public static Qtrin Log(Qtrin q, double bass)
        {
            //return NaN if the base is invalid
            if (bass <= 0.0) return Qtrin.NaN;

            //invokes the change of base forumla
            double temp = 1.0 / Math.Log(bass);
            return Qtrin.Log(q).Mult(temp);
        }

        /// <summary>
        /// Raises a quaternion to an arbitrary, real power. In general, 
        /// this function may have more that one solution. The value 
        /// returned by this method is the principle value.
        /// </summary>
        /// <param name="q">Base of the exponintial</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A quaternion raised to a given power</returns>
        public static Qtrin Pow(Qtrin q, double exp)
        {
            double vn, qn, temp1, temp2, real, imag, i, j, k;

            //stores both of the vector norms
            vn = q.ImAbs;
            qn = q.Abs;

            //calculates the result in the complex plain
            temp1 = Math.Pow(qn, exp);
            temp2 = Math.Acos(q.real / qn) * exp;
            real = temp1 * Math.Cos(temp2) / 1.0;
            imag = temp1 * Math.Sin(temp2) / vn;

            //checks if the answer is real
            if (vn.IsZero()) return new Qtrin(real);

            //builds the rest from the unit vector
            i = q.icof * imag;
            j = q.jcof * imag;
            k = q.kcof * imag;

            return new Qtrin(real, i, j, k);
        }

        /// <summary>
        /// This is the most general form of the exponential, as it allows any
        /// quaternion to be raised to any other quaternion. Not only is it
        /// mulit-valued, it has diffrent left and right derivations as well,
        /// similar to quaternion division. If either argument is known to be
        /// real, it is advised you use one of the other methods.
        /// </summary>
        /// <param name="q">Base of the exponential</param>
        /// <param name="exp">Power of the exponential</param>
        /// <returns>A quaternion raised to a quaternion power</returns>
        public static Qtrin Pow(Qtrin q, Qtrin exp)
        {
            //allows for computation of zero, avoiding ln(0)
            if (q.Abs.IsZero()) return new Qtrin();

            //uses the definition of the general exponential
            return Qtrin.Exp(Qtrin.Log(q).Mult(exp));
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Value Checking Functions...

        /// <summary>
        /// Determins if quaternion is infinate. If any part of the quaternion
        /// is infinate, then the entire quaternion is considred to be infinate.
        /// </summary>
        /// <returns>True if the quaternion is infinate</returns>
        public bool IsInfinity()
        {
            bool test = false;

            test |= Double.IsInfinity(real);
            test |= Double.IsInfinity(icof);
            test |= Double.IsInfinity(jcof);
            test |= Double.IsInfinity(kcof);

            return test;
        }

        /// <summary>
        /// Determins if the quaternion contains a NaN (Not a Number) value. If
        /// any part of the quaternion evaluates to NaN then the entire quaternion
        /// evaluates to NaN.
        /// </summary>
        /// <returns>True if the quaternion is a NaN value</returns>
        public bool IsNaN()
        {
            bool test = false;

            test |= Double.IsNaN(real);
            test |= Double.IsNaN(icof);
            test |= Double.IsNaN(jcof);
            test |= Double.IsNaN(kcof);

            return test;
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Creates a new quaternion through the real constructor.
        /// </summary>
        public static implicit operator Qtrin(Double n)
        { 
            return new Qtrin(n); 
        }

        /// <summary>
        /// converts a quaternion to a real number if the contribution of 
        /// the imaginary part is neglegable. Otherwise, it returns NaN. 
        /// </summary>
        public static explicit operator Double(Qtrin q)
        {
            //determins if the imaginary part is neglegable
            bool is_real = q.ImAbs < VMath.TOL;
            return (is_real) ? q.CofR : Double.NaN;
        }

        /// <summary>
        /// Creates a new quaternion through the complex constructor.
        /// </summary>
        public static implicit operator Qtrin(Cmplx z)
        { 
            return new Qtrin(z); 
        }

        /// <summary>
        /// Generates a complex number through decomposition.
        /// </summary>
        public static explicit operator Cmplx(Qtrin q)
        { 
            return q.Decomp();
        }

        /// <summary>
        /// Converts the complex number to a 4D vector containing the 
        /// real and imaginary coffecents as elements.
        /// </summary>
        public static implicit operator Vector(Qtrin q)
        { 
            return new Vector(q.real, q.icof, q.jcof, q.kcof); 
        }

        /// <summary>
        /// Converts a vector to a quaternion by treating the first four
        /// elements as the real and imaginary parts respectivly.
        /// </summary>
        public static explicit operator Qtrin(Vector v)
        {
            //extracts the vector components
            double r = v.GetExtended(0);
            double i = v.GetExtended(1);
            double j = v.GetExtended(2);
            double k = v.GetExtended(3);

            //generates the new quaternion
            return new Qtrin(r, i, j, k);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Operator Overlodes...

        //refferences the Add(q) function
        public static Qtrin operator +(Qtrin p, Qtrin q)
        { return p.Add(q); }

        //references the Sub(q) function
        public static Qtrin operator -(Qtrin p, Qtrin q)
        { return p.Sub(q); }

        //references the Mult(q) function
        public static Qtrin operator *(Qtrin p, Qtrin q)
        { return p.Mult(q); }

        //references the Div(q) function
        public static Qtrin operator /(Qtrin p, Qtrin q)
        { return p.Div(q); }

        //refrences the Mult(s) funciton
        public static Qtrin operator *(Qtrin p, Double s)
        { return p.Mult(s); }

        //references the Mult(s) function
        public static Qtrin operator *(Double s, Qtrin p)
        { return p.Mult(s); }

        //references the Mult(s) function
        public static Qtrin operator /(Qtrin p, Double s)
        { return p.Mult(1.0 / s); }

        //refrences the Neg() function
        public static Qtrin operator -(Qtrin q)
        { return q.Neg(); }

        //refrences the Conj() function
        public static Qtrin operator ~(Qtrin q)
        { return q.Conj(); }

        #endregion ////////////////////////////////////////////////////////////

    }
}
