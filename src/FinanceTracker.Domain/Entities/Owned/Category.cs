using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Domain.Entities.Owned;

[Owned]
public sealed record Category
{
    public string? Name { get; set; }
}