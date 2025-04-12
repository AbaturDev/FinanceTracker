namespace FinanceTracker.Client.Dtos.Incomes;

public sealed record CreateIncomeDto
{
    public string? Name { get; set; }
    public decimal? Amount { get; set; }
    public bool? IsActive { get; set; }
    public string? CurrencyCode { get; set; }
}