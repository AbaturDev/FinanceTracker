using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using FinanceTracker.Client.Dtos.Account;
using FinanceTracker.Client.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceTracker.Client.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }
    
    public async Task<string> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/login", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to login: {response.StatusCode}");
        }
        
        var token = await response.Content.ReadFromJsonAsync<string>(cancellationToken: ct);

        if (token is null)
        {
            throw new HttpRequestException($"Failed to login: jwt token was null");
        }
        
        await _localStorageService.SetItemAsync("token", token, ct);
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserAuthenticated(token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return token;
    }

    public async Task LogoutAsync(CancellationToken ct)
    {
        await _localStorageService.RemoveItemAsync("token", ct);
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        await _httpClient.PostAsJsonAsync("/api/register", dto, ct);
    }
}