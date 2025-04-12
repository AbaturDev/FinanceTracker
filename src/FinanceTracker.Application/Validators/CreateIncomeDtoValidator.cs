using FinanceTracker.Domain.Dtos.Incomes;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateIncomeDtoValidator : AbstractValidator<CreateIncomeDto>
{
    public CreateIncomeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.IsActive)
            .NotEmpty();
        
        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(10);
    }
}