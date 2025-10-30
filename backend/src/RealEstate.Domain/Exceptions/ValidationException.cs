namespace RealEstate.Domain.Exceptions;

/// <summary>
/// Exception thrown when domain validation fails.
/// </summary>
public class ValidationException : DomainException
{
    public ValidationException()
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Dictionary of validation errors, keyed by property name.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }
}
