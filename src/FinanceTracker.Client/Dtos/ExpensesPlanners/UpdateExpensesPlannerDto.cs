namespace FinanceTracker.Client.Dtos.ExpensesPlanners;

public sealed record UpdateExpensesPlannerDto
{
    public string? Name { get; set; }
    public decimal? Budget { get; set; }
    public string? CategoryName { get; set; }
}