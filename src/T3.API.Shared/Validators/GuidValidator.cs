namespace T3.API.Shared.Validators;

using FluentValidation;

public class GuidValidator : AbstractValidator<Guid>
{
    public GuidValidator()
    {
        this.RuleFor(x => x).NotEmpty();
    }
}
