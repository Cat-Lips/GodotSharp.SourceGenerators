namespace GodotSharp.SourceGenerators;

public abstract record CodeGenerationResult
{
    public record Success(string Code, IEnumerable<CodeDependency> Dependencies) : CodeGenerationResult
    {
        public Success(string Code) : this(Code, Enumerable.Empty<CodeDependency>()) {}
    }

    public record Error(DiagnosticDetail Detail) : CodeGenerationResult;
}
