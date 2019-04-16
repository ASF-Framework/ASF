using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class DateTimeExtension
    {
        public static long ToUnixTime64(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
        }

        public static int ToUnixTime32(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((date.ToUniversalTime() - epoch).TotalSeconds);
        }
    }
}
