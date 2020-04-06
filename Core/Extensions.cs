using System;

namespace MoneyManager.Core
{
    public static class Extensions
    {
        public static string ToISOString(this DateTime dt)
        {
            return dt.ToString("u").Replace(' ', 'T');
        }

        public static string ToISODateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
    }
}
