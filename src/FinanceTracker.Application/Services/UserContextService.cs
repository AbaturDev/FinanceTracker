using System.Security.Claims;
using FinanceTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceTracker.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            return null;
        }
        
        return new ClaimsPrincipal(user);
    }

    public Guid? GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return null;
        }
        
        return Guid.Parse(userId);
    }

    public string? GetCurrentUserCurrencyCode()
    {
        var userCurrencyCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("CurrencyCode");

        return userCurrencyCode;
    }
}