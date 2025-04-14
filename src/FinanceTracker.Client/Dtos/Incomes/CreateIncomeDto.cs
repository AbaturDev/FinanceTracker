namespace FinanceTracker.Client.Dtos.Incomes;

public sealed record CreateIncomeDto
{
    public string? Name { get; set; }
    public decimal? Amount { get; set; }
    public bool IsActive { get; set; } = false;
    public string? CurrencyCode { get; set; }
}