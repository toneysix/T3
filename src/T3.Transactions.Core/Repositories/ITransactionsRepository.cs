namespace T3.Transactions.Core.Repositories;

using T3.Domain.Shared.Repositories;
using T3.Transactions.Core.Entities;

/// <inheritdoc cref="IEntityRepository<Transaction>"/>
public interface ITransactionsRepository : IEntityRepository<Transaction>
{
}