namespace T3.Transactions.API.UseCases.GetTransaction;

using T3.API.Shared.Abstract;

public record TransactionGetResponse : Response
{
    public Guid Id { get; init; }

    public DateTime Date { get; init; }

    public decimal Amount { get; init; }
}