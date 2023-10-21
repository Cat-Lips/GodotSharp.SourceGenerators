using System;

namespace GodotSharp.BuildingBlocks
{
    public class Callable : Godot.Object
    {
        private readonly Action action;

        public Callable(Action action)
            => this.action = action;

        public void Invoke()
            => action();
    }

    public class Callable<T> : Godot.Object
    {
        private readonly Action<T> action;

        public Callable(Action<T> action)
            => this.action = action;

        public void Invoke(T arg)
            => action(arg);
    }
}
