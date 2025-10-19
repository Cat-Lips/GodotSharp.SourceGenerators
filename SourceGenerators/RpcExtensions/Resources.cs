using System.Reflection;

namespace GodotSharp.SourceGenerators.RpcExtensions;

internal static class Resources
{
    private const string rpcTemplate = "GodotSharp.SourceGenerators.RpcExtensions.RpcTemplate.scriban";
    public static readonly string RpcTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(rpcTemplate);
}
