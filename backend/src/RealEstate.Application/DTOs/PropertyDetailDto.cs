namespace RealEstate.Application.DTOs;

public class PropertyDetailDto
{
    public string IdProperty { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CodeInternal { get; set; }
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
    public OwnerDto? Owner { get; set; }
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
