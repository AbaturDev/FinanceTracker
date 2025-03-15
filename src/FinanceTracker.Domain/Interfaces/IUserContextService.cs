using System.Security.Claims;

namespace FinanceTracker.Domain.Interfaces;

public interface IUserContextService
{
    ClaimsPrincipal GetCurrentUser();
    int? GetCurrentUserId();
}