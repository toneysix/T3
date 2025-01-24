namespace T3.Data.AuditableBuilder.ClassBuilder;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using T3.Data.AuditableBuilder.Extensions;

internal class ConstructorDeclarationBuilder
{
    private readonly ClassDeclarationBuilder builder;

    internal ConstructorDeclarationBuilder(ClassDeclarationBuilder builder)
    {
        this.builder = builder;
    }

    internal ClassDeclarationBuilder EmptyFromBase()
    {
        var baseType = this.builder.BaseType as SimpleNameSyntax;
        string baseTypeName = baseType!.Identifier.ToString();

        if (baseType is GenericNameSyntax genericBaseType)
            baseTypeName += $"`{genericBaseType.TypeArgumentList.Arguments.Count}";

        var baseTypeInfo = this.builder.Compilation
            .GetTypeByMetadataName(baseTypeName);

        if (baseTypeInfo is not INamedTypeSymbol baseClassSymbol)
            throw new Exception($"Cannot create empty constructors from base class due to base class not found {this.builder.BaseType!.Kind()}");

        var baseConstructors = baseClassSymbol.Constructors
            .Where(c => c.Parameters.Length > 0)
            .Select(this.CreateConstructorFrom);

        return this.builder.WithMembers(baseConstructors.ToArray());
    }

    private ConstructorDeclarationSyntax CreateConstructorFrom(IMethodSymbol baseConstructor)
    {
        return SyntaxFactory.ConstructorDeclaration(this.builder.Identifier)     
            .AddModifiers(baseConstructor.DeclaredAccessibility.ToSyntaxToken())
            .AddParameterListParameters(
                baseConstructor.Parameters
                .Select(p => SyntaxFactory.Parameter(SyntaxFactory.Identifier(p.Name))
                    .WithType(SyntaxFactory.ParseTypeName(p.Type.ToDisplayString())))
                .ToArray())
            .WithInitializer(
                  SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                    .AddArgumentListArguments(baseConstructor.Parameters
                        .Select(p => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(p.Name)))
                        .ToArray())
              )
              .WithBody(SyntaxFactory.Block());
    }
}
