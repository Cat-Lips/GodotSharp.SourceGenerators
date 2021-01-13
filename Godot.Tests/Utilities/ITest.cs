using System;
using System.Collections.Generic;
using FluentAssertions.Execution;
using Godot;

namespace GodotTests.Utilities
{
    public interface ITest
    {
        Node Node => (Node)this;
        int RequiredFrames => 0;

        protected void InitTests() => throw new NotImplementedException();
        protected void ProcessTests() => throw new NotImplementedException();

        bool? RunInitTests(out IEnumerable<string> errors)
            => TestScope(InitTests, out errors);

        bool? EvaluateProcessTests(out IEnumerable<string> errors)
            => TestScope(ProcessTests, out errors);

        protected bool? TestScope(Action test, out IEnumerable<string> errors)
        {
            try
            {
                using (new AssertionScope())
                {
                    test();
                }

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
                return false;
            }
        }
    }
}
