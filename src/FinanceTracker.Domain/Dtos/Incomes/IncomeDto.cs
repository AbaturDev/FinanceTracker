using FinanceTracker.Domain.Dtos.Owned;

namespace FinanceTracker.Domain.Dtos.Incomes;

public sealed record IncomeDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public bool RegularIncome { get; set; }
    public bool IsActiveThisMonth { get; set; }
    public string? CurrencyCode { get; set; }
    public required Guid UserId { get; set; }
}