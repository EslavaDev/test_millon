using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Data;

/// <summary>
/// Seeds the database with sample data for testing
/// </summary>
public class DatabaseSeeder
{
    private readonly RealEstateDbContext _context;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPropertyRepository _propertyRepository;

    public DatabaseSeeder(
        RealEstateDbContext context,
        IOwnerRepository ownerRepository,
        IPropertyRepository propertyRepository)
    {
        _context = context;
        _ownerRepository = ownerRepository;
        _propertyRepository = propertyRepository;
    }

    /// <summary>
    /// Seeds the database with sample data
    /// </summary>
    /// <param name="force">If true, clears existing data before seeding</param>
    public async Task SeedAsync(bool force = false)
    {
        // Check if data already exists
        var existingOwnersCount = await _context.Owners.CountDocumentsAsync(FilterDefinition<Owner>.Empty);
        var existingPropertiesCount = await _context.Properties.CountDocumentsAsync(FilterDefinition<Property>.Empty);

        if (existingOwnersCount > 0 || existingPropertiesCount > 0)
        {
            if (!force)
            {
                Console.WriteLine($"Database already has data (Owners: {existingOwnersCount}, Properties: {existingPropertiesCount}). Skipping seed.");
                return;
            }

            // Clear existing data
            Console.WriteLine("Force flag enabled. Clearing existing data...");
            await _context.Owners.DeleteManyAsync(FilterDefinition<Owner>.Empty);
            await _context.Properties.DeleteManyAsync(FilterDefinition<Property>.Empty);
            await _context.PropertyImages.DeleteManyAsync(FilterDefinition<PropertyImage>.Empty);
            await _context.PropertyTraces.DeleteManyAsync(FilterDefinition<PropertyTrace>.Empty);
            Console.WriteLine("Existing data cleared.");
        }

        Console.WriteLine("Starting database seeding...");

        // Create sample owners
        var owners = await SeedOwnersAsync();
        Console.WriteLine($"✓ Created {owners.Count} owners");

        // Create sample properties
        var properties = await SeedPropertiesAsync(owners);
        Console.WriteLine($"✓ Created {properties.Count} properties");

        // Create sample property images
        var imageCount = await SeedPropertyImagesAsync(properties);
        Console.WriteLine($"✓ Created {imageCount} property images");

        // Create sample property traces
        var traceCount = await SeedPropertyTracesAsync(properties);
        Console.WriteLine($"✓ Created {traceCount} property traces");

        Console.WriteLine("Database seeding completed successfully!");
    }

    private async Task<List<Owner>> SeedOwnersAsync()
    {
        var owners = new List<Owner>
        {
            new Owner
            {
                IdOwner = ObjectId.GenerateNewId().ToString(),
                Name = "John Smith",
                Address = "123 Oak Street, New York, NY 10001",
                Photo = "https://i.pravatar.cc/150?img=12",
                Birthday = new DateTime(1975, 5, 15)
            },
            new Owner
            {
                IdOwner = ObjectId.GenerateNewId().ToString(),
                Name = "Maria Garcia",
                Address = "456 Elm Avenue, Los Angeles, CA 90001",
                Photo = "https://i.pravatar.cc/150?img=5",
                Birthday = new DateTime(1982, 8, 23)
            },
            new Owner
            {
                IdOwner = ObjectId.GenerateNewId().ToString(),
                Name = "Robert Johnson",
                Address = "789 Pine Road, Chicago, IL 60601",
                Photo = "https://i.pravatar.cc/150?img=33",
                Birthday = new DateTime(1968, 3, 10)
            },
            new Owner
            {
                IdOwner = ObjectId.GenerateNewId().ToString(),
                Name = "Emily Chen",
                Address = "321 Maple Drive, San Francisco, CA 94102",
                Photo = "https://i.pravatar.cc/150?img=47",
                Birthday = new DateTime(1990, 11, 5)
            },
            new Owner
            {
                IdOwner = ObjectId.GenerateNewId().ToString(),
                Name = "Michael Brown",
                Address = "654 Cedar Lane, Miami, FL 33101",
                Photo = "https://i.pravatar.cc/150?img=52",
                Birthday = new DateTime(1978, 7, 18)
            }
        };

        foreach (var owner in owners)
        {
            await _ownerRepository.AddAsync(owner);
        }

        return owners;
    }

