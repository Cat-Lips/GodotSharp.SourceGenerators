using System;
using Godot;

namespace GodotTests.Utilities
{
    public static class GodotExtensions
    {
        public static void CallDeferred(this Node node, Action action, int frameCount = 0)
        {
            var root = node.GetTree();
            root.ProcessFrame += OnIdle;

            void OnIdle()
            {
                if (--frameCount < 0)
                {
                    root.ProcessFrame -= OnIdle;
                    action();
                }
            }
        }
    }
}
