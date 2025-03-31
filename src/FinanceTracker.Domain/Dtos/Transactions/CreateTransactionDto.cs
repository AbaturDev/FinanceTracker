using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Dtos.Transactions;

public record CreateTransactionDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string? CurrencyCode { get; set; }
    public TransactionSource TransactionSource { get; set; }
    public int? UserMonthlyBudgetId { get; set; }
}