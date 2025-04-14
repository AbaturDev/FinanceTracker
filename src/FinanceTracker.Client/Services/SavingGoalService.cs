using System.Net.Http.Json;
using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.SavingGoals;
using FinanceTracker.Client.Dtos.Transactions;
using FinanceTracker.Client.Interfaces;
using FinanceTracker.Client.Utils;

namespace FinanceTracker.Client.Services;

public class SavingGoalService : ISavingGoalService
{
    private readonly HttpClient _httpClient;
    private readonly IAddBearerTokenService _addBearerTokenService;
    
    public SavingGoalService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
    {
        _httpClient = httpClient;
        _addBearerTokenService = addBearerTokenService;
    }
    
    public async Task<PaginatedResponse<SavingGoalDto>?> GetSavingGoalsAsync(int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var url = $"/api/saving-goals?pageSize={pageSize}&pageNumber={pageNumber}";
            var result = await _httpClient.GetFromJsonAsync<PaginatedResponse<SavingGoalDto>>(url, ct);

            if (result is null)
            {
                return null;
            }
            
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<PaginatedResponse<TransactionDto>?> GetSavingGoalTransactionsAsync(int id, int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var url = $"/api/saving-goals/{id}/transactions?pageSize={pageSize}&pageNumber={pageNumber}";
            var result = await _httpClient.GetFromJsonAsync<PaginatedResponse<TransactionDto>>(url, ct);

            if (result is null)
            {
                return null;
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }    
    }

    public async Task<RequestResult> CreateSavingGoalAsync(CreateSavingGoalDto dto, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

        var response = await _httpClient.PostAsJsonAsync("/api/saving-goals", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to create saving goal";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> UpdateSavingGoalAsync(UpdateSavingGoalDto dto, int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var response = await _httpClient.PutAsJsonAsync($"/api/saving-goals/{id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to update saving goal";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> DeleteSavingGoalAsync(int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var response = await _httpClient.DeleteAsync($"/api/saving-goals/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to delete saving goal";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> AddTransactionAsync(CreateTransactionDto dto, int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var url = $"/api/saving-goals/{id}/transactions";
        var response = await _httpClient.PostAsJsonAsync(url, dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to add transaction";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }
}