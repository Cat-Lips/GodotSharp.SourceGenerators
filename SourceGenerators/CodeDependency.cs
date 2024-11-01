namespace GodotSharp.SourceGenerators;

// We rely on the record equality here
public abstract record CodeDependency
{
    public record SceneTree(string TscnFileName, bool TraverseInstancedScenes) : CodeDependency
    {
        public string ClassName { get; } = TscnFileName;
    }
}
