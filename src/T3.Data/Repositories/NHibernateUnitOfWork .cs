namespace T3.Data.Repositories;

using NHibernate;
using System;
using T3.API.Shared.Abstract;
using T3.Domain.Shared.Extensions;

internal class NHibernateUnitOfWork : IUnitOfWork
{
    internal ISession Session { get; private set; }

    private readonly ISessionFactory sessionFactory;

    private ITransaction? transaction;

    public NHibernateUnitOfWork(ISessionFactory sessionFactory)
    {
        this.sessionFactory = sessionFactory;
        this.Session = this.sessionFactory.OpenSession();
    }

    public IDisposable BeginTransaction()
    {
        return this.transaction = this.Session.BeginTransaction();
    }

    public void Commit()
    {
        if (this.transaction is null)
            return;

        this.transaction.Commit();
    }

    public void Rollback()
    {
        if (this.transaction is null)
            return;

        this.transaction.Rollback();
    }

    public Task CommitAsync()
    {
        return TaskExtension.ToAsync(this.Commit);
    }

    public Task RollbackAsync()
    {
        return TaskExtension.ToAsync(this.Rollback);
    }

    public void Dispose()
    {
        if (this.transaction is not null)
        {
            this.transaction.Dispose();
            this.transaction = null;
        }

        if (this.Session is not null)
        {
            this.Session.Close();
            this.Session.Dispose();
            this.Session = null!;
        }
    }
}
