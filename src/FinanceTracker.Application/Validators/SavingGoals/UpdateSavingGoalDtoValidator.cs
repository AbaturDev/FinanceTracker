using FinanceTracker.Domain.Dtos.SavingGoals;
using FluentValidation;

namespace FinanceTracker.Application.Validators.SavingGoals;

public class UpdateSavingGoalDtoValidator : AbstractValidator<UpdateSavingGoalDto>
{
    public UpdateSavingGoalDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
        
        RuleFor(x => x.AmountOfMoney)
            .NotEmpty()
            .GreaterThan(0)
            .When(x => x.AmountOfMoney != null);

        RuleFor(x => x.Goal)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Goal));

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .When(x => x.DueDate != null);
    }
}