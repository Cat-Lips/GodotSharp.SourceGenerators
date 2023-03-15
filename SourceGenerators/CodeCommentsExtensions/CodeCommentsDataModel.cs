using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions
{
    internal class CodeCommentsDataModel : ClassDataModel
    {
        public List<(string Property, string Comment)> CodeComments { get; } = new();

        public CodeCommentsDataModel(INamedTypeSymbol symbol, SyntaxNode node, string strip) : base(symbol)
            => CodeComments = node.GetPropertyComments(strip).ToList();

        public override string ToString()
            => $"CodeComments: {string.Join("", CodeComments.Select(x => $"\n - {x}"))}";
    }
}
