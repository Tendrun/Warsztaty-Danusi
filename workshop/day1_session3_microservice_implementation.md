# Day 1: Afternoon Session 1 (13:15-14:45) - .NET Core Microservice Implementation

## Learning Objectives
- Set up a .NET 8 project structure with combined core and infrastructure layers
- Create database scripts for structure and seed data
- Implement database access using Dapper and Entity Framework
- Create configuration options for different environments

## Implementation Steps

### 1. Setting up the .NET 8 Project with Combined Core Layer (20 minutes)

#### Creating a New .NET 8 Web API Project
```powershell
# Create a new solution
dotnet new sln -n PackageTracker

# Create a new Web API project
dotnet new webapi -n PackageTracker.Api

# Create additional projects
dotnet new classlib -n PackageTracker.Core
dotnet new nunit -n PackageTracker.Tests
dotnet new nunit -n PackageTracker.IntegrationTests

# Add projects to solution
dotnet sln add PackageTracker.Api
dotnet sln add PackageTracker.Core
dotnet sln add PackageTracker.Tests
dotnet sln add PackageTracker.IntegrationTests

# Add project references
dotnet add PackageTracker.Api reference PackageTracker.Core
dotnet add PackageTracker.Tests reference PackageTracker.Core
dotnet add PackageTracker.IntegrationTests reference PackageTracker.Api
```

#### Project Structure
```
PackageTracker/
├── PackageTracker.sln
├── PackageTracker.Api/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── PackageTracker.Core/
│   ├── Entities/
│   ├── Interfaces/
│   ├── Services/
│   ├── DTOs/
│   ├── Exceptions/
│   ├── Data/
│   └── Repositories/
├── PackageTracker.Tests/
└── PackageTracker.IntegrationTests/
```

#### Adding Required NuGet Packages

```powershell
# API project packages
cd PackageTracker.Api
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Elasticsearch

# Core project packages (including infrastructure packages)
cd ../PackageTracker.Core
dotnet add package FluentValidation
dotnet add package AutoMapper
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Dapper
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Polly

# Test project packages
cd ../PackageTracker.Tests
dotnet add package NSubstitute
dotnet add package FluentAssertions
dotnet add package NUnit
dotnet add package NUnit3TestAdapter
dotnet add package Microsoft.NET.Test.Sdk

# Integration test packages
cd ../PackageTracker.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package FluentAssertions
dotnet add package NUnit
dotnet add package NUnit3TestAdapter
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Testcontainers.MsSql
```

### 2. Database Setup and Scripts (25 minutes)

#### Creating SQL Scripts for Database Structure

Create a file `PackageTracker.Core/Data/Scripts/01-create-database.sql` with tables:
- Users (Id, Username, Email, PasswordHash, FirstName, LastName, Address, CreatedAt)
- CarrierServices (Id, Name, Description, Price)
- Carriers (Id, Name, Email, PhoneNumber, IsActive)
- CarrierSupportedServices (CarrierId, ServiceId)
- PackageStatuses (Id, Name)
- Packages (Id, TrackingNumber, StatusId, Weight, Length, Width, Height, RecipientAddress, SenderAddress, CarrierId, UserId, ServiceType, CreatedAt, UpdatedAt)
- PackageStatusHistory (Id, PackageId, StatusId, Timestamp, Notes)

#### Creating SQL Scripts for Seed Data

Create a file `PackageTracker.Core/Data/Scripts/02-seed-data.sql` with:
- Predefined package statuses in Polish
- Carrier services (Standard, Express, International)
- Sample carriers with their supported services

#### Setting Up Entity Framework Core

1. Create entity classes in `PackageTracker.Core/Entities/`:
   - User
   - Carrier
   - CarrierService
   - PackageStatus
   - Package
   - PackageStatusHistory

2. Create DbContext in `PackageTracker.Core/Data/ApplicationDbContext.cs`

3. Create entity configurations in `PackageTracker.Core/Data/Configurations/`

