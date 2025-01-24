namespace T3.Web.Endpoints.v1;

using Microsoft.AspNetCore.Mvc;
using T3.API.Shared.Abstract;
using T3.Transactions.API.UseCases.CommitTransaction;
using T3.Transactions.API.UseCases.GetTransaction;
using T3.Web.Filters;

internal static class Transactions
{
    internal static RouteGroupBuilder AddTransactionEndpoints(this RouteGroupBuilder group)
    {
        var transactionsGroup = group.MapGroup("/transaction");
        transactionsGroup.AddEndpointFilter<ProblemDetailsResultFilter>();

        transactionsGroup.MapGet(
            "/{id:guid}",
            (Guid id, [FromServices]IUseCase<TransactionGetRequest, TransactionGetResponse> useCase)
                => useCase.UseAsync(new(id)));

        transactionsGroup.MapPost("/", ([FromBody]TransactionCommitRequest request, [FromServices]IUseCase<TransactionCommitRequest, TransactionCommitResponse> useCase) => useCase.UseAsync(request));

        return group;
    }
}
