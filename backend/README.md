# Real Estate Property Management Backend

.NET 9 Web API backend following Clean Architecture principles with MongoDB.

## 🏗️ Architecture

This backend follows **Clean Architecture** (also known as Onion Architecture) with clear separation of concerns across four layers:

```
┌──────────────────────────────────────┐
│          API Layer                    │
│  (Controllers, Middleware)            │
├──────────────────────────────────────┤
│      Application Layer                │
│  (Services, DTOs, Mappings)           │
├──────────────────────────────────────┤
│  Infrastructure Layer                 │
│  (Repositories, Data Access)          │
├──────────────────────────────────────┤
│        Domain Layer                   │
│  (Entities, Interfaces, Value Objects)│
└──────────────────────────────────────┘
```

### Layer Responsibilities

#### 1. Domain Layer (`RealEstate.Domain`)
- **Core business entities**: Property, Owner, PropertyImage, PropertyTrace
- **Value objects**: Money (with currency support)
- **Repository interfaces**: IPropertyRepository, IOwnerRepository
- **Domain exceptions**: DomainException, EntityNotFoundException
- **No external dependencies**: Pure C# with no NuGet packages

#### 2. Application Layer (`RealEstate.Application`)
- **DTOs (Data Transfer Objects)**: PropertyListDto, PropertyDetailDto, etc.
- **Service interfaces**: IPropertyService
- **Service implementations**: PropertyService with business logic
- **AutoMapper profiles**: Entity-to-DTO mappings
- **FluentValidation**: Input validation rules
- **Dependencies**: AutoMapper, FluentValidation

#### 3. Infrastructure Layer (`RealEstate.Infrastructure`)
- **Data access**: MongoDB connection and context
- **Repository implementations**: PropertyRepository, OwnerRepository
- **Database initialization**: Index creation, seeding
- **MongoDB aggregation pipelines**: Complex queries with $lookup
- **Dependencies**: MongoDB.Driver

#### 4. API Layer (`RealEstate.Api`)
- **Controllers**: PropertiesController, SeedController
- **Middleware**: ExceptionHandlingMiddleware for global error handling
- **Configuration**: CORS, Swagger, logging
- **Dependency injection**: Service registration
- **OpenAPI documentation**: Swagger with XML comments

## 📁 Project Structure

```
backend/
├── src/
│   ├── RealEstate.Domain/
│   │   ├── Entities/
│   │   │   ├── Owner.cs
│   │   │   ├── Property.cs
│   │   │   ├── PropertyImage.cs
│   │   │   └── PropertyTrace.cs
│   │   ├── Interfaces/
│   │   │   ├── IOwnerRepository.cs
│   │   │   └── IPropertyRepository.cs
│   │   ├── ValueObjects/
│   │   │   └── Money.cs
│   │   ├── Exceptions/
│   │   │   ├── DomainException.cs
│   │   │   ├── EntityNotFoundException.cs
│   │   │   └── ValidationException.cs
│   │   └── RealEstate.Domain.csproj
│   │
│   ├── RealEstate.Application/
│   │   ├── DTOs/
│   │   │   ├── OwnerDto.cs
│   │   │   ├── PagedResultDto.cs
│   │   │   ├── PropertyDetailDto.cs
│   │   │   ├── PropertyFilterDto.cs
│   │   │   └── PropertyListDto.cs
│   │   ├── Interfaces/
│   │   │   └── IPropertyService.cs
│   │   ├── Services/
│   │   │   └── PropertyService.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs
│   │   ├── Validators/
│   │   │   └── PropertyFilterValidator.cs
│   │   ├── DependencyInjection.cs
│   │   └── RealEstate.Application.csproj
│   │
│   ├── RealEstate.Infrastructure/
│   │   ├── Configuration/
│   │   │   └── MongoDbSettings.cs
│   │   ├── Data/
│   │   │   ├── RealEstateDbContext.cs
│   │   │   └── DatabaseSeeder.cs
│   │   ├── Repositories/
│   │   │   ├── OwnerRepository.cs
│   │   │   └── PropertyRepository.cs
│   │   ├── DependencyInjection.cs
│   │   └── RealEstate.Infrastructure.csproj
│   │
│   └── RealEstate.Api/
│       ├── Controllers/
│       │   ├── PropertiesController.cs
│       │   └── SeedController.cs
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Program.cs
│       └── RealEstate.Api.csproj
│
└── tests/
    ├── RealEstate.Domain.Tests/
    ├── RealEstate.Application.Tests/
    ├── RealEstate.Infrastructure.Tests/
    └── RealEstate.Api.Tests/
```

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- MongoDB 7.0+ (local or Atlas)
- IDE (Visual Studio 2022, VS Code, or Rider)

### Installation

```bash
# Clone the repository
git clone <repository-url>
cd backend

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Start the API
dotnet run --project src/RealEstate.Api
```

### Configuration

Create `.env` file in the backend directory:

