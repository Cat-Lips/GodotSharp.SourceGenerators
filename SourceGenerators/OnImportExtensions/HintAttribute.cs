namespace Godot
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class HintAttribute : Attribute
    {
        // TODO: Godot.PropertyHint
        public HintAttribute(int propertyHint = default, string hintString = default)
        {
            PropertyHint = propertyHint;
            HintString = hintString;
        }

        public int PropertyHint { get; }
        public string HintString { get; }
    }
}
