namespace T3.Data.AuditableBuilder;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using T3.Data.AuditableBuilder.ClassBuilder;
using T3.Data.AuditableBuilder.Extensions;

[Generator]
public class AuditableRepositoryImplementationGenerator 
    : IIncrementalGenerator
{
    public void Initialize(
        IncrementalGeneratorInitializationContext context)
    {
            var compilationAndClasses = context
            .CompilationProvider
                .Combine(context.SyntaxProvider
                    .CreateSyntaxProvider(
                        predicate: static (syntaxNode, _) => 
                            IsClassWithAuditablehAttribute(syntaxNode) || 
                            IsClassAuditableBaseClass(syntaxNode),
                        transform: static (ctx, _) => ctx.Node as ClassDeclarationSyntax)
                    .Where(static m => m is not null)
                    .Collect());

        context.RegisterSourceOutput(
            compilationAndClasses,
            (spc, src) =>
            {
                foreach (var classDeclaration in src.Right)
                {
                    if (IsClassAuditableBaseClass(classDeclaration!))
                        continue;

                    this.Generate(spc, src.Left, classDeclaration!);
                }
            });
    }

    private static bool IsClassWithAuditablehAttribute(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclaration 
            && classDeclaration.AttributeLists
                .Any(l => l.Attributes
                    .Any(a => a.Name.ToString() == "AuditableRepository"));
    }

    private static bool IsClassAuditableBaseClass(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclaration
            && classDeclaration.Identifier.ToString() == "AuditableRepositoryBase";
    }

    public void Generate(
        SourceProductionContext context,
        Compilation compilation,
        ClassDeclarationSyntax repoClassDeclaration)
    {
        var semanticModel = compilation.GetSemanticModel(repoClassDeclaration.SyntaxTree);
        if (semanticModel.GetDeclaredSymbol(repoClassDeclaration) is not INamedTypeSymbol classSymbol)
            throw new Exception("Declared symbol ain't INamedTypeSymbol");

        if (classSymbol.BaseType?.IsGenericType is not true)
            throw new Exception("Repository base class should be Generic Type");

        var repoBaseClassType = repoClassDeclaration.BaseList!.Types
            .Select(t => t.Type)
            .OfType<GenericNameSyntax>()
            .First();

        var repoEntityType = repoBaseClassType.TypeArgumentList.Arguments[0];
        if (semanticModel.GetSymbolInfo(repoEntityType).Symbol is not INamedTypeSymbol repoEntityTypeSymbol)
            throw new Exception($"Repository Entity Type must be named symbol");

        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

        var baseClass = SyntaxTypeFactory.GetTypeSyntax(
            "T3.Data.Auditable.AuditableRepositoryBase",
            repoEntityType);

        var auditableRepoClassDeclaration = new ClassDeclarationBuilder(
            identifier: $"{repoEntityTypeSymbol.Name}AuditableRepsitory",
            compilation: compilation)
                .WithModifier(SyntaxKind.InternalKeyword, SyntaxKind.SealedKeyword)
                .WithBaseType(baseClass)
                    .WithGenericTypes(repoEntityType!)
                .WithConstructor()
                    .EmptyFromBase()
                .Build();

        var namespaceDeclaration = SyntaxFactory
            .NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName))
            .AddMembers(auditableRepoClassDeclaration);
        
        var compilationUnit = SyntaxFactory.CompilationUnit()
             .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("T3.API.Shared.Abstract")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(
                    repoEntityTypeSymbol
                        .ContainingNamespace
                            .ToDisplayString()))
              )
             .AddMembers(namespaceDeclaration)
             .NormalizeWhitespace();

        context
            .AddSource($"{auditableRepoClassDeclaration.Identifier.ValueText}.g.cs", 
            compilationUnit.GetText(Encoding.UTF8).ToString());
    }
}