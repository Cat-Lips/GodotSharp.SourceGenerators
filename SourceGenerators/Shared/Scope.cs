namespace GodotSharp.SourceGenerators;

public enum Scope
{
    None,
    Public,
    Private,
    Internal,
    Protected,
    ProtectedOrInternal,    // protected internal
    ProtectedAndInternal,   // private protected
}

public static class ScopeExtensions
{
    public static string ToCodeString(this Scope x) => x switch
    {
        Scope.None => null,
        Scope.Public => "public",
        Scope.Private => "private",
        Scope.Internal => "internal",
        Scope.Protected => "protected",
        Scope.ProtectedOrInternal => "protected internal",
        Scope.ProtectedAndInternal => "private protected",
        _ => throw new NotImplementedException(),
    };
}
