using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Dtos.ExpensesPlanners;

public class UpdateExpensesPlannerDto
{
    public string? Name { get; set; }
    public decimal? Budget { get; set; }
    public string? CategoryName { get; set; }
    public ResetInterval? ResetInterval { get; set; }
}