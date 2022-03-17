using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators
{
    public static class RegexExtensions
    {
        public static string GetGroupsAsStr(this Regex source, Match match)
        {
            return $"[{string.Join(", ", GetGroupsAsStr())}]";

            IEnumerable<string> GetGroupsAsStr()
            {
                foreach (var name in source.GetGroupNames().Skip(1))
                {
                    var value = match.Groups[name].Value;
                    if (value is "") continue;
                    yield return $"{name}: {value}";
                }
            }
        }
    }
}
