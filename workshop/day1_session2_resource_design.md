# Day 1: Morning Session 2 (10:45-12:15) - Designing Resources

## Learning Objectives
- Design API resources for users, carriers, and packages
- Implement API versioning
- Set up authentication
- Define error handling patterns

## Implementation Steps

### 1. Designing the User Account Resource (20 minutes)

#### User Schema Definition
```yaml
User:
  type: object
  properties:
    id:
      type: string
      format: uuid
      description: Unique identifier for the user
    username:
      type: string
      description: Username for login
      example: "john.doe"
    email:
      type: string
      format: email
      description: Email address
      example: "john.doe@example.com"
    passwordHash:
      type: string
      description: Hashed password (not returned in responses)
    firstName:
      type: string
      description: User's first name
      example: "John"
    lastName:
      type: string
      description: User's last name
      example: "Doe"
    address:
      type: string
      description: User's address
      example: "123 Main St, Warsaw, Poland"
    createdAt:
      type: string
      format: date-time
      description: Account creation timestamp
  required:
    - username
    - email
    - passwordHash
```

#### User Management Operations
- **GET /users**: List all users (admin only)
- **GET /users/{id}**: Get user by ID
- **POST /users**: Create a new user (registration)
- **PUT /users/{id}**: Update user information
- **DELETE /users/{id}**: Delete a user (admin only)

#### Request/Response Examples
```json
// POST /users request
{
  "username": "john.doe",
  "email": "john.doe@example.com",
  "password": "securePassword123",
  "firstName": "John",
  "lastName": "Doe",
  "address": "123 Main St, Warsaw, Poland"
}

// GET /users/{id} response
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "username": "john.doe",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "address": "123 Main St, Warsaw, Poland",
  "createdAt": "2025-04-15T10:30:00Z"
}
```

### 2. Designing the Carrier Resource (20 minutes)

#### CarrierService Schema Definition
```yaml
CarrierService:
  type: object
  properties:
    id:
      type: string
      format: uuid
      description: Unique identifier for the service
    name:
      type: string
      enum: [Standard, Express, International]
      description: Service type name
    description:
      type: string
      description: Detailed description of the service
      example: "Delivery within 2-3 business days"
    price:
      type: number
      format: float
      description: Base price for the service
      example: 15.99
  required:
    - name
    - price
```

#### Carrier Schema Definition
```yaml
Carrier:
  type: object
  properties:
    id:
      type: string
      format: uuid
      description: Unique identifier for the carrier
    name:
      type: string
      description: Carrier's full name
      example: "Jan Kowalski"
    email:
      type: string
      format: email
      description: Carrier's email address
      example: "jan.kowalski@carrier.com"
    phoneNumber:
      type: string
      description: Carrier's phone number
      example: "+48123456789"
    isActive:
      type: boolean
      description: Whether the carrier is currently active
      default: true
    supportedServices:
      type: array
      items:
        $ref: '#/components/schemas/CarrierService'
      description: Services supported by this carrier
  required:
    - name
    - email
    - phoneNumber
```

#### Carrier Operations
- **GET /carriers**: List all carriers
- **GET /carriers/{id}**: Get carrier by ID
- **POST /carriers**: Create a new carrier
- **PUT /carriers/{id}**: Update carrier information
- **DELETE /carriers/{id}**: Delete a carrier
- **GET /carriers/{id}/services**: Get services supported by a carrier
- **PUT /carriers/{id}/services**: Update carrier's supported services

#### Request/Response Examples
```json
// POST /carriers request
{
  "name": "Jan Kowalski",
  "email": "jan.kowalski@carrier.com",
  "phoneNumber": "+48123456789",
  "supportedServices": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "name": "Standard"
    },
    {
      "id": "550e8400-e29b-41d4-a716-446655440002",
      "name": "Express"
    }
  ]
}

// GET /carriers/{id} response
{
  "id": "550e8400-e29b-41d4-a716-446655440003",
  "name": "Jan Kowalski",
  "email": "jan.kowalski@carrier.com",
  "phoneNumber": "+48123456789",
  "isActive": true,
  "supportedServices": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "name": "Standard",
      "description": "Standardowa dostawa w ciągu 2-3 dni roboczych",
      "price": 15.99
    },
    {
      "id": "550e8400-e29b-41d4-a716-446655440002",
      "name": "Express",
      "description": "Dostawa następnego dnia roboczego",
      "price": 24.99
    }
  ]
}
```

### 3. Designing the Package Resource (20 minutes)

#### Package Status Definition
```yaml
PackageStatus:
  type: string
  enum:
    - "Created"
    - "CollectedByCarrier"
    - "InTransit"
    - "InDelivery"
    - "Delivered"
  description: Current status of the package
```

#### Package Schema Definition
```yaml
Package:
  type: object
  properties:
    id:
      type: string
      format: uuid
      description: Unique identifier for the package
    trackingNumber:
      type: string
      description: Tracking number for the package
      example: "PL12345678"
    status:
      $ref: '#/components/schemas/PackageStatus'
      description: Current status of the package
    weight:
      type: number
      format: float
      description: Weight in kilograms
      example: 2.5
    dimensions:
      type: object
      properties:
        length:
          type: number
          description: Length in centimeters
          example: 30
        width:
          type: number
          description: Width in centimeters
          example: 20
        height:
          type: number
          description: Height in centimeters
          example: 15
    recipientAddress:
      type: string
      description: Delivery address
      example: "ul. Kwiatowa 1, 00-001 Warszawa"
    senderAddress:
      type: string
      description: Sender's address
      example: "ul. Polna 5, 61-001 Poznań"
    carrierId:
      type: string
      format: uuid
      description: ID of the assigned carrier
    userId:
      type: string
      format: uuid
      description: ID of the user who created the package
    serviceType:
      type: string
      enum: [Standard, Express, International]
      description: Type of delivery service
    createdAt:
      type: string
      format: date-time
      description: Package creation timestamp
    updatedAt:
      type: string
      format: date-time
      description: Last update timestamp
  required:
    - trackingNumber
    - status
    - weight
    - recipientAddress
    - senderAddress
    - userId
    - serviceType
```

