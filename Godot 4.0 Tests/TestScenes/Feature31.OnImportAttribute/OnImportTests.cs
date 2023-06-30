using System.ComponentModel;
using FluentAssertions;
using Godot;
using Godot.Collections;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class OnImportTests : Node, ITest
    {
        void ITest.InitTests()
        {
            var allArgs = new OnImportWithAllArgs();
            var minArgs = new OnImportWithMinArgs();
            var optArgs = new OnImportWithOptions();

            CheckCommon(allArgs, "GodotTests.TestScenes.OnImportWithAllArgs", "On Import With All Args", new[] { "zip", "zap", "zam" }, new[] { "a", "b", "c" });
            CheckCommon(minArgs, "GodotTests.TestScenes.OnImportWithMinArgs", "On Import With Min Args", new[] { "zip" }, new[] { "Default" });
            CheckCommon(optArgs, "GodotTests.TestScenes.OnImportWithOptions", "On Import With Options", new[] { "zip", "zap" }, new[] { "a", "b" });
            CheckHintOptions(optArgs);

            allArgs.Result.Should().Be(default);
            minArgs.Result.Should().Be(default);
            optArgs.Result.Should().Be(default);

            CallImportWithOptions(allArgs);
            CallImportWithOptions(minArgs);
            CallImportWithOptions(optArgs);

            allArgs.Result.Should().Be(("res://somepath/MySourceFile.obj", ".import/somepath/MySavePath.scn", "platform-variant", "generated-file"));
            minArgs.Result.Should().Be(("res://somepath/MySourceFile.obj", ".import/somepath/MySavePath.scn"));
            optArgs.Result.Should().Be(("res://somepath/MySourceFile.obj", ".import/somepath/MySavePath.scn", "MySourceFile",

                7, -7,
                false, true,
                7f, -7f,
                "", "", "Value", // Null converted to "" (null not editable in editor)
                OnImportWithOptions.DefaultPath, "EditorImportPlugin",
                "", 7, // (hint tests)

                7, -7,
                false, true,
                7f, -7f,
                "", "", "Value", // Null converted to "" (null not editable in editor)
                OnImportWithOptions.DefaultPath, "EditorImportPlugin",
                OnImportWithOptions.DefaultData));

            static void CallImportWithOptions(EditorImportPlugin plugin)
            {
                var options = new Dictionary();

                foreach (var item in plugin._GetImportOptions(default, default))
                    options.Add(item["name"], item["default_value"]);

                plugin._Import("res://somepath/MySourceFile.obj", ".import/somepath/MySavePath", options, new(), new());
            }

            static void CheckCommon(EditorImportPlugin plugin, string importerName, string displayName, string[] recognizedExtensions, string[] presets)
            {
                plugin._GetSaveExtension().Should().Be("scn");
                plugin._GetResourceType().Should().Be("PackedScene");
                plugin._GetImporterName().Should().Be(importerName);
                plugin._GetVisibleName().Should().Be(displayName);
                plugin._GetRecognizedExtensions().Should().BeEquivalentTo(recognizedExtensions);

                plugin._GetPriority().Should().Be(1);
                plugin._GetImportOrder().Should().Be(0);

                plugin._GetPresetCount().Should().Be(presets.Length);
                for (var i = 0; i < presets.Length; ++i)
                    plugin._GetPresetName(i).Should().Be(presets[i]);

                plugin._GetOptionVisibility(default, default, default).Should().BeTrue(); // TODO
                plugin._GetImportOptions(default, default).Should().NotBeNull(); // TODO: Import options per path & preset
            }

            static void CheckHintOptions(EditorImportPlugin plugin)
            {
                CheckHintOnly();
                CheckHintDefault();
                CheckHintAndString();

                void CheckHintOnly()
                {
                    var optionsWithHintOnly = plugin
                        ._GetImportOptions(default, default)
                        .Where(opt => (string)opt["name"] is "Hint Only Object Id");
                    optionsWithHintOnly.Should().HaveCount(1);

                    foreach (var opt in optionsWithHintOnly)
                    {
                        opt.Should().ContainKey("property_hint"); ((int)opt["property_hint"]).Should().Be((int)PropertyHint.ObjectId);
                        opt.Should().NotContainKey("hint_string");
                    }
                }

                void CheckHintDefault()
                {
                    var optionsWithHintDefault = plugin
                        ._GetImportOptions(default, default)
                        .Where(opt => (string)opt["name"] is "Empty Hint");
                    optionsWithHintDefault.Should().HaveCount(1);

                    foreach (var opt in optionsWithHintDefault)
                    {
                        opt.Should().ContainKey("property_hint"); ((int)opt["property_hint"]).Should().Be((int)PropertyHint.None);
                        opt.Should().NotContainKey("hint_string");
                    }
                }

                void CheckHintAndString()
                {
                    var optionsWithHintAndString = plugin
                        ._GetImportOptions(default, default)
                        .Where(opt => (string)opt["name"] is "Opt Str From Member" or "Atr Str From Member");
                    optionsWithHintAndString.Should().HaveCount(2);

                    foreach (var opt in optionsWithHintAndString)
                    {
                        opt.Should().ContainKey("property_hint"); ((int)opt["property_hint"]).Should().Be((int)PropertyHint.File);
                        opt.Should().ContainKey("hint_string"); ((string)opt["hint_string"]).Should().Be("*.cs,*.gd");
                    }
                }
            }
        }

        public static Error SaveTestScene(string savePath)
        {
            // Only need to save something to test in editor
            // FIXME: Import values not being saved (or loaded from .import file if changed in file)!!!
            if (!Engine.IsEditorHint()) return Error.Ok;

            var scene = new PackedScene();

            var err = scene.Pack(new());
            if (err is not Error.Ok) return err;

            err = ResourceSaver.Save(scene, savePath);
            if (err is not Error.Ok) return err;

            // ok
            return Error.Ok;
        }
    }

    [Tool]
    internal partial class OnImportWithAllArgs : OnImportEditorPlugin
    {
        public (string sourceFile, string savePath, string platformVariants, string generatedFiles) Result;

        [OnImport("zip|zap|zam", presets: "a|b|c")]
        private Error MyImportMethod(string sourceFile, string savePath, Array<string> platformVariants, Array<string> genFiles)
        {
            genFiles.Add("generated-file");
            platformVariants.Add("platform-variant");
            Result = (sourceFile, savePath, string.Join("|", platformVariants), string.Join("|", genFiles)); // Join arrays for easier comparison
            OnImportTests.SaveTestScene(savePath);
            return Error.Ok;
        }
    }

    [Tool]
    internal partial class OnImportWithMinArgs : OnImportEditorPlugin
    {
        public (string sourceFile, string savePath) Result;

        [OnImport("zip")]
        private Error MyImportMethod(string sourceFile, string savePath)
        {
            Result = (sourceFile, savePath);
            OnImportTests.SaveTestScene(savePath);
            return Error.Ok;
        }
    }

    [Tool]
    internal partial class OnImportWithOptions : OnImportEditorPlugin
    {
        public (string sourceFile, string savePath, string name,

            int optInt7, int optIntN7,
            bool optBoolF, bool optBoolT,
            float optFloat7, float optFloatN7,
            string optStrNull, string optStrEmpty, string optStrValue,
            string optStrFromMember, string optStrFromNonMember,
            string emptyHint, int hintOnlyObjectId,

            int atrInt7, int atrIntN7,
            bool atrBoolF, bool atrBoolT,
            float atrFloat7, float atrFloatN7,
            string atrStrNull, string atrStrEmpty, string atrStrValue,
            string atrStrFromMember, string atrStrFromNonMember,
            Variant atrNonStrFromMember) Result;

        public static Variant DefaultData => 7;
        public static string DefaultPath => $"res://{nameof(OnImportTests)}/{nameof(DefaultPath)}.cs";

        [OnImport("zip,zap", presets: "a,b")]
        private Error MyImportMethod(string sourceFile, string savePath, string name, // Name is optional, purely for convenience

            int optInt7 = 7, int optIntN7 = -7,
            bool optBoolF = false, bool optBoolT = true,
            float optFloat7 = 7, float optFloatN7 = -7,
            string optStrNull = null, string optStrEmpty = "", string optStrValue = "Value",
            [Hint(PropertyHint.File, "*.cs,*.gd")] string optStrFromMember = nameof(DefaultPath), string optStrFromNonMember = nameof(EditorImportPlugin),
            [Hint] string emptyHint = null, [Hint(PropertyHint.ObjectId)] int hintOnlyObjectId = 7,

            [DefaultValue(7)] int atrInt7 = default, [DefaultValue(-7)] int atrIntN7 = default,
            [DefaultValue(false)] bool atrBoolF = default, [DefaultValue(true)] bool atrBoolT = default,
            [DefaultValue(7)] float atrFloat7 = default, [DefaultValue(-7)] float atrFloatN7 = default,
            [DefaultValue(null)] string atrStrNull = default, [DefaultValue("")] string atrStrEmpty = default, [DefaultValue("Value")] string atrStrValue = default,
            [DefaultValue(nameof(DefaultPath)), Hint(PropertyHint.File, "*.cs,*.gd")] string atrStrFromMember = default, [DefaultValue(nameof(EditorImportPlugin))] string atrStrFromNonMember = default,
            [DefaultValue(nameof(DefaultData))] Variant atrNonStrFromMember = default) // This is the only case where DefaultValue atribute is required (ie, all other cases can be handled using explicit default)
        {
            Result = (sourceFile, savePath, name,

                optInt7, optIntN7,
                optBoolF, optBoolT,
                optFloat7, optFloatN7,
                optStrNull, optStrEmpty, optStrValue,
                optStrFromMember, optStrFromNonMember,
                emptyHint, hintOnlyObjectId,

                atrInt7, atrIntN7,
                atrBoolF, atrBoolT,
                atrFloat7, atrFloatN7,
                atrStrNull, atrStrEmpty, atrStrValue,
                atrStrFromMember, atrStrFromNonMember,
                atrNonStrFromMember);

            if (Engine.IsEditorHint())
                GD.Print(Result);

            OnImportTests.SaveTestScene(savePath);
            return Error.Ok;
        }
    }
}
