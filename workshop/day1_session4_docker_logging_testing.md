# Day 1: Afternoon Session 2 (15:00-16:30) - Docker, Logging, and Testing

## Learning Objectives
- Implement structured logging with Serilog
- Containerize the application with Docker
- Write unit tests and integration tests

## Implementation Steps

### 1. Structured Logging with Serilog (30 minutes)

#### Configuring Serilog in Program.cs

```csharp
// Add Serilog
builder.Host.UseSerilog((context, configuration) => 
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console(new JsonFormatter())
        .WriteTo.Conditional(
            ctx => context.HostingEnvironment.IsProduction(),
            wt => wt.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["Elasticsearch:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"packagetracker-{DateTime.UtcNow:yyyy-MM}"
            })
        ));
```

#### Setting up appsettings.json for Serilog

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "Enrich": [
    "FromLogContext",
    "WithMachineName",
    "WithThreadId"
  ],
  "Properties": {
    "Application": "PackageTracker.Api"
  }
}
```

#### Implementing Request/Response Logging Middleware

```csharp
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        _logger.LogInformation("HTTP {RequestMethod} {RequestPath} started", 
        context.Request.Method, context.Request.Path);

        // Capture the original body stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream and use it for the response body
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue down the middleware pipeline
            await _next(context);

            // Log the response
            _logger.LogInformation("HTTP {RequestMethod} {RequestPath} completed with status code {StatusCode}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode);

            // Copy the response body to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            // Restore the original body stream
            context.Response.Body = originalBodyStream;
        }
    }
}

// Extension method to add the middleware to the pipeline
public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
```

Register the middleware in `Program.cs`:

```csharp
// Add request/response logging
app.UseRequestResponseLogging();
```

#### Adding Contextual Logging

```csharp
// In a service or controller
public async Task<Package> CreateAsync(CreatePackageDto dto)
{
    using var scope = _logger.BeginScope(new Dictionary<string, object>
    {
        ["UserId"] = dto.UserId,
        ["TrackingNumber"] = dto.TrackingNumber,
        ["Operation"] = "CreatePackage"
    });

    _logger.LogInformation("Creating new package");
    
    // Implementation...
    
    _logger.LogInformation("Package {TrackingNumber} created successfully", package.TrackingNumber);
    return package;
}
```

### 2. Docker Containerization (30 minutes)

#### Creating a Dockerfile

Create a file named `Dockerfile` in the root of the solution:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PackageTracker.Api/PackageTracker.Api.csproj", "PackageTracker.Api/"]
COPY ["PackageTracker.Core/PackageTracker.Core.csproj", "PackageTracker.Core/"]
COPY ["PackageTracker.Infrastructure/PackageTracker.Infrastructure.csproj", "PackageTracker.Infrastructure/"]
RUN dotnet restore "PackageTracker.Api/PackageTracker.Api.csproj"
COPY . .
WORKDIR "/src/PackageTracker.Api"
RUN dotnet build "PackageTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PackageTracker.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PackageTracker.Api.dll"]
```

#### Creating a docker-compose.yml File

Create a file named `docker-compose.yml` in the root of the solution:

```yaml
version: '3.8'
services:
  api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    depends_on:
      - db
      - elasticsearch
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=db;Database=PackageTracker;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
      - Elasticsearch__Uri=http://elasticsearch:9200
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/root/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Passw0rd
    volumes:
      - sqldata:/var/opt/mssql
      - ./PackageTracker.Infrastructure/Data/Scripts:/scripts
    command: >
      bash -c "
        /opt/mssql/bin/sqlservr &
        sleep 30s &&
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -i /scripts/01-create-database.sql &&
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -i /scripts/02-seed-data.sql &&
        tail -f /dev/null
      "

  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.11.0
    ports:
      - "9200:9200"
    environment:
      - discovery.type=single-node
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
      - xpack.security.enabled=false
    volumes:
      - esdata:/usr/share/elasticsearch/data

  kibana:
    image: docker.elastic.co/kibana/kibana:8.11.0
    ports:
      - "5601:5601"
    depends_on:
      - elasticsearch
    environment:
      - ELASTICSEARCH_HOSTS=http://elasticsearch:9200

volumes:
  sqldata:
  esdata:
```

#### Building and Running Docker Containers

