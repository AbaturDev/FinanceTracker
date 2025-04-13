using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Infrastructure.Context;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateSavingGoalDtoValidator : AbstractValidator<CreateSavingGoalDto>
{
    public CreateSavingGoalDtoValidator(FinanceTrackerDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25)
            .Custom((value, context) =>
            {
                var nameInUse = dbContext.SavingGoals.Any(x => x.Name == value);

                if (nameInUse)
                {
                    context.AddFailure("Saving goal name already exists");
                }
            });
        
        RuleFor(x => x.AmountOfMoney)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Goal)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3);
    }
}