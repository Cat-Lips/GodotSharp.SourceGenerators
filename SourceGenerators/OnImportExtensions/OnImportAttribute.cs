namespace Godot
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnImportAttribute : Attribute
    {
        public OnImportAttribute(string saveExtension, string resourceType, string importerName, string visibleName, string recognizedExtensions, float priority = 1, int importOrder = 0, string presets = "Default")
        {
            SaveExtension = saveExtension;
            ResourceType = resourceType;
            ImporterName = importerName;
            VisibleName = visibleName;
            RecognizedExtensions = recognizedExtensions.Split(',', '|');
            Priority = priority;
            ImportOrder = importOrder;
            PresetNames = presets.Split(',', '|');
        }

        public string SaveExtension { get; }
        public string ResourceType { get; }
        public string ImporterName { get; }
        public string VisibleName { get; }
        public string[] RecognizedExtensions { get; }
        public float Priority { get; }
        public int ImportOrder { get; }
        public string[] PresetNames { get; }
    }
}
