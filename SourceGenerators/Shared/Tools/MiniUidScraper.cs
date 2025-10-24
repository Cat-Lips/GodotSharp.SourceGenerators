namespace GodotSharp.SourceGenerators;

internal static class MiniUidScraper
{
    public static string GetUid(string file)
    {
        Log.Debug($">>> GetUid {file}");
        var uid = File.ReadAllText(file).Trim();
        Log.Debug($"<<< {uid}");
        return uid;
    }
}
