using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel
    {
        public string NSOpen { get; }
        public string NSClose { get; }
        public string NSIndent { get; }
        public string ClassName { get; }
        public ICollection<SceneTreeNode> Properties { get; }

        public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string tscnFile)
        {
            ClassName = symbol.ClassDef();
            (NSOpen, NSClose, NSIndent) = symbol.GetNamespaceDeclaration();
            Properties = SceneTreeScraper.GetNodes(compilation, tscnFile);
        }
    }
}
