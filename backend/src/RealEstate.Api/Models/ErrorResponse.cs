namespace RealEstate.Api.Models;

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error type/title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// Trace ID for error tracking
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Additional error details (validation errors, etc.)
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }
}
