using System;
using System.Runtime.CompilerServices;

namespace Godot
{
    /// <summary>
    /// Apply this attribute to a partial class to instruct the
    /// <see cref="LocalizationKeysGenerator"/> to emit strongly typed
    /// localization keys.  The <paramref name="filePath"/> should point to
    /// a CSV file (or any line‑based file) containing translation keys in
    /// the first column.  The optional <paramref name="dataType"/> controls
    /// whether generated members use <c>StringName</c> (the default) or
    /// <c>string</c> fields.  The <paramref name="classPath"/> argument is
    /// automatically supplied by the compiler and used to resolve relative
    /// file paths; it should not be set explicitly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LocalizationKeysAttribute : Attribute
    {
        public LocalizationKeysAttribute(string filePath, string dataType = "StringName", [CallerFilePath] string classPath = null)
        {
            FilePath = filePath;
            DataType = dataType;
            ClassPath = classPath;
        }

        /// <summary>Relative or absolute path to the translation file.</summary>
        public string FilePath { get; }
        /// <summary>Type used for generated constants; either <c>StringName</c> or <c>string</c>.</summary>
        public string DataType { get; }
        /// <summary>Path to the .cs file where the attribute is applied; supplied by the compiler.</summary>
        public string ClassPath { get; }
    }
}