```powershell
# Build and start the containers
docker-compose up -d

# View running containers
docker-compose ps

# View logs
docker-compose logs -f

# Stop containers
docker-compose down
```

### 3. Unit Testing (30 minutes)

#### Setting Up the Test Project

```powershell
cd PackageTracker.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
```

#### Writing Unit Tests for Services

```csharp
// PackageServiceTests.cs
public class PackageServiceTests
{
    private readonly Mock<IPackageRepository> _mockPackageRepository;
    private readonly Mock<ILogger<PackageService>> _mockLogger;
    private readonly PackageService _packageService;

    public PackageServiceTests()
    {
        _mockPackageRepository = new Mock<IPackageRepository>();
        _mockLogger = new Mock<ILogger<PackageService>>();
        _packageService = new PackageService(_mockPackageRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPackage()
    {
        // Arrange
        var packageId = Guid.NewGuid();
        var package = new Package { Id = packageId, TrackingNumber = "TEST123" };
        _mockPackageRepository.Setup(repo => repo.GetByIdAsync(packageId))
            .ReturnsAsync(package);

        // Act
        var result = await _packageService.GetByIdAsync(packageId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(packageId);
        result.TrackingNumber.Should().Be("TEST123");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var packageId = Guid.NewGuid();
        _mockPackageRepository.Setup(repo => repo.GetByIdAsync(packageId))
            .ReturnsAsync((Package)null);

        // Act
        var result = await _packageService.GetByIdAsync(packageId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidDto_CreatesAndReturnsPackage()
    {
        // Arrange
        var createDto = new CreatePackageDto
        {
            TrackingNumber = "TEST123",
            Weight = 2.5m,
            RecipientAddress = "Test Address",
            SenderAddress = "Sender Address",
            UserId = Guid.NewGuid(),
            ServiceType = "Standard"
        };

        var createdPackage = new Package
        {
            Id = Guid.NewGuid(),
            TrackingNumber = createDto.TrackingNumber,
            Weight = createDto.Weight,
            RecipientAddress = createDto.RecipientAddress,
            SenderAddress = createDto.SenderAddress,
            UserId = createDto.UserId,
            ServiceType = createDto.ServiceType
        };

        _mockPackageRepository.Setup(repo => repo.AddAsync(It.IsAny<Package>()))
            .ReturnsAsync(createdPackage);

        // Act
        var result = await _packageService.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.TrackingNumber.Should().Be(createDto.TrackingNumber);
        result.Weight.Should().Be(createDto.Weight);
        result.RecipientAddress.Should().Be(createDto.RecipientAddress);
        result.SenderAddress.Should().Be(createDto.SenderAddress);
        result.UserId.Should().Be(createDto.UserId);
        result.ServiceType.Should().Be(createDto.ServiceType);

        _mockPackageRepository.Verify(repo => repo.AddAsync(It.IsAny<Package>()), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusAsync_ValidIdAndStatus_UpdatesStatus()
    {
        // Arrange
        var packageId = Guid.NewGuid();
        var status = "Delivered";
        var notes = "Delivered to recipient";

        _mockPackageRepository.Setup(repo => repo.UpdateStatusAsync(packageId, status, notes))
            .Returns(Task.CompletedTask);

        // Act
        await _packageService.UpdateStatusAsync(packageId, status, notes);

        // Assert
        _mockPackageRepository.Verify(repo => repo.UpdateStatusAsync(packageId, status, notes), Times.Once);
    }
}
```

#### Writing Unit Tests for Controllers

