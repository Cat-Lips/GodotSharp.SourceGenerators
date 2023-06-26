using System.Globalization;
using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators
{
    internal static class StringExtensions
    {
        private const string SplitRegexStr = "[ _-]+|(?<=[a-z])(?=[A-Z])";
        private static readonly Regex SplitRegex = new(SplitRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static string ToTitleCase(this string source)
            => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SplitRegex.Replace(source, " ").ToLower());

        public static string Truncate(this string source, int maxChars)
            => source.Length <= maxChars ? source : source[..maxChars];
    }
}
