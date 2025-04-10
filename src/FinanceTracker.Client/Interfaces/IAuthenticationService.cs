using FinanceTracker.Client.Dtos.Account;
using FinanceTracker.Client.Dtos.Common;

namespace FinanceTracker.Client.Interfaces;

public interface IAuthenticationService
{
    Task<RequestResult> LoginAsync(LoginDto dto, CancellationToken ct);
    Task LogoutAsync(CancellationToken ct);
    Task<RequestResult> RegisterAsync(RegisterDto dto, CancellationToken ct);
}