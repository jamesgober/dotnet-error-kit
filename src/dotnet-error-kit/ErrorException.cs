using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit;

/// <summary>
/// Represents an exception that wraps an <see cref="IError"/> instance.
/// </summary>
public sealed class ErrorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorException"/> class.
    /// </summary>
    /// <param name="error">The error to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is null.</exception>
    public ErrorException(IError error)
        : this(error, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorException"/> class.
    /// </summary>
    /// <param name="error">The error to wrap.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is null.</exception>
    public ErrorException(IError error, Exception? innerException)
        : base(GetMessage(error), innerException)
    {
        Error = error;
    }

    /// <summary>
    /// Gets the wrapped error.
    /// </summary>
    public IError Error { get; }

    private static string GetMessage(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return error.Message;
    }
}
