namespace T3.Data.Interceptors;

using System;
using NHibernate;
using T3.Domain.Shared.Entities;

public class AuditableEntityInterceptor : EmptyInterceptor
{
    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
    {
        if (entity is not Entity)
            return false;

        var prop = propertyNames
            .Select((p, index) => new
            {
                Index = index,
                Property = p
            })
            .FirstOrDefault(a => a.Property == "CreatedDt");

        if (prop is null)
            return false;

        var creationDate = (DateTimeOffset?)state[prop.Index];
        if (creationDate is not null)
            return false;

        state[prop.Index] = DateTime.Now;
        return true;
    }
}
