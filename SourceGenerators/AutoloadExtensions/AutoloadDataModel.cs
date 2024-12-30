using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    internal class AutoloadDataModel : ClassDataModel
    {
        public record AutoloadData(string Name, string Type);

        public IList<AutoloadData> Autoloads { get; }

        public AutoloadDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot)
            : base(symbol)
        {
            Autoloads = AutoloadScraper.GetAutoloads(csPath, gdRoot)
                .Select(x => new AutoloadData(x.Name, GetType(x.Path)))
                .ToArray();

            static string GetType(string resource)
            {
                return
                    resource.EndsWith(".gd") ? "Node" :
                    resource.EndsWith(".cs") ? Path.GetFileNameWithoutExtension(resource) : // Assuming class name matches file name
                    resource.EndsWith(".tscn") ? "Node" : // TODO
                    resource.EndsWith(".scn") ? "Node" : // TODO?
                    "Node";
            }
        }

        protected override string Str()
            => string.Join("\n", Autoloads);
    }
}
