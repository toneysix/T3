namespace T3.Transactions.API.UseCases.CommitTransaction;

using T3.API.Shared.Abstract;

public record TransactionCommitResponse : Response
{
    public DateTime InsertDateTime { get; init; }
}