```csharp
// PackagesControllerTests.cs
public class PackagesControllerTests
{
    private readonly Mock<IPackageService> _mockPackageService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PackagesController>> _mockLogger;
    private readonly PackagesController _controller;

    public PackagesControllerTests()
    {
        _mockPackageService = new Mock<IPackageService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PackagesController>>();
        _controller = new PackagesController(
            _mockPackageService.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetPackages_ReturnsOkResultWithPackages()
    {
        // Arrange
        var packages = new List<Package>
        {
            new Package { Id = Guid.NewGuid(), TrackingNumber = "TEST123" },
            new Package { Id = Guid.NewGuid(), TrackingNumber = "TEST456" }
        };

        var packageDtos = new List<PackageDto>
        {
            new PackageDto { Id = packages[0].Id, TrackingNumber = packages[0].TrackingNumber },
            new PackageDto { Id = packages[1].Id, TrackingNumber = packages[1].TrackingNumber }
        };

        _mockPackageService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(packages);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PackageDto>>(packages))
            .Returns(packageDtos);

        // Act
        var result = await _controller.GetPackages();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var returnedPackages = okResult.Value as IEnumerable<PackageDto>;
        returnedPackages.Should().NotBeNull();
        returnedPackages.Should().HaveCount(2);
        returnedPackages.Should().BeEquivalentTo(packageDtos);
    }

    [Fact]
    public async Task GetPackage_ExistingId_ReturnsOkResultWithPackage()
    {
        // Arrange
        var packageId = Guid.NewGuid();
        var package = new Package { Id = packageId, TrackingNumber = "TEST123" };
        var packageDto = new PackageDto { Id = packageId, TrackingNumber = "TEST123" };

        _mockPackageService.Setup(service => service.GetByIdAsync(packageId))
            .ReturnsAsync(package);
        _mockMapper.Setup(mapper => mapper.Map<PackageDto>(package))
            .Returns(packageDto);

        // Act
        var result = await _controller.GetPackage(packageId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var returnedPackage = okResult.Value as PackageDto;
        returnedPackage.Should().NotBeNull();
        returnedPackage.Id.Should().Be(packageId);
        returnedPackage.TrackingNumber.Should().Be("TEST123");
    }

    [Fact]
    public async Task GetPackage_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var packageId = Guid.NewGuid();
        _mockPackageService.Setup(service => service.GetByIdAsync(packageId))
            .ReturnsAsync((Package)null);

        // Act
        var result = await _controller.GetPackage(packageId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreatePackage_ValidDto_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createDto = new CreatePackageDto
        {
            TrackingNumber = "TEST123",
            Weight = 2.5m,
            RecipientAddress = "Test Address",
            SenderAddress = "Sender Address",
            UserId = Guid.NewGuid(),
            ServiceType = "Standard"
        };

        var createdPackage = new Package
        {
            Id = Guid.NewGuid(),
            TrackingNumber = createDto.TrackingNumber,
            Weight = createDto.Weight,
            RecipientAddress = createDto.RecipientAddress,
            SenderAddress = createDto.SenderAddress,
            UserId = createDto.UserId,
            ServiceType = createDto.ServiceType
        };

        var packageDto = new PackageDto
        {
            Id = createdPackage.Id,
            TrackingNumber = createdPackage.TrackingNumber
        };

        _mockPackageService.Setup(service => service.CreateAsync(createDto))
            .ReturnsAsync(createdPackage);
        _mockMapper.Setup(mapper => mapper.Map<PackageDto>(createdPackage))
            .Returns(packageDto);

        // Set up HttpContext for API version
        var httpContext = new DefaultHttpContext();
        var apiVersion = new ApiVersion(1, 0);
        httpContext.Items[ApiVersioningConstants.ApiVersionKey] = apiVersion;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = await _controller.CreatePackage(createDto);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult.ActionName.Should().Be(nameof(PackagesController.GetPackage));
        createdAtActionResult.RouteValues["id"].Should().Be(createdPackage.Id);
        createdAtActionResult.RouteValues["version"].Should().Be("1.0");
        
        var returnedPackage = createdAtActionResult.Value as PackageDto;
        returnedPackage.Should().NotBeNull();
        returnedPackage.Id.Should().Be(createdPackage.Id);
        returnedPackage.TrackingNumber.Should().Be(createdPackage.TrackingNumber);
    }
}
```

#### Running Unit Tests

```powershell
cd PackageTracker.Tests
dotnet test
```

### 4. Integration Testing (30 minutes)

#### Setting Up the Integration Test Project

```powershell
cd PackageTracker.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package FluentAssertions
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Testcontainers.MsSql
```

#### Creating a Custom WebApplicationFactory