    private async Task<List<Property>> SeedPropertiesAsync(List<Owner> owners)
    {
        var random = new Random(42); // Fixed seed for consistency
        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Luxury Beachfront Villa",
                Address = "100 Ocean Drive, Malibu, CA 90265",
                Price = 1250000m,
                CodeInternal = "PROP-001",
                Year = 2020,
                IdOwner = owners[0].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Downtown Penthouse Suite",
                Address = "500 Park Avenue, New York, NY 10022",
                Price = 2800000m,
                CodeInternal = "PROP-002",
                Year = 2019,
                IdOwner = owners[0].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Modern Family Home",
                Address = "234 Willow Street, Austin, TX 78701",
                Price = 450000m,
                CodeInternal = "PROP-003",
                Year = 2021,
                IdOwner = owners[1].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Cozy Mountain Cabin",
                Address = "789 Pine Mountain Road, Aspen, CO 81611",
                Price = 685000m,
                CodeInternal = "PROP-004",
                Year = 2018,
                IdOwner = owners[1].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Urban Loft Apartment",
                Address = "1200 Market Street, San Francisco, CA 94102",
                Price = 920000m,
                CodeInternal = "PROP-005",
                Year = 2022,
                IdOwner = owners[2].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Suburban Ranch House",
                Address = "567 Meadow Lane, Dallas, TX 75201",
                Price = 325000m,
                CodeInternal = "PROP-006",
                Year = 2015,
                IdOwner = owners[2].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Lakefront Cottage",
                Address = "88 Lakeview Drive, Seattle, WA 98101",
                Price = 575000m,
                CodeInternal = "PROP-007",
                Year = 2017,
                IdOwner = owners[3].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Historic Brownstone",
                Address = "45 Beacon Hill, Boston, MA 02108",
                Price = 1650000m,
                CodeInternal = "PROP-008",
                Year = 1895,
                IdOwner = owners[3].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Beach Condo Paradise",
                Address = "2020 Collins Avenue, Miami Beach, FL 33139",
                Price = 780000m,
                CodeInternal = "PROP-009",
                Year = 2020,
                IdOwner = owners[4].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Country Estate",
                Address = "1500 Rolling Hills Road, Nashville, TN 37201",
                Price = 1100000m,
                CodeInternal = "PROP-010",
                Year = 2016,
                IdOwner = owners[4].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "City View Apartment",
                Address = "777 Michigan Avenue, Chicago, IL 60611",
                Price = 495000m,
                CodeInternal = "PROP-011",
                Year = 2021,
                IdOwner = owners[1].IdOwner
            },
            new Property
            {
                IdProperty = ObjectId.GenerateNewId().ToString(),
                Name = "Garden Townhouse",
                Address = "123 Cherry Blossom Lane, Portland, OR 97201",
                Price = 650000m,
                CodeInternal = "PROP-012",
                Year = 2019,
                IdOwner = owners[3].IdOwner
            }
        };

        foreach (var property in properties)
        {
            await _propertyRepository.AddAsync(property);
        }

        return properties;
    }

    private async Task<int> SeedPropertyImagesAsync(List<Property> properties)
    {
        var random = new Random(42);
        var imageCount = 0;

        // Sample image URLs (using placeholder service)
        var imageUrls = new List<string>
        {
            "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
            "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800",
            "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800",
            "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
            "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800",
            "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
            "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800",
            "https://images.unsplash.com/photo-1600607687644-aac4c3eac7f4?w=800",
            "https://images.unsplash.com/photo-1600210492493-0946911123ea?w=800",
            "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?w=800"
        };

        foreach (var property in properties)
        {
            // Each property gets 1-3 images
            var numImages = random.Next(1, 4);

            for (int i = 0; i < numImages; i++)
            {
                var image = new PropertyImage
                {
                    IdPropertyImage = ObjectId.GenerateNewId().ToString(),
                    IdProperty = property.IdProperty,
                    File = imageUrls[random.Next(imageUrls.Count)],
                    Enabled = i == 0 // First image is always enabled
                };

                await _context.PropertyImages.InsertOneAsync(image);
                imageCount++;
            }
        }

        return imageCount;
    }

    private async Task<int> SeedPropertyTracesAsync(List<Property> properties)
    {
        var random = new Random(42);
        var traceCount = 0;

        // Add traces for some properties (about 60% of them)
        var propertiesWithHistory = properties.OrderBy(x => random.Next()).Take((int)(properties.Count * 0.6)).ToList();

        foreach (var property in propertiesWithHistory)
        {
            // Each property gets 1-3 traces
            var numTraces = random.Next(1, 4);
            var currentDate = DateTime.UtcNow;

            for (int i = 0; i < numTraces; i++)
            {
                // Create traces going back in time
                var daysBack = random.Next(30, 1095); // Between 1 month and 3 years ago
                var dateSale = currentDate.AddDays(-daysBack);

                // Sale price is usually within 80-120% of current price
                var priceVariation = random.Next(80, 121) / 100.0m;
                var saleValue = property.Price * priceVariation;

                var trace = new PropertyTrace
                {
                    IdPropertyTrace = ObjectId.GenerateNewId().ToString(),
                    IdProperty = property.IdProperty,
                    DateSale = dateSale,
                    Name = $"Sale - {dateSale:MMM yyyy}",
                    Value = saleValue,
                    Tax = saleValue * 0.02m // 2% tax
                };

                await _context.PropertyTraces.InsertOneAsync(trace);
                traceCount++;
            }
        }

        return traceCount;
    }
}
