using FinanceTracker.Client.Dtos.Common;

namespace FinanceTracker.Client.Dtos.Transactions;

public sealed record TransactionDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal CalculatedAmount { get; set; }
    public ExchangeRateDto? BudgetExchangeRate { get; set; }
    public ExchangeRateDto? TargetExchangeRate { get; set; }
    public required Guid UserId { get; set; }
}