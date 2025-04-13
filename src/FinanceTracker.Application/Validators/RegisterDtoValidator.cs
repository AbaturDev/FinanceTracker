using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Infrastructure.Context;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator(FinanceTrackerDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(e => e.Email == value);

                if (emailInUse)
                {
                    context.AddFailure("Email", "Email is already in use");
                }
            });
        
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(25)
            .Custom((value, context) =>
            {
                var usernameInUse = dbContext.Users.Any(e => e.UserName == value);

                if (usernameInUse)
                {
                    context.AddFailure("Username", "Username is already in use");
                }
            });
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.VerifyPassword)
            .NotEmpty()
            .MaximumLength(25);
        
        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(3);
    }
}