#### Package Operations
- **GET /packages**: List all packages
- **GET /packages/{id}**: Get package by ID
- **GET /packages/{trackingNumber}/track**: Track package by tracking number
- **POST /packages**: Create a new package
- **PUT /packages/{id}**: Update package information
- **DELETE /packages/{id}**: Delete a package
- **PUT /packages/{id}/status**: Update package status
- **GET /packages/{id}/history**: Get package status history

#### Request/Response Examples
```json
// POST /packages request
{
  "trackingNumber": "PL12345678",
  "weight": 2.5,
  "dimensions": {
    "length": 30,
    "width": 20,
    "height": 15
  },
  "recipientAddress": "ul. Kwiatowa 1, 00-001 Warszawa",
  "senderAddress": "ul. Polna 5, 61-001 Poznań",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "serviceType": "Standard"
}

// GET /packages/{id} response
{
  "id": "550e8400-e29b-41d4-a716-446655440004",
  "trackingNumber": "PL12345678",
  "status": "Created",
  "weight": 2.5,
  "dimensions": {
    "length": 30,
    "width": 20,
    "height": 15
  },
  "recipientAddress": "ul. Kwiatowa 1, 00-001 Warszawa",
  "senderAddress": "ul. Polna 5, 61-001 Poznań",
  "carrierId": null,
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "serviceType": "Standard",
  "createdAt": "2025-04-15T11:00:00Z",
  "updatedAt": "2025-04-15T11:00:00Z"
}
```

### 4. Package Status Management (15 minutes)

#### Status Transition Rules
- **Created** → **CollectedByCarrier**
- **CollectedByCarrier** → **InTransit**
- **InTransit** → **InDelivery**
- **InDelivery** → **Delivered**

#### Status Update Endpoint
```yaml
/packages/{id}/status:
  put:
    summary: Update package status
    parameters:
      - name: id
        in: path
        required: true
        schema:
          type: string
          format: uuid
    requestBody:
      required: true
      content:
        application/json:
          schema:
            type: object
            properties:
              status:
                $ref: '#/components/schemas/PackageStatus'
              notes:
                type: string
                description: Optional notes about the status change
            required:
              - status
    responses:
      200:
        description: Status updated successfully
      400:
        description: Invalid status transition
      404:
        description: Package not found
```

#### Status History Schema
```yaml
PackageStatusHistory:
  type: object
  properties:
    id:
      type: string
      format: uuid
    packageId:
      type: string
      format: uuid
    status:
      $ref: '#/components/schemas/PackageStatus'
    timestamp:
      type: string
      format: date-time
    notes:
      type: string
```

### 5. API Versioning and Authentication (15 minutes)

#### API Versioning
- URL path versioning: `/api/v1/packages`
- Version in Accept header: `Accept: application/vnd.packagetracker.v1+json`
- Version in custom header: `Api-Version: 1.0`

#### JWT Authentication
- Token-based authentication
- JWT structure: header, payload, signature
- Token expiration and refresh

#### API Key Authentication
- For external services and partners
- Rate limiting based on API key

#### Security Schemes in OpenAPI
```yaml
components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
    apiKeyAuth:
      type: apiKey
      in: header
      name: X-API-Key
```

### 6. Error Handling Patterns (15 minutes)

#### Standard Error Response Format
```json
{
  "error": {
    "code": "RESOURCE_NOT_FOUND",
    "message": "The requested resource was not found",
    "details": "Package with ID '12345' does not exist"
  }
}
```

#### HTTP Status Code Mapping
- 400 Bad Request: Invalid input, validation errors
- 401 Unauthorized: Authentication required
- 403 Forbidden: Authenticated but not authorized
- 404 Not Found: Resource doesn't exist
- 409 Conflict: Request conflicts with current state
- 422 Unprocessable Entity: Semantic errors
- 500 Internal Server Error: Unexpected server errors

#### OpenAPI Error Documentation
```yaml
responses:
  400:
    description: Bad Request
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/Error'
        example:
          error:
            code: "VALIDATION_ERROR"
            message: "Validation failed"
            details: "Weight must be greater than 0"
  404:
    description: Not Found
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/Error'
        example:
          error:
            code: "RESOURCE_NOT_FOUND"
            message: "The requested resource was not found"
            details: "Package with ID '12345' does not exist"
```

## Workshop Exercise

### Exercise 1: Design the Complete API Schema
1. Create a complete OpenAPI specification for the Package Tracker API
2. Include all resources: Users, Carriers, CarrierServices, Packages
3. Define all endpoints with request/response schemas
4. Document error responses

### Exercise 2: Implement API Versioning
1. Update the OpenAPI specification to include versioning
2. Document how versioning works in the API
3. Provide examples of accessing different API versions

## Resources and References

- [OpenAPI Specification](https://swagger.io/specification/)
- [REST API Design Best Practices](https://restfulapi.net/)
- [JWT Introduction](https://jwt.io/introduction)
- [API Versioning Strategies](https://www.troyhunt.com/your-api-versioning-is-wrong-which-is/)