using FinanceTracker.Application.Validators;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Incomes;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Dtos.Account;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace FinanceTracker.API.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<CreateExpensesPlannerDto>, CreateExpensesPlannerDtoValidator>();
        services.AddScoped<IValidator<CreateIncomeDto>, CreateIncomeDtoValidator>();
        services.AddScoped<IValidator<CreateSavingGoalDto>, CreateSavingGoalDtoValidator>();
        services.AddScoped<IValidator<CreateTransactionDto>, CreateTransactionDtoValidator>();
        services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
        services.AddScoped<IValidator<UpdateExpensesPlannerDto>, UpdateExpensesPlannerDtoValidator>();
        services.AddScoped<IValidator<UpdateIncomeDto>, UpdateIncomeDtoValidator>();
        services.AddScoped<IValidator<UpdateSavingGoalDto>, UpdateSavingGoalDtoValidator>();
        
        return services;
    }
}