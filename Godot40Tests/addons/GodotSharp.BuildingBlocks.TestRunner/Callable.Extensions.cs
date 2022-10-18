using Object = Godot.Object;

namespace GodotSharp.BuildingBlocks
{
    public static class CallableExtensions
    {
        public static void Connect(this Object source, string signal, Action action)
            => source.Connect(signal, new Callable(action), nameof(Callable.Invoke));

        public static void Connect<T>(this Object source, string signal, Action<T> action)
            => source.Connect(signal, new Callable<T>(action), nameof(Callable.Invoke));
    }
}
