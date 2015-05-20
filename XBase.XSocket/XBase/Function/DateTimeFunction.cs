/**********************************************************************
 * Package  XBase.Function
 * Class	DateTimeFunction.cs
 *********************************************************************/
using System;

namespace XBase.Function
{
    /// <summary>
    /// DateTimeFunction Class
    /// </summary>
    public class DateTimeFunction
    {
        /// <summary>
        /// Get week number based 0001-01-01
        /// </summary>
        /// <param name="vTime">DateTime</param>
        /// <returns>Number</returns>
        public static int GetWeekNumberForDateTime(DateTime vTime)
        {
            TimeSpan ts = vTime - DateTime.MinValue;
            int iWeek = (int)(ts.TotalDays / 7) + 1;
            return iWeek;
        }
    }
}

