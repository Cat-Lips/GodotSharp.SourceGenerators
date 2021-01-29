using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, TDeclarationSyntax> : ISourceGenerator
        where TAttribute : Attribute
        where TDeclarationSyntax : MemberDeclarationSyntax
    {
        private static readonly string attributeType = typeof(TAttribute).Name;
        private static readonly string attributeName = Regex.Replace(attributeType, "Attribute$", "", RegexOptions.Compiled);

        public void Initialize(GeneratorInitializationContext context)
            => context.RegisterForSyntaxNotifications(() => new AttributeReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            foreach (var type in ((AttributeReceiver)context.SyntaxReceiver).DeclaredTypesWithAttribute)
            {
                var model = compilation.GetSemanticModel(type.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(type);
                var attribute = symbol.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == attributeType);
                if (attribute is null) continue;

                var (generatedCode, error) = _GenerateCode(compilation, symbol, attribute);
                if (generatedCode is null)
                {
                    var descriptor = new DiagnosticDescriptor(error.Id ?? attributeName, error.Title, error.Message, error.Category ?? "Usage", DiagnosticSeverity.Error, true);
                    var diagnostic = Diagnostic.Create(descriptor, attribute.ApplicationSyntaxReference.GetSyntax().GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    continue;
                }

                context.AddSource(GenerateFilename(symbol), generatedCode);
            }

            (string GeneratedCode, DiagnosticDetail Error) _GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            {
                try { return GenerateCode(compilation, symbol, attribute); }
                catch (Exception e) { throw new Exception($"Failed to generate code for {symbol.Name} [{e.ToString().Replace('\r', '|').Replace('\n', '|')}]", e); }
            }
        }

        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute);

        protected virtual string GenerateFilename(ISymbol symbol)
        {
            // Hash namespace to reduce risk of reaching 259 char path limit...
            var ns = symbol.NamespaceOrNull();
            var symbolStr = ns is null ? symbol.ToDisplayString()
                : $"{symbol.ToDisplayString()[(ns.Length + 1)..]}.{ns.GetHashCode()}";
            symbolStr = string.Join("_", symbolStr.Split(Path.GetInvalidFileNameChars()));
            return $"{symbolStr}.g.cs";
        }

        private class AttributeReceiver : ISyntaxReceiver
        {
            public List<TDeclarationSyntax> DeclaredTypesWithAttribute { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode node)
            {
                if (node is TDeclarationSyntax type && TypeContainsAttribute())
                    DeclaredTypesWithAttribute.Add(type);

                bool TypeContainsAttribute()
                {
                    return type.AttributeLists.SelectMany(x => x.Attributes)
                        .Any(x => x.Name.ToString() == attributeName);
                }
            }
        }
    }
}
