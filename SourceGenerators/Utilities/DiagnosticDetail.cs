namespace GodotSharp.SourceGenerators.Utilities
{
    internal record DiagnosticDetail
    {
        public string Id { get; init; }
        public string Category { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
    }
}
