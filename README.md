# TaskManager — Client-Server Architecture

Bug-tracker / task management system with separated backend API and WPF client.

## Architecture

```
TaskManager.sln
├── src/
│   ├── TaskManager.Domain      — Entities, Enums, DbContext, Repositories (EF Core + SQLite)
│   ├── TaskManager.Shared      — DTOs, API contracts shared between API and Client
│   ├── TaskManager.API         — ASP.NET Core Web API, JWT Auth, Business Logic Services
│   └── TaskManager.Client      — WPF + Prism desktop client (communicates via HTTP)
└── tests/
    ├── TaskManager.API.Tests   — xUnit tests for API services and controllers
    └── TaskManager.Client.Tests — xUnit tests for ViewModels and client services
```

## Tech Stack

- **Backend**: ASP.NET Core Web API, .NET 9, Entity Framework Core, SQLite
- **Frontend**: WPF + Prism (DryIoc), MahApps.Metro
- **Auth**: Custom JWT (Access + Refresh tokens)
- **Tests**: xUnit, Moq
- **Languages**: English, Russian

## Getting Started

```bash
# Restore packages
dotnet restore

# Run API
dotnet run --project src/TaskManager.API

# Run WPF Client
dotnet run --project src/TaskManager.Client

# Run tests
dotnet test
```
