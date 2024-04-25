using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using LayerName = (string MemberName, uint LayerValue);

namespace GodotSharp.SourceGenerators.LayerNamesExtensions
{
    internal class LayerNamesDataModel : ClassDataModel
    {
        public ILookup<string, LayerName> LayerNames { get; }

        public LayerNamesDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot) : base(symbol)
        {
            LayerNames = LayerNamesScraper
                .GetLayerNames(csPath, gdRoot)
                .ToLookup(x => SafeName(x.Category), x => new LayerName(SafeName(x.MemberName), x.LayerValue));

            static string SafeName(string source)
            {
                source = source.ToTitleCase().Replace(" ", "");

                // Replace invalid characters with underscores
                source = Regex.Replace(source, @"[^\w]+", "_");

                // Remove invalid characters from the start of the string
                const string ValidVariableNameRegexStr = @"^[^a-zA-Z_]+";
                if (Regex.IsMatch(source, ValidVariableNameRegexStr))
                {
                    source = "_" + source;
                }

                return source;
                
            }
        }

        protected override string Str()
        {
            return string.Join("\n", LayerCategories());

            IEnumerable<string> LayerCategories()
            {
                foreach (var lookup in this.LayerNames)
                {
                    foreach (var (MemberName, LayerValue) in lookup)
                        yield return $"ClassName: {lookup.Key}, MemberName: {MemberName}, LayerValue: {LayerValue}";
                }
            }
        }
    }
}
