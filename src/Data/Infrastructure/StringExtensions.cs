using System;

namespace Data.Infrastructure
{
    public static class StringExtensions
    {
        public static string AddHeading(this string headingText, string heading)
        {
            return headingText += heading + "\r\n\r\n";
        }
    }
}
