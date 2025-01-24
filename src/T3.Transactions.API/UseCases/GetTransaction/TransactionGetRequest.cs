namespace T3.Transactions.API.UseCases.GetTransaction;

using T3.API.Shared.Abstract;

public record TransactionGetRequest(Guid Id) : Request
{
}
