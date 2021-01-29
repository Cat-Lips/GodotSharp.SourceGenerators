using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel
    {
        public string NSOpen { get; }
        public string NSClose { get; }
        public string NSIndent { get; }
        public string ClassName { get; }
        public ICollection<string> Usings { get; }
        public ICollection<SceneTreeNode> Properties { get; }

        public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string tscnFile)
        {
            ClassName = symbol.ClassDef();
            (NSOpen, NSClose, NSIndent) = symbol.GetNamespaceDeclaration();

            var (childNodes, customTypes) = SceneTreeScraper.GetNodes(tscnFile);

            Properties = childNodes;
            Usings = customTypes
                .Select(GetNamespace)
                .Where(x => x is not null)
                .ToList();

            string GetNamespace(string type)
            {
                return compilation
                    .GetSymbolsWithName(type, SymbolFilter.Type)
                    .First().NamespaceOrNull();
            }
        }
    }
}
