# GodotSharp.SourceGenerators

Some C# Source Generators for use with the Godot Game Engine:
* `SceneTree` class attribute: Provides strongly typed access to a scene's hierarchy via the `_` operator (ie, an effective equivalent to GDScript's `$` operator)
* `GodotOverride` method attribute: Allows any virtual _* overide to be replaced with On*

## Basic Usage
```
using Godot;

namespace MyGame
{
    [SceneTree]
    public partial class MyControl : Control
    {
        [Export]
        public string PlayerName
        {
            get => _.VBox.Label1.Text;
            set => _.VBox.Label1.Text = value;
        }

        [GodotOverride]
        private void OnEnterTree()
            => PlayerName = "Player 1";
    }
}
```

For more advanced usage scenarios, see Godot.Tests...