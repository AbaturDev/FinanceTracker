namespace FinanceTracker.Client.Dtos.Account;

public sealed record RegisterDto(string Username, string Password, string ConfirmPassword, string Email, string? CurrencyCode);