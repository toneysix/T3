namespace T3.Transactions.API;

using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using T3.API.Shared.Abstract;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services, Assembly fromAssembly)
    {
        var useCases = fromAssembly.GetTypes()
             .Where(t => t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(UseCase<,>))
             .Select(t => new
             {
                 Implementation = t,
                 Interface = t.BaseType!.GetInterface(typeof(IUseCase<,>).Name)
             })
             .ToList();

        useCases.ForEach(
            useCase => services.AddScoped(useCase.Interface!, useCase.Implementation));

        TypeAdapterConfig.GlobalSettings.Scan(fromAssembly);
        return services;
    }
}
