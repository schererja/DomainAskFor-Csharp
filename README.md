# DomainAskFor-Csharp

DomainAskFor-Csharp is a domain name availability checker and suggestion tool built with .NET 9, Blazor WebAssembly, and ASP.NET Core Web API. The application helps users find available domain names by checking the availability of their desired domain and providing synonym-based alternatives.

## Features

- **Domain Availability Checking**: Uses WHOIS lookup to check if domain names are available
- **Synonym-Based Suggestions**: Integrates with Merriam-Webster's Thesaurus API to generate domain name alternatives using synonyms
- **Multiple TLD Support**: Supports popular top-level domains (.com, .io, .me, .net, .org)
- **Flexible Domain Construction**: Allows users to specify prefix, word, and suffix combinations
- **Modern UI**: Built with Blazor WebAssembly for a responsive single-page application experience
- **Caching**: Implements Redis caching for improved performance

## Architecture

The application follows a clean architecture pattern with three main projects:

- **DomainAskFor.API**: ASP.NET Core Web API backend providing domain checking and synonym services
- **DomainAskFor.Client**: Blazor WebAssembly frontend for user interaction
- **DomainAskFor.Models**: Shared data models and DTOs

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Merriam-Webster Thesaurus API key (for synonym functionality)
- Redis server (for caching) - optional but recommended

## Installation & Setup

1. Clone the repository:

```bash
git clone https://github.com/schererja/DomainAskFor-Csharp.git
cd DomainAskFor-Csharp
```

2. Configure the API settings:
   - Update `src/DomainAskFor.API/appsettings.json` with your Merriam-Webster API key
   - Configure Redis connection string if using caching

3. Run the API server:

```bash
cd src/DomainAskFor.API
dotnet watch run
```

4. Run the Blazor client (in a separate terminal):

```bash
cd src/DomainAskFor.Client
dotnet watch run
```

The applications will be available at:

- **API**: <https://localhost:5001> (with Swagger at <https://localhost:5001/swagger>)
- **Blazor Client**: <https://localhost:5003>

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[Apache v2](https://www.apache.org/licenses/LICENSE-2.0)
