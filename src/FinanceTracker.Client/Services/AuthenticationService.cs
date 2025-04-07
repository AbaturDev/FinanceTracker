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
        var response = await _httpClient.PostAsJsonAsync("/api/login", new LoginDto("domi", "Haslo123!"), ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to login: {response.StatusCode}");
        }
        
        var token = await response.Content.ReadFromJsonAsync<string>(cancellationToken: ct);
        
        await _localStorageService.SetItemAsync("token", token, ct);
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserAuthenticated(token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Console.WriteLine(token);
        
        return token;
    }

    public Task LogoutAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}