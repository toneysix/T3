namespace T3.Transactions.API.UseCases.CommitTransaction;

using Mapster;
using T3.API.Shared.Abstract;
using T3.Domain.Shared.Errors;
using T3.Domain.Shared.Specs;
using T3.Transactions.Core.Entities;
using T3.Transactions.Core.Services;

public class CommitTransactionUseCase : UseCase<TransactionCommitRequest, TransactionCommitResponse>
{
    private readonly ITransactionService transactionService;

    private readonly IAuditable<Transaction> auditableTransaction;

    private readonly IUnitOfWork unitOfWork;

    public CommitTransactionUseCase(
        ITransactionService transactionService, 
        IUnitOfWork unitOfWork,
        IAuditable<Transaction> auditableTransaction)
    {
        this.transactionService = transactionService;
        this.unitOfWork = unitOfWork;
        this.auditableTransaction = auditableTransaction;
    }

    public override TransactionCommitResponse Use(TransactionCommitRequest request)
    {
        var validationResult = new TransactionCommitRequestValidator()
            .Validate(request);

        if (!validationResult.IsValid)
        {
            return Response
                .FromValidationErrors<TransactionCommitResponse>(
                    validationResult.ToDictionary());
        }

        var transactionAudit = this.GetAuditDetailsBy(request.Id);

        if (transactionAudit is not null)
            return Response
                .FromResultNValue<TransactionCommitResponse, DateTime>(
                    Result.Success(), transactionAudit.CreatedDt);

        using var unitTransaction = this.unitOfWork.BeginTransaction();
        var transaction = request.Adapt<Transaction>();
        var result = this.transactionService.Commit(transaction);
        
        if (!result.IsSuccess)
            return Response.FromResult<TransactionCommitResponse>(result);

        this.unitOfWork.Commit();
        transactionAudit = this.GetAuditDetailsBy(request.Id);
        
        return Response
            .FromResultNValue<TransactionCommitResponse, DateTime>(
                result, transactionAudit.CreatedDt);
    }

    private AuditableEntity GetAuditDetailsBy(Guid id)
    {
        return this.auditableTransaction.Get(
            new EntityByGuidSpec<Transaction>() { Id = id });
    }
}
