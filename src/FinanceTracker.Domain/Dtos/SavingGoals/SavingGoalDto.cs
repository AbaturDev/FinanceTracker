using FinanceTracker.Domain.Dtos.Owned;

namespace FinanceTracker.Domain.Dtos.SavingGoals;

public sealed record SavingGoalDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AmountOfMoney { get; set; }
    public required string Goal { get; set; }
    public DateOnly? DueDate { get; set; }
    public required string CurrencyCode { get; set; }
    public required Guid UserId { get; set; }
}