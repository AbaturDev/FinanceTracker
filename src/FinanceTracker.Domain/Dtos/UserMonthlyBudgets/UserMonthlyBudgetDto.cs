namespace FinanceTracker.Domain.Dtos.UserMonthlyBudgets;

public sealed record UserMonthlyBudgetDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal TotalExpenses { get; set; }
    public required string CurrencyCode { get; set; }
    public Guid UserId { get; set; }
}