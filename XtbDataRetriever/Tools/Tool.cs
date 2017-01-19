using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtbDataRetriever.Tools
{
    class Tool
    {
        public static DateTime LongUnixTimeStampToDateTime(long? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(unixTimeStamp / 1000)).ToUniversalTime();
            return dtDateTime;
        }

        public static long LongDateTimeToUnixTimeStamp(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double timeStamp = (date.ToUniversalTime() - epoch).TotalSeconds;
            return Convert.ToInt64(timeStamp) * 1000;
        }

        public static DateTime DoubleUnixTimeStampToDateTime(double? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds((double)unixTimeStamp).ToUniversalTime();
        }

        public static double DoubleDateTimeToUnixTimeStamp(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 1, 0, 0, DateTimeKind.Utc);
            return (date.ToUniversalTime() - epoch).TotalSeconds;
        }
        
    }
}
