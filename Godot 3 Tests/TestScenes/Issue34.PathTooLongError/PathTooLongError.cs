using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	public partial class PathTooLongError : Node, ITest
	{
		void ITest.InitTests()
		{
			// If it compiles, test passes
		}
	}

	[Tool]
	internal partial class MaxLengthForFilenameIs260 : Node
	{
		// PASS - Generated Filename (102): GodotTests.TestScenes.MaxLengthForFilenameIs260.CurrentLengthOfGeneratedFileIs102(string, string).g.cs
		// FAIL - Current filename (272) triggered error
		// FAIL - Truncated filename (260) - ended with "...FileIs272(str.g.cs"
		// FAIL - Truncated filename (259) - ended with "...FileIs272(st.g.cs"
		// FAIL - Truncated filename (258) - ended with "...FileIs272(s.g.cs"
		// FAIL - Truncated filename (257) - ended with "...FileIs272(.g.cs"
		// FAIL - Truncated filename (256) - ended with "...FileIs272.g.cs"
		// PASS - Truncated filename (255) - ended with "...FileIs27.g.cs"
		// (So actual max length is 255, not 260)
		[OnInstantiate]
		private void CurrentLengthOfGeneratedFileIs102_CurrentLengthOfGeneratedFileIs136_CurrentLengthOfGeneratedFileIs170_CurrentLengthOfGeneratedFileIs204_CurrentLengthOfGeneratedFileIs238_CurrentLengthOfGeneratedFileIs272(string a, string b) { }
	}
}
