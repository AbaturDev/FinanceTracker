namespace FinanceTracker.Client.Dtos.Account;

public sealed record LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}