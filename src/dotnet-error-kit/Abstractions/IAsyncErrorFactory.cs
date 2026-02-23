namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Creates errors asynchronously with validated inputs.
/// </summary>
public interface IAsyncErrorFactory
{
    /// <summary>
    /// Creates a new error instance asynchronously.
    /// </summary>
    /// <param name="code">The error code metadata.</param>
    /// <param name="message">The error message override.</param>
    /// <param name="innerError">An optional inner error.</param>
    /// <param name="context">Optional context entries.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is null.</exception>
    ValueTask<IError> CreateAsync(
        IErrorCode code,
        string? message = null,
        IError? innerError = null,
        IReadOnlyList<IErrorContext>? context = null,
        CancellationToken cancellationToken = default);
}
