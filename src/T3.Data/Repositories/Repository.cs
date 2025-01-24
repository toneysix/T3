namespace T3.Data.Repositories;

using NHibernate;
using NHibernate.Linq;
using T3.API.Shared.Abstract;
using T3.Domain.Shared.Entities;
using T3.Domain.Shared.Repositories;
using T3.Domain.Shared.Specs;

internal abstract class Repository<TEntity> : IEntityRepository<TEntity> 
    where TEntity : Entity
{
    protected readonly ISession session;

    internal Repository(IUnitOfWork unitOfWork)
    {
        this.session = ((NHibernateUnitOfWork)unitOfWork).Session;
    }

    public void Add(TEntity entity)
    {
        this.session.Save(entity);
    }

    public TEntity? Get(Specification<TEntity> spec)
    {
        return this.session.Query<TEntity>().FirstOrDefault(spec);
    }

    public long Count(Specification<TEntity> spec)
    {
        return this.session.QueryOver<TEntity>().Where(spec).RowCountInt64();
    }

    public long CountAll()
    {
        return this.session.QueryOver<TEntity>().RowCountInt64();
    }

    public Task AddAsync(TEntity entity)
    {
        return this.session.SaveAsync(entity);
    }

    public Task<TEntity> GetAsync(Specification<TEntity> spec)
    {
        return this.session.Query<TEntity>().FirstOrDefaultAsync(spec);
    }

    public Task<long> CountAsync(Specification<TEntity> spec)
    {
        return this.session.QueryOver<TEntity>().Where(spec).RowCountInt64Async();
    }

    public Task<long> CountAllAsync()
    {
        return this.session.QueryOver<TEntity>().RowCountInt64Async();
    }
}
