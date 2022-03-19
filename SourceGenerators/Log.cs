using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GodotSharp.SourceGenerators
{
    public static class Log
    {
        private const string LogFile = "D:\\GodotSharp.SourceGenerators.log";

        static Log()
            => ResetLog();

        [Conditional("DEBUG")]
        public static void Debug()
            => Print(Environment.NewLine);

        [Conditional("DEBUG")]
        public static void Debug(object msg, [CallerFilePath] string filePath = null)
            => Print($"[{Path.GetFileNameWithoutExtension(filePath)}] {msg}{Environment.NewLine}");

        [Conditional("DEBUG")]
        private static void ResetLog()
        {
            File.Delete(LogFile);
            Log.Debug($"*** NEW COMPILATION DETECTED: {DateTime.Now:HH:mm:ss.fff} ***");
        }

        private static void Print(string msg)
        {
            lock (LogFile)
            {
                try { File.AppendAllText(LogFile, msg); } catch { }
            }
        }
    }
}
