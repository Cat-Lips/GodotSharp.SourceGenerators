namespace GodotSharp.SourceGenerators;

internal static class UID
{
    public static string Get(string file)
    {
        Log.Debug($">>> UID.Get({file})");
        var uid = File.ReadAllText(file).Trim();
        Log.Debug($"<<< {uid}");
        return uid;
    }

    #region Cache

    private const string UidUrl = "uid://";
    private const string ResUrl = "res://";
    private static readonly int UidPrefix = UidUrl.Length;
    private static readonly int ResPrefix = ResUrl.Length;

    private static readonly char[] uid_chars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', '0', '1', '2', '3', '4', '5', '6', '7', '8'];
    private static readonly int uid_char_count = uid_chars.Length;
    private static readonly int max_uid_length = 13;

    private static readonly Dictionary<string, string> Cache = [];
    private static DateTime cacheTimestamp;

    public static string GetRes(string uid)
    {
        return Cache.TryGetValue(
            uid.StartsWith(UidUrl) ? uid[UidPrefix..] : uid, out var res)
                ? res : throw new Exception($"Could not find res path for {uid}");
    }

    public static void Init(string gdRoot)
    {
        lock (Cache)
        {
            if (InitRequired(out var source))
            {
                Log.Debug($"UID Init [Source: {source}]");
                using (var x = new BinaryReader(File.OpenRead(source)))
                {
                    var count = x.ReadInt32();
                    Log.Debug($" - Count: {count}");

                    for (var i = 0; i < count; ++i)
                    {
                        var uid = ConvertUID(x.ReadInt64());
                        var res = new string(x.ReadChars(x.ReadInt32()));

                        if (!File.Exists(Path.Combine(gdRoot, res[ResPrefix..])))
                        {
                            Log.Warn($" - Ignoring orphan UID: {uid}, RES: {res}");
                            continue;
                        }

                        if (Cache.TryGetValue(uid, out var _res))
                        {
                            if (_res == res)
                            {
                                Log.Info($" - Ignoring repeat UID: {uid}, RES: {res}");
                                continue;
                            }

                            throw new Exception($"UID Conflict: {uid}, RES 1: {res}, RES 2: {_res}");
                        }

                        Log.Debug($" - Adding UID: {uid}, RES: {res}");
                        Cache.Add(uid, res);
                    }
                }
            }
        }

        bool InitRequired(out string source)
        {
            const string Source = ".godot/uid_cache.bin";
            source = Path.Combine(gdRoot, Source);

            var timestamp = File.GetLastWriteTime(source);
            if (cacheTimestamp != timestamp)
            {
                cacheTimestamp = timestamp;
                Cache.Clear();
            }

            return Cache.Count is 0;
        }

        string ConvertUID(long id)
        {
            var pos = max_uid_length;
            Span<char> buffer = stackalloc char[max_uid_length];

            do
            {
                var c = id % uid_char_count;
                buffer[--pos] = uid_chars[c];
                id /= uid_char_count;
            }
            while (id > 0);

            return buffer[pos..].ToString();
        }
    }

    #endregion
}
