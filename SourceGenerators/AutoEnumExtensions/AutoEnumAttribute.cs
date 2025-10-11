using System;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoEnumAttribute(string identityProperty = "Name") : Attribute
    {
        /// <summary>
        /// The property name used as the instance identifier when generating conversion methods.
        /// Default is <c>"Name"</c>.
        /// </summary>
        public string IdentityProperty { get; } = identityProperty;
    }
}
