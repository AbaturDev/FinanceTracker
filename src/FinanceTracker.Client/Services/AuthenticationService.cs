using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using FinanceTracker.Client.Dtos.Account;
using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Interfaces;
using FinanceTracker.Client.Utils;
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

    public async Task<RequestResult> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/login", dto, ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to login: {response.StatusCode}");
            }

            var token = await response.Content.ReadFromJsonAsync<string>(cancellationToken: ct);

            if (token is null)
            {
                return new RequestResult(false, "Failed to login");
            }

            await _localStorageService.SetItemAsync("token", token, ct);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserAuthenticated(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return new RequestResult(true);
        }
        catch (Exception ex)
        {
            return new RequestResult(false, ex.Message);
        }
    }

    public async Task LogoutAsync(CancellationToken ct)
    {
        await _localStorageService.RemoveItemAsync("token", ct);
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<RequestResult> RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/register", dto, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await GetErrorMessageUtils.GetErrorMessage(response, ct) ?? "Failed to register account"; 
                
                return new RequestResult(false, errorMessage);
            }

            return new RequestResult(true);
        }
        catch (Exception ex)
        {
            return new RequestResult(false, ex.Message);
        }
    }
}