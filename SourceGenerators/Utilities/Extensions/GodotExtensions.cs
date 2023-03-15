namespace GodotSharp.SourceGenerators
{
    internal static class GD
    {
        private const string GodotProjectFile = "project.godot";

        private static string _resPath = null;
        public static string GetProjectRoot(string path)
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

        public static string GetProjectFile(string path)
            => $"{GetProjectRoot(path)}/{GodotProjectFile}";
    }
}
