/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2021 Benjamin Jacob Dawson
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

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace QuickTests
{
    public static class PadeAprox
    {

        /// <summary>
        /// Generates the coffecents of the [N/M] Pade Aproximate for a known
        /// funciton, given it's talor series up to order (N + M). It lists the
        /// An coeffencents first, followed by the Bn coeffeents. It is assumed
        /// that [B0 = 1] so it is excluded from the output.
        /// </summary>
        /// <param name="n">Degree of the Numerator</param>
        /// <param name="m">Degree of the Denominator</param>
        /// <param name="talor">The coffecents of the tailor series</param>
        /// <returns>The coffecents of the pade aproximate</returns>
        public static Vector GenPade(int n, int m, Vector talor)
        {
            int size = n + m + 1;
            Matrix trans = new Matrix(size, size);
            Vector input = new Vector(size);


            for (int i = 0; i < size; i++) //i = row
            {
                input[i] = -talor[i];

                for (int j = 0; j < size; j++) //j = col
                {
                    if (j <= n)
                    {
                        if (j == i) trans[i, j] = -1.0;
                        else trans[i, j] = 0.0;
                    }
                    else
                    {
                        int id = (i + n) - j; // -1;

                        if (id < 0) trans[i, j] = 0.0;
                        else trans[i, j] = talor[id];
                    }
                }
            }

            return trans.InvAx(input);

            //return trans * input;

            //return new Vector();
        }

        public static readonly double[] sine = 
        {
            0.0,
            1.0,
            0.0,
            -0.16666666666666666666666666666667,
            0.0,
            0.00833333333333333333333333333333,
            0.0,
            -1.984126984126984126984126984127e-4,
            0.0,
            2.7557319223985890652557319223986e-6,
            0.0,
            -2.5052108385441718775052108385442e-8,
            0.0,
            1.6059043836821614599392377170155e-10,
            0.0,
            -7.6471637318198164759011319857881e-13,
            0.0,
            2.8114572543455207631989455830103e-15,
            0.0,
            -8.2206352466243297169559812368723e-18,
            0.0,
        };

        public static readonly double[] exp =
        {
            1.0,
            1.0,
            0.5,
            0.16666666666666666666666666666667,
            0.04166666666666666666666666666667,
            0.00833333333333333333333333333333,
            0.00138888888888888888888888888889,
            1.984126984126984126984126984127e-4,
            2.4801587301587301587301587301587e-5,
            2.7557319223985890652557319223986e-6,
            2.7557319223985890652557319223986e-7,
            2.5052108385441718775052108385442e-8,
            2.0876756987868098979210090321201e-9,
            1.6059043836821614599392377170155e-10,
            1.1470745597729724713851697978682e-11,
            7.6471637318198164759011319857881e-13,
            4.7794773323873852974382074911175e-14,
            2.8114572543455207631989455830103e-15,
            1.5619206968586226462216364350057e-16,
            8.2206352466243297169559812368723e-18,
            4.1103176233121648584779906184361e-19,

        };


        public static readonly double[] jacobi_sn =
        {
            +0.0,                       //O(1) 
            +1.0,                       //O(x)
            +0.0,                       //O(x^2)
            -0.25,                      //O(x^3)
            +0.0,                       //O(x^4)
            +0.06875,                   //O(x^5)
            +0.0,                       //O(x^6)
            -0.0203125,                 //O(x^7)
            +0.0,                       //O(x^8)
            +5.891927083333333e-3,      //O(x^9)
            +0.0,                       //O(x^10)
            -1.7138671875e-3,           //O(x^11)
            +0.0,                       //O(x^12)
            +4.986259264823718e-4,      //O(x^13)
            +0.0,                       //O(x^14)
            -1.450469188201122e-4,      //O(x^15)
            +0.0,                       //O(x^16)
            +4.219439666195693e-5,      //O(x^17)
            +0.0,                       //O(x^18)
            -1.227441021040374e-5,      //O(x^19)
        };


        public static readonly double[] jacobi_sn2 =
        {
            0.0,
            1.0,
            0.0,
            -0.25,
            0.0,
            0.06875,
            0.0,
            -0.0203125,
            0.0,
            0.00589192708333333333333333333333,
            0.0,
            -0.0017138671875,
            0.0,
            4.9862592648237179487179487179487e-4,
            0.0,
            -1.4504691882011217948717948717949e-4,
            0.0,
            4.2194396661956925197963800904977e-5,
            0.0,
            -1.2274410210403741574754901960784e-5,
            0.0,
            3.5706401800258247141504839115133e-6,
            0.0,
            -1.0387036438081778732000612745098e-6,
            0.0,
            3.0216016669281467593093803026018e-7,
            0.0,
            -8.7898763515149499253373097433521e-8,
            0.0,
            2.5569858290202534912238620234952e-8,
            0.0,
            -7.4383031868082401467687632019745e-9,
            0.0,
            2.1638115342947234191237567313157e-9,
            0.0,
            -6.2945543337301652631958555700231e-10,
            0.0,
            1.8310935879667605801413921691327e-10,
            0.0,
            -5.326673740707422319763143507959674e-11,
            0.0,
            1.5495359345040360737862676100361493e-11,
            0.0,
        };


        public static readonly double[] F_Inv_Root2 =
        {
            0.0,
            1.0,
            0.0,
            0.11785113019775792073347406035081,
            0.0,
            0.01392977396044841585330518792984,
            0.0,
            1.7127408382368192172787708937961e-4,
            0.0,
            -6.3871680850746374913738293770737e-4,
            0.0,
            -2.5258833485591575809618898592039e-4,
            0.0,
            -5.6058346087761963445586736385841e-5,
            0.0,
            -3.8095577907650706027201946199897e-6,
            0.0,
            3.0066109390989652141118837329252957e-6,
            0.0,
            1.65567408337432145330918677606735886e-6,
            0.0,
        };


        public static readonly double[] SM_Series =
        {
            0.0,
            1.0,
            0.0,
            0.0,
            1.0 / 6.0,
            0.0,
            0.0,
            2.0 / 63.0,
            0.0,
            0.0,
            0.00573192239858906525573,
            0.0,
            0.0,
            0.00104011215122326233437,
            0.0,
            0.0,
            1.886279994745603211212e-4,
            0.0,
            0.0,
        };


        public static readonly double[] CL_Series =
        {
            1.0,
            0.0,
            -1.0,
            0.0,
            0.5,
            0.0,
            -0.3,
            0.0,
            0.175,
            0.0,
            -0.10166666666666666667,
            0.0,
            0.059166666666666666667,
            0.0,
            -0.034423076923076923077,
            0.0,
            0.0200272435897435897436,
            0.0,
            -0.0116519293614881850176,
            0.0,
            0.00677911953242835595777,
            0.0,
            -0.0039441082202111613876,
            0.0,
            0.0022946918991955756662,
            0.0,
            -0.0013350573804965773292,
            0.0,
            7.76739663904680873007e-4,
            0.0,
            -4.51909044831120232899e-4,
            0.0,
            2.62921792532621115961e-4,
            0.0,
        };


        public static readonly double[] SL_Series =
        {
            0.0,
            1.0,
            0.0,
            0.0,
            0.0,
            -0.1,
            0.0,
            0.0,
            0.0,
            1.0 / 120.0,
            0.0,
            0.0,
            0.0,
            -7.0512820512820512821e-4,
            0.0,
            0.0,
            0.0,
            5.96719457013574660633e-5,
            0.0,
            0.0,
            0.0,
            -5.0496480643539467069e-6,
            0.0,
            0.0,
            0.0,
            4.27319004524886877828e-7,
            0.0,
            0.0,
            0.0,
            -3.6161240380892432695e-8,
            0.0,
            0.0,
            0.0,
            3.06009161823495182834e-9,
            0.0,
            0.0,
            0.0,
        };

        public static readonly double[] W_Series =
        {
            1.0, 
            -1.0, 
            3.0 / 2.0, 
            -8.0 / 3.0, 
            125.0 / 24.0, 
            -54.0 / 5.0, 
            16807.0 / 720.0, 
            -16384.0 / 315.0, 
            531441.0 / 4480.0, 
            -156250.0 /567.0,
            649.78717234347442681,
            -1551.1605194805194805, 
            3741.4497029592385495,  
            -9104.5002411580189358,  
            22324.308512706601434,  
            -55103.621972903835338, 
            136808.86090394293563, 
            -341422.05066583836332,  
            855992.96599660755146,   
            -2154990.2060910882893,  
        };





        public const int PN = 8;
        public const int PM = 10;


        public static void TryPade()
        {
            Console.WriteLine("Constructing Pade Aproximate [{0}/{1}]: ", PN, PM);
            Console.WriteLine();

            Vector talor = new Vector(W_Series);
            Vector pade = GenPade(PN, PM, talor);

            for (int i = 0; i < pade.Length; i++)
            {
                if (i <= PN)
                {
                    Console.WriteLine("A{0:00} = {1:E18}", i, pade[i]);
                }
                else
                {
                    Console.WriteLine("B{0:00} = {1:E18}", i - PN, pade[i]);
                }

            }

            Console.WriteLine();

        }

    }
}
