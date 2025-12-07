using DomainAskFor.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;

namespace DomainAskFor.Client.Services
{
  public interface IAuthService
  {
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegisterRequest request);
    Task Logout();
    Task<bool> IsAuthenticated();
    Task<string> GetUserEmail();
  }

  public class AuthService : IAuthService
  {
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(
      HttpClient httpClient,
      ILocalStorageService localStorage,
      AuthenticationStateProvider authStateProvider)
    {
      _httpClient = httpClient;
      _localStorage = localStorage;
      _authStateProvider = authStateProvider;
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
      try
      {
        var response = await _httpClient.PostAsJsonAsync("api/v1/Auth/login", request);
        if (response.IsSuccessStatusCode)
        {
          var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
          if (authResponse.Success)
          {
            await _localStorage.SetItemAsync("authToken", authResponse.Token);
            await _localStorage.SetItemAsync("userEmail", authResponse.Email);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.Token);
          }
          return authResponse;
        }
        return new AuthResponse { Success = false, ErrorMessage = "Login failed" };
      }
      catch (Exception ex)
      {
        return new AuthResponse { Success = false, ErrorMessage = ex.Message };
      }
    }

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
      try
      {
        var response = await _httpClient.PostAsJsonAsync("api/v1/Auth/register", request);
        if (response.IsSuccessStatusCode)
        {
          var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
          if (authResponse.Success)
          {
            await _localStorage.SetItemAsync("authToken", authResponse.Token);
            await _localStorage.SetItemAsync("userEmail", authResponse.Email);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.Token);
          }
          return authResponse;
        }
        return new AuthResponse { Success = false, ErrorMessage = "Registration failed" };
      }
      catch (Exception ex)
      {
        return new AuthResponse { Success = false, ErrorMessage = ex.Message };
      }
    }

    public async Task Logout()
    {
      await _localStorage.RemoveItemAsync("authToken");
      await _localStorage.RemoveItemAsync("userEmail");
      ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<bool> IsAuthenticated()
    {
      var token = await _localStorage.GetItemAsync<string>("authToken");
      return !string.IsNullOrEmpty(token);
    }

    public async Task<string> GetUserEmail()
    {
      return await _localStorage.GetItemAsync<string>("userEmail");
    }
  }
}
