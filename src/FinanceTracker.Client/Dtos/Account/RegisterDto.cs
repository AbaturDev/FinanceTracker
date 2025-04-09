namespace FinanceTracker.Client.Dtos.Account;

public sealed record RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string VerifyPassword { get; set; }
    public string CurrencyCode { get; set; }
};