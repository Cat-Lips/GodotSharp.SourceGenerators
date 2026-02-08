using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators;

internal static class StringExtensions
{
    private const string SplitRegexStr = @"[_\W]+|(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|(?<=\d)(?=[A-Za-z])";
    private static readonly Regex SplitRegex = new(SplitRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private enum Case { Title, Camel, Pascal }
    public static string ToTitleCase(this string source) => source.SafeName(Case.Title);
    public static string ToCamelCase(this string source) => source.SafeName(Case.Camel);
    public static string ToPascalCase(this string source) => source.SafeName(Case.Pascal);
    private static string SafeName(this string source, Case @case)
    {
        return @case switch
        {
            Case.Title => SafeCase(" ", UppercaseFirstChar),
            Case.Camel => SafeCase("", LowercaseFirstWord),
            Case.Pascal => SafeCase("", UppercaseFirstChar),
            _ => throw new NotImplementedException($"Unknown {nameof(Case)}: {@case}"),
        };

        string SafeCase(string spacer, Func<string, int, char> FirstChar)
        {
            var parts = SplitRegex.Split(source).Where(x => x is not "")
                .Select((x, i) => FirstChar(x, i) + OtherChars(x));
            var result = string.Join(spacer, parts);
            return char.IsDigit(source[0]) ? $"_{result}" : result;

            static string OtherChars(string x)
                => x[1..].ToLowerInvariant();
        }

        static char UppercaseFirstChar(string x, int _ = default)
            => char.ToUpperInvariant(x[0]);

        static char LowercaseFirstChar(string x, int _ = default)
            => char.ToLowerInvariant(x[0]);

        static char LowercaseFirstWord(string x, int i)
            => i is 0 ? LowercaseFirstChar(x) : UppercaseFirstChar(x);
    }

    public static string AddPrefix(this string source, string prefix)
        => source.StartsWith(prefix, StringComparison.Ordinal) ? source : $"{prefix}{source}";

    public static string AddSuffix(this string source, string suffix)
        => source.EndsWith(suffix, StringComparison.Ordinal) ? source : $"{source}{suffix}";

    public static string TrimPrefix(this string source, string prefix)
        => source.StartsWith(prefix, StringComparison.Ordinal) ? source[prefix.Length..] : source;

    public static string TrimSuffix(this string source, string suffix)
        => source.EndsWith(suffix, StringComparison.Ordinal) ? source[..^suffix.Length] : source;

    public static string Truncate(this string source, int maxChars)
        => source.Length <= maxChars ? source : source[..maxChars];

    public static string NullIfEmpty(this string source)
        => source is "" ? null : source;

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
