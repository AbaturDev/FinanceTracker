namespace FinanceTracker.Domain.Dtos.Account;

public sealed record RegisterDto
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VerifyPassword { get; init; }
    public string? CurrencyCode { get; set; }
}