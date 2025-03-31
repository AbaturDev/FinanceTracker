using System.Security.Claims;

namespace FinanceTracker.Domain.Interfaces;

public interface IUserContextService
{
    ClaimsPrincipal? GetCurrentUser();
    Guid? GetCurrentUserId();
    string? GetCurrentUserCurrencyCode();
}