using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GodotSharp.SourceGenerators
{
    internal static class Log
    {
        public static bool EnableTimestamp { get; set; } = true;
        public static bool EnableRuntime { get; set; } = true;
        public static bool EnableThreadId { get; set; } = true;
        public static bool EnableFileName { get; set; } = true;
        public static bool EnableMemberName { get; set; } = false;

        //[Conditional("DEBUG")]
        public static void Debug(object msg = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null) => Print(Format(filePath, memberName, msg));
        public static void Info(object msg = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null) => Print(Format(filePath, memberName, $"[INFO] {msg}"));
        public static void Warn(object msg = null, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null) => Print(Format(filePath, memberName, $"[WARN] {msg}"));
        public static void Error(Exception e, [CallerFilePath] string filePath = null, [CallerMemberName] string memberName = null) => Print(Format(filePath, memberName, e));

        private static string Format(string filePath, string memberName, object msg)
            => msg is null ? Environment.NewLine : $"{Timestamp()}{Runtime()}{ThreadId()}{FileName(filePath)}{MemberName(memberName)}{msg}{Environment.NewLine}";

        private static string Timestamp() => EnableTimestamp ? DateTime.Now.ToString("[HH:mm:ss.fff] ") : null;
        private static string Runtime() => EnableRuntime ? $"[{Stopwatch.Elapsed.Format()}] " : null;
        private static string ThreadId() => EnableThreadId ? $"[Thread {Thread.CurrentThread.ManagedThreadId}] " : null;
        private static string FileName(string x) => EnableFileName ? $"[{Path.GetFileNameWithoutExtension(x)}] " : null;
        private static string MemberName(string x) => EnableMemberName && x is not null ? $"[{x}] " : null;

        private static string Format(this TimeSpan value, string noTimeStr = "0ms")
        {
            var timeStr = value.ToString("d'.'hh':'mm':'ss'.'fff'ms'").TrimStart('0', ':', '.');
            return timeStr == "ms" ? noTimeStr
                : !timeStr.Contains('.') ? $".{timeStr}"
                : timeStr;
        }

        private static readonly string LogFile;
        private static readonly Stopwatch Stopwatch;

        static Log()
        {
            Stopwatch = Stopwatch.StartNew();
            LogFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/GodotSharp.SourceGenerators.log";

            TryDeleteLogFile();
            Log.Debug($"*** NEW COMPILATION DETECTED: {DateTime.Now:HH:mm:ss.fff} ***");

            static void TryDeleteLogFile()
            {
                try { File.Delete(LogFile); }
                catch { TryClearLogFile(); }

                static void TryClearLogFile()
                {
                    try { File.WriteAllText(LogFile, string.Empty); }
                    catch { }
                }
            }
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
