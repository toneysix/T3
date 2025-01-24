namespace T3.Data.AuditableBuilder.ClassBuilder;

using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class BaseTypeBuilder
{
    private readonly ClassDeclarationBuilder builder;

    internal BaseTypeBuilder(ClassDeclarationBuilder builder)
    {
        this.builder = builder;
    }

    internal ClassDeclarationBuilder WithGenericTypes(params TypeSyntax[] types)
    {
        if (this.builder.BaseType is not GenericNameSyntax genericType)
            throw new Exception();
        
        genericType.TypeArgumentList.Arguments.AddRange(types);

        return this.builder;
    }

    internal ClassDeclarationBuilder Done()
    {
        return this.builder;
    }
}
