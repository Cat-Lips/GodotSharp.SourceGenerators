namespace GodotSharp.SourceGenerators
{
    internal static class GD
    {
        private const string GodotProjectFile = "project.godot";

        private static string _resPath = null;
        private static string GetProjectRoot(string path)
        {
            return _resPath is null || !path.StartsWith(_resPath)
                ? _resPath = GetProjectRoot(path) : _resPath;

            static string GetProjectRoot(string path)
            {
                var dir = Path.GetDirectoryName(path);

                while (dir is not null)
                {
                    if (File.Exists($"{dir}/{GodotProjectFile}"))
                        return dir;

                    dir = Path.GetDirectoryName(dir);
                }

                throw new Exception($"Could not find {GodotProjectFile} in path {Path.GetDirectoryName(path)}");
            }
        }

        public static string GetProjectFile(string path, string projectDir = null)
            => Path.Combine(projectDir ?? GetProjectRoot(path), GodotProjectFile);

        public static string GetResourcePath(string path, string projectDir = null)
            => $"res://{path[(projectDir ?? GetProjectRoot(path)).Length..].Replace("\\", "/").TrimStart('/')}";
    }
}
