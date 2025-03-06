namespace FinanceTracker.Domain.Common;

public abstract record BaseEntity
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
