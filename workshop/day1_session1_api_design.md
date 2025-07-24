# Day 1: Morning Session 1 (9:00-10:30) - Introduction to API Design and OpenAPI 3.0

## Learning Objectives
- Understand RESTful API principles
- Learn OpenAPI 3.0 specification basics
- Set up Postman for API testing

## Preparation Steps
1. Create a new folder for the workshop project
2. Initialize a Git repository
3. Create a basic project structure

## Implementation Steps

### 1. Introduction to RESTful API Principles (30 minutes)

#### HTTP Methods
- **GET**: Retrieve a resource or collection of resources
- **POST**: Create a new resource
- **PUT**: Update an existing resource (full update)
- **PATCH**: Update an existing resource (partial update)
- **DELETE**: Remove a resource

#### Resource-Based URL Structure
- Use nouns, not verbs (e.g., `/packages` not `/getPackages`)
- Use plural nouns for collections (e.g., `/packages` not `/package`)
- Use hierarchical relationships (e.g., `/users/{userId}/packages`)
- Use query parameters for filtering, sorting, and pagination

#### HTTP Status Codes
- **2xx**: Success
  - 200 OK: Standard success response
  - 201 Created: Resource created successfully
  - 204 No Content: Success with no response body
- **4xx**: Client errors
  - 400 Bad Request: Invalid input
  - 401 Unauthorized: Authentication required
  - 403 Forbidden: Authenticated but not authorized
  - 404 Not Found: Resource doesn't exist
  - 409 Conflict: Request conflicts with current state
- **5xx**: Server errors
  - 500 Internal Server Error: Unexpected server error

#### Request/Response Formats (JSON)
- Use consistent naming conventions (camelCase)
- Include appropriate content-type headers
- Structure responses consistently
- Include metadata for collections (pagination, total count)

### 2. OpenAPI 3.0 Specification Overview (30 minutes)

#### Structure of an OpenAPI Document
```yaml
openapi: 3.0.3
info:
  title: Package Tracker API
  description: API for tracking courier packages
  version: 1.0.0
servers:
  - url: https://api.packagetracker.com/v1
    description: Production server
  - url: https://dev-api.packagetracker.com/v1
    description: Development server
paths:
  /packages:
    get:
      summary: List all packages
      # ...
    post:
      summary: Create a new package
      # ...
  /packages/{id}:
    get:
      summary: Get a package by ID
      # ...
components:
  schemas:
    Package:
      # ...
  securitySchemes:
    # ...
```

#### Defining Paths, Operations, and Schemas
- Organize endpoints by resource
- Document request parameters, headers, and body
- Define response schemas for different status codes
- Include examples for requests and responses

#### Documentation Generation
- Tools for generating documentation from OpenAPI specs
- Swagger UI for interactive documentation
- ReDoc for static documentation
- Postman collection generation

#### Tools for Working with OpenAPI
- Swagger Editor: https://editor.swagger.io/
- Stoplight Studio: https://stoplight.io/studio/
- OpenAPI Generator: https://openapi-generator.tech/
- Visual Studio Code extensions

### 3. Setting up Postman (30 minutes)

#### Creating a New Collection
- Organize requests by resource
- Use folders for different API versions
- Add descriptions for better documentation

#### Setting up Environment Variables
- Create environments for development, testing, production
- Define variables for base URL, API keys, tokens
- Use variables in requests for flexibility

#### Creating and Sending Requests
- Configure request methods, URLs, headers, and body
- Set up authentication
- Add pre-request scripts for dynamic data
- Write tests for response validation

#### Saving and Organizing Requests
- Save requests to collections
- Use examples for different scenarios
- Export and share collections with team members

## Workshop Exercise

### Exercise 1: Create a Basic OpenAPI Specification
1. Create a new file named `package_tracker_api.yaml`
2. Define basic info, servers, and security schemes
3. Add endpoints for packages resource (GET, POST)
4. Define the Package schema

### Exercise 2: Set up Postman for API Testing
1. Create a new Postman collection named "Package Tracker API"
2. Set up environment variables for development
3. Create requests for package endpoints
4. Test the requests with mock data

## Resources and References

- [OpenAPI Specification](https://swagger.io/specification/)
- [RESTful API Design Best Practices](https://restfulapi.net/)
- [Postman Learning Center](https://learning.postman.com/)
- [Swagger UI](https://swagger.io/tools/swagger-ui/)