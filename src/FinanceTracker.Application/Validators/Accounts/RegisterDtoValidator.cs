using FinanceTracker.Domain.Dtos.Account;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Accounts;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.VerifyPassword).Equal(x => x.Password);
        
        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3)
            .When(x => !string.IsNullOrWhiteSpace(x.CurrencyCode));
    }
}