using AutoMapper;
using FluentAssertions;
using Moq;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Tests.Services;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _mockRepository = null!;
    private IMapper _mapper = null!;
    private PropertyService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IPropertyRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new PropertyService(_mockRepository.Object, _mapper);
    }

    [Test]
    public async Task GetPropertiesAsync_WithValidFilter_ShouldReturnPagedResult()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Villa",
            PageNumber = 1,
            PageSize = 10
        };

        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "1",
                Name = "Villa 1",
                Address = "Address 1",
                Price = 100000,
                Year = 2020,
                IdOwner = "owner1",
                Owner = new Owner { IdOwner = "owner1", Name = "Owner 1" }
            },
            new Property
            {
                IdProperty = "2",
                Name = "Villa 2",
                Address = "Address 2",
                Price = 200000,
                Year = 2021,
                IdOwner = "owner2",
                Owner = new Owner { IdOwner = "owner2", Name = "Owner 2" }
            }
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, 2L));

        // Act
        var result = await _service.GetPropertiesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Test]
    public async Task GetPropertiesAsync_ShouldMapFilterCorrectly()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Villa",
            Address = "Miami",
            MinPrice = 100000,
            MaxPrice = 500000,
            Year = 2020,
            SortBy = "price",
            SortDescending = true,
            PageNumber = 2,
            PageSize = 25
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((new List<Property>(), 0L));

        // Act
        await _service.GetPropertiesAsync(filter);

        // Assert
        _mockRepository.Verify(r => r.GetFilteredAsync(It.Is<PropertyFilter>(f =>
            f.Name == "Villa" &&
            f.Address == "Miami" &&
            f.MinPrice == 100000 &&
            f.MaxPrice == 500000 &&
            f.Year == 2020 &&
            f.SortBy == "price" &&
            f.SortDescending == true &&
            f.PageNumber == 2 &&
            f.PageSize == 25
        )), Times.Once);
    }

    [Test]
    public async Task GetPropertiesAsync_WithEmptyResult_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "NonExistent",
            PageNumber = 1,
            PageSize = 10
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((new List<Property>(), 0L));

        // Act
        var result = await _service.GetPropertiesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Test]
    public async Task GetPropertiesAsync_WithMultiplePages_ShouldCalculatePaginationCorrectly()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            PageNumber = 2,
            PageSize = 10
        };

        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "11",
                Name = "Property 11",
                Address = "Address 11",
                Price = 100000,
                Year = 2020,
                IdOwner = "owner1"
            }
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, 25L)); // Total 25 items, 3 pages

        // Act
        var result = await _service.GetPropertiesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Test]
    public async Task GetPropertiesAsync_WithLastPage_ShouldIndicateNoNextPage()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            PageNumber = 3,
            PageSize = 10
        };

        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "21",
                Name = "Property 21",
                Address = "Address 21",
                Price = 100000,
                Year = 2020,
                IdOwner = "owner1"
            }
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, 25L)); // Total 25 items, 3 pages

        // Act
        var result = await _service.GetPropertiesAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(3);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Test]
    public async Task GetPropertiesAsync_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var filter = new PropertyFilterDto();

        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "507f1f77bcf86cd799439011",
                Name = "Luxury Villa",
                Address = "123 Ocean Drive",
                Price = 850000,
                CodeInternal = "PROP-001",
                Year = 2020,
                IdOwner = "owner1",
                Owner = new Owner { IdOwner = "owner1", Name = "John Doe" },
                Images = new List<PropertyImage>
                {
                    new PropertyImage
                    {
                        IdPropertyImage = "img1",
                        File = "https://example.com/image.jpg",
                        Enabled = true
                    }
                }
            }
        };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, 1L));

        // Act
        var result = await _service.GetPropertiesAsync(filter);

        // Assert
        result.Items.Should().HaveCount(1);
        var dto = result.Items[0];
        dto.IdProperty.Should().Be("507f1f77bcf86cd799439011");
        dto.Name.Should().Be("Luxury Villa");
        dto.Address.Should().Be("123 Ocean Drive");
        dto.Price.Should().Be(850000);
        dto.CodeInternal.Should().Be("PROP-001");
        dto.Year.Should().Be(2020);
        dto.Owner.Should().NotBeNull();
        dto.Owner!.Name.Should().Be("John Doe");
        dto.ImageUrl.Should().Be("https://example.com/image.jpg");
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ShouldReturnProperty()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = new Property
        {
            IdProperty = propertyId,
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            CodeInternal = "PROP-001",
            Year = 2020,
            IdOwner = "owner1",
            Owner = new Owner
            {
                IdOwner = "owner1",
                Name = "John Doe",
                Address = "456 Elm St",
                Photo = "john.jpg",
                Birthday = new DateTime(1980, 5, 15)
            },
            Images = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = "img1",
                    File = "https://example.com/image1.jpg",
                    Enabled = true
                }
            },
            Traces = new List<PropertyTrace>
            {
                new PropertyTrace
                {
                    IdPropertyTrace = "trace1",
                    DateSale = new DateTime(2022, 1, 15),
                    Name = "Initial Purchase",
                    Value = 800000,
                    Tax = 80000,
                    IdProperty = propertyId
                }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var result = await _service.GetPropertyByIdAsync(propertyId);

        // Assert
        result.Should().NotBeNull();
        result!.IdProperty.Should().Be(propertyId);
        result.Name.Should().Be("Luxury Villa");
        result.Address.Should().Be("123 Ocean Drive");
        result.Price.Should().Be(850000);
        result.CodeInternal.Should().Be("PROP-001");
        result.Year.Should().Be(2020);
        result.IdOwner.Should().Be("owner1");

        result.Owner.Should().NotBeNull();
        result.Owner!.Name.Should().Be("John Doe");
        result.Owner.Address.Should().Be("456 Elm St");

        result.Images.Should().HaveCount(1);
        result.Images[0].File.Should().Be("https://example.com/image1.jpg");

        result.Traces.Should().HaveCount(1);
        result.Traces[0].Name.Should().Be("Initial Purchase");
        result.Traces[0].Value.Should().Be(800000);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439999";

        _mockRepository
            .Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _service.GetPropertyByIdAsync(propertyId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetPropertyByIdAsync_ShouldCallRepositoryOnce()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";

        _mockRepository
            .Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((Property?)null);

        // Act
        await _service.GetPropertyByIdAsync(propertyId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task GetPropertiesAsync_ShouldCallRepositoryOnce()
    {
        // Arrange
        var filter = new PropertyFilterDto();

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((new List<Property>(), 0L));

        // Act
        await _service.GetPropertiesAsync(filter);

        // Assert
        _mockRepository.Verify(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()), Times.Once);
    }

    [Test]
    public async Task GetPropertiesAsync_WithDifferentFilters_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var filter1 = new PropertyFilterDto { Name = "Villa" };
        var filter2 = new PropertyFilterDto { Address = "Miami" };

        _mockRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((new List<Property>(), 0L));

        // Act
        await _service.GetPropertiesAsync(filter1);
        await _service.GetPropertiesAsync(filter2);

        // Assert
        _mockRepository.Verify(r => r.GetFilteredAsync(It.Is<PropertyFilter>(f => f.Name == "Villa")), Times.Once);
        _mockRepository.Verify(r => r.GetFilteredAsync(It.Is<PropertyFilter>(f => f.Address == "Miami")), Times.Once);
    }
}
