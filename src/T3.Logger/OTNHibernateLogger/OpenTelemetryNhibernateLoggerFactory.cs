namespace T3.Logger.OTNHibernateLogger;

using NHibernate = NHibernate;
using Microsoft.Extensions.Logging;

public class OpenTelemetryNhibernateLoggerFactory : NHibernate.INHibernateLoggerFactory
{
    private readonly ILoggerFactory loggerFactory;

    public OpenTelemetryNhibernateLoggerFactory(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
    }

    public NHibernate.INHibernateLogger LoggerFor(string keyName)
    {
        return new OpenTelemetryNHibernateLogger(
            this.loggerFactory.CreateLogger(keyName), 
            new EventId());
    }

    public NHibernate.INHibernateLogger LoggerFor(Type type)
    {
        return new OpenTelemetryNHibernateLogger(
            this.loggerFactory.CreateLogger(type.FullName!), 
            new EventId());
    }
}