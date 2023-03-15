using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public static class SyntaxExtensions
    {
        public static string GetLeadingComments(this SyntaxNode node)
        {
            var comments = node.GetLeadingTrivia().Where(x =>
                x.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                x.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                x.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia) ||
                x.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
            return string.Join(Environment.NewLine, comments);
        }

        public static IEnumerable<(string Property, string Comment)> GetPropertyComments(this SyntaxNode node, string strip = "// ")
        {
            return node.DescendantNodes()
                .Where(x => x.IsKind(SyntaxKind.PropertyDeclaration))
                .Cast<PropertyDeclarationSyntax>()
                .Select(x => (x.Identifier.ToString(), x.GetLeadingComments().Replace(strip, "")))
                .Where(x => !string.IsNullOrWhiteSpace(x.Item2));
        }
    }
}
