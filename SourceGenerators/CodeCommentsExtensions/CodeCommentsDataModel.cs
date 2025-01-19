using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions;

internal class CodeCommentsDataModel : ClassDataModel
{
    public List<(string Property, string Comment)> CodeComments { get; } = [];

    public CodeCommentsDataModel(INamedTypeSymbol symbol, SyntaxNode node, string strip) : base(symbol)
        => CodeComments = node.GetPropertyComments(strip).ToList();

    protected override string Str()
        => $"CodeComments: {string.Join("", CodeComments.Select(x => $"\n - {x}"))}";
}
