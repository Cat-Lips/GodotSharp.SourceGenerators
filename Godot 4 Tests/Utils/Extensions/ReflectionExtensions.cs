﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;

public static class ReflectionExtensions
{
    private const BindingFlags All = BindingFlags.Public/* | BindingFlags.NonPublic*/ | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
    private static bool IsCompilerGenerated(this MemberInfo m) => Attribute.IsDefined(m, typeof(CompilerGeneratedAttribute));

    public static IEnumerable<string> Events(this Type t) => t.GetEvents(All).Where(x => !x.IsSpecialName && !x.IsCompilerGenerated()).Select(x => x.Name);
    public static IEnumerable<string> Fields(this Type t) => t.GetFields(All).Where(x => !x.IsSpecialName && !x.IsCompilerGenerated()).Select(x => x.Name);
    public static IEnumerable<string> Methods(this Type t) => t.GetMethods(All).Where(x => !x.IsSpecialName && !x.IsCompilerGenerated()).Select(x => x.Name);
    public static IEnumerable<string> Properties(this Type t) => t.GetProperties(All).Where(x => !x.IsSpecialName && !x.IsCompilerGenerated()).Select(x => x.Name);
    public static IEnumerable<string> NestedTypes(this Type t) => t.GetNestedTypes(All).Where(x => !x.IsSpecialName && !x.IsCompilerGenerated()).Select(x => x.Name);

    public static void ShouldConsistOf(this Type t,
        IEnumerable<string> Events = null,
        IEnumerable<string> Fields = null,
        IEnumerable<string> Methods = null,
        IEnumerable<string> Properties = null,
        IEnumerable<string> NestedTypes = null)
    {
        t.Events().Should().BeEquivalentTo(Events ?? []);
        t.Fields().Should().BeEquivalentTo(Fields ?? []);
        t.Methods().Should().BeEquivalentTo(Methods ?? []);
        t.Properties().Should().BeEquivalentTo(Properties ?? []);
        t.NestedTypes().Should().BeEquivalentTo(NestedTypes ?? []);
    }
}
