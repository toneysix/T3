namespace T3.Logger.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#if DOCKER_BUILD
using NHibernate;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using T3.Logger.OTNHibernateLogger;
#endif

public static class LoggerExtension
{
    /// <summary>
    /// Добавляет логгер в приложение в зависимости от сборки.
    /// </summary>
    /// <param name="services">Коллекция сервисов, в которую нужно добавить логгер</param>
    /// <returns>Возвращает коллекцию сервисов</returns>
    public static IServiceCollection AddCustomLogger(this IServiceCollection services, LogLevel logLevel)
    {
#if DOCKER_BUILD
        return services.AddSeqLogger(logLevel);
#else
        return services.AddSimpleLogger();
#endif
    }

#if DOCKER_BUILD
    private static IServiceCollection AddSeqLogger(this IServiceCollection services, LogLevel logLevel)
    {
        var seqOtlpPort = Environment.GetEnvironmentVariable("SEQ_API_INGESTIONPORT");
        var otlpExporter = (OtlpExporterOptions exporter, string kind) =>
        {
            exporter.Endpoint = new Uri(
                $"http://logs:{seqOtlpPort}/ingest/otlp/v1/{kind}");
            exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
        };

        services.AddOpenTelemetry()
          .ConfigureResource(r => r.AddService("T3"))
          .WithTracing(tracing =>
          {
              tracing.AddAspNetCoreInstrumentation();
              tracing.AddOtlpExporter((exporter) => otlpExporter(exporter, "traces"));
          })
          .WithMetrics(metrics =>
          {
              metrics.AddAspNetCoreInstrumentation();
              metrics.AddOtlpExporter((exporter) => otlpExporter(exporter, "metrics"));
          });

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(c =>
            {
                c.IncludeScopes = true;
                c.IncludeFormattedMessage = true;

                c.AddOtlpExporter((exporter) => otlpExporter(exporter, "logs"));
            });

            builder.SetMinimumLevel(logLevel);
        });

        services.AddSingleton(loggerFactory);

        NHibernateLogger.SetLoggersFactory(new OpenTelemetryNhibernateLoggerFactory(loggerFactory));
        return services;
    }
#else
    private static IServiceCollection AddSimpleLogger(this IServiceCollection services)
    {
        // TODO

        return services;
    }
#endif
}
