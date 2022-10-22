using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	internal partial class EditableChildrenTest : SceneWithEditableChildren, ITest
	{
		void ITest.InitTests()
		{
			TestUniqueNames();
			TestEditedValues();

			void TestUniqueNames()
			{
				TestScenes();
				TestScenesWithEditableChildren();

				void TestScenes()
				{
					Scene.Should().Be(_.Scene);
					EditableScene.Should().Be(_.EditableScene);
					EditedScene.Should().Be(_.EditedScene);

					SceneInInheritedScene.Should().Be(_.Child2.SceneInInheritedScene);
					EditableSceneInInheritedScene.Should().Be(_.Child2.EditableSceneInInheritedScene);
					EditedSceneInInheritedScene.Should().Be(_.Child2.EditedSceneInInheritedScene);

					SceneInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.SceneInInstancedScene);
					EditableSceneInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.EditableSceneInInstancedScene);
					EditedSceneInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.EditedSceneInInstancedScene);

					SceneInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.SceneInChildOfInstancedScene);
					EditableSceneInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.EditableSceneInChildOfInstancedScene);
					EditedSceneInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.EditedSceneInChildOfInstancedScene);
				}

				void TestScenesWithEditableChildren()
				{
					SceneWithEditableChildren.Should().Be(_.SceneWithEditableChildren);
					EditableSceneWithEditableChildren.Should().Be(_.EditableSceneWithEditableChildren.Get());
					EditedSceneWithEditableChildren.Should().Be(_.EditedSceneWithEditableChildren.Get());

					SceneWithEditableChildrenInInheritedScene.Should().Be(_.Child2.SceneWithEditableChildrenInInheritedScene);
					EditableSceneWithEditableChildrenInInheritedScene.Should().Be(_.Child2.EditableSceneWithEditableChildrenInInheritedScene.Get());
					EditedSceneWithEditableChildrenInInheritedScene.Should().Be(_.Child2.EditedSceneWithEditableChildrenInInheritedScene.Get());

					SceneWithEditableChildrenInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.SceneWithEditableChildrenInInstancedScene);
					EditableSceneWithEditableChildrenInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.EditableSceneWithEditableChildrenInInstancedScene.Get());
					EditedSceneWithEditableChildrenInInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.EditedSceneWithEditableChildrenInInstancedScene.Get());

					SceneWithEditableChildrenInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.SceneWithEditableChildrenInChildOfInstancedScene);
					EditableSceneWithEditableChildrenInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.EditableSceneWithEditableChildrenInChildOfInstancedScene.Get());
					EditedSceneWithEditableChildrenInChildOfInstancedScene.Should().Be(_.EditedSceneWithEditableChildren.Child2.EditedSceneWithEditableChildrenInChildOfInstancedScene.Get());
				}
			}

			void TestEditedValues()
			{
				_.Child1.MyLabel.Should().BeOfType<Label>();
				_.Child2.MyLabel.Should().BeOfType<MyLabel>();

				_.Child1.MyLabel.Text.Should().Be("Scene");
				_.Child2.MyLabel.Text.Should().Be("Inherited scene edited from test");

				TestScenes();
				TestScenesWithEditableChildren();

				void TestScenes()
				{
					_.Child2.SceneInInheritedScene.MyLabel.Should().BeOfType<Label>();
					_.Child2.EditableSceneInInheritedScene.MyLabel.Should().BeOfType<Label>();
					_.Child2.EditedSceneInInheritedScene.MyLabel.Should().BeOfType<Label>();

					_.Child2.SceneInInheritedScene.MyLabel.Text.Should().Be("Scene");
					_.Child2.EditableSceneInInheritedScene.MyLabel.Text.Should().Be("Scene");
					_.Child2.EditedSceneInInheritedScene.MyLabel.Text.Should().Be("Child of inherited scene edited from test");

					_.Scene.MyLabel.Should().BeOfType<Label>();
					_.EditableScene.MyLabel.Should().BeOfType<Label>();
					_.EditedScene.MyLabel.Should().BeOfType<Label>();

					_.Scene.MyLabel.Text.Should().Be("Scene");
					_.EditableScene.MyLabel.Text.Should().Be("Scene");
					_.EditedScene.MyLabel.Text.Should().Be("Instanced scene edited from test");

					_.EditedSceneWithEditableChildren.SceneInInstancedScene.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.EditableSceneInInstancedScene.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.EditedSceneInInstancedScene.MyLabel.Should().BeOfType<Label>();

					_.EditedSceneWithEditableChildren.SceneInInstancedScene.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.EditableSceneInInstancedScene.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.EditedSceneInInstancedScene.MyLabel.Text.Should().Be("Child of instanced scene edited from test");

					_.EditedSceneWithEditableChildren.Child2.SceneInChildOfInstancedScene.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.EditableSceneInChildOfInstancedScene.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.EditedSceneInChildOfInstancedScene.MyLabel.Should().BeOfType<Label>();

					_.EditedSceneWithEditableChildren.Child2.SceneInChildOfInstancedScene.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.EditableSceneInChildOfInstancedScene.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.EditedSceneInChildOfInstancedScene.MyLabel.Text.Should().Be("Child of instanced scene child edited from test");
				}

				void TestScenesWithEditableChildren()
				{
					_.Child2.SceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.Child2.SceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.Child2.EditableSceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.Child2.EditableSceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.Child2.EditedSceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.Child2.EditedSceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();

					_.Child2.SceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.Child2.SceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.Child2.EditableSceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.Child2.EditableSceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.Child2.EditedSceneWithEditableChildrenInInheritedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.Child2.EditedSceneWithEditableChildrenInInheritedScene.Child2.MyLabel.Text.Should().Be("Child of inherited scene edited from test");

					_.SceneWithEditableChildren.Child1.MyLabel.Should().BeOfType<Label>();
					_.SceneWithEditableChildren.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditableSceneWithEditableChildren.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditableSceneWithEditableChildren.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditedSceneWithEditableChildren.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.Get().MyLabel.Should().BeOfType<MyLabel>(); // Use 'SceneTree(traverseInstancedScenes: true)' to access 'Child2.MyLabel' directly

					_.SceneWithEditableChildren.Child1.MyLabel.Text.Should().Be("Scene");
					_.SceneWithEditableChildren.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditableSceneWithEditableChildren.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditableSceneWithEditableChildren.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditedSceneWithEditableChildren.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.Get().MyLabel.Text.Should().Be("Instanced scene edited from test"); // Use 'SceneTree(traverseInstancedScenes: true)' to access 'Child2.MyLabel' directly

					_.EditedSceneWithEditableChildren.SceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.SceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditedSceneWithEditableChildren.EditableSceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.EditableSceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditedSceneWithEditableChildren.EditedSceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.EditedSceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();

					_.EditedSceneWithEditableChildren.SceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.SceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditedSceneWithEditableChildren.EditableSceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.EditableSceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditedSceneWithEditableChildren.EditedSceneWithEditableChildrenInInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.EditedSceneWithEditableChildrenInInstancedScene.Child2.MyLabel.Text.Should().Be("Child of instanced scene edited from test");

					_.EditedSceneWithEditableChildren.Child2.SceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.SceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditedSceneWithEditableChildren.Child2.EditableSceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.EditableSceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();
					_.EditedSceneWithEditableChildren.Child2.EditedSceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Should().BeOfType<Label>();
					_.EditedSceneWithEditableChildren.Child2.EditedSceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Should().BeOfType<MyLabel>();

					_.EditedSceneWithEditableChildren.Child2.SceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.SceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditedSceneWithEditableChildren.Child2.EditableSceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.EditableSceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Text.Should().Be("Edited!");
					_.EditedSceneWithEditableChildren.Child2.EditedSceneWithEditableChildrenInChildOfInstancedScene.Child1.MyLabel.Text.Should().Be("Scene");
					_.EditedSceneWithEditableChildren.Child2.EditedSceneWithEditableChildrenInChildOfInstancedScene.Child2.MyLabel.Text.Should().Be("Child of instanced scene child edited from test");
				}
			}
		}
	}
}
