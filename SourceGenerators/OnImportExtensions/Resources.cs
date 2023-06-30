using System.Reflection;

namespace GodotSharp.SourceGenerators.OnImportExtensions
{
    internal static class Resources
    {
        private const string onImportTemplate = "GodotSharp.SourceGenerators.OnImportExtensions.OnImportTemplate.sbncs";
        public static readonly string OnImportTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(onImportTemplate);

        public static readonly string HintAttribute = @"
#if GODOT
#if NET6_0 || NET7_0  // Godot 4.0 only
namespace Godot
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class HintAttribute : Attribute
    {
        public HintAttribute(PropertyHint propertyHint = default, string hintString = default)
        {
            PropertyHint = propertyHint;
            HintString = hintString;
        }

        public PropertyHint PropertyHint { get; }
        public string HintString { get; }
    }
}
#endif
#endif".Trim();

        public static readonly string OnImportEditorPlugin = @"
#if GODOT
#if NET6_0 || NET7_0  // Godot 4.0 only
using Godot.Collections;

namespace Godot
{
    internal abstract partial class OnImportEditorPlugin : EditorImportPlugin
    {
        public override abstract string _GetSaveExtension();
        public override abstract string _GetResourceType();
        public override abstract string _GetImporterName();
        public override abstract string _GetVisibleName();
        public override abstract string[] _GetRecognizedExtensions();

        public override abstract float _GetPriority();
        public override abstract int _GetImportOrder();

        public override abstract int _GetPresetCount();
        public override abstract string _GetPresetName(int presetIndex);

        public override abstract bool _GetOptionVisibility(string path, StringName optionName, Dictionary options);

        public override abstract Array<Dictionary> _GetImportOptions(string path, int presetIndex);

        public override abstract Error _Import(string sourceFile, string savePath, Dictionary options, Array<string> platformVariants, Array<string> genFiles);
    }
}
#endif
#endif".Trim();
    }
}
