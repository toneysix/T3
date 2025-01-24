namespace T3.Data.Extensions;

using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Extensions.Sqlite;
using System.Reflection;
using T3.API.Shared.Abstract;
using T3.Data.Repositories;
using T3.Data.Interceptors;

#if DOCKER_BUILD
using NHibernate.Caches.StackExchangeRedis;
using NHibernate.Caches.Util.JsonSerializer;
#endif

/// <summary>
/// Класс-расширение над NHibernate ORM.
/// </summary>
public static class NHibernateExtensions
{
    /// <summary>
    /// Добавляет в коллекцию сервисов Sqlite БД.
    /// </summary>
    /// <param name="services">Коллекция сервисов, в которую необходимо добавить хранилища</param>
    /// <param name="connectionString">Строка подключения к Sqlite БД</param>
    /// <returns>Возвращает коллекцию сервисов</returns>
    public static IServiceCollection AddSqlite(this IServiceCollection services, string connectionString)
    {
        services.AddNHibernate<SqliteDriver, SqliteDialect>(connectionString, (cfg) =>
        {
#if DOCKER_BUILD
            Console.WriteLine("REDIS WILL BE USED");
            cfg.AddRedis();
#endif
        });

        return services;
    }

    /// <summary>
    /// Добавляет NHibernate ORM с БД <typeparamref name="TDriver"/>, диалектом <typeparamref name="TDialect"/> в коллекцию сервисов.
    /// </summary>
    /// <typeparam name="TDriver">Драйвер БД</typeparam>
    /// <typeparam name="TDialect">Диалект БД</typeparam>
    /// <param name="services">Коллекция сервисов, в которую необходимо добавить NHibernate ORM с БД <typeparamref name="TDriver"/></param>
    /// <param name="connectionString">Строка подключения к <typeparamref name="TDriver"/> БД</param>
    /// <param name="mappings"></param>
    /// <returns>Возвращает коллекцию сервисов</returns>
    private static IServiceCollection AddNHibernate<TDriver, TDialect>(
        this IServiceCollection services,
        string connectionString,
        Action<Configuration> conf)
            where TDriver : IDriver
            where TDialect : Dialect
    {
        var configuration = new Configuration();
        configuration.DataBaseIntegration(db =>
        {
            db.ConnectionString = connectionString;
            db.Driver<TDriver>();
            db.Dialect<TDialect>();
        });

        configuration.AddAssembly(Assembly.GetExecutingAssembly());
        configuration.SetInterceptor(new AuditableEntityInterceptor());

        var schema = new SchemaExport(configuration);
        schema.Execute(true, true, false);

        var update = new SchemaUpdate(configuration);
        update.Execute(true, true);

        conf(configuration);

        var sessionFactory = configuration.BuildSessionFactory();
        services.AddSingleton(sessionFactory);
        services.AddScoped<IUnitOfWork, NHibernateUnitOfWork>();

        return services;
    }

#if DOCKER_BUILD
    private static Configuration AddRedis(this Configuration configuration)
    {
        var jsonCacheSerializerAssemblyQualifiedNam = typeof(JsonCacheSerializer).AssemblyQualifiedName;
        configuration.Cache(c =>
        {
            var port = System.Environment.GetEnvironmentVariable("REDIS_PORT");
            var password = System.Environment.GetEnvironmentVariable("REDIS_PASSWORD");

            c.UseQueryCache = true;

            configuration.Properties.Add("cache.default_expiration", "900");
            configuration.Properties.Add("cache.use_sliding_expiration", "true");
            configuration.Properties.Add("cache.serializer", jsonCacheSerializerAssemblyQualifiedNam);
            configuration.Properties.Add($"cache.configuration",
                $"cache:{port}," +
                $"password={password}," +
                $"allowAdmin=true," +
                $"abortConnect=false");
            c.Provider<RedisCacheProvider>();
        });

        return configuration;
    }
#endif
}
