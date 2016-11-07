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
    /// There are many ways to visuilise the quaternions. At their core, they contain
    /// one real component, and three imaginary components, all orthoginal to each other.
    /// They can also be thought of as a pair of complex numbers, through Cayely-Dickson
    /// expantion. Practaly anything that can be done with complex numbers can also be
    /// done with quaternions. Multiplication, however, is non-comunitive, so care must
    /// be taken when using algbracic laws that imply comunitivity.
    /// </summary>
    /// <remarks>Last Update: 2013-09-22</remarks>
    public struct Qtrin : Algebraic<Qtrin, Double>
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
        /// Converts a vector to a quaternion.
        /// </summary>
        /// <param name="v">Vector to convert</param>
        public Qtrin(Vector v)
        {
            //initialises values
            real = v.GetElement(0);
            icof = v.GetElement(1);
            jcof = v.GetElement(2);
            kcof = v.GetElement(3);
        }

        /// <summary>
        /// Converts a complex number to a quaternion.
        /// </summary>
        /// <param name="z">Value to convert</param>
        public Qtrin(Cmplx z)
        {
            //initialises values
            jcof = kcof = 0.0;
            real = z.CofR;
            icof = z.CofI;
        }

        /// <summary>
        /// Constructs a quaternion from a pair of complex numbers, based
        /// on the Cayely-Dickson expantion. In this way, a quaterion can
        /// be thought of as a nested complex number, where each imaginary
        /// axis is unique and orthoginal to each other.
        /// </summary>
        /// <param name="first">The first complex number</param>
        /// <param name="second">The second complex number</param>
        public Qtrin(Cmplx first, Cmplx second)
        {
            //sets each of the cofecents individualy
            real = first.CofR;
            icof = first.CofI;
            jcof = second.CofR;
            kcof = second.CofI;
        }

        /// <summary>
        /// Generates a string representation of the quaternion.
        /// </summary>
        /// <returns>The quaternion as a string</returns>
        public override string ToString()
        {
            //uses the default format
            return ToString("0.000");
        }

        /// <summary>
        /// Generates a string representation of the current quaternion, 
        /// using a custom format. A seperate format can be provided for
        /// all four coffecents, by seperating them by a virtical pipe.
        /// </summary>
        /// <param name="format">A custom format string</param>
        /// <returns>The number as a string</returns>
        public string ToString(string format)
        {
            //splits the format string around the comma
            string[] f = format.Split('|');
            string xs = (f.Length < 4) ? format : f[0];
            string ys = (f.Length < 4) ? format : f[1];
            string zs = (f.Length < 4) ? format : f[2];
            string ws = (f.Length < 4) ? format : f[3];

            //formats the diffrent parts seperatly
            xs = real.ToString(xs);
            ys = icof.ToString(ys);
            zs = jcof.ToString(zs);
            ws = kcof.ToString(ws);

            //builds and returns the output
            return String.Format("({0}, {1}, {2}, {3})", 
                xs, ys, zs, ws);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The coffecent of the real compoent (1) making up the
        /// quaternion. Read-Only
        /// </summary>
        public double CofR
        {
            get { return real; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (I) makeing up
        /// the quaternion. Read-Only
        /// </summary>
        public double CofI
        {
            get { return icof; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (J) makeing up
        /// the quaternion. Read-Only
        /// </summary>
        public double CofJ
        {
            get { return jcof; }
        }

        /// <summary>
        /// The coffecent of the imaginary component (K) makeing up
        /// the quaternion. Read-Only
        /// </summary>
        public double CofK
        {
            get { return kcof; }
        }

        /// <summary>
        /// Obtains the absolute value. In quaternion space this is 
        /// equivlent to the radius, or the distance from the origin. 
        /// Read-Only
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
        /// Computes the negative of the current quaternion. It
        /// overloads the uinary (-) operatoer.
        /// </summary>
        /// <returns>The negative quaternion</returns>
        public Qtrin Neg()
        {
            //negates each peice of the quaternion
            return new Qtrin(-real, -icof, -jcof, -kcof);
        }

        /// <summary>
        /// Computes the conjgate of the curent quaternion, defined
        /// by the negation of the imaginary coefecents. It overloads
        /// the (~) opperator.
        /// </summary>
        /// <returns>The quaternionic conjigate</returns>
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

            //inverts each of the four parts of the quaternion
            double a = real / norm;
            double b = icof / norm;
            double c = jcof / norm;
            double d = kcof / norm;

            //returns the inverted quaternion
            return new Qtrin(a, -b, -c, -d);
        }

        /// <summary>
        /// Every quaternion can be decomposed into one of two complex numbers,
        /// which are conjagets. This method returns the number that has a
        /// positive imaginary coffecent.
        /// </summary>
        /// <returns>The decomposed from of the quaternion</returns>
        public Cmplx Decomp()
        {
            //calculates the magnitude of the vector portion
            double temp = (icof * icof) + (jcof * jcof) + (kcof * kcof);

            //builds the degenerate complex number
            return new Cmplx(real, Math.Sqrt(temp));
        }

        /// <summary>
        /// The versor is the purly directional, purly imaginary, part of the
        /// quaternion. It can be thought of as a unit vector in the three
        /// dimentional space fromed by the imaginary axies (I), (J), and (K).
        /// It is exactly the information left-over after decomposing a 
        /// quaternion into a complex number.
        /// </summary>
        /// <returns>The versor of the quaternion</returns>
        public Qtrin Versor()
        {
            //calculates the magnitude of the versor portion
            double temp = (icof * icof) + (jcof * jcof) + (kcof * kcof);
            temp = 1.0 / Math.Sqrt(temp);

            //we default to the (I) axis if we have no direction
            if (Double.IsInfinity(temp) || temp <= 0.0)
            return new Qtrin(0.0, 1.0, 0.0, 0.0);

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
        /// Adds the opperand to the curent quaternion and returns 
        /// the result. It overloads the (+) opperator.
        /// </summary>
        /// <param name="q">The number to add</param>
        /// <returns>The sum of the quaternions</returns>
        public Qtrin Add(Qtrin q)
        {
            //dose a peicewise adition of all compoents
            double a = real + q.real;
            double b = icof + q.icof;
            double c = jcof + q.jcof;
            double d = kcof + q.kcof;

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Subtracts the opperand from the curent quaternion and returns 
        /// the result. It overloads the (-) opperator.
        /// </summary>
        /// <param name="q">The number to subtract</param>
        /// <returns>The diffrence of the quaternions</returns>
        public Qtrin Sub(Qtrin q)
        {
            //dose a peicewise subtraction of all compoents
            double a = real - q.real;
            double b = icof - q.icof;
            double c = jcof - q.jcof;
            double d = kcof - q.kcof;

            //generates the resultant quaternion
            return new Qtrin(a, b, c, d);
        }

        /// <summary>
        /// Multiplys the current quaternion by the opperand. Note that
        /// quaternion multiplication is non-comunitive, meaning that the
        /// order in which two quaternions are multiplied can affect the
        /// retult. It overloads the (*) opperator.
        /// </summary>
        /// <param name="q">The number used to mutiply</param>
        /// <returns>The product of the quaternions</returns>
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
        /// Computes the right-handed division of the current quaternion
        /// by the opperand. That is, it multiplies the curent number
        /// by the recpricial of the operanad. It overloads the (/) opperator.
        /// </summary>
        /// <param name="q">The number used to devide</param>
        /// <returns>The right-hand quotient of the two numbers</returns>
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
        /// <returns>The distance between the numbers in quaternion space
        /// </returns>
        public double Dist(Qtrin q)
        {
            //subtracts the two parts seperatly
            double xr = q.real - real;
            double yr = q.icof - icof;
            double zr = q.jcof - icof;
            double wr = q.kcof - icof;

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

        #region Class Conversions...

        //creates a new quaternion through the constructor
        public static implicit operator Qtrin(Cmplx z)
        { return new Qtrin(z); }

        //generates a complex number through decomposition
        public static explicit operator Cmplx(Qtrin q)
        { return q.Decomp(); }

        //creates a new quaternion through the constructor
        public static implicit operator Qtrin(Double n)
        { return new Qtrin(n); }

        //returns the real component, ingnoring the imaginary
        public static explicit operator Double(Qtrin n)
        { return n.real; }

        //creates a vector containing the real and imagnary components
        public static implicit operator Vector(Qtrin n)
        { return new Vector(n.real, n.icof, n.jcof, n.kcof); }

        //converts the vector by calling the constructor
        public static explicit operator Qtrin(Vector v)
        { return new Qtrin(v); }

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

        //refrences the Neg() function
        public static Qtrin operator -(Qtrin q)
        { return q.Neg(); }

        //refrences the Conj() function
        public static Qtrin operator ~(Qtrin q)
        { return q.Conj(); }

        #endregion ////////////////////////////////////////////////////////////

        Qtrin Euclidean<Qtrin, Double>.Mult(double scalar)
        { return this.Mult((Qtrin)scalar); }

    }
}
