using Godot;

[Tool]
public partial class EditorDefaultTest : Node
{
    [Export, Notify] public partial int PartialNoDefault { get; set; }
    //[Export, Notify] public partial int PartialCtorDefault { get; set; }
    [Export, Notify] public partial int PartialInlineDefault { get; set; } = 7;

    [Export, Notify] public int NonPartialNoDefault { get => _nonPartialNoDefault.Get(); set => _nonPartialNoDefault.Set(value); }
    //[Export, Notify] public int NonPartialCtorDefault { get => _nonPartialCtorDefault.Get(); set => _nonPartialCtorDefault.Set(value); }
    [Export, Notify] public int NonPartialInlineDefault { get => field = _nonPartialInlineDefault.Get(); set => _nonPartialInlineDefault.Set(field = value); } = 7;

    public EditorDefaultTest()
    {
        //PartialCtorDefault = 7;
        //NonPartialCtorDefault = 7;
    }
}
