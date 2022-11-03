using GodotSharp.SourceGenerators;
using Microsoft.CodeAnalysis;

namespace CustomGeneratorTests
{
    [Generator]
    internal class MyMethodAttributeGenerator : SourceGeneratorForDeclaredMethodWithAttribute<MyMethodAttribute>
    {
        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, IMethodSymbol symbol, AttributeData attribute)
        {
            var content = symbol.ContainingType.GeneratePartialClass(Content());
            return (content, null);

            static IEnumerable<string> Content()
            {
                yield return "private void MyMethodAttributeGeneratedThisMethod()";
                yield return "{";
                yield return "}";
            }
        }
    }
}
