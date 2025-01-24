namespace T3.Data.AuditableBuilder.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class BaseTypeDeclarationExtension
{
    internal static BaseTypeDeclarationSyntax AddAttributes(
        this BaseTypeDeclarationSyntax classDeclarationSyntax,
        params string[] attributeIdentifiers)
    {
        var attributes = attributeIdentifiers.Select(a => SyntaxFactory.Attribute(
            SyntaxFactory.IdentifierName(a)));

        classDeclarationSyntax.
            AttributeLists
                .Add(SyntaxFactory.AttributeList(
                    SeparatedSyntaxList
                        .Create(
                            new ReadOnlySpan<AttributeSyntax>(attributes.ToArray()))));

        return classDeclarationSyntax;
    }
}
