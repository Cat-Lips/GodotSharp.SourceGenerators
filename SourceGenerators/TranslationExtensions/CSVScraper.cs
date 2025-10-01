using System.Text.RegularExpressions;
using NotVisualBasic.FileIO;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

using Data = (ICollection<string> Locs, ICollection<(string Key, int Args)> Keys);
using Keys = ICollection<(string Key, int Args)>;
using Locs = ICollection<string>;

internal static class CSVScraper
{
    private const string ArgRegexStr = @"{(?<ArgPos>\d+)";
    private static readonly Regex ArgRegex = new(ArgRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static Data ParseCSV(string csv)
    {
        Log.Debug($"Parsing {csv}");
        ParseCSV(out var locs, out var keys);
        return new Data(locs, keys);

        void ParseCSV(out Locs locs, out Keys keys)
        {
            using (var parser = new CsvTextFieldParser(csv))
            {
                keys = [];
                locs = null;
                HashSet<int> cols = [];
                while (true)
                {
                    var row = parser.ReadFields();
                    if (row is null) return;

                    Log.Debug($"Row: {string.Join("|", row)}");

                    if (row.Length is 0) continue;
                    if (row[0].StartsWith("#")) continue;
                    if (row.All(string.IsNullOrWhiteSpace)) continue;

                    if (locs is null)
                    {
                        locs = [.. GetLocs(row.Skip(1))];
                        Log.Debug($" - Locs: {string.Join("|", locs)}");
                        continue;
                    }

                    var key = row.First();
                    if (key is "") continue;

                    var args = CountArgs(row.Skip(1), key, locs);
                    Log.Debug($" - Key: {key} (Args: {args})");
                    keys.Add((key, args));
                }

                IEnumerable<string> GetLocs(IEnumerable<string> header)
                {
                    foreach (var (loc, idx) in header.Select((x, i) => (x.Trim(), i)))
                    {
                        if (!loc.StartsWith("_") &&     // Ignore comment columns
                            !string.IsNullOrEmpty(loc)) // Ignore empty columns
                        {
                            cols.Add(idx);
                            yield return loc;
                        }
                    }
                }

                int CountArgs(IEnumerable<string> cells, string dbg_key, Locs dbg_locs)
                {
                    var all = ArgCounts().ToArray();
                    Log.Debug($" - ArgCounts: {ArgCountStr()}");

                    return all.Distinct().Count() > 1
                        ? throw new Exception(ArgCountMismatch())
                        : all.FirstOrDefault();

                    IEnumerable<int> ArgCounts()
                    {
                        return cells
                            .Where((x, i) =>
                                cols.Contains(i) &&
                                !string.IsNullOrEmpty(x)) // Ignore empty cells (no translation)
                            .Select(ArgCount);

                        int ArgCount(string cell)
                        {
                            return MatchArgs(cell)
                                .DefaultIfEmpty(-1)
                                .Max() + 1;

                            IEnumerable<int> MatchArgs(string cell)
                            {
                                foreach (Match match in ArgRegex.Matches(cell))
                                {
                                    Log.Debug($" - ArgPos {ArgRegex.GetGroupsAsStr(match)}");
                                    yield return int.Parse(match.Groups["ArgPos"].Value);
                                }
                            }
                        }
                    }

                    string ArgCountMismatch()
                        => $"ArgCount Mismatch: {ArgCountStr()}";

                    string ArgCountStr()
                        => $"{dbg_key} [{string.Join(", ", all.Select((x, i) => $"{dbg_locs.ElementAt(i)}: {x}"))}]";
                }
            }
        }
    }
}
