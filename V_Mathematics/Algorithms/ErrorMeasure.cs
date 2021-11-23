using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc.Algorithms
{
    /// <summary>
    /// Represents diffrent types of error measurment, ment to be used in itterative methods
    /// designed to run untill some desired level of precission is reached. The actual significance
    /// of this precision is determened by the error measurment in question.
    /// </summary>
    public enum ErrorMeasure
    {
        /// <summary>
        /// Uses the distance between the actual and expected values, as the measurment of
        /// error. This is usefull if you need to know the exact range an error value represents,
        /// some value plus or minus some offset. However, this might not be best for all sinerios,
        /// as large values will have the same error as small values, even if only off by one.
        /// </summary>
        Absolute = 0,

        /// <summary>
        /// Takes the absolute error and scales it by the size of the expected value, thus making 
        /// it usefull for values of all sizes. If exact error represents an exact offset, then 
        /// relative error represents some precentage offset. It is usefull if you need a certin
        /// number of signifiant figures. Unfortunatly it dose not work in all cases. In paticular
        /// it dose not work if the expected value is zero.
        /// </summary>
        Relative = 1,

        /// <summary>
        /// Agumented error dose not corispond with some offset, rather it can be thought of as a
        /// blend between the absolute and reletive measuments. It behaves like absolute error when 
        /// the expected value is small, and relitive error when it is large. If you are uncertan
        /// whether absolute or reletive error would be more apropriate, augmented error is a good
        /// compromise between the two. It also avoids the divide by zero issue that relative error
        /// sometimes has.
        /// </summary>
        Augmented = 2



    }
}
