namespace T3.Transactions.API.UseCases.CommitTransaction;

using FluentValidation;
using T3.API.Shared.Validators;

internal class TransactionCommitRequestValidator : AbstractValidator<TransactionCommitRequest>
{
    public TransactionCommitRequestValidator()
    {
        this.RuleFor(r => r.Id).SetValidator(new GuidValidator())
            .WithMessage("Incorrect Transaction Id");
    }
}