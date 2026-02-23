using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Codes;

/// <summary>
/// Defines a strongly typed error code.
/// </summary>
public sealed class ErrorCode : IErrorCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorCode"/> class.
    /// </summary>
    /// <param name="value">The unique error code value.</param>
    /// <param name="description">The human-readable error description.</param>
    /// <param name="severity">The error severity.</param>
    /// <param name="category">The optional error category.</param>
    /// <param name="documentationUri">The optional documentation URI.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> or <paramref name="description"/> are invalid.</exception>
    public ErrorCode(
        string value,
        string description,
        ErrorSeverity severity = ErrorSeverity.Error,
        string? category = null,
        Uri? documentationUri = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Error code value must not be null or whitespace.", nameof(value));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Error code description must not be null or whitespace.", nameof(description));
        }

        if (category is not null && string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Error code category must not be whitespace.", nameof(category));
        }

        Value = value;
        Description = description;
        Severity = severity;
        Category = category;
        DocumentationUri = documentationUri;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public string Description { get; }

    /// <inheritdoc />
    public ErrorSeverity Severity { get; }

    /// <inheritdoc />
    public string? Category { get; }

    /// <inheritdoc />
    public Uri? DocumentationUri { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}
