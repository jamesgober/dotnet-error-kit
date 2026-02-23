using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Internal;

/// <summary>
/// Reports errors via the configured error hub.
/// </summary>
internal sealed class ErrorReporter : IErrorReporter
{
    private readonly IErrorHub _hub;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorReporter"/> class.
    /// </summary>
    /// <param name="hub">The error hub.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="hub"/> is null.</exception>
    public ErrorReporter(IErrorHub hub)
    {
        ArgumentNullException.ThrowIfNull(hub);
        _hub = hub;
    }

    /// <inheritdoc />
    public void Report(IError errorInfo)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);
        _hub.Publish(errorInfo);
    }

    /// <summary>
    /// Reports the error asynchronously via the configured error hub.
    /// </summary>
    /// <param name="errorInfo">The error information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous report operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    public ValueTask ReportAsync(IError errorInfo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);
        return _hub.PublishAsync(errorInfo, cancellationToken);
    }
}
