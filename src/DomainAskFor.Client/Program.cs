using DomainAskFor.Client;
using DomainAskFor.Client.HttpRepository;
using DomainAskFor.Client.HttpRepository.SynonymsRepository;
using DomainAskFor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Configure HttpClient - use environment variable or default to localhost
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<IWhoIsHttpRepository, WhoIsRepository>();
builder.Services.AddScoped<ISynonymRepository, SynonymRepository>();

// Add authentication services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISearchHistoryService, SearchHistoryService>();

await builder.Build().RunAsync();
