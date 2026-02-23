using System.Threading.Tasks;

namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Defines an asynchronous observer that is notified when errors are published.
/// </summary>
public interface IAsyncErrorObserver
{
    /// <summary>
    /// Handles an error notification asynchronously.
    /// </summary>
    /// <param name="errorInfo">The error being published.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the observer finishes handling the error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    ValueTask OnErrorAsync(IError errorInfo, CancellationToken cancellationToken = default);
}
