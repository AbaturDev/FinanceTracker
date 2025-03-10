using FinanceTracker.Domain.Dtos.Owned;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Dtos.Transactions;

public sealed record TransactionDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public ExchangeRateDto? ExchangeRate { get; set; }
    public TransactionSource TransactionSource { get; set; }
    public required int UserId { get; set; }
    public int? UserMonthlyBudgetId { get; set; }
    public int? SavingGoalId { get; set; }
    public int? ExpensesPlannerId { get; set; }
}