using Godot;

namespace GodotTests.TestScenes.Issue72
{
	public partial class Scene3 : Node
	{
		public enum TestEnum { a, b, c }

		public int i1;
		public int? i2;
		public bool b1;
		public bool? b2;
		public float f1;
		public float? f2;
		public string s1;
		public string s2;
		public TestEnum e1;
		public TestEnum? e2;

		[OnInstantiate]
		private void Init(
			ref int a, out int b,
			int i1 = 7, int? i2 = -7,
			bool b1 = true, bool? b2 = false,
			float f1 = .7f, float? f2 = -.7f,
			string s1 = "x", string s2 = "",
			TestEnum e1 = TestEnum.b, TestEnum? e2 = TestEnum.c)
		{
			a = 7; b = 7;
			this.i1 = i1;
			this.i2 = i2;
			this.b1 = b1;
			this.b2 = b2;
			this.f1 = f1;
			this.f2 = f2;
			this.s1 = s1;
			this.s2 = s2;
			this.e1 = e1;
			this.e2 = e2;
		}
	}
}
