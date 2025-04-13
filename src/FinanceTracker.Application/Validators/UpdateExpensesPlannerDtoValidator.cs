using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Infrastructure.Context;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class UpdateExpensesPlannerDtoValidator : AbstractValidator<UpdateExpensesPlannerDto>
{
    public UpdateExpensesPlannerDtoValidator(FinanceTrackerDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty().
            MaximumLength(25)
            .Custom((value, context) =>
            {
                var nameInUse = dbContext.ExpensesPlanners.Any(e => e.Name == value);

                if (nameInUse)
                {
                    context.AddFailure("Name is already in use");
                }
            });
        
        RuleFor(x => x.Budget)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(25);
    }
}