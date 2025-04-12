using System.Net.Http.Json;
using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.Incomes;
using FinanceTracker.Client.Interfaces;
using FinanceTracker.Client.Utils;

namespace FinanceTracker.Client.Services;

public class IncomeService : IIncomeService
{
    private readonly HttpClient _httpClient;
    private readonly IAddBearerTokenService _addBearerTokenService;
    
    public IncomeService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
    {
        _httpClient = httpClient;
        _addBearerTokenService = addBearerTokenService;
    }
    
    public async Task<PaginatedResponse<IncomeDto>?> GetIncomesAsync(int pageNumber, int pageSize, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
            var url = $"/api/incomes?pageSize={pageSize}&pageNumber={pageNumber}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<IncomeDto>>(url, ct);

            if (response is null)
            {
                return null;
            }

            return response;
        }
        catch
        {
            return null;
        }
    }

    public async Task<RequestResult> CreateIncomeAsync(CreateIncomeDto dto, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        var response = await _httpClient.PostAsJsonAsync("/api/incomes", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to create income";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> UpdateIncomeAsync(UpdateIncomeDto dto, int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        var response = await _httpClient.PutAsJsonAsync($"/api/incomes/{id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to update income";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> DeleteIncomeAsync(int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        var response = await _httpClient.DeleteAsync($"/api/incomes/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to delete income";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);    }
}