4. Configure Entity Framework in `Program.cs`:
   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
       sqlOptions => sqlOptions.EnableRetryOnFailure(
           maxRetryCount: 5,
           maxRetryDelay: TimeSpan.FromSeconds(30),
           errorNumbersToAdd: null)));
   ```

5. Run Entity Framework migrations:
   ```powershell
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### 3. Implementing Database Access (30 minutes)

#### Repository Interfaces

Create repository interfaces in `PackageTracker.Core/Interfaces/Repositories/`:

```csharp
// IRepository.cs - Generic repository interface
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

// IPackageRepository.cs - Package-specific repository interface
public interface IPackageRepository : IRepository<Package>
{
    Task<IEnumerable<Package>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Package>> GetByCarrierIdAsync(Guid carrierId);
    Task<IEnumerable<Package>> GetByStatusAsync(string status);
    Task<Package> GetByTrackingNumberAsync(string trackingNumber);
    Task UpdateStatusAsync(Guid id, string status, string notes = null);
    Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId);
}

// Similar interfaces for ICarrierRepository and IUserRepository
```

#### Entity Framework Repository Implementation

Create EF repositories in `PackageTracker.Core/Repositories/EF/`:

```csharp
// EfRepository.cs - Generic EF repository
public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public EfRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Implementation of IRepository methods
}

// EfPackageRepository.cs - Package-specific EF repository
public class EfPackageRepository : EfRepository<Package>, IPackageRepository
{
    public EfPackageRepository(ApplicationDbContext context) : base(context) { }

    // Implementation of IPackageRepository methods
    // Include related entities with Include()
    // Use transactions for status updates
}

// Similar implementations for EfCarrierRepository and EfUserRepository
```

#### Dapper Repository Implementation

Create Dapper repositories in `PackageTracker.Core/Repositories/Dapper/`:

```csharp
// DapperRepository.cs - Base Dapper repository
public abstract class DapperRepository<T> : IRepository<T> where T : class
{
    protected readonly string _connectionString;

    protected DapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    protected IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // Abstract methods to be implemented by derived classes
}

// DapperPackageRepository.cs - Package-specific Dapper repository
public class DapperPackageRepository : DapperRepository<Package>, IPackageRepository
{
    public DapperPackageRepository(IConfiguration configuration) : base(configuration) { }

    // Implementation using SQL queries and Dapper methods
    // Use JOINs to fetch related entities
    // Use transactions for status updates
}

// Similar implementations for DapperCarrierRepository and DapperUserRepository
```

#### Repository Factory Pattern

Create a factory to switch between implementations:

```csharp
// IRepositoryFactory.cs
public interface IRepositoryFactory
{
    IPackageRepository CreatePackageRepository();
    ICarrierRepository CreateCarrierRepository();
    IUserRepository CreateUserRepository();
}

// RepositoryFactory.cs
public class RepositoryFactory : IRepositoryFactory
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ApplicationDbContext _dbContext;
    private readonly string _dataAccessStrategy;

    public RepositoryFactory(
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        ApplicationDbContext dbContext)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
        _dbContext = dbContext;
        _dataAccessStrategy = configuration["DataAccess:Strategy"] ?? "EntityFramework";
    }

    public IPackageRepository CreatePackageRepository()
    {
        return _dataAccessStrategy.ToLower() switch
        {
            "dapper" => new DapperPackageRepository(_configuration),
            _ => new EfPackageRepository(_dbContext)
        };
    }

    // Similar methods for CreateCarrierRepository and CreateUserRepository
}
```

Register the factory in `Program.cs`:

```csharp
// Register repository factory
builder.Services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

// Register repositories using the factory
builder.Services.AddScoped(provider => 
    provider.GetRequiredService<IRepositoryFactory>().CreatePackageRepository());
builder.Services.AddScoped(provider => 
    provider.GetRequiredService<IRepositoryFactory>().CreateCarrierRepository());
builder.Services.AddScoped(provider => 
    provider.GetRequiredService<IRepositoryFactory>().CreateUserRepository());
```

