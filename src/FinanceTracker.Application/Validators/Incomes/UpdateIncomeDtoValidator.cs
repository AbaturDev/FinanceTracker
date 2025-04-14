using FinanceTracker.Domain.Dtos.Incomes;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Incomes;

public class UpdateIncomeDtoValidator : AbstractValidator<UpdateIncomeDto>
{
    public UpdateIncomeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
        
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .When(x => x.Amount != null);
    }
}