using Scriban;
using Scriban.Runtime;

namespace GodotSharp.SourceGenerators;

internal static class ScribanExtensions
{
    public static string Render(this Template source, object model, params ScriptObject[] xtras)
    {
        ImportMain(out var main);
        InitContext(out var context);
        return source.Render(context);

        void ImportMain(out ScriptObject main)
            => (main = []).Import(model, null, member => member.Name);

        void InitContext(out TemplateContext context)
        {
            context = new TemplateContext
            {
                LoopLimit = 0,
                RecursiveLimit = 0,
                MemberRenamer = member => member.Name,
            };

            foreach (var content in xtras)
                context.PushGlobal(content);

            context.PushGlobal(main);
        }
    }

    public static ScriptObject ToScribanScript(this Template source)
    {
        var content = new ScriptObject();
        var context = new TemplateContext();
        context.PushGlobal(content);
        source.Render(context);
        return content;
    }
}