### 4. Implementing API Controllers (30 minutes)

Create controllers in `PackageTracker.Api/Controllers/`:

```csharp
// PackagesController.cs
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/packages")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly IMapper _mapper;
    private readonly ILogger<PackagesController> _logger;

    public PackagesController(
        IPackageService packageService,
        IMapper mapper,
        ILogger<PackagesController> logger)
    {
        _packageService = packageService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages()
    {
        var packages = await _packageService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<PackageDto>>(packages));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PackageDto>> GetPackage(Guid id)
    {
        var package = await _packageService.GetByIdAsync(id);
        if (package == null)
            return NotFound();

        return Ok(_mapper.Map<PackageDto>(package));
    }

    [HttpPost]
    public async Task<ActionResult<PackageDto>> CreatePackage(CreatePackageDto createPackageDto)
    {
        var package = await _packageService.CreateAsync(createPackageDto);
        return CreatedAtAction(
            nameof(GetPackage), 
            new { id = package.Id, version = HttpContext.GetRequestedApiVersion().ToString() }, 
            _mapper.Map<PackageDto>(package));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(Guid id, UpdatePackageDto updatePackageDto)
    {
        await _packageService.UpdateAsync(id, updatePackageDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(Guid id)
    {
        await _packageService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto updateStatusDto)
    {
        await _packageService.UpdateStatusAsync(id, updateStatusDto.Status, updateStatusDto.Notes);
        return NoContent();
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<StatusHistoryDto>>> GetStatusHistory(Guid id)
    {
        var history = await _packageService.GetStatusHistoryAsync(id);
        return Ok(_mapper.Map<IEnumerable<StatusHistoryDto>>(history));
    }

    [HttpGet("track/{trackingNumber}")]
    public async Task<ActionResult<PackageDto>> TrackPackage(string trackingNumber)
    {
        var package = await _packageService.GetByTrackingNumberAsync(trackingNumber);
        if (package == null)
            return NotFound();

        return Ok(_mapper.Map<PackageDto>(package));
    }
}

// Similar controllers for CarriersController and UsersController
```

### 5. Configuration Management (15 minutes)

#### Setting up appsettings.json

Create environment-specific configuration files:

```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PackageTracker;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  },
  "DataAccess": {
    "Strategy": "EntityFramework"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong",
    "Issuer": "PackageTracker",
    "Audience": "PackageTrackerClients",
    "ExpiryInMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      }
    ]
  }
}

// appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PackageTracker;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  },
  "DataAccess": {
    "Strategy": "EntityFramework"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Information"
      }
    }
  }
}

// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-production-server;Database=PackageTracker;User=your-prod-user;Password=your-prod-password;TrustServerCertificate=True;"
  },
  "DataAccess": {
    "Strategy": "Dapper"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://elasticsearch:9200",
          "indexFormat": "packagetracker-{0:yyyy.MM}",
          "autoRegisterTemplate": true
        }
      }
    ]
  }
}
```

#### Implementing Options Pattern

Create configuration classes:

```csharp
// JwtSettings.cs
public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
}

// DataAccessSettings.cs
public class DataAccessSettings
{
    public string Strategy { get; set; }
}
```

Register configuration in `Program.cs`:

```csharp
// Configure options
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
    
builder.Services.Configure<DataAccessSettings>(
    builder.Configuration.GetSection("DataAccess"));
```

## Workshop Exercise

### Exercise 1: Set up the .NET 8 Project Structure with Combined Core Layer
1. Create the solution and projects
2. Add required NuGet packages
3. Set up project references

### Exercise 2: Implement the Repository Pattern
1. Create entity classes in the Core project
2. Create repository interfaces
3. Implement EF Core repositories
4. Implement Dapper repositories
5. Create a repository factory

### Exercise 3: Create API Controllers
1. Implement CRUD operations for packages
2. Add validation and error handling
3. Test the API with Postman

## Resources and References

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
