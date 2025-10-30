using FluentValidation;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Validators;

public class PropertyFilterValidator : AbstractValidator<PropertyFilterDto>
{
    private static readonly string[] AllowedSortFields = { "name", "address", "price", "year" };

    public PropertyFilterValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage("Minimum price must be greater than or equal to 0");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue)
            .WithMessage("Maximum price must be greater than or equal to 0");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("Minimum price must be less than or equal to maximum price")
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

        RuleFor(x => x.SortBy)
            .Must(sortBy => AllowedSortFields.Contains(sortBy.ToLower()))
            .WithMessage($"Sort by must be one of: {string.Join(", ", AllowedSortFields)}");
    }
}
