using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Mappings;
using RealEstate.Application.Services;
using RealEstate.Application.Validators;

namespace RealEstate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<PropertyFilterValidator>();

        // Register services
        services.AddScoped<IPropertyService, PropertyService>();

        return services;
    }
}
