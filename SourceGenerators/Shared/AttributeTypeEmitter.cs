using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

[Generator]
internal sealed class AttributeTypeEmitter : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        foreach (var (name, source) in AttributeTypeSources.All)
            context.RegisterPostInitializationOutput(ctx => ctx.AddSource($"{name}.g.cs", source));
    }
}
