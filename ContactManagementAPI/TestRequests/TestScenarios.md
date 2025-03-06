# Contact Management API Test Scenarios

## Authentication Tests

### Valid Login Tests
- ? Admin login should return a token and 200 OK
- ? User login should return a token and 200 OK
- ? Token endpoint should return just the token as plain text

### Invalid Login Tests
- ? Invalid username should return 401 Unauthorized
- ? Invalid password should return 401 Unauthorized
- ? Missing credentials should return 400 Bad Request

## Authorization Tests

### Role-Based Access Control
- ? Admin should access admin-only endpoints
- ? User should access user policy endpoints
- ? User should be denied access to admin-only endpoints
- ? Requests without tokens should be rejected with 401 Unauthorized
- ? Requests with invalid tokens should be rejected with 401 Unauthorized
- ? Requests with expired tokens should be rejected with 401 Unauthorized

## Contact API v1 Tests

### GET /api/v1/contacts
- ? Should return all contacts for Admin
- ? Should return all contacts for User
- ? Should filter contacts by name
- ? Should filter contacts by city
- ? Should filter contacts by state
- ? Should sort contacts ascending
- ? Should sort contacts descending
- ? Should handle multiple filter and sort parameters

### GET /api/v1/contacts/{id}
- ? Should return a specific contact
- ? Should return 404 for non-existent contact

### GET /api/v1/contacts/paged
- ? Should return paginated results
- ? Should handle different page sizes
- ? Should handle filters with pagination
- ? Should return correct pagination metadata

### POST /api/v1/contacts
- ? Admin should create a valid contact
- ? User should be denied access (403)
- ? Should validate required fields
- ? Should validate field formats (email, phone, etc.)
- ? Should return 201 Created with the new resource

### PUT /api/v1/contacts/{id}
- ? Admin should update an existing contact
- ? User should be denied access (403)
- ? Should return 404 for non-existent contact
- ? Should validate ID match
- ? Should validate field formats

### PATCH /api/v1/contacts/{id}
- ? Admin should partially update a contact
- ? User should be denied access (403)
- ? Should return 404 for non-existent contact
- ? Should handle individual field updates

### DELETE /api/v1/contacts/{id}
- ? Admin should delete a contact
- ? User should be denied access (403)
- ? Should return 404 for non-existent contact

## Contact API v2 Tests

### GET /api/v2/contacts
- ? Should return all contacts with enhanced metadata
- ? Should include API version in response
- ? Should include filter metadata in response
- ? Should include sorting metadata in response

### GET /api/v2/contacts/summary
- ? Should return contact summary by state
- ? Should return total count and state breakdowns
- ? Should work for User role

### Other v2 Endpoints
- ? Should match v1 functionality with v2 enhancements
- ? Should handle versioning in CreatedAtAction results

## Error Handling Tests

- ? Should return 500 for internal server errors
- ? Should return proper error messages
- ? Should not expose sensitive information in errors