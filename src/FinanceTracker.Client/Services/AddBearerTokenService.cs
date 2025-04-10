using System.Net.Http.Headers;
using Blazored.LocalStorage;
using FinanceTracker.Client.Interfaces;

namespace FinanceTracker.Client.Services;

public class AddBearerTokenService : IAddBearerTokenService
{
    private readonly ILocalStorageService _localStorageService;
    public AddBearerTokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    public async Task AddBearerTokenAsync(HttpClient httpClient, CancellationToken ct)
    {
        if(await _localStorageService.ContainKeyAsync("token", ct))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsync<string>("token", ct));
        }
    }
}