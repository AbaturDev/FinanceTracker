using FinanceTracker.Domain.Dtos.Transactions;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0);
    }
}