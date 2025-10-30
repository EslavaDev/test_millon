namespace RealEstate.Application.DTOs;

public class PropertyTraceDto
{
    public string IdPropertyTrace { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
