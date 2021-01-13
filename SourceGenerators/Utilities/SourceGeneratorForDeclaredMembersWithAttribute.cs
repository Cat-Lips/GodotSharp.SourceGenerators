using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GodotSharp.SourceGenerators.Utilities.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators.Utilities
{
    internal abstract class SourceGeneratorForDeclaredTypesWithAttribute<TAttribute> : SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, TypeDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, INamedTypeSymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, (INamedTypeSymbol)symbol, attribute);
        protected override string GetGeneratedFilename(ISymbol symbol, string @default)
            => $"{((INamedTypeSymbol)symbol).NamespaceOrNull()}.{@default}";
    }

    internal abstract class SourceGeneratorForDeclaredMethodsWithAttribute<TAttribute> : SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, MethodDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, IMethodSymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, (IMethodSymbol)symbol, attribute);
        protected override string GetGeneratedFilename(ISymbol symbol, string @default)
            => $"{((IMethodSymbol)symbol).ReceiverType}.{@default}";
    }

    internal abstract class SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, TDeclarationSyntax> : ISourceGenerator
        where TAttribute : Attribute
        where TDeclarationSyntax : MemberDeclarationSyntax
    {
        private static readonly string attributeName = typeof(TAttribute).Name;
        private static readonly string attributeCode = Regex.Replace(attributeName, "Attribute$", "", RegexOptions.Compiled);

        public void Initialize(GeneratorInitializationContext context)
            => context.RegisterForSyntaxNotifications(() => new AttributeReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var type in ((AttributeReceiver)context.SyntaxReceiver).DeclaredTypesWithAttribute)
            {
                var compilation = context.Compilation;
                var model = compilation.GetSemanticModel(type.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(type);
                var attribute = symbol.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == attributeName);
                if (attribute is null) continue;

                var (generatedCode, error) = _GenerateCode(compilation, symbol, attribute);
                if (generatedCode is null)
                {
                    var descriptor = new DiagnosticDescriptor(error.Id ?? attributeCode, error.Title, error.Message, error.Category ?? "Usage", DiagnosticSeverity.Error, true);
                    var diagnostic = Diagnostic.Create(descriptor, attribute.ApplicationSyntaxReference.GetSyntax().GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    continue;
                }

                context.AddSource(GetGeneratedFilename(symbol, $"{symbol.Name}.{attributeCode}.g.cs"), generatedCode);
            }

            (string GeneratedCode, DiagnosticDetail Error) _GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            {
                try
                {
                    return GenerateCode(compilation, symbol, attribute);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to generate code for {symbol.Name} [{e.Message}]", e);
                }
            }
        }

        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute);
        protected virtual string GetGeneratedFilename(ISymbol symbol, string @default) => @default;

        private class AttributeReceiver : ISyntaxReceiver
        {
            public List<TDeclarationSyntax> DeclaredTypesWithAttribute { get; } = new List<TDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode node)
            {
                if (node is TDeclarationSyntax type && TypeContainsAttribute())
                {
                    DeclaredTypesWithAttribute.Add(type);
                }

                bool TypeContainsAttribute()
                {
                    return type.AttributeLists.SelectMany(x => x.Attributes)
                        .Any(x => x.Name.ToString() == attributeCode);
                }
            }
        }
    }
}
