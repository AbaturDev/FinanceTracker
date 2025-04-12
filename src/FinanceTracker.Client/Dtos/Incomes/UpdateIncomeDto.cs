namespace FinanceTracker.Client.Dtos.Incomes;

public sealed record UpdateIncomeDto
{
    public string? Name { get; set; }
    public decimal? Amount { get; set; }
    public bool? IsActive { get; set; }
}