namespace T3.API.Shared.Abstract;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();

    Task RollbackAsync();

    void Commit();

    void Rollback();

    IDisposable BeginTransaction();
}
