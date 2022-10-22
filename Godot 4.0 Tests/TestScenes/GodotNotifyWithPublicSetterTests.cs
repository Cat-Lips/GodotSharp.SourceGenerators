using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	internal partial class GodotNotifyWithPublicSetterTests : Node, ITest
	{
		[Notify] private int value1;
		[Notify("public")] private int value2;
		[Notify("internal")] private int value3;
		[Notify("protected")] private int value4;
		[Notify("protected internal")] private int value5;
		[Notify("private protected")] private int value6;

		[Notify(export: true)] private int value11;
		[Notify("public", true)] private int value12;
		[Notify("internal", true)] private int value13;
		[Notify("protected", true)] private int value14;
		[Notify("protected internal", true)] private int value15;
		[Notify("private protected", true)] private int value16;

		void ITest.InitTests()
		{
			Check(nameof(Value1), x => x.IsPrivate, x => x.BeNull());
			Check(nameof(Value2), x => x.IsPublic, x => x.BeNull());
			Check(nameof(Value3), x => x.IsAssembly, x => x.BeNull());
			Check(nameof(Value4), x => x.IsFamily, x => x.BeNull());
			Check(nameof(Value5), x => x.IsFamilyOrAssembly, x => x.BeNull());
			Check(nameof(Value6), x => x.IsFamilyAndAssembly, x => x.BeNull());

			Check(nameof(Value11), x => x.IsPublic, x => x.NotBeNull());
			Check(nameof(Value12), x => x.IsPublic, x => x.NotBeNull());
			Check(nameof(Value13), x => x.IsPublic, x => x.NotBeNull());
			Check(nameof(Value14), x => x.IsPublic, x => x.NotBeNull());
			Check(nameof(Value15), x => x.IsPublic, x => x.NotBeNull());
			Check(nameof(Value16), x => x.IsPublic, x => x.NotBeNull());

			void Check(string name, Func<MethodInfo, bool> checkScope, Action<FluentAssertions.Primitives.ObjectAssertions> checkExport)
			{
				checkScope(GetType().GetProperty(name).GetSetMethod(true)).Should().BeTrue();
				checkExport(GetType().GetProperty(name).GetCustomAttribute<ExportAttribute>().Should());
			}
		}
	}
}
