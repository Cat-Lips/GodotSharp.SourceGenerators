using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class BaseDataModel(ISymbol symbol, INamedTypeSymbol @class)
{
    public string ClassName { get; } = @class.ClassDef();
    public string Namespace { get; } = symbol.GetNamespaceDeclaration();
    public string OuterType { get; } = @class.ContainingType?.ClassDef();
    public bool IsStaticClass { get; } = @class.IsStatic;
    public bool IsStaticOuter { get; } = @class.ContainingType?.IsStatic ?? false;

    protected abstract string Str();
    public sealed override string ToString() => Str();
}
