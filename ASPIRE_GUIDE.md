# Running with .NET Aspire

## Quick Start

Now you can run both the API and Client with a single command!

```bash
dotnet run --project src/DomainAskFor.AppHost
```

This will:

- ✅ Start the API on `https://localhost:5001`
- ✅ Start the Blazor Client on `https://localhost:5003`
- ✅ Open the **Aspire Dashboard** at `http://localhost:15000` (or similar)

## What is Aspire Dashboard?

The Aspire Dashboard gives you:

- 📊 **Live status** of all running services
- 📝 **Logs** from API and Client in real-time
- 🔍 **Distributed tracing** to see requests flow between services
- 📈 **Metrics** and performance monitoring
- 🌐 **Environment variables** and configuration

## Alternative: Run Projects Individually

If you prefer the old way:

**Terminal 1 - API:**

```bash
cd src/DomainAskFor.API
dotnet run
```

**Terminal 2 - Client:**

```bash
cd src/DomainAskFor.Client
dotnet run
```

## What Changed?

- Added `DomainAskFor.AppHost` - orchestrates running multiple projects
- Added `DomainAskFor.ServiceDefaults` - shared configuration (API only)
- API now has health checks and telemetry
- Everything still works the same way!

## Ports

- **API**: <https://localhost:5001>
- **Client**: <https://localhost:5003>
- **Aspire Dashboard**: <http://localhost:15000> (check console output for exact port)

## Benefits of Aspire

1. **Single command** to run everything
2. **Better debugging** with centralized logs
3. **Service discovery** - services can find each other automatically
4. **Production-ready** patterns built-in
5. **No configuration needed** for local development
