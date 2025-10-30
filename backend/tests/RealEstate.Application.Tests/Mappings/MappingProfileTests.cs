using AutoMapper;
using FluentAssertions;
using RealEstate.Application.DTOs;
using RealEstate.Application.Mappings;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Tests.Mappings;

[TestFixture]
public class MappingProfileTests
{
    private IMapper _mapper = null!;
    private MapperConfiguration _configuration = null!;

    [SetUp]
    public void SetUp()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Assert
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    public void Map_PropertyFilterDto_To_PropertyFilter_ShouldMapAllProperties()
    {
        // Arrange
        var dto = new PropertyFilterDto
        {
            Name = "Villa",
            Address = "Miami Beach",
            MinPrice = 100000,
            MaxPrice = 500000,
            Year = 2020,
            SortBy = "price",
            SortDescending = true,
            PageNumber = 2,
            PageSize = 25
        };

        // Act
        var result = _mapper.Map<PropertyFilter>(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        result.Address.Should().Be(dto.Address);
        result.MinPrice.Should().Be(dto.MinPrice);
        result.MaxPrice.Should().Be(dto.MaxPrice);
        result.Year.Should().Be(dto.Year);
        result.SortBy.Should().Be(dto.SortBy);
        result.SortDescending.Should().Be(dto.SortDescending);
        result.PageNumber.Should().Be(dto.PageNumber);
        result.PageSize.Should().Be(dto.PageSize);
    }

    [Test]
    public void Map_Owner_To_OwnerBasicDto_ShouldMapRequiredProperties()
    {
        // Arrange
        var owner = new Owner
        {
            IdOwner = "507f1f77bcf86cd799439011",
            Name = "John Doe",
            Address = "123 Main St",
            Photo = "photo.jpg",
            Birthday = new DateTime(1980, 5, 15),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<OwnerBasicDto>(owner);

        // Assert
        result.Should().NotBeNull();
        result.IdOwner.Should().Be(owner.IdOwner);
        result.Name.Should().Be(owner.Name);
    }

    [Test]
    public void Map_Owner_To_OwnerDto_ShouldMapAllProperties()
    {
        // Arrange
        var owner = new Owner
        {
            IdOwner = "507f1f77bcf86cd799439011",
            Name = "John Doe",
            Address = "123 Main St",
            Photo = "photo.jpg",
            Birthday = new DateTime(1980, 5, 15),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<OwnerDto>(owner);

        // Assert
        result.Should().NotBeNull();
        result.IdOwner.Should().Be(owner.IdOwner);
        result.Name.Should().Be(owner.Name);
        result.Address.Should().Be(owner.Address);
        result.Photo.Should().Be(owner.Photo);
        result.Birthday.Should().Be(owner.Birthday);
    }

    [Test]
    public void Map_PropertyImage_To_PropertyImageDto_ShouldMapAllProperties()
    {
        // Arrange
        var image = new PropertyImage
        {
            IdPropertyImage = "507f1f77bcf86cd799439012",
            IdProperty = "507f1f77bcf86cd799439011",
            File = "https://example.com/image.jpg",
            Enabled = true
        };

        // Act
        var result = _mapper.Map<PropertyImageDto>(image);

        // Assert
        result.Should().NotBeNull();
        result.IdPropertyImage.Should().Be(image.IdPropertyImage);
        result.IdProperty.Should().Be(image.IdProperty);
        result.File.Should().Be(image.File);
        result.Enabled.Should().Be(image.Enabled);
    }

    [Test]
    public void Map_PropertyTrace_To_PropertyTraceDto_ShouldMapAllProperties()
    {
        // Arrange
        var trace = new PropertyTrace
        {
            IdPropertyTrace = "507f1f77bcf86cd799439013",
            DateSale = new DateTime(2023, 6, 15),
            Name = "First Sale",
            Value = 450000.00m,
            Tax = 45000.00m,
            IdProperty = "507f1f77bcf86cd799439011"
        };

        // Act
        var result = _mapper.Map<PropertyTraceDto>(trace);

        // Assert
        result.Should().NotBeNull();
        result.IdPropertyTrace.Should().Be(trace.IdPropertyTrace);
        result.DateSale.Should().Be(trace.DateSale);
        result.Name.Should().Be(trace.Name);
        result.Value.Should().Be(trace.Value);
        result.Tax.Should().Be(trace.Tax);
        result.IdProperty.Should().Be(trace.IdProperty);
    }

    [Test]
    public void Map_Property_To_PropertyListDto_ShouldMapAllProperties()
    {
        // Arrange
        var owner = new Owner
        {
            IdOwner = "507f1f77bcf86cd799439011",
            Name = "John Doe"
        };

        var property = new Property
        {
            IdProperty = "507f1f77bcf86cd799439012",
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            CodeInternal = "PROP-001",
            Year = 2020,
            IdOwner = owner.IdOwner,
            Owner = owner,
            Images = new List<PropertyImage>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<PropertyListDto>(property);

        // Assert
        result.Should().NotBeNull();
        result.IdProperty.Should().Be(property.IdProperty);
        result.Name.Should().Be(property.Name);
        result.Address.Should().Be(property.Address);
        result.Price.Should().Be(property.Price);
        result.CodeInternal.Should().Be(property.CodeInternal);
        result.Year.Should().Be(property.Year);
        result.Owner.Should().NotBeNull();
        result.Owner!.IdOwner.Should().Be(owner.IdOwner);
        result.Owner.Name.Should().Be(owner.Name);
    }

    [Test]
    public void Map_Property_To_PropertyListDto_WithEnabledImage_ShouldMapImageUrl()
    {
        // Arrange
        var property = new Property
        {
            IdProperty = "507f1f77bcf86cd799439012",
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            Year = 2020,
            IdOwner = "507f1f77bcf86cd799439011",
            Images = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = "img1",
                    File = "https://example.com/image1.jpg",
                    Enabled = false
                },
                new PropertyImage
                {
                    IdPropertyImage = "img2",
                    File = "https://example.com/image2.jpg",
                    Enabled = true
                }
            }
        };

        // Act
        var result = _mapper.Map<PropertyListDto>(property);

        // Assert
        result.Should().NotBeNull();
        result.ImageUrl.Should().Be("https://example.com/image2.jpg");
    }

    [Test]
    public void Map_Property_To_PropertyListDto_WithNoEnabledImages_ShouldMapImageUrlToNull()
    {
        // Arrange
        var property = new Property
        {
            IdProperty = "507f1f77bcf86cd799439012",
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            Year = 2020,
            IdOwner = "507f1f77bcf86cd799439011",
            Images = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = "img1",
                    File = "https://example.com/image1.jpg",
                    Enabled = false
                }
            }
        };

        // Act
        var result = _mapper.Map<PropertyListDto>(property);

        // Assert
        result.Should().NotBeNull();
        result.ImageUrl.Should().BeNull();
    }

