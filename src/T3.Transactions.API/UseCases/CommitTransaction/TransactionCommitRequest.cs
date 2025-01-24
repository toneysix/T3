namespace T3.Transactions.API.UseCases.CommitTransaction;

using T3.API.Shared.Abstract;

public record TransactionCommitRequest : Request
{
    public required Guid Id { get; init; }

    public required DateTime Date { get; init; }

    public required decimal Amount { get; init; }
}