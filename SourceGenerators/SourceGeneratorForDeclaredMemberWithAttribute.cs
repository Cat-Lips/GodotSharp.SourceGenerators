using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using GeneratorContext = Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext;

namespace GodotSharp.SourceGenerators
{
    public abstract class
        SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, TDeclarationSyntax> : IIncrementalGenerator
        where TAttribute : Attribute
        where TDeclarationSyntax : MemberDeclarationSyntax
    {
        private static readonly string attributeType = typeof(TAttribute).Name;

        private static readonly string attributeName = Regex.Replace(
            attributeType,
            "Attribute$",
            "",
            RegexOptions.Compiled);

        protected virtual IEnumerable<(string Name, string Source)> StaticSources =>
            Enumerable.Empty<(string Name, string Source)>();

        public void Initialize(GeneratorContext context)
        {
            foreach (var (name, source) in StaticSources)
                context.RegisterPostInitializationOutput(x => x.AddSource($"{name}.g.cs", source));

            var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(IsSyntaxTarget, GetSyntaxTarget);
            var compilationProvider = context.CompilationProvider.Combine(syntaxProvider.Collect())
                .Combine(context.AnalyzerConfigOptionsProvider);
            context.RegisterImplementationSourceOutput(
                compilationProvider,
                (context, provider) => OnExecute(context, provider.Left.Left, provider.Left.Right, provider.Right));

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
                                return true;
                        }
                    }

                    return false;
                }
            }

            static TDeclarationSyntax GetSyntaxTarget(GeneratorSyntaxContext context, CancellationToken _)
                => (TDeclarationSyntax)context.Node;

            void OnExecute(
                SourceProductionContext context,
                Compilation compilation,
                ImmutableArray<TDeclarationSyntax> nodes,
                AnalyzerConfigOptionsProvider options)
            {
                try
                {
                    var unresolvedDependencies = new Queue<CodeDependency>();
                    foreach (var node in nodes.Distinct())
                    {
                        if (context.CancellationToken.IsCancellationRequested)
                            return;

                        var model = compilation.GetSemanticModel(node.SyntaxTree);
                        var symbol = model.GetDeclaredSymbol(Node(node));
                        var attribute = symbol.GetAttributes()
                            .SingleOrDefault(x => x.AttributeClass.Name == attributeType);
                        if (attribute is null) continue;

                        var result = _GenerateCode(compilation, node, symbol, attribute, options.GlobalOptions);

                        switch (result)
                        {
                            case CodeGenerationResult.Error { Detail: var detail }:
                                var descriptor = new DiagnosticDescriptor(
                                    detail.Id ?? attributeName,
                                    detail.Title,
                                    detail.Message,
                                    detail.Category ?? "Usage",
                                    DiagnosticSeverity.Error,
                                    true);
                                var diagnostic = Diagnostic.Create(
                                    descriptor,
                                    attribute.ApplicationSyntaxReference.GetSyntax().GetLocation());
                                context.ReportDiagnostic(diagnostic);
                                continue;
                            case CodeGenerationResult.Success
                            {
                                Code: var generatedCode, Dependencies: var dependencies
                            }:
                                context.AddSource(GenerateFilename(symbol), generatedCode);
                                foreach (var d in dependencies)
                                {
                                    unresolvedDependencies.Enqueue(d);
                                }

                                continue;
                            default:
                                throw new InvalidOperationException(
                                    $"Unexpected code generation result {result.GetType().Name}.");
                        }
                    }


                    var resolvedDependencies = new HashSet<CodeDependency>();
                    var resolver = new DependencyResolver();
                    while (unresolvedDependencies.TryDequeue(out var dependency))
                    {
                        if (resolvedDependencies.Contains(dependency))
                        {
                            continue;
                        }

                        var newDependencies = resolver.ResolveDependency(context, compilation, dependency);
                        resolvedDependencies.Add(dependency);
                        foreach (var newDependency in newDependencies)
                        {
                            unresolvedDependencies.Enqueue(newDependency);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }
            }
        }

        protected abstract CodeGenerationResult GenerateCode(
            Compilation compilation,
            SyntaxNode node,
            ISymbol symbol,
            AttributeData attribute,
            AnalyzerConfigOptions options);

        private CodeGenerationResult _GenerateCode(
            Compilation compilation,
            SyntaxNode node,
            ISymbol symbol,
            AttributeData attribute,
            AnalyzerConfigOptions options)
        {
            try
            {
                return GenerateCode(compilation, node, symbol, attribute, options);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new CodeGenerationResult.Error(InternalError(e));
            }

            static DiagnosticDetail InternalError(Exception e)
                => new() { Title = "Internal Error", Message = e.Message };
        }

        private const string Ext = ".g.cs";
        private const int MaxFileLength = 255;

        protected virtual string GenerateFilename(ISymbol symbol)
        {
            var gn = $"{Format(symbol)}{Ext}";
            Log.Debug($"Generated Filename ({gn.Length}): {gn}\n");
            return gn;

            static string Format(ISymbol symbol)
                => string.Join("_", $"{symbol}".Split(InvalidFileNameChars)).Truncate(MaxFileLength - Ext.Length);
        }

        protected virtual SyntaxNode Node(TDeclarationSyntax node)
            => node;

        private static readonly char[] InvalidFileNameChars = new char[]
        {
            '\"', '<', '>', '|', '\0', (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8,
            (char)9, (char)10, (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18,
            (char)19, (char)20, (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28,
            (char)29, (char)30, (char)31, ':', '*', '?', '\\', '/'
        };
    }
}
