using System;
using System.Runtime.CompilerServices;

namespace Godot
{
    /// <summary>
    /// Marks a class so that localization keys are automatically generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LocalizationKeysAttribute : Attribute
    {
        /// <summary>
        /// Creates a new LocalizationKeysAttribute.
        /// </summary>
        /// <param name="filePath">Relative or absolute path to the translation file.  Paths starting with <c>res://</c> will be resolved relative to the Godot project root at build time.</param>
        /// <param name="dataType">Name of the data type to generate fields for.  Use <c>"StringName"</c> (default) or <c>"string"</c>.</param>
        /// <param name="classPath">Compiler supplied file path to the source file that declares this attribute.  Do not supply this parameter manually.</param>
        public LocalizationKeysAttribute(string filePath, string dataType = "StringName", [CallerFilePath] string classPath = null)
        {
            FilePath = filePath;
            DataType = dataType;
            ClassPath = classPath;
        }

        public string FilePath { get; }
        public string DataType { get; }
        public string ClassPath { get; }
    }
}