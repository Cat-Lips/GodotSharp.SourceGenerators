namespace GodotSharp.SourceGenerators;

public record DiagnosticDetail
{
    public string? Id { get; init; }
    public string? Category { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
}
