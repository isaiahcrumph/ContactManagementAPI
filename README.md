# Contact Management API

A .NET Core Web API for managing contact information, providing full CRUD operations for contact records.

## Features
- Create, Read, Update, and Delete contacts
- Input validation
- Error handling
- Swagger documentation

## Technologies
- .NET Core
- Entity Framework Core
- SQL Server (LocalDB)
- Swagger/OpenAPI

## Getting Started
1. Clone the repository
2. Ensure you have .NET Core SDK installed
3. Update the connection string in appsettings.json if needed
4. Run migrations using: `dotnet ef database update`
5. Run the application: `dotnet run`

## API Endpoints
- GET /api/contacts - Get all contacts
- GET /api/contacts/{id} - Get a specific contact
- POST /api/contacts - Create a new contact
- PUT /api/contacts/{id} - Update an existing contact
- DELETE /api/contacts/{id} - Delete a contact
