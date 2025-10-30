namespace RealEstate.Application.DTOs;

public class PropertyListDto
{
    public string IdProperty { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CodeInternal { get; set; }
    public int Year { get; set; }
    public string? ImageUrl { get; set; }
    public OwnerBasicDto? Owner { get; set; }
}
