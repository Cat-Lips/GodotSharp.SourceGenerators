using Microsoft.CodeAnalysis;
using InputAction = (string GodotAction, string MemberName);
using NestedInputAction = (string ClassName, (string GodotAction, string MemberName) InputAction);

namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    internal class AutoloadDataModel : ClassDataModel
    {
        public IList<AutoloadNode> Autoloads { get; }

        public AutoloadDataModel(Compilation compilation, INamedTypeSymbol symbol, string csPath, string gdRoot) : base(symbol)
            => Autoloads = AutoloadScraper.GetAutoloads(compilation, csPath, gdRoot).ToList();

        protected override string Str()
        {
            return string.Join("\n", Autoloads);
        }
    }
}
