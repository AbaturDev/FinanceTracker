using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FluentValidation;

namespace FinanceTracker.Application.Validators.ExpensesPlanners;

public class UpdateExpensesPlannerDtoValidator : AbstractValidator<UpdateExpensesPlannerDto>
{
    public UpdateExpensesPlannerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
        
        RuleFor(x => x.Budget)
            .NotEmpty()
            .GreaterThan(0)
            .When(x => x.Budget != null);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(25)
            .When(x => !string.IsNullOrWhiteSpace(x.CategoryName));
    }
}