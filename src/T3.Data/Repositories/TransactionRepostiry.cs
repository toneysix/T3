namespace T3.Data.Repositories;

using T3.API.Shared.Abstract;
using T3.Data.Auditable;
using T3.Transactions.Core.Entities;
using T3.Transactions.Core.Repositories;

[AuditableRepository]
internal sealed class TransactionRepostiry(IUnitOfWork unitOfWork) :
    Repository<Transaction>(unitOfWork), ITransactionsRepository
{
}