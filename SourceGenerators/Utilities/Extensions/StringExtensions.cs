using System.Globalization;
using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators
{
    public static class StringExtensions
    {
        private const string SplitRegexStr = "[ _-]+|(?<=[a-z])(?=[A-Z])";
        private static readonly Regex SplitRegex = new(SplitRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        //public static string ToTitleCase(this string source)
        //    => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);

        public static string ToTitleCase(this string source)
            => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SplitRegex.Replace(source, " ").ToLower());
    }
}
