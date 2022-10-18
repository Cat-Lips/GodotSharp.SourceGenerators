namespace GodotSharp.BuildingBlocks
{
    public partial class Callable : Godot.Object
    {
        private readonly Action action;

        public Callable(Action action)
            => this.action = action;

        public void Invoke()
            => action();
    }

    public partial class Callable<T> : Godot.Object
    {
        private readonly Action<T> action;

        public Callable(Action<T> action)
            => this.action = action;

        public void Invoke(T arg)
            => action(arg);
    }
}
