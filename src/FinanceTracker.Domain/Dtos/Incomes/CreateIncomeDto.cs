namespace FinanceTracker.Domain.Dtos.Incomes;

public sealed record CreateIncomeDto
{
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public bool RegularIncome { get; set; }
    public bool IsActiveThisMonth { get; set; }
    public string? CurrencyCode { get; set; }
}