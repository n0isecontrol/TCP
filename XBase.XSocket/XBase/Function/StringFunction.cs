using System;
using System.Globalization;

namespace XBase.Function
{
    public class StringFunction
    {
        /// <summary>
        /// Search Ignore case
        /// </summary>
        public static int IndexOfIgnoreCase(string vSource, string vFind)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(vSource, vFind, CompareOptions.IgnoreCase);
        }
    }
}

