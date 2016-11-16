using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Calc
{
    /// <summary>
    /// Contains extention methods for the IFormattable interface.
    /// </summary>
    /// <remarks>Last Update: 2016-11-14</remarks>
    public static class FormatExtentions
    {
        /// <summary>
        /// Provides a short-hand method for formating objects that implement the
        /// IFormatable interface, when the system-default format provider is required,
        /// or when the format provider is not used by the underlying implementation.
        /// </summary>
        /// <param name="obj">Object to be formated</param>
        /// <param name="format">A format defining string</param>
        /// <returns>A formated representation of the object as a string</returns>
        public static string ToString(this IFormattable obj, string format)
        {
            return obj.ToString(format, null);
        }


    }
}
