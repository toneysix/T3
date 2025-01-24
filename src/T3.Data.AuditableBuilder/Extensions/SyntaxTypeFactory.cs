namespace T3.Data.AuditableBuilder.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class SyntaxTypeFactory
{
    internal static TypeSyntax GetTypeSyntax(string identifier)
    {
        return SyntaxFactory
            .IdentifierName(SyntaxFactory.Identifier(identifier));
    }

    internal static TypeSyntax GetTypeSyntax(string identifier, params string[] arguments)
    {
        return GetTypeSyntax(identifier, arguments.Select(GetTypeSyntax).ToArray());
    }

    internal static TypeSyntax GetTypeSyntax(string identifier, params TypeSyntax[] arguments)
    {
        return SyntaxFactory.GenericName(
            SyntaxFactory.Identifier(identifier),
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SeparatedList(arguments.Select(t =>
                {
                    if (t is not GenericNameSyntax genericType)
                        return t;

                    return GetTypeSyntax(
                        genericType.Identifier.ToString(),
                        genericType.TypeArgumentList.Arguments.ToArray());
                }))));
    }

    internal static BaseTypeDeclarationSyntax ToBaseTypeDeclaration(this TypeSyntax typeSyntax, SemanticModel semanticModel)
    {
        var typeSymbol = semanticModel.GetTypeInfo(typeSyntax).Type;
        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
            throw new Exception();

        if (namedTypeSymbol.TypeKind != TypeKind.Class)
            throw new Exception();

        var originalDefinition = namedTypeSymbol.OriginalDefinition;
        if (!originalDefinition.Locations.Any())
            throw new Exception();

        return originalDefinition.Locations
            .First()
                .SourceTree!
                .GetRoot()
                .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .FirstOrDefault(x => x.Identifier.ValueText == namedTypeSymbol.Name);
    }
}
