using FinanceTracker.Domain.Dtos.Owned;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Dtos.ExpensesPlanners;

public sealed record ExpensesPlannerDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public decimal Budget { get; set; }
    public decimal SpentAmount { get; set; }
    public ExchangeRateDto? OriginalExchangeRate { get; set; }
    public CategoryDto? Category { get; set; }
    public ResetInterval ResetInterval { get; set; }
    public required Guid UserId { get; set; }
}