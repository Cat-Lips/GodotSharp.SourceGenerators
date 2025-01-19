namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnImportAttribute : Attribute
{
    public OnImportAttribute(string recognizedExtensions, string importAs = null, string resourceType = "PackedScene", string saveExtension = "scn", float priority = 1, int importOrder = 0, string presets = "Default")
    {
        DisplayName = importAs;
        ResourceType = resourceType;
        SaveExtension = saveExtension;
        RecognizedExtensions = recognizedExtensions.Split(',', '|');

        Priority = priority;
        ImportOrder = importOrder;
        Presets = presets.Split(',', '|');
    }

    public string DisplayName { get; }
    public string ResourceType { get; }
    public string SaveExtension { get; }
    public string[] RecognizedExtensions { get; }

    public float Priority { get; }
    public int ImportOrder { get; }
    public string[] Presets { get; }
}
