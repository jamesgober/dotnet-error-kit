using System.Collections.Generic;

namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Represents a typed error with context and severity metadata.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets the error code metadata.
    /// </summary>
    IErrorCode Code { get; }

    /// <summary>
    /// Gets the message for the error.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Gets the severity level for the error.
    /// </summary>
    ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets the context entries associated with the error.
    /// </summary>
    IReadOnlyList<IErrorContext> Context { get; }

    /// <summary>
    /// Gets the structured metadata payload for the error.
    /// </summary>
    IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Gets the inner error that caused this error, if any.
    /// </summary>
    IError? InnerError { get; }

    /// <summary>
    /// Gets the timestamp when the error was created.
    /// </summary>
    DateTimeOffset Timestamp { get; }
}
