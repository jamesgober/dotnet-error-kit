namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Bridges between typed errors and exceptions.
/// </summary>
public interface IErrorExceptionBridge
{
    /// <summary>
    /// Creates an exception from a typed error.
    /// </summary>
    /// <param name="errorInfo">The error to wrap.</param>
    /// <returns>The created exception.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    Exception ToException(IError errorInfo);

    /// <summary>
    /// Attempts to extract a typed error from an exception.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <param name="errorInfo">The extracted error.</param>
    /// <returns><c>true</c> if an error was extracted; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is null.</exception>
    bool TryGetError(Exception exception, out IError? errorInfo);

    /// <summary>
    /// Converts an exception into a typed error.
    /// </summary>
    /// <param name="exception">The exception to convert.</param>
    /// <returns>The converted error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is null.</exception>
    IError FromException(Exception exception);
}
