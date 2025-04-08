namespace FinanceTracker.Domain.Dtos.Account;

public class LoginDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}