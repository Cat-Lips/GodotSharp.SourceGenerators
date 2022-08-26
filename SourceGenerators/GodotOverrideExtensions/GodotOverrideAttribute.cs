namespace Godot
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GodotOverrideAttribute : Attribute
    {
        public GodotOverrideAttribute(bool replace = false)
            => Replace = replace;

        public bool Replace { get; }
    }
}
