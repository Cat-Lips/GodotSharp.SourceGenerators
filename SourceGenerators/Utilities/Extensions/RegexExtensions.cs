using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators
{
    internal static class RegexExtensions
    {
        public static Dictionary<string, string> ToDictionary(this Group keys, Group values) => keys.Captures.ToDictionary(values.Captures);
        public static Dictionary<string, string> ToDictionary(this CaptureCollection keys, CaptureCollection values)
        {

            var i = -1;
            Debug.Assert(keys.Count == values.Count);
            return keys.Cast<Capture>().ToDictionary(x => x.Value, _ => values[++i].Value.Trim('"'));
        }

        public static string GetGroupsAsStr(this Regex source, Match match)
        {
            return $"[{string.Join(", ", GetGroupsAsStr())}]";

            IEnumerable<string> GetGroupsAsStr()
            {
                foreach (var name in source.GetGroupNames().Skip(1))
                {
                    var groupCaptures = match.Groups[name].Captures.Cast<Capture>().Select(x => x.Value);
                    var value = string.Join("|", groupCaptures);
                    if (value is "") continue;
                    yield return $"{name}: {value}";
                }
            }
        }
    }
}
