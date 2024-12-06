# Contact Management API - Setup Guide

## Prerequisites
- Visual Studio 2022
- .NET Core SDK
- SQL Server LocalDB

## Installation Steps
1. Clone the repository
2. Open ContactManagementAPI.sln in Visual Studio
3. Check connection string in appsettings.json
4. Run the following commands in Package Manager Console:
   - dotnet ef migrations add InitialCreate
   - dotnet ef database update
5. Run the application (F5 in Visual Studio)

## Configuration
- Database configuration is in appsettings.json
- Default connection uses LocalDB
- Swagger is enabled in development mode

## See Also
- [Project References](References.md)
- [Setup Guide](Setup_Guide.md)
- [API Usage Guide](API_Usage_Guide.md)
- [Architecture Overview](Architecture_Overview.md)