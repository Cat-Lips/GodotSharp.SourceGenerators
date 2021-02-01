using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using GodotTests.TestScenes;
using GodotTests.TestScenes.Script;
using GodotTests.Utilities;

namespace GodotTests.TestRunner
{
    public class TestRunner : HSplitContainer
    {
        private ITest curTest;
        private int failCount;
        private int frameCount;
        private int successCount;
        private readonly Queue<Func<ITest>> tests = new(Tests);

        public static IEnumerable<Func<ITest>> Tests
        {
            get
            {
                yield return () => GetTest<RootScene>();
                yield return () => GetTest<RootSceneWithNoNamespace>();
                yield return () => GetTest<EmptyScene>();
                yield return () => GetTest<EmptySceneWithNoNamespace>();
                yield return () => GetTest<InheritedScene>();
                yield return () => GetTest<InstancedScene>();
                yield return () => GetTest<InstancedSceneFromDifferentNamespace>();
                yield return () => GetTest<ScriptForSceneWithDifferentName>();
                yield return () => GetTest<ScriptForSceneWithDifferentPath>();
            }
        }

        public Node TestView { get; private set; }
        public RichTextLabel LogView { get; private set; }
        public RichTextLabel ErrorView { get; private set; }
        public SplitContainer SplitView { get; private set; }

        public override void _EnterTree()
        {
            TestView = GetNode("SplitView/TestView");
            LogView = GetNode<RichTextLabel>("LogView");
            SplitView = GetNode<SplitContainer>("SplitView");
            ErrorView = GetNode<RichTextLabel>("SplitView/ErrorView");

            DisplayTestHeader();
        }

        public override void _Process(float _)
        {
            ++frameCount;

            if (CurrentTestComplete())
            {
                UnloadCurrentTest();
                StartNextTest();
            }

            if (AllTestsComplete())
            {
                FinaliseTesting(quit: false);
            }
        }

        public override void _UnhandledInput(InputEvent e)
        {
            if (AllTestsComplete() && e.IsActionPressed("ui_cancel"))
            {
                GetTree().Quit(failCount);
            }
        }

        private bool CurrentTestComplete()
        {
            if (curTest is null)
                return true;

            if (frameCount > curTest.RequiredFrames)
            {
                DisplayTestResult("ProcessTests", curTest.EvaluateProcessTests(out var errors), errors);
                return true;
            }

            return false;
        }

        private void UnloadCurrentTest()
        {
            if (curTest is null) return;
            TestView.RemoveChild(curTest.Node);
            curTest.Node.QueueFree();
            curTest = null;
        }

        private void StartNextTest()
        {
            frameCount = 0;
            if (tests.Count == 0) return;
            curTest = tests.Dequeue().Invoke();

            DisplayTestResult("InitTests", curTest.RunInitTests(out var errors), errors);

            TestView.AddChild(curTest.Node);
        }

        private bool AllTestsComplete()
            => curTest is null && tests.Count is 0;

        private void FinaliseTesting(bool quit)
        {
            SetProcess(false);
            DisposeTestView();
            DisplayTestFooter();
            if (quit) GetTree().Quit(failCount);
            if (failCount is 0) DisposeErrorView();
        }

        private void DisplayTestHeader()
        {
            var header = "Running Tests...";
            GD.Print(); GD.Print(header);

            LogView.PushColor(Colors.Gray);
            LogView.AddText(header);
            LogView.Pop();

            LogView.Newline();
            LogView.PushTable(3, VAlign.Center);
        }

        private void DisplayTestResult(string test, bool? result, IEnumerable<string> errors)
        {
            switch (result)
            {
                case null:
                    DisplayTestNone(test);
                    break;
                case true:
                    DisplayTestPass(test);
                    break;
                case false:
                    DisplayTestFail(test, errors);
                    break;
            }

            void DisplayTestNone(string test)
            {
                GD.Print($"{curTest.Node.Name}.{test}: N/A");

                DisplayCell(curTest.Node.Name, Colors.Gray);
                DisplayCell(test, Colors.Gray);
                DisplayCell("N/A", Colors.Gray);
            }

            void DisplayTestPass(string test)
            {
                ++successCount;
                GD.Print($"{curTest.Node.Name}.{test}: PASS");

                DisplayCell(curTest.Node.Name);
                DisplayCell(test);
                DisplayCell("PASS", Colors.Green);
            }

            void DisplayTestFail(string test, IEnumerable<string> errors)
            {
                ++failCount;
                GD.Print($"{curTest.Node.Name}.{test}: FAIL");

                DisplayCell(curTest.Node.Name);
                DisplayCell(test);
                DisplayCell("FAIL", Colors.Red);

                ErrorView.PushColor(Colors.Red);
                ErrorView.AddText($"{curTest.Node.Name}.{test}:");
                ErrorView.Pop();

                ErrorView.Newline();
                foreach (var error in errors)
                {
                    GD.Print($" - {error}");
                    ErrorView.AddText($" - {error}");
                    ErrorView.Newline();
                }
            }

            void DisplayCell(string label, Color? colour = null)
            {
                LogView.PushCell();
                if (colour is not null) LogView.PushColor(colour.Value);
                LogView.AddText($"  {label}  ");
                if (colour is not null) LogView.Pop();
                LogView.Pop();
            }

        }

        private void DisplayTestFooter()
        {
            var footer = $"Tests Complete: {successCount} Passed, {failCount} Failed";
            GD.Print(footer); GD.Print();

            LogView.Pop(); // Table
            LogView.Newline();

            LogView.PushColor(Colors.Gray);
            LogView.AddText(footer);
            LogView.Pop();

            LogView.Newline();
        }

        private void DisposeTestView()
        {
            SplitView.DraggerVisibility = DraggerVisibilityEnum.HiddenCollapsed;
            TestView.QueueFree();
        }

        private void DisposeErrorView()
        {
            DraggerVisibility = DraggerVisibilityEnum.HiddenCollapsed;
            SplitView.QueueFree();
        }

        private static T GetTest<T>() where T : Node, ITest
        {
            var tscn = typeof(T).GetCustomAttribute<SceneTreeAttribute>().SceneFile;
            var test = (T)ResourceLoader.Load<PackedScene>(tscn).Instance();
            test.Name = typeof(T).Name;
            return test;
        }
    }
}
