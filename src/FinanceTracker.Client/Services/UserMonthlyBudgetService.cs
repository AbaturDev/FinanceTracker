using System.Net.Http.Json;
using FinanceTracker.Client.Dtos.Budgets;
using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Interfaces;

namespace FinanceTracker.Client.Services;

public class UserMonthlyBudgetService : IUserMonthlyBudgetService
{
    private readonly HttpClient _httpClient;
    private readonly IAddBearerTokenService _addBearerTokenService;
    
    public UserMonthlyBudgetService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
    {
        _httpClient = httpClient;
        _addBearerTokenService = addBearerTokenService;
    }
    
    public async Task<UserMonthlyBudgetDto?> GetUserCurrentMonthlyBudgetAsync(CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var response = await _httpClient.GetFromJsonAsync<UserMonthlyBudgetDto>("/api/user-monthly-budgets/current-budget", ct);

            if (response is null)
            {
                return null;
            }
        
            return response;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<PaginatedResponse<UserMonthlyBudgetDto>?> GetUserMonthlyBudgetsHistoryAsync(int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var url = $"/api/user-monthly-budgets/budget-history?pageSize={pageSize}&pageNumber={pageNumber}";
            var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<UserMonthlyBudgetDto>>(url, ct);

            if (response is null)
            {
                return null;
            }
        
            return response;
        }
        catch (Exception)
        {
            return null;
        }
    }
}