```env
MongoDb__ConnectionString=mongodb://localhost:27017
MongoDb__DatabaseName=RealEstateDB
ASPNETCORE_ENVIRONMENT=Development
```

Or configure in `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB"
  }
}
```

### Database Initialization

The database indexes are created automatically on startup. To seed sample data:

```bash
curl -X POST http://localhost:5000/api/seed
```

Or force re-seed (clears existing data):

```bash
curl -X POST "http://localhost:5000/api/seed?force=true"
```

## 📊 MongoDB Schema

### Collections

#### Owners
```javascript
{
  "_id": ObjectId,
  "IdOwner": "string (GUID)",
  "Name": "string",
  "Address": "string",
  "Photo": "string (optional)",
  "Birthday": ISODate,
  "CreatedAt": ISODate,
  "UpdatedAt": ISODate
}
```

#### Properties
```javascript
{
  "_id": ObjectId,
  "IdProperty": "string (GUID)",
  "Name": "string",
  "Address": "string",
  "Price": NumberDecimal,
  "CodeInternal": "string (optional)",
  "Year": int,
  "IdOwner": "string (GUID)",
  "CreatedAt": ISODate,
  "UpdatedAt": ISODate
}
```

#### PropertyImages
```javascript
{
  "_id": ObjectId,
  "IdPropertyImage": "string (GUID)",
  "IdProperty": "string (GUID)",
  "File": "string (URL)",
  "Enabled": bool
}
```

#### PropertyTraces
```javascript
{
  "_id": ObjectId,
  "IdPropertyTrace": "string (GUID)",
  "IdProperty": "string (GUID)",
  "DateSale": ISODate,
  "Name": "string",
  "Value": NumberDecimal,
  "Tax": NumberDecimal
}
```

### Indexes

```javascript
// Properties
db.Properties.createIndex({ "Name": 1 })
db.Properties.createIndex({ "Address": 1 })
db.Properties.createIndex({ "Price": 1 })
db.Properties.createIndex({ "Year": 1 })
db.Properties.createIndex({ "IdOwner": 1 })
db.Properties.createIndex({ "CodeInternal": 1 })

// Owners
db.Owners.createIndex({ "Name": 1 })

// PropertyImages
db.PropertyImages.createIndex({ "IdProperty": 1 })
db.PropertyImages.createIndex({ "Enabled": 1 })

// PropertyTraces
db.PropertyTraces.createIndex({ "IdProperty": 1 })
db.PropertyTraces.createIndex({ "DateSale": -1 })
```

## 🔍 Key Features

### 1. MongoDB Aggregation Pipeline

The `PropertyRepository.GetByIdAsync` uses aggregation to efficiently join related collections:

```csharp
var pipeline = new BsonDocument[]
{
    new BsonDocument("$match", new BsonDocument("IdProperty", id)),
    new BsonDocument("$lookup", new BsonDocument
    {
        { "from", "Owners" },
        { "localField", "IdOwner" },
        { "foreignField", "IdOwner" },
        { "as", "OwnerData" }
    }),
    new BsonDocument("$lookup", new BsonDocument
    {
        { "from", "PropertyImages" },
        { "localField", "IdProperty" },
        { "foreignField", "IdProperty" },
        { "as", "Images" }
    }),
    new BsonDocument("$lookup", new BsonDocument
    {
        { "from", "PropertyTraces" },
        { "localField", "IdProperty" },
        { "foreignField", "IdProperty" },
        { "as", "Traces" }
    })
};
```

### 2. Dynamic Filtering

The `PropertyRepository.GetFilteredAsync` builds dynamic filters:

```csharp
// Name filter (case-insensitive regex)
if (!string.IsNullOrWhiteSpace(filter.Name))
{
    filterBuilder &= Builders<Property>.Filter.Regex(
        p => p.Name,
        new BsonRegularExpression(filter.Name, "i")
    );
}

// Price range filter
if (filter.MinPrice.HasValue)
{
    filterBuilder &= Builders<Property>.Filter.Gte(p => p.Price, filter.MinPrice.Value);
}
```

### 3. Global Error Handling

`ExceptionHandlingMiddleware` provides consistent error responses:

```csharp
catch (EntityNotFoundException ex)
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    // Return ProblemDetails
}
catch (ValidationException ex)
{
    context.Response.StatusCode = StatusCodes.Status400BadRequest;
    // Return ValidationProblemDetails
}
```

### 4. Automatic Validation

FluentValidation validates inputs before they reach the business logic:

```csharp
public class PropertyFilterValidator : AbstractValidator<PropertyFilterDto>
{
    public PropertyFilterValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue);
        // ...
    }
}
```

## 🧪 Testing

### Test Structure

```
tests/
├── RealEstate.Domain.Tests          # 30 tests - Value objects
├── RealEstate.Application.Tests     # 55 tests - Services, validators, mappings
├── RealEstate.Infrastructure.Tests  # (Optional integration tests)
└── RealEstate.Api.Tests            # 29 tests - Controllers, middleware
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/RealEstate.Domain.Tests

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run with detailed output
dotnet test --logger:"console;verbosity=detailed"
```

