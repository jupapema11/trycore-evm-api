# Trycore EVM API

Backend API for Earned Value Management (EVM) calculations.

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger / OpenAPI
- xUnit

## Architecture

Clean Architecture:

- API
- Application
- Domain
- Infrastructure
- UnitTests
- IntegrationTests

## Features

- Projects CRUD
- Activities CRUD
- EVM calculations:
  - PV
  - EV
  - CV
  - SV
  - CPI
  - SPI
  - EAC
  - VAC
- Project financial summary
- Validation
- Unit Tests

## Run project

```bash
dotnet restore
dotnet ef database update --project Trycore.EVM.Infrastructure --startup-project Trycore.EVM.API
dotnet run --project Trycore.EVM.API
```

Swagger UI: `https://localhost:44391/swagger-ui`