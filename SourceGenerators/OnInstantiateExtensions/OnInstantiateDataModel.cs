using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions
{
    internal class OnInstantiateDataModel : MemberDataModel
    {
        public string MethodName { get; }
        public string MethodArgs { get; }
        public string PassedArgs { get; }
        public string ResourcePath { get; }

        public OnInstantiateDataModel(IMethodSymbol method, string godotProjectDir = null)
            : base(method)
        {
            MethodName = method.Name;
            MethodArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Type} {x.Name}"));
            PassedArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Name}"));
            ResourcePath = ConstructResourcePath();

            string ConstructResourcePath()
            {
                var classPath = method.ContainingType.ClassPath();
                var resourcePath = GD.GetResourcePath(classPath, godotProjectDir);
                return Path.ChangeExtension(resourcePath, "tscn").Replace(@"\", "/");
            }
        }

        protected override string Str()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return $" - Method Signature: void {MethodName}({MethodArgs})";
                yield return $" - Calling Declaration: {MethodName}({PassedArgs})";
                yield return $" - Resource Path: {ResourcePath}";
            }
        }
    }
}
