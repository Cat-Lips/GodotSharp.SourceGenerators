using System.Diagnostics;
using System.IO;

namespace GodotSharp.SourceGenerators.Utilities
{
    internal static class Log
    {
        [Conditional("DEBUG")]
        public static void Debug(string msg)
            => File.AppendAllText("GodotSharp.SourceGenerators.log", msg + '\n');
    }
}
