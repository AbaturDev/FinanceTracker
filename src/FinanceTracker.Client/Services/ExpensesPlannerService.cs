using System.Net.Http.Json;
using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.ExpensesPlanners;
using FinanceTracker.Client.Dtos.Transactions;
using FinanceTracker.Client.Interfaces;
using FinanceTracker.Client.Utils;

namespace FinanceTracker.Client.Services;

public class ExpensesPlannerService : IExpensesPlannerService
{
    private readonly HttpClient _httpClient;
    private readonly IAddBearerTokenService _addBearerTokenService;
    
    public ExpensesPlannerService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
    {
        _httpClient = httpClient;
        _addBearerTokenService = addBearerTokenService;
    }
    
    public async Task<PaginatedResponse<ExpensesPlannerDto>?> GetExpensesPlannersAsync(int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var url = $"/api/expenses-planners?pageSize={pageSize}&pageNumber={pageNumber}";
            var result = await _httpClient.GetFromJsonAsync<PaginatedResponse<ExpensesPlannerDto>>(url, ct);

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

    public async Task<PaginatedResponse<TransactionDto>?> GetExpensesPlannerTransactionsAsync(int id, int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

            var url = $"/api/expenses-planners/{id}/transactions?pageSize={pageSize}&pageNumber={pageNumber}";
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

    public async Task<RequestResult> CreateExpensesPlannerAsync(CreateExpensesPlannerDto dto, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);

        var response = await _httpClient.PostAsJsonAsync("/api/expenses-planners", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to create expenses planner";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> UpdateExpensesPlannerAsync(UpdateExpensesPlannerDto dto, int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var response = await _httpClient.PutAsJsonAsync($"/api/expenses-planners/{id}", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to update expenses planner";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> DeleteExpensesPlannerAsync(int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var response = await _httpClient.DeleteAsync($"/api/expenses-planners/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to delete expenses planner";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }

    public async Task<RequestResult> AddTransactionAsync(CreateTransactionDto dto, int id, CancellationToken ct)
    {
        await _addBearerTokenService.AddBearerTokenAsync(_httpClient, ct);
        
        var url = $"/api/expenses-planners/{id}/transactions";
        var response = await _httpClient.PostAsJsonAsync(url, dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to add transaction";
            
            return new RequestResult(false, errorMessage);
        }
        
        return new RequestResult(true);
    }
}