namespace T3.Data.Auditable;

using NHibernate.Criterion;
using NHibernate.Transform;
using T3.API.Shared.Abstract;
using T3.Data.Repositories;
using T3.Domain.Shared.Entities;
using T3.Domain.Shared.Specs;

internal abstract class AuditableRepositoryBase<TEntity> : IAuditable<TEntity>
    where TEntity : Entity
{
    protected readonly NHibernateUnitOfWork nHibernateUnitOfWork;

    public AuditableRepositoryBase(IUnitOfWork nHibernateUnitOfWork)
    {
        this.nHibernateUnitOfWork = (NHibernateUnitOfWork)nHibernateUnitOfWork;
    }

    AuditableEntity IAuditable<TEntity>.Get(Specification<TEntity> specification)
    {
        var result = this.nHibernateUnitOfWork.Session.QueryOver<TEntity>()
            .Where(specification)
            .SelectList(list => list
                .Select(Projections.Property("CreatedDt").WithAlias(() => new AuditableEntity().CreatedDt)))
            .TransformUsing(Transformers.AliasToBean<AuditableEntity>())
            .SingleOrDefault<AuditableEntity>();

        return result;
    }
}