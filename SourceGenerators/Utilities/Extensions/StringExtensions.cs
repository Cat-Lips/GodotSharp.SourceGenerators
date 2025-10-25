using System.Globalization;
using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators;

internal static class StringExtensions
{
    private const string SplitRegexStr = "[ _-]+|(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])";
    private static readonly Regex SplitRegex = new(SplitRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string UnsafeCharsRegexStr = @"[^\w]+";
    private static readonly Regex UnsafeCharsRegex = new(UnsafeCharsRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string UnsafeFirstCharRegexStr = "^[^a-zA-Z_]+";
    private static readonly Regex UnsafeFirstCharRegex = new(UnsafeFirstCharRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static string ToTitleCase(this string source)
        => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(SplitRegex.Replace(source, " ").ToLowerInvariant());

    public static string ToSafeName(this string source)
        => source.ToTitleCase().Replace(" ", "").ReplaceUnsafeChars(capitalise: false);

    public static string ReplaceUnsafeChars(this string source, string with = "_", bool capitalise = true)
    {
        source = UnsafeCharsRegex.Replace(source, with);
        return UnsafeFirstCharRegex.IsMatch(source) ? $"{with}{source}"
            : capitalise ? source.Capitalise() : source;
    }

    public static string Capitalise(this string source)
        => source.Length > 0 ? char.ToUpperInvariant(source[0]) + source[1..] : source;

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

    public static string NullIfEmpty(this string source)
        => source is "" ? null : source;

    public static string ToPascalCase(this string source)
    {
        return string.Join("", source
            .Split(['_'], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => char.ToUpperInvariant(x[0]) + (x.Length > 1 ? x[1..] : "")));
    }

    public static string Format(this object[] source, Func<string, string> format, string sep = ", ") => source.Format(sep, format);
    public static string Format(this object[] source, string sep = ", ", Func<string, string> format = null)
    {
        var x = string.Join(sep, source);
        return format?.Invoke(x) ?? x;
    }

    public static T[] NullIfEmpty<T>(this IEnumerable<T> source)
        => source?.Any() is null or false ? null : [.. source];

    public static string Join(this IEnumerable<object> source, string sep)
        => string.Join(sep, source);
}