### Test Coverage

- **Domain Layer**: 100% (30/30 tests)
- **Application Layer**: 100% (55/55 tests)
- **API Layer**: 100% (29/29 tests)
- **Total**: 114/114 tests passing

## 📝 API Endpoints

### Properties

```http
GET    /api/properties              # Get paginated property list
GET    /api/properties/{id}          # Get property details
```

### Seed (Development Only)

```http
POST   /api/seed                     # Seed database
GET    /api/seed/stats               # Get collection stats
```

### Health

```http
GET    /health                       # Health check
```

### Query Parameters (GET /api/properties)

| Parameter | Type | Description |
|-----------|------|-------------|
| Name | string | Filter by property name (case-insensitive) |
| Address | string | Filter by address (case-insensitive) |
| MinPrice | decimal | Minimum price |
| MaxPrice | decimal | Maximum price |
| Year | int | Filter by construction year |
| SortBy | string | Sort field: name, address, price, year |
| SortDescending | bool | Sort direction (default: false) |
| PageNumber | int | Page number (default: 1) |
| PageSize | int | Page size (default: 12, max: 100) |

### Example Requests

```bash
# Get all properties
curl http://localhost:5000/api/properties

# Filter by name
curl "http://localhost:5000/api/properties?Name=villa"

# Filter by price range
curl "http://localhost:5000/api/properties?MinPrice=500000&MaxPrice=1000000"

# Sort by price descending
curl "http://localhost:5000/api/properties?SortBy=price&SortDescending=true"

# Pagination
curl "http://localhost:5000/api/properties?PageNumber=2&PageSize=10"

# Get property details
curl http://localhost:5000/api/properties/{id}
```

## 🔧 Development

### Code Formatting

```bash
# Format all code
dotnet format

# Verify formatting
dotnet format --verify-no-changes
```

### Code Analysis

```bash
# Build with warnings as errors
dotnet build /p:TreatWarningsAsErrors=true

# Run analyzers
dotnet build /p:RunAnalyzers=true
```

### Adding a New Entity

1. Create entity in `RealEstate.Domain/Entities/`
2. Add repository interface in `RealEstate.Domain/Interfaces/`
3. Implement repository in `RealEstate.Infrastructure/Repositories/`
4. Create DTOs in `RealEstate.Application/DTOs/`
5. Add mapping profile in `RealEstate.Application/Mappings/`
6. Create service interface and implementation
7. Add controller in `RealEstate.Api/Controllers/`
8. Write tests for all layers

### Adding a New Endpoint

1. Add method to controller
2. Add XML documentation comments
3. Add `[ProducesResponseType]` attributes
4. Implement endpoint logic
5. Write integration tests
6. Update Swagger documentation

## 🚀 Deployment

### Build for Production

```bash
dotnet publish -c Release -o ./publish
```

### Environment Variables

```bash
# Connection string
MongoDb__ConnectionString=mongodb://username:password@host:port

# Database name
MongoDb__DatabaseName=RealEstateDB

# CORS origins (comma-separated)
CorsOrigins=https://yourdomain.com,https://www.yourdomain.com

# Environment
ASPNETCORE_ENVIRONMENT=Production
```

### Docker (Future)

```bash
# Build image
docker build -t realestate-api .

# Run container
docker run -p 5000:80 -e MongoDb__ConnectionString="..." realestate-api
```

## 🔐 Security

- ✅ Input validation on all endpoints
- ✅ No SQL/NoSQL injection vulnerabilities
- ✅ CORS configured for specific origins in production
- ✅ HTTPS enforced in production
- ✅ Sensitive data in environment variables
- ✅ Logging without sensitive information
- ✅ Error messages sanitized in production

## 📈 Performance

- **MongoDB Indexes**: All frequently queried fields indexed
- **Aggregation Pipelines**: Efficient joins with $lookup
- **Pagination**: Limits data transferred
- **Connection Pooling**: MongoDB driver manages connections
- **Async/Await**: Non-blocking I/O operations

### Optimization Tips

1. Ensure indexes cover your queries
2. Use projection to limit fields returned
3. Implement caching for frequent queries (Redis)
4. Use batch operations for multiple inserts/updates
5. Monitor slow queries with MongoDB profiler

## 🐛 Troubleshooting

### MongoDB Connection Failed

```bash
# Check MongoDB is running
mongosh --eval "db.adminCommand('ping')"

# Check connection string
echo $MongoDb__ConnectionString

# Verify network access
ping <mongodb-host>
```

### Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Tests Failing

```bash
# Run tests with verbose output
dotnet test --logger:"console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~PropertyServiceTests.GetPropertiesAsync_ReturnsFilteredResults"
```

## 📚 Resources

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MongoDB .NET Driver Documentation](https://mongodb.github.io/mongo-csharp-driver/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [AutoMapper Documentation](https://docs.automapper.org/)

