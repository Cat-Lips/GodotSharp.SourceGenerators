using System.Globalization;
using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators;

internal static class StringExtensions
{
    private const string SplitRegexStr = "[ _-]+|(?<=[a-z])(?=[A-Z])";
    private static readonly Regex SplitRegex = new(SplitRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string UnsafeCharsRegexStr = @"[^\w]+";
    private static readonly Regex UnsafeCharsRegex = new(UnsafeCharsRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string UnsafeFirstCharRegexStr = "^[^a-zA-Z_]+";
    private static readonly Regex UnsafeFirstCharRegex = new(UnsafeFirstCharRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static string ToTitleCase(this string source)
        => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(SplitRegex.Replace(source, " ").ToLower());

    public static string ToSafeName(this string source)
    {
        source = source.ToTitleCase().Replace(" ", "");
        source = UnsafeCharsRegex.Replace(source, "_");
        return UnsafeFirstCharRegex.IsMatch(source) ? $"_{source}" : source;
    }

    public static string TrimPrefix(this string source, string prefix)
        => source.StartsWith(prefix, StringComparison.Ordinal) ? source[prefix.Length..] : source;

    public static string TrimSuffix(this string source, string suffix)
        => source.EndsWith(suffix, StringComparison.Ordinal) ? source[..^suffix.Length] : source;

    public static string AddPrefix(this string source, string prefix)
        => source.StartsWith(prefix, StringComparison.Ordinal) ? source : $"{prefix}{source}";

    public static string AddSuffix(this string source, string suffix)
        => source.EndsWith(suffix, StringComparison.Ordinal) ? source : $"{source}{suffix}";

    public static string Truncate(this string source, int maxChars)
        => source.Length <= maxChars ? source : source[..maxChars];

    public static string Join(this IEnumerable<string> source, string sep)
        => string.Join(sep, source);

    public static string NullIfEmpty(this string source)
        => source is "" ? null : source;

    public static string ToPascalCase(this string source)
    {
        return string.Join("", source
            .Split(['_'], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => char.ToUpperInvariant(x[0]) + (x.Length > 1 ? x[1..] : "")));
    }
}
