using FinanceTracker.Client.Dtos.Account;

namespace FinanceTracker.Client.Interfaces;

public interface IAuthenticationService
{
    Task<string> LoginAsync(LoginDto dto, CancellationToken ct);
    Task LogoutAsync(CancellationToken ct);
    Task RegisterAsync(RegisterDto dto, CancellationToken ct);
}