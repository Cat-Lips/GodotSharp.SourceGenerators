#if TOOLS
using Godot;
using GodotTests.TestScenes;

namespace GodotTests.PluginTests;

[Tool]
public partial class Plugin : EditorPlugin
{
    public Plugin()
    {
        OnImportWithAllArgs onImportWithAllArgs = null;
        OnImportWithMinArgs onImportWithMinArgs = null;
        OnImportWithOptions onImportWithOptions = null;

        TreeEntered += EnablePlugin;
        TreeExiting += DisablePlugin;

        void EnablePlugin()
        {
            AddImportPlugin(onImportWithAllArgs = new());
            AddImportPlugin(onImportWithMinArgs = new());
            AddImportPlugin(onImportWithOptions = new());
        }

        void DisablePlugin()
        {
            RemoveImportPlugin(onImportWithAllArgs); onImportWithAllArgs = null;
            RemoveImportPlugin(onImportWithMinArgs); onImportWithMinArgs = null;
            RemoveImportPlugin(onImportWithOptions); onImportWithOptions = null;
        }
    }
}
#endif
