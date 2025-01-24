namespace T3.Transactions.API.UseCases.GetTransaction;

using T3.API.Shared.Abstract;
using T3.Transactions.Core.Entities;
using T3.Transactions.Core.Services;
using T3.Transactions.API.UseCases.CommitTransaction;

public class GetTransactionUseCase : UseCase<TransactionGetRequest, TransactionGetResponse>
{
    private readonly ITransactionService transactionService;

    public GetTransactionUseCase(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    public override TransactionGetResponse Use(TransactionGetRequest request)
    {
        var validationResult = new TransactionGetRequestValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return Response
                .FromValidationErrors<TransactionGetResponse>(
                    validationResult.ToDictionary());
        }

        var result = this.transactionService.GetBy(request.Id);

        return Response
            .FromResult<TransactionGetResponse, Transaction>(result);
    }
}
