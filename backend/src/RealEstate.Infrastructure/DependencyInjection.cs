using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Configuration;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure MongoDB settings
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));

        // Register DbContext
        services.AddSingleton<RealEstateDbContext>();

        // Register repositories
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IOwnerRepository, OwnerRepository>();

        // Register database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<RealEstateDbContext>();
        await dbContext.CreateIndexesAsync();
    }
}
