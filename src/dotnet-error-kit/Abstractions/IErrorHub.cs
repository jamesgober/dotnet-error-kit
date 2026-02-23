namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Publishes errors to registered observers.
/// </summary>
public interface IErrorHub
{
    /// <summary>
    /// Gets the current number of observers.
    /// </summary>
    int ObserverCount { get; }

    /// <summary>
    /// Gets the current number of asynchronous observers.
    /// </summary>
    int AsyncObserverCount { get; }

    /// <summary>
    /// Registers an observer to receive error notifications.
    /// </summary>
    /// <param name="observer">The observer to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
    void RegisterObserver(IErrorObserver observer);

    /// <summary>
    /// Registers an asynchronous observer to receive error notifications.
    /// </summary>
    /// <param name="observer">The observer to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
    void RegisterAsyncObserver(IAsyncErrorObserver observer);

    /// <summary>
    /// Unregisters an observer from receiving error notifications.
    /// </summary>
    /// <param name="observer">The observer to unregister.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
    void UnregisterObserver(IErrorObserver observer);

    /// <summary>
    /// Unregisters an asynchronous observer from receiving error notifications.
    /// </summary>
    /// <param name="observer">The observer to unregister.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is null.</exception>
    void UnregisterAsyncObserver(IAsyncErrorObserver observer);

    /// <summary>
    /// Publishes an error to all registered observers.
    /// </summary>
    /// <param name="errorInfo">The error to publish.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    void Publish(IError errorInfo);

    /// <summary>
    /// Publishes an error to all registered asynchronous observers.
    /// </summary>
    /// <param name="errorInfo">The error to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when observers finish handling the error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    ValueTask PublishAsync(IError errorInfo, CancellationToken cancellationToken = default);
}
