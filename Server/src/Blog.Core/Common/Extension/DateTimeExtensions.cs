using System;

namespace Medium.Core.Common.Extension
{
    public static class DateTimeExtensions
    {
        public static DateTime DefaultFormat(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime
                .ToString("yyyy-MM-dd HH:mm"));
        }
    }
}
