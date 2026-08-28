namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TRAttribute(string source = "Assets/tr/tr", bool xtras = true, char sep = ',', bool FormatPlurals = true, bool FormatNumbers = true, bool ConvertFormats = true, bool ConfirmFormats = true) : Attribute
{
    public char Sep { get; } = sep;
    public bool Xtras { get; } = xtras;
    public string Source { get; } = source;
    public bool FormatPlurals { get; set; } = FormatPlurals; // Inserts numbers into format strings
    public bool FormatNumbers { get; set; } = FormatNumbers; // Formats non-western numbers
    public bool ConvertFormats { get; set; } = ConvertFormats; // Converts %d to {0}
    public bool ConfirmFormats { get; set; } = ConfirmFormats; // Checks for {0} (or %d)
}