    [Test]
    public void Map_Property_To_PropertyListDto_WithEmptyImages_ShouldMapImageUrlToNull()
    {
        // Arrange
        var property = new Property
        {
            IdProperty = "507f1f77bcf86cd799439012",
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            Year = 2020,
            IdOwner = "507f1f77bcf86cd799439011",
            Images = new List<PropertyImage>()
        };

        // Act
        var result = _mapper.Map<PropertyListDto>(property);

        // Assert
        result.Should().NotBeNull();
        result.ImageUrl.Should().BeNull();
    }

    [Test]
    public void Map_Property_To_PropertyDetailDto_ShouldMapAllProperties()
    {
        // Arrange
        var owner = new Owner
        {
            IdOwner = "507f1f77bcf86cd799439011",
            Name = "John Doe",
            Address = "456 Elm St",
            Photo = "john.jpg",
            Birthday = new DateTime(1980, 5, 15)
        };

        var images = new List<PropertyImage>
        {
            new PropertyImage
            {
                IdPropertyImage = "img1",
                IdProperty = "507f1f77bcf86cd799439012",
                File = "https://example.com/image1.jpg",
                Enabled = true
            },
            new PropertyImage
            {
                IdPropertyImage = "img2",
                IdProperty = "507f1f77bcf86cd799439012",
                File = "https://example.com/image2.jpg",
                Enabled = true
            }
        };

        var traces = new List<PropertyTrace>
        {
            new PropertyTrace
            {
                IdPropertyTrace = "trace1",
                DateSale = new DateTime(2022, 1, 15),
                Name = "Initial Purchase",
                Value = 800000,
                Tax = 80000,
                IdProperty = "507f1f77bcf86cd799439012"
            }
        };

        var property = new Property
        {
            IdProperty = "507f1f77bcf86cd799439012",
            Name = "Luxury Villa",
            Address = "123 Ocean Drive",
            Price = 850000,
            CodeInternal = "PROP-001",
            Year = 2020,
            IdOwner = owner.IdOwner,
            Owner = owner,
            Images = images,
            Traces = traces,
            CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = _mapper.Map<PropertyDetailDto>(property);

        // Assert
        result.Should().NotBeNull();
        result.IdProperty.Should().Be(property.IdProperty);
        result.Name.Should().Be(property.Name);
        result.Address.Should().Be(property.Address);
        result.Price.Should().Be(property.Price);
        result.CodeInternal.Should().Be(property.CodeInternal);
        result.Year.Should().Be(property.Year);
        result.IdOwner.Should().Be(property.IdOwner);
        result.CreatedAt.Should().Be(property.CreatedAt);
        result.UpdatedAt.Should().Be(property.UpdatedAt);

        result.Owner.Should().NotBeNull();
        result.Owner!.IdOwner.Should().Be(owner.IdOwner);
        result.Owner.Name.Should().Be(owner.Name);
        result.Owner.Address.Should().Be(owner.Address);

        result.Images.Should().HaveCount(2);
        result.Images[0].File.Should().Be("https://example.com/image1.jpg");
        result.Images[1].File.Should().Be("https://example.com/image2.jpg");

        result.Traces.Should().HaveCount(1);
        result.Traces[0].Name.Should().Be("Initial Purchase");
        result.Traces[0].Value.Should().Be(800000);
    }

    [Test]
    public void Map_PropertyCollection_To_PropertyListDtoCollection_ShouldMapAll()
    {
        // Arrange
        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "1",
                Name = "Property 1",
                Address = "Address 1",
                Price = 100000,
                Year = 2020,
                IdOwner = "owner1"
            },
            new Property
            {
                IdProperty = "2",
                Name = "Property 2",
                Address = "Address 2",
                Price = 200000,
                Year = 2021,
                IdOwner = "owner2"
            }
        };

        // Act
        var result = _mapper.Map<List<PropertyListDto>>(properties);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].IdProperty.Should().Be("1");
        result[0].Name.Should().Be("Property 1");
        result[1].IdProperty.Should().Be("2");
        result[1].Name.Should().Be("Property 2");
    }
}
