using DomainAskFor.API.Controllers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations
builder.AddServiceDefaults();

// Configure CORS
const string AllowedOrigins = "_allowedSpecificOrigins";
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: AllowedOrigins, policy =>
  {
    policy.WithOrigins("https://localhost:44386", "https://localhost:5003", "http://localhost:5002")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true); // Allow any origin in development
  });
});

// Add services
builder.Services.AddHttpClient<ISynonymsController, SynonymsController>();
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "DomainAskFor API", Version = "v1" });
});

// Add Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
  options.Configuration = "localhost:6379";
  options.InstanceName = "master";
});

var app = builder.Build();

// Map default endpoints for health checks, etc.
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DomainAskFor API v1"));
}

app.UseCors(AllowedOrigins);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
