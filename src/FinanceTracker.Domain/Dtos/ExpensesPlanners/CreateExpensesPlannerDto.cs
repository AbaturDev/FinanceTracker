using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Dtos.ExpensesPlanners;

public sealed record CreateExpensesPlannerDto
{
    public required string Name { get; set; }
    public decimal Budget { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CategoryName { get; set; }
    public ResetInterval ResetInterval { get; set; }
}