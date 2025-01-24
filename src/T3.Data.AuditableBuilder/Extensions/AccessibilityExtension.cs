namespace T3.Data.AuditableBuilder.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal static class AccessibilityExtension
{
    internal static SyntaxToken ToSyntaxToken(this Accessibility accessibility)
    {
        return accessibility switch
        {
            Accessibility.Private => SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
            Accessibility.Protected => SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
            Accessibility.Internal => SyntaxFactory.Token(SyntaxKind.InternalKeyword),
            Accessibility.Public => SyntaxFactory.Token(SyntaxKind.PublicKeyword),
            Accessibility.ProtectedOrInternal => SyntaxFactory.Token(SyntaxKind.ProtectedKeyword).WithTrailingTrivia(SyntaxFactory.Space).WithTriviaFrom(SyntaxFactory.Token(SyntaxKind.InternalKeyword)),
            Accessibility.ProtectedAndInternal => SyntaxFactory.Token(SyntaxKind.PrivateKeyword).WithTrailingTrivia(SyntaxFactory.Space).WithTriviaFrom(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)),
            _ => SyntaxFactory.Token(SyntaxKind.PublicKeyword)
        };
    }
}
