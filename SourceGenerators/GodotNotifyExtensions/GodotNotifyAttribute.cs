namespace Godot
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class NotifyAttribute : Attribute
    {
        public NotifyAttribute(string set = "private")
            => Setter = set;

        public string Setter { get; }
    }
}
