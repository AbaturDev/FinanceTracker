using FinanceTracker.Domain.Dtos.Account;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IAccountService
{
    Task<Result<string>> LoginAsync(LoginDto loginDto, CancellationToken ct);
    Task<Result> RegisterAsync(RegisterDto registerDto, CancellationToken ct);
    // Task<Result<string>> RefreshTokenAsync(CancellationToken ct);
}