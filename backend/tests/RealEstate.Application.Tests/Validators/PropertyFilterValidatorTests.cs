using FluentAssertions;
using RealEstate.Application.DTOs;
using RealEstate.Application.Validators;

namespace RealEstate.Application.Tests.Validators;

[TestFixture]
public class PropertyFilterValidatorTests
{
    private PropertyFilterValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new PropertyFilterValidator();
    }

    [Test]
    public void Validate_WithValidFilter_ShouldPass()
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
            SortDescending = false,
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Validate_WithDefaultValues_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto();

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Validate_WithPageNumberZero_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageNumber = 0 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("PageNumber");
        result.Errors.First().ErrorMessage.Should().Be("Page number must be greater than 0");
    }

    [Test]
    public void Validate_WithNegativePageNumber_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageNumber = -1 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("PageNumber");
    }

    [Test]
    public void Validate_WithPageSizeZero_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageSize = 0 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("PageSize");
        result.Errors.First().ErrorMessage.Should().Be("Page size must be between 1 and 100");
    }

    [Test]
    public void Validate_WithPageSizeNegative_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageSize = -5 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("PageSize");
    }

    [Test]
    public void Validate_WithPageSizeGreaterThan100_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageSize = 101 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("PageSize");
        result.Errors.First().ErrorMessage.Should().Be("Page size must be between 1 and 100");
    }

    [Test]
    public void Validate_WithPageSize100_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageSize = 100 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithPageSize1_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { PageSize = 1 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithNegativeMinPrice_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { MinPrice = -100 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("MinPrice");
        result.Errors.First().ErrorMessage.Should().Be("Minimum price must be greater than or equal to 0");
    }

    [Test]
    public void Validate_WithMinPriceZero_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { MinPrice = 0 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithNullMinPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { MinPrice = null };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithNegativeMaxPrice_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { MaxPrice = -500 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("MaxPrice");
        result.Errors.First().ErrorMessage.Should().Be("Maximum price must be greater than or equal to 0");
    }

    [Test]
    public void Validate_WithMaxPriceZero_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { MaxPrice = 0 };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithNullMaxPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto { MaxPrice = null };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithMinPriceGreaterThanMaxPrice_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = 500000,
            MaxPrice = 100000
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().ErrorMessage.Should().Be("Minimum price must be less than or equal to maximum price");
    }

    [Test]
    public void Validate_WithMinPriceEqualToMaxPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = 200000,
            MaxPrice = 200000
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithMinPriceLessThanMaxPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = 100000,
            MaxPrice = 500000
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithOnlyMinPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = 100000,
            MaxPrice = null
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithOnlyMaxPrice_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = null,
            MaxPrice = 500000
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithInvalidSortBy_ShouldFail()
    {
        // Arrange
        var filter = new PropertyFilterDto { SortBy = "invalid" };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("SortBy");
        result.Errors.First().ErrorMessage.Should().Contain("Sort by must be one of:");
    }

    [Test]
    [TestCase("name")]
    [TestCase("address")]
    [TestCase("price")]
    [TestCase("year")]
    public void Validate_WithValidSortBy_ShouldPass(string sortBy)
    {
        // Arrange
        var filter = new PropertyFilterDto { SortBy = sortBy };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    [TestCase("NAME")]
    [TestCase("Address")]
    [TestCase("PRICE")]
    [TestCase("Year")]
    public void Validate_WithMixedCaseSortBy_ShouldPass(string sortBy)
    {
        // Arrange
        var filter = new PropertyFilterDto { SortBy = sortBy };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithMultipleErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            PageNumber = 0,
            PageSize = 101,
            MinPrice = -100,
            MaxPrice = -200,
            SortBy = "invalid"
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(4);
        result.Errors.Select(e => e.PropertyName).Should().Contain("PageNumber");
        result.Errors.Select(e => e.PropertyName).Should().Contain("PageSize");
        result.Errors.Select(e => e.PropertyName).Should().Contain("MinPrice");
        result.Errors.Select(e => e.PropertyName).Should().Contain("MaxPrice");
        result.Errors.Select(e => e.PropertyName).Should().Contain("SortBy");
    }

    [Test]
    public void Validate_WithAllOptionalFieldsNull_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = null,
            Address = null,
            MinPrice = null,
            MaxPrice = null,
            Year = null,
            SortBy = "name",
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validate_WithValidCompleteFilter_ShouldPass()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Modern Villa",
            Address = "123 Ocean Drive",
            MinPrice = 250000,
            MaxPrice = 750000,
            Year = 2022,
            SortBy = "price",
            SortDescending = true,
            PageNumber = 2,
            PageSize = 25
        };

        // Act
        var result = _validator.Validate(filter);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
