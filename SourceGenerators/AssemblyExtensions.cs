using System;
using System.IO;
using System.Reflection;

namespace GodotSharp.SourceGenerators
{
    public static class AssemblyExtensions
    {
        public static string GetEmbeddedResource(this Assembly assembly, string resource)
        {
            using (var resourceStream = new StreamReader(assembly.GetManifestResourceStream(resource)
                    ?? throw new Exception($"Failed to find EmbeddedResource '{resource}' in Assembly '{assembly}' (Available Resources: {string.Join(", ", assembly.GetManifestResourceNames())})")))
            {
                return resourceStream.ReadToEnd();
            }
        }
    }
}
