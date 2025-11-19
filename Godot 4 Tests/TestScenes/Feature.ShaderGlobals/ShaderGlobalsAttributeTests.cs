using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[ShaderGlobals]
public static partial class ShaderGlobals;

[SceneTree]
public partial class ShaderGlobalsAttributeTests : Node, ITest
{
    void ITest.InitTests()
    {
        ShaderGlobals.A.Should().BeTrue();
        ShaderGlobals.B.Should().Be(2);
        ShaderGlobals.C.Should().Be(0);
        ShaderGlobals.D.Should().Be(9);
        ShaderGlobals.E.Should().Be(875);
        ShaderGlobals.F.Should().Be(new Vector2I(565, 0));
        ShaderGlobals.G.Should().Be(new Vector3I(0, 410, 0));
        ShaderGlobals.H.Should().Be(new Vector4I(0, 475, 0, 180));
        ShaderGlobals.I.Should().Be(new Rect2I(50, 0, 145, 0));
        ShaderGlobals.J.Should().Be(345);
        ShaderGlobals.K.Should().Be(new Vector2I(295, 355));
        ShaderGlobals.L.Should().Be(new Vector3I(0, 195, 0));
        ShaderGlobals.M.Should().Be(new Vector4I(0, 0, 275, 0));
        ShaderGlobals.N.Should().Be(0.205f);
        ShaderGlobals.O.Should().Be(new Vector2(0.23f, 0.385f));
        ShaderGlobals.P.Should().Be(new Vector3(0.0f, 0.435f, 0.0f));
        ShaderGlobals.Q.Should().Be(new Vector4(0.22f, 0.0f, 0.275f, 0.0f));
        ShaderGlobals.R.Should().Be(new Color(0.517647f, 0.921569f, 0.52549f, 0.788235f));
        ShaderGlobals.S.Should().Be(new Rect2(0.19f, 0.0f, 0.065f, 0.1f));
        ShaderGlobals.T.Should().Be(new Vector4(1.36f, 0.44f, 0.22f, 1.0f));
        ShaderGlobals.U.Should().Be(new Basis(1.0f, 0.205f, 1.37f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f));
        ShaderGlobals.V.Should().Be(new Projection(1.0f, 0.46f, 0.92f, 0.0f, 0.0f, 1.315f, 0.92f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f));
        ShaderGlobals.W.Should().Be(new Transform2D(1.0f, 0.0f, 1.885f, 1.0f, 0.755f, 0.0f));
        ShaderGlobals.X.Should().Be(new Transform3D(1.0f, 0.0f, 0.0f, 0.0f, 1.35f, 0.59f, 0.0f, 0.0f, 1.0f, 0.0f, 0.54f, 0.0f));
        ShaderGlobals.Y.Should().BeNull();
        ShaderGlobals.Y.GetDeclaredType().Should().Be(typeof(Texture2D));
        ShaderGlobals.Z.Should().BeNull();
        ShaderGlobals.Z.GetDeclaredType().Should().Be(typeof(Texture2DArray));
        ShaderGlobals.Ä.Should().Be(GD.Load<Texture3D>("res://TestScenes/Feature.ShaderGlobals/Noise.tres"));
        ShaderGlobals.Ö.Should().BeNull();
        ShaderGlobals.Ö.GetDeclaredType().Should().Be(typeof(Cubemap));
        ShaderGlobals.Ü.Should().BeNull();
        ShaderGlobals.Ü.GetDeclaredType().Should().Be(typeof(ExternalTexture));
    }
}
