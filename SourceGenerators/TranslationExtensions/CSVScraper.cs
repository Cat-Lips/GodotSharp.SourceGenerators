using NotVisualBasic.FileIO;

using Data = (System.Collections.Generic.ICollection<string> Locs, System.Collections.Generic.ICollection<(string Key, string Plural, string Context)> Keys);
using Keys = System.Collections.Generic.ICollection<(string Key, string Plural, string Context)>;
using Locs = System.Collections.Generic.ICollection<string>;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

internal static class CSVScraper
{
    public static Data ParseCSV(string csv, char sep, out bool HasPlurals, out bool HasContext)
    {
        Log.Debug($"Parsing {csv}");
        ParseCSV(out var locs, out var keys, out HasPlurals, out HasContext);
        return new Data(locs, keys);

        void ParseCSV(out Locs locs, out Keys keys, out bool HasPlurals, out bool HasContext)
        {
            using (var parser = new CsvTextFieldParser(csv))
            {
                parser.SetDelimiter(sep);
                parser.TrimWhiteSpace = true;

                keys = [];
                locs = null;
                HashSet<int> cols = [];
                int? pluralColumn = null;
                int? contextColumn = null;
                ParseRows(ref locs, ref keys);
                HasPlurals = pluralColumn is not null;
                HasContext = contextColumn is not null;

                void ParseRows(ref Locs locs, ref Keys keys)
                {
                    while (true)
                    {
                        var row = parser.ReadFields();
                        if (row is null) break;

                        Log.Debug($"Row: {string.Join("|", row)}");

                        if (IsValidRow())
                        {
                            if (locs is null)
                                ParseHeader(ref locs);
                            else ParseRow(ref keys);
                        }

                        bool IsValidRow()
                        {
                            if (row.Length is 0) return false;
                            if (row[0].StartsWith("#")) return false;
                            if (row[0].StartsWith("?")) return false; // ?pluralrule
                            return !row.All(string.IsNullOrWhiteSpace);
                        }

                        void ParseHeader(ref Locs locs)
                        {
                            locs = [.. GetLocs(row)];
                            Log.Debug($" - Locs: {string.Join("|", locs)}");

                            IEnumerable<string> GetLocs(IEnumerable<string> header)
                            {
                                foreach (var (loc, idx) in header.Select((x, i) => (x, i)))
                                {
                                    if (idx is 0 && loc.ContainsN("key")) continue;
                                    if (loc.StartsWith("_")) continue; // Ignore comment columns
                                    if (string.IsNullOrEmpty(loc)) continue; // Ignore empty columns
                                    if (loc is "?plural") { pluralColumn = idx; continue; }
                                    if (loc is "?context") { contextColumn = idx; continue; }

                                    cols.Add(idx);
                                    yield return loc;
                                }
                            }
                        }

                        void ParseRow(ref Keys keys)
                        {
                            var key = row[0];
                            if (key is "") return;

                            keys.Add((key, Plural(), Context()));

                            string Plural()
                            {
                                if (pluralColumn is null) return null;
                                var plural = row[pluralColumn.Value];
                                return plural.NullIfEmpty();
                            }

                            string Context()
                            {
                                if (contextColumn is null) return null;
                                var context = row[contextColumn.Value];
                                return context.NullIfEmpty();
                            }
                        }
                    }
                }
            }
        }
    }
}
