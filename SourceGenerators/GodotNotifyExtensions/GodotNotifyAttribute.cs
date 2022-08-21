namespace Godot
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class NotifyAttribute : Attribute
    {
        public NotifyAttribute(string set = "private", bool export = false)
        {
            Setter = export ? "public" : set;
            Export = export;
        }

        public bool Export { get; }
        public string Setter { get; }
    }
}
