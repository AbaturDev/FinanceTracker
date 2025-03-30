namespace FinanceTracker.Domain.Dtos.Incomes;

public sealed record UpdateIncomeDto
{
    public string? Name { get; set; }
    public decimal? Amount { get; set; }
    public bool? RegularIncome { get; set; }
}