using Object = Godot.Object;

namespace GodotSharp.BuildingBlocks
{
    public static class CallableExtensions
    {
        public static void Connect(this Object source,new Callable(string signal,Action action))
            => source.Connect(signal,new Callable(new Callable(action),nameof(Callable.Invoke)));

        public static void Connect<T>(this Object source, string signal, Action<T> action)
            => source.Connect(signal,new Callable(new Callable<T>(action),nameof(Callable.Invoke)));
    }
}
