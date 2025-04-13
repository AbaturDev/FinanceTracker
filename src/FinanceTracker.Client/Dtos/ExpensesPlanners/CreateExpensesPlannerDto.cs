namespace FinanceTracker.Client.Dtos.ExpensesPlanners;

public sealed record CreateExpensesPlannerDto
{
    public string? Name { get; set; }
    public decimal? Budget { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CategoryName { get; set; }
}