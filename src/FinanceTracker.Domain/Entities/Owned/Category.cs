namespace FinanceTracker.Domain.Entities.Owned;

public sealed record Category
{
    public required string Name { get; set; }
    public decimal Budget { get; set; }
}