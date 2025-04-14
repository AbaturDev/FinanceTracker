using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FluentValidation;

namespace FinanceTracker.Application.Validators.ExpensesPlanners;

public class CreateExpensesPlannerDtoValidator : AbstractValidator<CreateExpensesPlannerDto>
{
    public CreateExpensesPlannerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.Budget)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(25)
            .When(x => !string.IsNullOrWhiteSpace(x.CategoryName));
    }
}