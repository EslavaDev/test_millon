using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory for testing with mocked services
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public IPropertyService? PropertyServiceMock { get; set; }
    public IPropertyRepository? PropertyRepositoryMock { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the DatabaseSeeder as it requires real database connections
            services.RemoveAll(typeof(DatabaseSeeder));

            // Remove the existing services
            services.RemoveAll(typeof(IPropertyService));
            services.RemoveAll(typeof(IPropertyRepository));
            services.RemoveAll(typeof(IOwnerRepository));

            // Add mocked services if provided
            if (PropertyServiceMock != null)
            {
                services.AddSingleton(PropertyServiceMock);
            }

            if (PropertyRepositoryMock != null)
            {
                services.AddSingleton(PropertyRepositoryMock);
            }
            else
            {
                // If no repository mock provided, add a default one
                var mockRepo = new Mock<IPropertyRepository>();
                services.AddSingleton(mockRepo.Object);
            }

            // Add mock for IOwnerRepository to prevent DI errors
            var mockOwnerRepo = new Mock<IOwnerRepository>();
            services.AddSingleton(mockOwnerRepo.Object);

            // Override environment to Development for testing
            builder.UseEnvironment("Development");
        });

        // Disable HTTPS redirection for testing
        builder.UseSetting("HTTPS_PORT", "");

        // Disable database initialization
        builder.UseSetting("InitializeDatabase", "false");
    }
}
