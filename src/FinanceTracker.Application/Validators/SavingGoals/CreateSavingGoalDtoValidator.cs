using FinanceTracker.Domain.Dtos.SavingGoals;
using FluentValidation;

namespace FinanceTracker.Application.Validators.SavingGoals;

public class CreateSavingGoalDtoValidator : AbstractValidator<CreateSavingGoalDto>
{
    public CreateSavingGoalDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.AmountOfMoney)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Goal)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .When(x => x.DueDate != null);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .When(x => !string.IsNullOrWhiteSpace(x.CurrencyCode));
    }
}