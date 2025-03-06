using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public sealed record Transaction : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? CurrencyCode { get; set; }
    public ExchangeRate? ExchangeRate { get; set; }
    public TransactionSource TransactionSource { get; set; }
    public required int UserId { get; set; }

    public required User User { get; set; }
}