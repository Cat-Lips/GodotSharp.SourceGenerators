using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class BaseDataModel(ISymbol symbol, INamedTypeSymbol @class)
{
    public string Namespace { get; } = symbol.GetNamespaceDeclaration();
    public string ClassName { get; } = @class.ClassDef();

    protected abstract string Str();
    public sealed override string ToString() => Str();
}
