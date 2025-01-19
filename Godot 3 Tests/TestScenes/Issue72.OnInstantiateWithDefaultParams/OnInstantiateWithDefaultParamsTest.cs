using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes.Issue72;

[SceneTree]
public partial class OnInstantiateWithDefaultParamsTest : Node, ITest
{
	void ITest.InitTests()
	{
		TestWithArgs();
		TestDefaults();

		void TestWithArgs()
		{
			Test1();
			Test2();
			Test3();

			void Test1()
			{
				var sut = Scene1.Instantiate(
					7, -7,
					true, false,
					.7f, -.7f,
					"x", "",
					Scene1.TestEnum.b, Scene1.TestEnum.c,
					new Scene1.TestStruct { x = Scene1.TestEnum.b }, new Scene1.TestStruct { x = Scene1.TestEnum.c },
					Scene2.TestEnum.b, Scene2.TestEnum.c,
					new Scene2.TestStruct { x = Scene2.TestEnum.b }, new Scene2.TestStruct { x = Scene2.TestEnum.c });

				sut.i1.Should().Be(7); sut.i2.Should().Be(-7);
				sut.b1.Should().Be(true); sut.b2.Should().Be(false);
				sut.f1.Should().Be(.7f); sut.f2.Should().Be(-.7f);
				sut.s1.Should().Be("x"); sut.s2.Should().Be("");
				sut.e1.Should().Be(Scene1.TestEnum.b); sut.e2.Should().Be(Scene1.TestEnum.c);
				sut.x1.x.Should().Be(Scene1.TestEnum.b); sut.x2.Value.x.Should().Be(Scene1.TestEnum.c);
				sut.ee1.Should().Be(Scene2.TestEnum.b); sut.ee2.Should().Be(Scene2.TestEnum.c);
				sut.xx1.x.Should().Be(Scene2.TestEnum.b); sut.xx2.Value.x.Should().Be(Scene2.TestEnum.c);
			}

			void Test2()
			{
				var sut = Scene2.Instantiate(
					7, -7,
					true, false,
					.7f, -.7f,
					"x", "",
					Scene2.TestEnum.b, Scene2.TestEnum.c,
					new Scene2.TestStruct { x = Scene2.TestEnum.b }, new Scene2.TestStruct { x = Scene2.TestEnum.c },
					Scene1.TestEnum.b, Scene1.TestEnum.c,
					new Scene1.TestStruct { x = Scene1.TestEnum.b }, new Scene1.TestStruct { x = Scene1.TestEnum.c });

				sut.i1.Should().Be(7); sut.i2.Should().Be(-7);
				sut.b1.Should().Be(true); sut.b2.Should().Be(false);
				sut.f1.Should().Be(.7f); sut.f2.Should().Be(-.7f);
				sut.s1.Should().Be("x"); sut.s2.Should().Be("");
				sut.e1.Should().Be(Scene2.TestEnum.b); sut.e2.Should().Be(Scene2.TestEnum.c);
				sut.x1.x.Should().Be(Scene2.TestEnum.b); sut.x2.Value.x.Should().Be(Scene2.TestEnum.c);
				sut.ee1.Should().Be(Scene1.TestEnum.b); sut.ee2.Should().Be(Scene1.TestEnum.c);
				sut.xx1.x.Should().Be(Scene1.TestEnum.b); sut.xx2.Value.x.Should().Be(Scene1.TestEnum.c);
			}

			void Test3()
			{
				var a = 0;
				var sut = Scene3.Instantiate(
					ref a, out var b,
					7, -7,
					true, false,
					.7f, -.7f,
					"x", "",
					Scene3.TestEnum.b, Scene3.TestEnum.c);

				a.Should().Be(7); b.Should().Be(7);
				sut.i1.Should().Be(7); sut.i2.Should().Be(-7);
				sut.b1.Should().Be(true); sut.b2.Should().Be(false);
				sut.f1.Should().Be(.7f); sut.f2.Should().Be(-.7f);
				sut.s1.Should().Be("x"); sut.s2.Should().Be("");
				sut.e1.Should().Be(Scene3.TestEnum.b); sut.e2.Should().Be(Scene3.TestEnum.c);
			}
		}

		void TestDefaults()
		{
			Test1();
			Test2();
			Test3();

			void Test1()
			{
				var sut = Scene1.Instantiate(
					default, null,
					default, null,
					default, null,
					default, null,
					default, null,
					default, null,
					default, null,
					default, null);

				sut.i1.Should().Be(default); sut.i2.Should().Be(null);
				sut.b1.Should().Be(default); sut.b2.Should().Be(null);
				sut.f1.Should().Be(default); sut.f2.Should().Be(null);
				sut.s1.Should().Be(default); sut.s2.Should().Be(null);
				sut.e1.Should().Be(default); sut.e2.Should().Be(null);
				sut.x1.Should().Be(default(Scene1.TestStruct)); sut.x2.Should().Be(null);
				sut.ee1.Should().Be(default); sut.ee2.Should().Be(null);
				sut.xx1.Should().Be(default(Scene2.TestStruct)); sut.xx2.Should().Be(null);
			}

			void Test2()
			{
				var sut = Scene2.Instantiate();

				sut.i1.Should().Be(default); sut.i2.Should().Be(null);
				sut.b1.Should().Be(default); sut.b2.Should().Be(null);
				sut.f1.Should().Be(default); sut.f2.Should().Be(null);
				sut.s1.Should().Be(default); sut.s2.Should().Be(null);
				sut.e1.Should().Be(default); sut.e2.Should().Be(null);
				sut.x1.Should().Be(default(Scene2.TestStruct)); sut.x2.Should().Be(null);
				sut.ee1.Should().Be(default); sut.ee2.Should().Be(null);
				sut.xx1.Should().Be(default(Scene1.TestStruct)); sut.xx2.Should().Be(null);
			}

			void Test3()
			{
				var a = 0;
				var sut = Scene3.Instantiate(ref a, out var b);

				a.Should().Be(7); b.Should().Be(7);
				sut.i1.Should().Be(7); sut.i2.Should().Be(-7);
				sut.b1.Should().Be(true); sut.b2.Should().Be(false);
				sut.f1.Should().Be(.7f); sut.f2.Should().Be(-.7f);
				sut.s1.Should().Be("x"); sut.s2.Should().Be("");
				sut.e1.Should().Be(Scene3.TestEnum.b); sut.e2.Should().Be(Scene3.TestEnum.c);
			}
		}
	}
}
