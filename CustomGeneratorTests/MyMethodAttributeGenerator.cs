using System.Collections.Generic;
using GodotSharp.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CustomGeneratorTests
{
    [Generator]
    internal class MyMethodAttributeGenerator : SourceGeneratorForDeclaredMethodWithAttribute<MyMethodAttribute>
    {
        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
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
