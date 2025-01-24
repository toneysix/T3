namespace T3.Transactions.API.UseCases.CommitTransaction;

using FluentValidation;
using T3.API.Shared.Validators;
using T3.Transactions.API.UseCases.GetTransaction;

internal class TransactionGetRequestValidator : AbstractValidator<TransactionGetRequest>
{
    public TransactionGetRequestValidator()
    {
        this.RuleFor(r => r.Id).SetValidator(new GuidValidator())
            .WithMessage("Incorrect Transaction Id");
    }
}