namespace RealEstate.Application.DTOs;

public class PropertyImageDto
{
    public string IdPropertyImage { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
