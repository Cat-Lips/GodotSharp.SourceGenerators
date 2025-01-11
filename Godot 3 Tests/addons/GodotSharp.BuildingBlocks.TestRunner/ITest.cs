using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions.Execution;
using Godot;

namespace GodotSharp.BuildingBlocks.TestRunner;

public interface ITest
{
    Node Node => (Node)this;
    int RequiredFrames => 0;

    /// <summary>
    /// Implement to test initial state of scene before being added to tree (ie, after tscn load)
    /// </summary>
    protected void InitTests() => throw new NotImplementedException();

    /// <summary>
    /// Implement to test state of scene after being added to tree
    /// </summary>
    protected void EnterTests() => throw new NotImplementedException();

    /// <summary>
    /// Implement to test state of scene before OnProcess is called
    /// </summary>
    protected void ReadyTests() => throw new NotImplementedException();

    /// <summary>
    /// Implement to test state of scene after OnProcess has been called x times (where x = RequiredFrames)
    /// </summary>
    protected void ProcessTests() => throw new NotImplementedException();

    /// <summary>
    /// Implement to test state of scene after being removed from tree
    /// </summary>
    protected void ExitTests() => throw new NotImplementedException();

    bool? RunInitTests(out IEnumerable<string> errors) => TestScope(InitTests, out errors);
    bool? RunEnterTests(out IEnumerable<string> errors) => TestScope(EnterTests, out errors);
    bool? RunReadyTests(out IEnumerable<string> errors) => TestScope(ReadyTests, out errors);
    bool? RunProcessTests(out IEnumerable<string> errors) => TestScope(ProcessTests, out errors);
    bool? RunExitTests(out IEnumerable<string> errors) => TestScope(ExitTests, out errors);

    protected bool? TestScope(Action test, out IEnumerable<string> errors)
    {
        try
        {
            using (new AssertionScope()) test();

            errors = null;
            return true;
        }
        catch (NotImplementedException)
        {
            errors = null;
            return null;
        }
        catch (Exception e)
        {
            errors = e.Message.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            GD.PushError(e.ToString());
            return false;
        }
    }

    public static T GetTest<T>() where T : Node, ITest
    {
        var tscn = typeof(T).GetCustomAttribute<SceneTreeAttribute>().SceneFile;
        var test = GD.Load<PackedScene>(tscn).Instance<T>();
        test.Name = typeof(T).Name;
        return test;
    }
}
