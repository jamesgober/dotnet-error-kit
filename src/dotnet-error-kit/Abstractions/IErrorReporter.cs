namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Reports errors to registered observers.
/// </summary>
public interface IErrorReporter
{
    /// <summary>
    /// Reports an error to the global error hub.
    /// </summary>
    /// <param name="errorInfo">The error to report.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    void Report(IError errorInfo);

    /// <summary>
    /// Reports an error to asynchronous observers.
    /// </summary>
    /// <param name="errorInfo">The error to report.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when observers finish handling the error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    ValueTask ReportAsync(IError errorInfo, CancellationToken cancellationToken = default);
}
