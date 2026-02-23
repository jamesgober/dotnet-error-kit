namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Provides asynchronous access to an error code registry.
/// </summary>
public interface IAsyncErrorRegistry
{
    /// <summary>
    /// Gets the number of registered error codes.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Registers a new error code asynchronously.
    /// </summary>
    /// <param name="errorCode">The error code to register.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> when the error code was registered; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorCode"/> is null.</exception>
    ValueTask<bool> TryRegisterAsync(IErrorCode errorCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to get a registered error code by value asynchronously.
    /// </summary>
    /// <param name="code">The error code value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resolved error code, or <c>null</c> when not found.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is null or whitespace.</exception>
    ValueTask<IErrorCode?> TryGetAsync(string code, CancellationToken cancellationToken = default);
}
