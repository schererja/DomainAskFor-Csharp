using DomainAskFor.Client;
using DomainAskFor.Client.HttpRepository;
using DomainAskFor.Client.HttpRepository.SynonymsRepository;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
builder.Services.AddScoped<IWhoIsHttpRepository, WhoIsRepository>();
builder.Services.AddScoped<ISynonymRepository, SynonymRepository>();

await builder.Build().RunAsync();
