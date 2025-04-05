using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    [MaxLength(10)]
    public string CurrencyCode { get; set; } = "PLN";
}