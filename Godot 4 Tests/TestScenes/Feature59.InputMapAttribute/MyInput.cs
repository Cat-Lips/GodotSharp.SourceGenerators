using Godot;

namespace GodotTests.TestScenes;

[InputMap]
public partial class MyInput;

[InputMap]
public static partial class MyStaticInput;

[InputMap(nameof(GameInput))]
public partial class MyGameInput;

[InputMap(nameof(GameInput))]
public static partial class MyStaticGameInput;

public class GameInput(StringName action)
{
    public StringName Action => action;

    public bool IsPressed(bool exactMatch = false) => Input.IsActionPressed(action, exactMatch);
    public bool IsJustPressed(bool exactMatch = false) => Input.IsActionJustPressed(action, exactMatch);
    public bool IsJustReleased(bool exactMatch = false) => Input.IsActionJustReleased(action, exactMatch);
    public float GetStrength(bool exactMatch = false) => Input.GetActionStrength(action, exactMatch);
}

public static class InputEventExtensions
{
    public static bool IsPressed(this InputEvent e, GameInput x, bool includeEcho = false, bool exactMatch = false) => e.IsActionPressed(x.Action, includeEcho, exactMatch);
    public static bool IsReleased(this InputEvent e, GameInput x, bool exactMatch = false) => e.IsActionReleased(x.Action, exactMatch);
    public static float GetStrength(this InputEvent e, GameInput x, bool exactMatch = false) => e.GetActionStrength(x.Action, exactMatch);
}
