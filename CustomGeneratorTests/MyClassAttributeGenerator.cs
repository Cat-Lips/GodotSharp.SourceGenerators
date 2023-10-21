using System.Collections.Generic;
using GodotSharp.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CustomGeneratorTests
{
    [Generator]
    internal class MyClassAttributeGenerator : SourceGeneratorForDeclaredTypeWithAttribute<MyClassAttribute>
    {
        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        {
            var content = symbol.GeneratePartialClass(Content(), Usings());
            return (content, null);

            static IEnumerable<string> Content()
            {
                yield return "private void MyClassAttributeGeneratedThisMethod()";
                yield return "{";
                yield return "}";
            }

            static IEnumerable<string> Usings()
            {
                yield return "using System;";
            }
        }
    }
}
