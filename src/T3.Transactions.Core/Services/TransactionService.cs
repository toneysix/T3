namespace T3.Transactions.Core.Services;

using System;
using T3.Domain.Shared.Extensions;
using T3.Domain.Shared.Specs;
using T3.Transactions.Core.Entities;
using T3.Transactions.Core.Repositories;
using T3.Domain.Shared.Errors;
using DomainErrors = Errors.Errors;

public class TransactionService : ITransactionService
{
    private readonly ITransactionsRepository transactions;

    public TransactionService(ITransactionsRepository transactions)
    {
        this.transactions = transactions;
    }

    public Result Commit(Transaction transaction)
    {
        if (this.transactions.CountAll() >= 100)
            return DomainErrors.Transaction.MaxTransactionsExceeded;

        if (transaction.Date.IsDateTimeInFuture())
            return DomainErrors.Transaction.DateTimeIsInFuture;

        if (transaction.Amount <= 0)
            return DomainErrors.Transaction.AmountIsNotPositive;

        if (this.GetBy(transaction.Id).IsSuccess)
            return Errors.Entities.AlreadyExists<Transaction>();

        this.transactions.Add(transaction);

        return Result.Success();
    }

    public Result<Transaction> GetBy(Guid id)
    {
        var transactionById = new EntityByGuidSpec<Transaction> { Id = id };
        var transaction = this.transactions.Get(transactionById);

        if (transaction is null)
            return Errors.Entities.NotFound<Transaction>();

        return transaction;
    }

    public Task<Result> CommitAsync(Transaction transaction)
    {
        return TaskExtension.ToAsync(() => this.Commit(transaction));
    }

    public Task<Result<Transaction>> GetByAsync(Guid id)
    {
        return TaskExtension.ToAsync(() => this.GetBy(id));
    }
}