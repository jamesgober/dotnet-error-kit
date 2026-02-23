using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit;

/// <summary>
/// Represents an application error with context and severity metadata.
/// </summary>
public sealed class AppError : IError
{
    private static readonly IReadOnlyDictionary<string, object?> EmptyMetadata =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    private readonly IErrorContext[] _context;
    private readonly IReadOnlyDictionary<string, object?> _metadata;

    private AppError(
        IErrorCode code,
        string message,
        IErrorContext[] context,
        IReadOnlyDictionary<string, object?> metadata,
        IError? innerError,
        DateTimeOffset timestamp)
    {
        Code = code;
        Message = message;
        Severity = code.Severity;
        _context = context;
        _metadata = metadata;
        InnerError = innerError;
        Timestamp = timestamp;
    }

    /// <summary>
    /// Creates a new error from the provided error code.
    /// </summary>
    /// <param name="code">The error code metadata.</param>
    /// <param name="message">An optional message override.</param>
    /// <param name="innerError">An optional inner error.</param>
    /// <returns>The created error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> is invalid.</exception>
    public static AppError From(IErrorCode code, string? message = null, IError? innerError = null)
    {
        ArgumentNullException.ThrowIfNull(code);

        var resolvedMessage = message ?? code.Description;
        if (string.IsNullOrWhiteSpace(resolvedMessage))
        {
            throw new ArgumentException("Error message must not be null or whitespace.", nameof(message));
        }

        return new AppError(code, resolvedMessage, Array.Empty<IErrorContext>(), EmptyMetadata, innerError, DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Creates a new error with the specified context entry.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <param name="value">The context value.</param>
    /// <returns>A new error with the context appended.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> or <paramref name="value"/> are invalid.</exception>
    public AppError WithContext(string key, string value)
    {
        return WithContext(new ErrorContext(key, value));
    }

    /// <summary>
    /// Creates a new error with the specified context entry.
    /// </summary>
    /// <param name="context">The context entry to append.</param>
    /// <returns>A new error with the context appended.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public AppError WithContext(IErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var current = _context;
        var next = new IErrorContext[current.Length + 1];
        if (current.Length > 0)
        {
            Array.Copy(current, next, current.Length);
        }

        next[^1] = context;
        return new AppError(Code, Message, next, _metadata, InnerError, Timestamp);
    }

    /// <summary>
    /// Creates a new error with the specified metadata entry.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>A new error with metadata appended.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is invalid.</exception>
    public AppError WithMetadata(string key, object? value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Metadata key must not be null or whitespace.", nameof(key));
        }

        var next = new Dictionary<string, object?>(_metadata, StringComparer.Ordinal)
        {
            [key] = value
        };

        return new AppError(Code, Message, _context, next, InnerError, Timestamp);
    }

    /// <summary>
    /// Creates a new error with the specified metadata entries.
    /// </summary>
    /// <param name="metadata">The metadata entries to append.</param>
    /// <returns>A new error with metadata appended.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="metadata"/> is null.</exception>
    public AppError WithMetadata(IReadOnlyDictionary<string, object?> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        if (metadata.Count == 0)
        {
            return this;
        }

        var next = new Dictionary<string, object?>(_metadata, StringComparer.Ordinal);
        foreach (var entry in metadata)
        {
            if (string.IsNullOrWhiteSpace(entry.Key))
            {
                throw new ArgumentException("Metadata key must not be null or whitespace.", nameof(metadata));
            }

            next[entry.Key] = entry.Value;
        }

        return new AppError(Code, Message, _context, next, InnerError, Timestamp);
    }

    /// <summary>
    /// Creates a new error with the specified inner error.
    /// </summary>
    /// <param name="innerError">The inner error.</param>
    /// <returns>A new error with the inner error assigned.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="innerError"/> is null.</exception>
    public AppError WithInnerError(IError innerError)
    {
        ArgumentNullException.ThrowIfNull(innerError);

        return new AppError(Code, Message, _context, _metadata, innerError, Timestamp);
    }

    /// <inheritdoc />
    public IErrorCode Code { get; }

    /// <inheritdoc />
    public string Message { get; }

    /// <inheritdoc />
    public ErrorSeverity Severity { get; }

    /// <inheritdoc />
    public IReadOnlyList<IErrorContext> Context => _context;

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

    /// <inheritdoc />
    public IError? InnerError { get; }

    /// <inheritdoc />
    public DateTimeOffset Timestamp { get; }
}
