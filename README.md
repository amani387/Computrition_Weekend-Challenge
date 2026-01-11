# Patient Menu Microservice

## Project Overview
A cloud-native microservice for managing patient dietary restrictions and menu items, built as part of Computrition's cloud migration strategy using the "Strangler Fig Pattern" to modernize the legacy Hospitality Suite.

## Technology Stack
- **Framework**: .NET 8 (ASP.NET Core Web API)
- **Database**: SQLite (for development), easily switchable to SQL Server
- **ORM Strategy**: Hybrid approach
  - **Entity Framework Core**: For write operations (POST /api/menu-items)
  - **Dapper**: For high-performance read operations (GET /api/patients/{id}/allowed-menu)
- **Testing**: xUnit with Moq for mocking
- **Architecture**: Clean Architecture with Controllers → Services → Repositories

## Why Hybrid ORM Approach?

### Dapper for Read Operations (GET /api/patients/{id}/allowed-menu)
- **Performance**: Raw SQL execution with minimal overhead, crucial for high-volume endpoints
- **Control**: Full SQL query optimization for complex filtering logic
- **Scalability**: Efficiently handles thousands of concurrent requests from bedside tablets
- **Simplicity**: Direct mapping to domain objects without change tracking overhead

### Entity Framework Core for Write Operations (POST /api/menu-items)
- **Safety**: Built-in change tracking and validation prevents data corruption
- **Maintainability**: Strongly-typed LINQ queries and migration support
- **Auditability**: Easier to implement auditing trails and soft deletes
- **Productivity**: Faster development with automatic schema management

## Business Logic
The service implements patient dietary restriction logic:
- **GF (Gluten Free)**: Returns only `IsGlutenFree == true` items
- **SF (Sugar Free)**: Returns only `IsSugarFree == true` items  
- **NONE**: Returns all menu items

## Multi-Tenancy Implementation
The service supports multi-tenancy through:
- `X-Tenant-Id` header required on all requests
- Tenant isolation at database query level
- No cross-tenant data leakage
- Simulated tenant context for development/testing

## API Endpoints

### POST /api/menu-items
Creates a new menu item using Entity Framework Core.
```bash
curl -X POST "https://localhost:7171/api/menu-items" \
  -H "Content-Type: application/json" \
  -H "X-Tenant-Id: 1" \
  -d '{"name":"Grilled Salmon","category":"Main","isGlutenFree":true,"isSugarFree":true,"isHeartHealthy":true}'
```

## Quick Start

### Run Locally
```bash
git clone (https://github.com/amani387/Computrition_Weekend-Challenge)
cd Computrition.MenuService
dotnet run --project Computrition.MenuService.API
