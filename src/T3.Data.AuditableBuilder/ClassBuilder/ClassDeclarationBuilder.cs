namespace T3.Data.AuditableBuilder.ClassBuilder;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class ClassDeclarationBuilder
{
    internal readonly string Identifier;

    internal TypeSyntax? BaseType { get; private set; }

    internal Compilation Compilation { get; private set; }

    private List<SyntaxKind> modifiers = new List<SyntaxKind>();

    private List<MemberDeclarationSyntax> members = new List<MemberDeclarationSyntax>();

    internal ClassDeclarationBuilder(string identifier, Compilation compilation)
    {
        this.Identifier = identifier;
        this.Compilation = compilation;
    }

    internal BaseTypeBuilder WithBaseType(TypeSyntax baseType)
    {
        this.BaseType = baseType;

        return new BaseTypeBuilder(this);
    }

    internal ClassDeclarationBuilder WithModifier(params SyntaxKind[] modifiers)
    {
        this.modifiers.AddRange(modifiers);

        return this;
    }

    internal ClassDeclarationBuilder WithMembers(params MemberDeclarationSyntax[] members)
    {
        this.members.AddRange(members);

        return this;
    }

    internal ConstructorDeclarationBuilder WithConstructor()
    {
        return new ConstructorDeclarationBuilder(this);
    }

    internal ClassDeclarationSyntax Build()
    {
        var classDeclaration = SyntaxFactory.ClassDeclaration(this.Identifier);

        if (this.BaseType is not null)
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(this.BaseType));

        if (this.modifiers.Any())
            classDeclaration = classDeclaration.AddModifiers(
                this.modifiers
                    .Select(SyntaxFactory.Token).ToArray());

        if (this.members.Any())
            classDeclaration = classDeclaration.AddMembers(this.members.ToArray());

        return classDeclaration;
    }
}
