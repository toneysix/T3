namespace T3.Logger.OTNHibernateLogger;

using NHibernate = NHibernate;
using Microsoft.Extensions.Logging;

public class OpenTelemetryNHibernateLogger : NHibernate.INHibernateLogger
{
    private readonly ILogger logger;

    private readonly EventId eventId;

    public OpenTelemetryNHibernateLogger(ILogger logger, EventId eventId)
    {
        this.logger = logger;
        this.eventId = eventId;
    }

    public bool IsEnabled(NHibernate.NHibernateLogLevel logLevel)
    {
        return this.logger.IsEnabled(this.MapNHibernateLvlToStandartLogLvl(logLevel));
    }

    public void Log(
        NHibernate.NHibernateLogLevel logLevel, 
        NHibernate.NHibernateLogValues state, 
        Exception exception)
    {
        this.logger.Log(
            this.MapNHibernateLvlToStandartLogLvl(logLevel),
            eventId,
            state,
            exception,
            (s, ex) => state.ToString());
    }

    private LogLevel MapNHibernateLvlToStandartLogLvl(NHibernate.NHibernateLogLevel logLevel)
    {
        return logLevel switch
        {
            NHibernate.NHibernateLogLevel.Debug => LogLevel.Debug,
            NHibernate.NHibernateLogLevel.Error => LogLevel.Error,
            NHibernate.NHibernateLogLevel.Fatal => LogLevel.Critical,
            NHibernate.NHibernateLogLevel.Info => LogLevel.Information,
            NHibernate.NHibernateLogLevel.Warn => LogLevel.Warning,
            _ => LogLevel.Trace
        };
    }
}