```csharp
// CustomWebApplicationFactory.cs
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly MsSqlContainer _dbContainer;

    public CustomWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong!Passw0rd")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using the test container
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            // Ensure the database is created and apply migrations
            db.Database.EnsureCreated();

            try
            {
                // Seed the database with test data
                SeedDatabase(db);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
            }
        });
    }

    private void SeedDatabase(ApplicationDbContext context)
    {
        // Add package statuses
        var statuses = new List<PackageStatus>
        {
            new PackageStatus { Id = Guid.NewGuid(), Name = "Created" },
            new PackageStatus { Id = Guid.NewGuid(), Name = "CollectedByCarrier" },
            new PackageStatus { Id = Guid.NewGuid(), Name = "InTransit" },
            new PackageStatus { Id = Guid.NewGuid(), Name = "InDelivery" },
            new PackageStatus { Id = Guid.NewGuid(), Name = "Delivered" }
        };
        context.PackageStatuses.AddRange(statuses);

        // Add a test user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            FirstName = "Test",
            LastName = "User",
            Address = "Test Address",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);

        // Add a test carrier
        var carrier = new Carrier
        {
            Id = Guid.NewGuid(),
            Name = "Test Carrier",
            Email = "carrier@example.com",
            PhoneNumber = "+48123456789",
            IsActive = true
        };
        context.Carriers.Add(carrier);

        // Add test packages
        var packages = new List<Package>
        {
            new Package
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "TEST123",
                StatusId = statuses[0].Id,
                Weight = 2.5m,
                RecipientAddress = "Recipient Address",
                SenderAddress = "Sender Address",
                UserId = user.Id,
                ServiceType = "Standard",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Package
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "TEST456",
                StatusId = statuses[1].Id,
                Weight = 1.5m,
                RecipientAddress = "Another Recipient",
                SenderAddress = "Another Sender",
                UserId = user.Id,
                CarrierId = carrier.Id,
                ServiceType = "Express",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        context.Packages.AddRange(packages);

        context.SaveChanges();
    }

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
```

#### Writing Integration Tests for API Endpoints

```csharp
// PackagesControllerIntegrationTests.cs
public class PackagesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PackagesControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetPackages_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/packages");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task GetPackages_ReturnsExpectedPackages()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/packages");
        var content = await response.Content.ReadAsStringAsync();
        var packages = JsonSerializer.Deserialize<List<PackageDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        packages.Should().NotBeNull();
        packages.Should().HaveCountGreaterThanOrEqualTo(2);
        packages.Should().Contain(p => p.TrackingNumber == "TEST123");
        packages.Should().Contain(p => p.TrackingNumber == "TEST456");
    }

    [Fact]
    public async Task GetPackageByTrackingNumber_ExistingTrackingNumber_ReturnsPackage()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/packages/track/TEST123");
        var content = await response.Content.ReadAsStringAsync();
        var package = JsonSerializer.Deserialize<PackageDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        response.EnsureSuccessStatusCode();
        package.Should().NotBeNull();
        package.TrackingNumber.Should().Be("TEST123");
    }

    [Fact]
    public async Task GetPackageByTrackingNumber_NonExistingTrackingNumber_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/packages/track/NONEXISTENT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreatePackage_ValidData_ReturnsCreatedAndNewPackage()
    {
        // Arrange
        var newPackage = new CreatePackageDto
        {
            TrackingNumber = "TEST789",
            Weight = 3.5m,
            RecipientAddress = "New Recipient",
            SenderAddress = "New Sender",
            UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Use a known ID from seed data
            ServiceType = "Standard"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newPackage),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/packages", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdPackage = JsonSerializer.Deserialize<PackageDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        createdPackage.Should().NotBeNull();
        createdPackage.TrackingNumber.Should().Be("TEST789");
        
        // Verify the package was actually created
        var getResponse = await _client.GetAsync($"/api/v1/packages/{createdPackage.Id}");
        getResponse.EnsureSuccessStatusCode();
    }
}
```

#### Running Integration Tests

```powershell
cd PackageTracker.IntegrationTests
dotnet test
```

## Workshop Exercise

### Exercise 1: Implement Structured Logging
1. Configure Serilog in Program.cs
2. Create a request/response logging middleware
3. Add contextual logging to a service

### Exercise 2: Containerize the Application
1. Create a Dockerfile
2. Create a docker-compose.yml file
3. Build and run the Docker containers
4. Verify the application is working

### Exercise 3: Write Unit Tests
1. Create unit tests for a service
2. Create unit tests for a controller
3. Run the tests and verify they pass

### Exercise 4: Write Integration Tests
1. Set up a test database using Testcontainers
2. Create integration tests for API endpoints
3. Run the tests and verify they pass

## Resources and References

- [Serilog Documentation](https://serilog.net/)
- [Docker Documentation](https://docs.docker.com/)
- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Testcontainers Documentation](https://www.testcontainers.org/)