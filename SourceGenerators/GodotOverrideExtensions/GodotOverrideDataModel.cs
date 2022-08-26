using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions
{
    internal class GodotOverrideDataModel
    {
        public string NSOpen { get; }
        public string NSClose { get; }
        public string NSIndent { get; }
        public string ClassName { get; }

        public bool Replace { get; }
        public string ReturnType { get; }
        public string MethodName { get; }
        public string MethodArgs { get; }
        public string PassedArgs { get; }

        public GodotOverrideDataModel(IMethodSymbol method, bool replace)
        {
            ClassName = method.ContainingType.ClassDef();
            (NSOpen, NSClose, NSIndent) = method.GetNamespaceDeclaration();

            ReturnType = $"{method.ReturnType}";
            Replace = replace || ReturnType is not "void";
            MethodName = Regex.Replace(method.Name, "^On", "", RegexOptions.Compiled);

            var paramIndex = 0;
            MethodArgs = string.Join(", ", method.Parameters.Select(x => $"{x} _{++paramIndex}"));

            paramIndex = 0;
            PassedArgs = string.Join(", ", method.Parameters.Select(x => $"_{++paramIndex}"));
        }
    }
}
