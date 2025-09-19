using System.Reflection;
using Scriban;
using Scriban.Runtime;

namespace GodotSharp.SourceGenerators;

internal static class Shared
{
    private const string utils = "GodotSharp.SourceGenerators.Shared.Utils.scriban";
    public static ScriptObject Utils = Template.Parse(Assembly.GetExecutingAssembly().GetEmbeddedResource(utils)).ToScribanScript();
}
