namespace GodotSharp.SourceGenerators
{
    public static class LinqExtensions
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
            => source.TryGetValue(key, out var value) ? value : default;
    }
}
