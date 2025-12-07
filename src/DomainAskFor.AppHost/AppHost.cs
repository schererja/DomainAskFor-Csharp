var builder = DistributedApplication.CreateBuilder(args);

// Add the API project
var api = builder.AddProject<Projects.DomainAskFor_API>("api")
    .WithHttpsEndpoint(port: 5001, name: "https")
    .WithHttpEndpoint(port: 5000, name: "http");

// Add the Blazor Client project
var client = builder.AddProject<Projects.DomainAskFor_Client>("client")
    .WithHttpsEndpoint(port: 5003, name: "https")
    .WithHttpEndpoint(port: 5002, name: "http")
    .WithReference(api);

builder.Build().Run();
