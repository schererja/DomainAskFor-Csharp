using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace DomainAskFor.Client.Services
{
  public class CustomAuthStateProvider : AuthenticationStateProvider
  {
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
      _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var token = await _localStorage.GetItemAsync<string>("authToken");
      var email = await _localStorage.GetItemAsync<string>("userEmail");

      if (string.IsNullOrEmpty(token))
      {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
      }

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, email ?? ""),
        new Claim(ClaimTypes.Email, email ?? "")
      };

      var identity = new ClaimsIdentity(claims, "jwt");
      var user = new ClaimsPrincipal(identity);

      return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
      var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
      {
        new Claim(ClaimTypes.Name, "user")
      }, "jwt"));

      var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
      NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
      var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
      var authState = Task.FromResult(new AuthenticationState(anonymousUser));
      NotifyAuthenticationStateChanged(authState);
    }
  }
}
