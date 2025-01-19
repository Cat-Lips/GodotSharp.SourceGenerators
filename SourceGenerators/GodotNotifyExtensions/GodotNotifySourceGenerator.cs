﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions;

[Generator]
internal class GodotNotifySourceGenerator : SourceGeneratorForDeclaredPropertyWithAttribute<Godot.NotifyAttribute>
{
    private static Template GodotNotifyTemplate => field ??= Template.Parse(Resources.GodotNotifyTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IPropertySymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new GodotNotifyDataModel(symbol, node);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = GodotNotifyTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
