namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Represents a unique error code and its metadata.
/// </summary>
public interface IErrorCode
{
    /// <summary>
    /// Gets the unique error code value.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Gets the human-readable description for the error.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the severity of the error.
    /// </summary>
    ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets the optional category for the error code.
    /// </summary>
    string? Category { get; }

    /// <summary>
    /// Gets the optional documentation URI for the error code.
    /// </summary>
    Uri? DocumentationUri { get; }
}
