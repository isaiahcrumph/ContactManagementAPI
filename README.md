# Contact Management API
A .NET Core Web API for managing contact information, providing full CRUD operations with advanced querying capabilities.

## Features
- Create, Read, Update, Delete, and Patch contacts
- Advanced filtering (by name, city, state)
- Sorting (multiple fields, ascending/descending)
- Pagination support
- Input validation
- Error handling
- Swagger documentation

## Technologies
- .NET Core
- Entity Framework Core
- SQL Server (LocalDB)
- Swagger/OpenAPI
- Postman test suite

## Getting Started
1. Clone the repository
2. Ensure you have .NET Core SDK installed
3. Update the connection string in appsettings.json if needed
4. Run migrations using: `dotnet ef database update`
5. Run the application: `dotnet run`

## API Endpoints
### Basic Operations
- GET /api/contacts - Get all contacts
- GET /api/contacts/{id} - Get a specific contact
- POST /api/contacts - Create a new contact
- PUT /api/contacts/{id} - Update an existing contact
- PATCH /api/contacts/{id} - Update specific fields
- DELETE /api/contacts/{id} - Delete a contact

### Advanced Querying
- Filtering: `/api/contacts?name=John&city=Seattle&state=WA`
- Sorting: `/api/contacts?sortBy=name&order=desc`
- Paging: `/api/contacts/paged?pageNumber=1&pageSize=10`
- Combined: `/api/contacts/paged?name=John&sortBy=city&pageNumber=1&pageSize=10`

## Testing
- Complete Postman collection included
- 1000 sample contacts for testing
- Comprehensive test scenarios for all features
