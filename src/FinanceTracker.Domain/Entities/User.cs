using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public sealed record User : BaseEntity
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string CurrencyCode { get; set; } = "PLN";
}