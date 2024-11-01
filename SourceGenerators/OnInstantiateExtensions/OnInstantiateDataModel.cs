using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions
{
    internal class OnInstantiateDataModel : MemberDataModel
    {
        public string MethodName { get; }
        public string MethodArgs { get; }
        public string PassedArgs { get; }
        public string ResourcePath { get; }
        public string ConstructorScope { get; }

        public OnInstantiateDataModel(Compilation compilation, IMethodSymbol method, SyntaxNode node, string ctor, string godotProjectDir)
            : base(method)
        {
            MethodName = method.Name;
            (MethodArgs, PassedArgs) = GetArgs();
            ResourcePath = GetResourcePath();
            ConstructorScope = ctor;

            (string, string) GetArgs()
            {
                var args = ((MethodDeclarationSyntax)node).ParameterList.Parameters;
                var sm = compilation.GetSemanticModel(node.SyntaxTree);
                return (GetMethodArgs(), GetPassedArgs());

                string GetMethodArgs()
                {
                    return string.Join(", ", args.Select(x => string.Join(" ", ArgParts(x))));

                    IEnumerable<string> ArgParts(ParameterSyntax x)
                    {
                        var fullTypeName = GetFullTypeName();

                        if (x.Modifiers.Count > 0) yield return $"{x.Modifiers}";
                        yield return fullTypeName; yield return $"{x.Identifier}";
                        if (x.Default is not null) yield return GetDefaultValue();

                        string GetFullTypeName()
                        {
                            var givenType = $"{x.Type}";
                            var qualifiedType = $"{sm.GetTypeInfo(x.Type).Type}";
                            if (givenType.EndsWith("?") && !qualifiedType.EndsWith("?"))
                                qualifiedType += "?";
                            return qualifiedType;
                        }

                        string GetDefaultValue()
                        {
                            var dflt = $"{x.Default}";
                            var typeSep = fullTypeName.LastIndexOf('.');
                            return typeSep is -1 || !dflt.Contains('.') ? dflt :
                                $"= {fullTypeName[..typeSep]}.{x.Default.Value}";
                        }
                    }
                }

                string GetPassedArgs()
                {
                    return string.Join(", ", args.Select(x => string.Join(" ", ArgParts(x))));

                    IEnumerable<string> ArgParts(ParameterSyntax x)
                    {
                        if (x.Modifiers.Count > 0)
                            yield return $"{x.Modifiers}";
                        yield return $"{x.Identifier}";
                    }
                }
            }

            string GetResourcePath()
            {
                var classPath = node.SyntaxTree.FilePath;
                var resourcePath = GD.GetResourcePath(classPath, godotProjectDir);
                return Path.ChangeExtension(resourcePath, "tscn");
            }
        }

        protected override string Str()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return $" - Method Signature: {MethodName}({MethodArgs})";
                yield return $" - Calling Declaration: {MethodName}({PassedArgs})";
                yield return $" - Resource Path: {ResourcePath}";
                yield return $" - Constructor Scope: {ConstructorScope}";
            }
        }
    }
}
