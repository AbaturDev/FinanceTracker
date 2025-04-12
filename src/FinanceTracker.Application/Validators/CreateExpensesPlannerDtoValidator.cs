using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Infrastructure.Context;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateExpensesPlannerDtoValidator : AbstractValidator<CreateExpensesPlannerDto>
{
    public CreateExpensesPlannerDtoValidator(FinanceTrackerDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25)
            .Custom((value, context) =>
            {
                var nameInUse = dbContext.ExpensesPlanners.Any(x => x.Name == value);

                if (nameInUse)
                {
                    context.AddFailure("ExpensesPlanner already exists");
                }
            });
        
        RuleFor(x => x.Budget)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(25);
    }
}