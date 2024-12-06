# Contact Management API - Architecture Overview

## Project Structure
- Controllers: Handle HTTP requests
- Models: Data models and validation
- Services: Business logic layer
- Data: Database context and configurations

## Design Patterns
- Repository Pattern: Separates data access from business logic
- Dependency Injection: Used for service layer
- MVC Pattern: Separation of concerns

## Error Handling
- Global exception handling
- Appropriate HTTP status codes
- Meaningful error messages

## Database
- SQL Server LocalDB
- Code-First approach with EF Core
- Migration support for version control

## See Also
- [Project References](References.md)
- [Setup Guide](Setup_Guide.md)
- [API Usage Guide](API_Usage_Guide.md)
- [Architecture Overview](Architecture_Overview.md)