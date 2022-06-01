using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, TDeclarationSyntax> : IIncrementalGenerator
        where TAttribute : Attribute
        where TDeclarationSyntax : MemberDeclarationSyntax
    {
        private static readonly string attributeType = typeof(TAttribute).Name;
        private static readonly string attributeName = Regex.Replace(attributeType, "Attribute$", "", RegexOptions.Compiled);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(IsSyntaxTarget, GetSyntaxTarget);
            var compilationProvider = context.CompilationProvider.Combine(syntaxProvider.Collect());
            context.RegisterSourceOutput(compilationProvider, (c, s) => OnExecute(s.Right, s.Left, c));

            static bool IsSyntaxTarget(SyntaxNode node, CancellationToken _)
            {
                return node is TDeclarationSyntax type && HasAttributeType();

                bool HasAttributeType()
                {
                    if (type.AttributeLists.Count is 0) return false;

                    foreach (var attributeList in type.AttributeLists)
                    {
                        foreach (var attribute in attributeList.Attributes)
                        {
                            if (attribute.Name.ToString() == attributeName)
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }

            static TDeclarationSyntax GetSyntaxTarget(GeneratorSyntaxContext context, CancellationToken _)
                => (TDeclarationSyntax)context.Node;

            void OnExecute(ImmutableArray<TDeclarationSyntax> nodes, Compilation compilation, SourceProductionContext context)
            {
                try
                {
                    foreach (var node in nodes.Distinct())
                    {
                        context.CancellationToken.ThrowIfCancellationRequested();

                        var model = compilation.GetSemanticModel(node.SyntaxTree);
                        var symbol = model.GetDeclaredSymbol(node);
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
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }
            }
        }

        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute);

        private (string GeneratedCode, DiagnosticDetail Error) _GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
        {
            try
            {
                return GenerateCode(compilation, symbol, attribute);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return (null, InternalError(e));
            }

            static DiagnosticDetail InternalError(Exception e) => new() { Title = "Internal Error", Message = e.Message };
        }

        protected virtual string GenerateFilename(ISymbol symbol)
        {
            // Hash namespace to reduce risk of reaching 259 char path limit...
            var ns = symbol.NamespaceOrNull();
            var symbolStr = ns is null ? symbol.ToDisplayString()
                : $"{symbol.ToDisplayString()[(ns.Length + 1)..]}.{ns.GetHashCode()}";
            symbolStr = string.Join("_", symbolStr.Split(Path.GetInvalidFileNameChars()));
            return $"{symbolStr}.g.cs";
        }
    }
}
