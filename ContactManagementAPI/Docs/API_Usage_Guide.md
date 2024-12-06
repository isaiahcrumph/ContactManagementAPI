# Contact Management API - Usage Guide

## Available Endpoints

### Get All Contacts
- Endpoint: GET /api/contacts
- Returns: List of all contacts
- Response: 200 OK with contacts array

### Get Single Contact
- Endpoint: GET /api/contacts/{id}
- Returns: Single contact details
- Response: 200 OK or 404 Not Found

### Create Contact
- Endpoint: POST /api/contacts
- Body: Contact object
- Required fields: Name, Email
- Response: 201 Created with new contact

### Update Contact
- Endpoint: PUT /api/contacts/{id}
- Body: Updated contact object
- Response: 204 No Content or 404 Not Found

### Delete Contact
- Endpoint: DELETE /api/contacts/{id}
- Response: 204 No Content or 404 Not Found

## Data Validation Rules
- Name: Required, max 100 characters
- Email: Required, valid email format
- Phone: Valid phone format (###-#### or ###-###-####)
- ZipCode: 5 digits or 5+4 format

## See Also
- [Project References](References.md)
- [Setup Guide](Setup_Guide.md)
- [API Usage Guide](API_Usage_Guide.md)
- [Architecture Overview](Architecture_Overview.md)