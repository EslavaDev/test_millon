namespace RealEstate.Infrastructure.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string OwnersCollectionName { get; set; } = "Owners";
    public string PropertiesCollectionName { get; set; } = "Properties";
    public string PropertyImagesCollectionName { get; set; } = "PropertyImages";
    public string PropertyTracesCollectionName { get; set; } = "PropertyTraces";
}
