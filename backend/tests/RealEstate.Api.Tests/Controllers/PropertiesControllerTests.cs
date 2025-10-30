using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Api.Tests.Controllers;

[TestFixture]
public class PropertiesControllerTests
{
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;
    private Mock<IPropertyService> _mockPropertyService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockPropertyService = new Mock<IPropertyService>();
        _factory = new CustomWebApplicationFactory
        {
            PropertyServiceMock = _mockPropertyService.Object
        };
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    #region GET /api/properties Tests

    [Test]
    public async Task GetProperties_WithNoFilters_ShouldReturn200WithProperties()
    {
        // Arrange
        var properties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                IdProperty = "1",
                Name = "Property 1",
                Address = "Address 1",
                Price = 100000,
                Year = 2020
            }
        };

        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = properties,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 1
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Be("Property 1");
    }

    [Test]
    public async Task GetProperties_WithNameFilter_ShouldReturn200WithFilteredProperties()
    {
        // Arrange
        var properties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                IdProperty = "1",
                Name = "Villa Paradise",
                Address = "Miami Beach",
                Price = 850000,
                Year = 2020
            }
        };

        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = properties,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 1
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.Is<PropertyFilterDto>(f => f.Name == "Villa")))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties?name=Villa");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Contain("Villa");
    }

    [Test]
    public async Task GetProperties_WithPriceRangeFilter_ShouldReturn200WithFilteredProperties()
    {
        // Arrange
        var properties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                IdProperty = "1",
                Name = "Property 1",
                Address = "Address 1",
                Price = 250000,
                Year = 2020
            }
        };

        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = properties,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 1
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.Is<PropertyFilterDto>(
                f => f.MinPrice == 200000 && f.MaxPrice == 300000)))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties?minPrice=200000&maxPrice=300000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(1);
        result.Items[0].Price.Should().BeInRange(200000, 300000);
    }

    [Test]
    public async Task GetProperties_WithPagination_ShouldReturn200WithCorrectPage()
    {
        // Arrange
        var properties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                IdProperty = "11",
                Name = "Property 11",
                Address = "Address 11",
                Price = 100000,
                Year = 2020
            }
        };

        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = properties,
            PageNumber = 2,
            PageSize = 10,
            TotalCount = 25
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.Is<PropertyFilterDto>(
                f => f.PageNumber == 2 && f.PageSize == 10)))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties?pageNumber=2&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3);
    }

    [Test]
    public async Task GetProperties_WithSorting_ShouldReturn200WithSortedProperties()
    {
        // Arrange
        var properties = new List<PropertyListDto>
        {
            new PropertyListDto { IdProperty = "1", Name = "Property A", Price = 100000, Year = 2020, Address = "Address 1" },
            new PropertyListDto { IdProperty = "2", Name = "Property B", Price = 200000, Year = 2021, Address = "Address 2" }
        };

        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = properties,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 2
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.Is<PropertyFilterDto>(
                f => f.SortBy == "price" && f.SortDescending == true)))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties?sortBy=price&sortDescending=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(2);
    }

    [Test]
    public async Task GetProperties_WithInvalidPageNumber_ShouldReturn400()
    {
        // Act
        var response = await _client.GetAsync("/api/properties?pageNumber=0");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Page number must be greater than 0");
    }

    [Test]
    public async Task GetProperties_WithInvalidPageSize_ShouldReturn400()
    {
        // Act
        var response = await _client.GetAsync("/api/properties?pageSize=101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Page size must be between 1 and 100");
    }

    [Test]
    public async Task GetProperties_WithNegativeMinPrice_ShouldReturn400()
    {
        // Act
        var response = await _client.GetAsync("/api/properties?minPrice=-100");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Minimum price must be greater than or equal to 0");
    }

    [Test]
    public async Task GetProperties_WithMinPriceGreaterThanMaxPrice_ShouldReturn400()
    {
        // Act
        var response = await _client.GetAsync("/api/properties?minPrice=500000&maxPrice=100000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Minimum price must be less than or equal to maximum price");
    }

    [Test]
    public async Task GetProperties_WithInvalidSortBy_ShouldReturn400()
    {
        // Act
        var response = await _client.GetAsync("/api/properties?sortBy=invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Sort by must be one of:");
    }

    [Test]
    public async Task GetProperties_WithEmptyResult_ShouldReturn200WithEmptyList()
    {
        // Arrange
        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = new List<PropertyListDto>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<PropertyListDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region GET /api/properties/{id} Tests

    [Test]
    public async Task GetPropertyById_WithValidId_ShouldReturn200WithProperty()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = new PropertyDetailDto
        {
            IdProperty = propertyId,
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            Year = 2020,
            IdOwner = "owner1",
            Owner = new OwnerDto
            {
                IdOwner = "owner1",
                Name = "John Doe",
                Address = "456 Elm St",
                Birthday = new DateTime(1980, 5, 15)
            },
            Images = new List<PropertyImageDto>
            {
                new PropertyImageDto
                {
                    IdPropertyImage = "img1",
                    File = "https://example.com/image.jpg",
                    Enabled = true,
                    IdProperty = propertyId
                }
            },
            Traces = new List<PropertyTraceDto>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockPropertyService
            .Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var response = await _client.GetAsync($"/api/properties/{propertyId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
        result.Should().NotBeNull();
        result!.IdProperty.Should().Be(propertyId);
        result.Name.Should().Be("Luxury Villa");
        result.Owner.Should().NotBeNull();
        result.Owner!.Name.Should().Be("John Doe");
        result.Images.Should().HaveCount(1);
    }

    [Test]
    public async Task GetPropertyById_WithNonExistentId_ShouldReturn404()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439999";

        _mockPropertyService
            .Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync((PropertyDetailDto?)null);

        // Act
        var response = await _client.GetAsync($"/api/properties/{propertyId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Property Not Found");
        content.Should().Contain(propertyId);
    }

    [Test]
    public async Task GetPropertyById_WithWhitespaceId_ShouldReturn400()
    {
        // Note: URL encoding converts space to %20
        // ASP.NET Core treats this as a missing/required parameter
        // Act
        var response = await _client.GetAsync("/api/properties/%20");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        // ASP.NET Core model binding returns "required" error for whitespace
        content.Should().Contain("validation errors");
    }

    [Test]
    public async Task GetPropertyById_ShouldCallServiceOnce()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = new PropertyDetailDto
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            Year = 2020,
            IdOwner = "owner1",
            Images = new List<PropertyImageDto>(),
            Traces = new List<PropertyTraceDto>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockPropertyService
            .Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        await _client.GetAsync($"/api/properties/{propertyId}");

        // Assert
        _mockPropertyService.Verify(
            s => s.GetPropertyByIdAsync(propertyId),
            Times.Once);
    }

    #endregion

    #region Response Content Type Tests

    [Test]
    public async Task GetProperties_ShouldReturnJsonContentType()
    {
        // Arrange
        var pagedResult = new PagedResultDto<PropertyListDto>
        {
            Items = new List<PropertyListDto>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _mockPropertyService
            .Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await _client.GetAsync("/api/properties");

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Test]
    public async Task GetPropertyById_ShouldReturnJsonContentType()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = new PropertyDetailDto
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            Year = 2020,
            IdOwner = "owner1",
            Images = new List<PropertyImageDto>(),
            Traces = new List<PropertyTraceDto>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockPropertyService
            .Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var response = await _client.GetAsync($"/api/properties/{propertyId}");

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    #endregion
}
