namespace T3.Data.Extensions;

using Microsoft.Extensions.DependencyInjection;
using T3.API.Shared.Abstract;
using T3.Data.Auditable;
using T3.Domain.Shared.Repositories;

/// <summary>
/// Расширение над хранилищем.
/// </summary>
public static class RepositoryExtension
{
    /// <summary>
    /// Добавляет в коллекцию сервисов доступные хранилища.
    /// </summary>
    /// <param name="services">Коллекция сервисов, в которую необходимо добавить хранилища</param>
    /// <returns>Возвращает коллекцию сервисов</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var repositories = typeof(RepositoryExtension).Assembly.GetTypes()
             .Where(t => t.GetInterfaces()
                .Any(i => i.GetInterface(typeof(IEntityRepository<>).Name) is not null))
             .Select(t => new
             {
                 Implementation = t,
                 Interface = t.GetInterfaces()
                    .FirstOrDefault(i => i.GetInterface(typeof(IEntityRepository<>).Name) is not null)
             })
             .ToList();

        repositories.ForEach(
            rep => services.AddScoped(rep.Interface!, rep.Implementation));

        services.AddAuditRepositories();

        return services;
    }

    /// <summary>
    /// Добавляет в коллекцию сервисов доступные хранилища.
    /// </summary>
    /// <param name="services">Коллекция сервисов, в которую необходимо добавить хранилища</param>
    /// <returns>Возвращает коллекцию сервисов</returns>
    internal static IServiceCollection AddAuditRepositories(this IServiceCollection services)
    {
        var repositories = typeof(RepositoryExtension).Assembly.GetTypes()
             .Where(t => t.BaseType?.Name == typeof(AuditableRepositoryBase<>).Name)
             .Select(t => new
             {
                 Implementation = t,
                 Interface = t.BaseType!.GetInterface(typeof(IAuditable<>).Name)!
             })
             .ToList();

        repositories.ForEach(rep => services.AddScoped(rep.Interface, rep.Implementation));

        return services;
    }
}
