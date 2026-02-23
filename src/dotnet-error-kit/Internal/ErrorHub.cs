using System.Threading;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Internal;

/// <summary>
/// Publishes errors to registered observers.
/// </summary>
internal sealed class ErrorHub : IErrorHub
{
    private IErrorObserver[] _observers = Array.Empty<IErrorObserver>();
    private IAsyncErrorObserver[] _asyncObservers = Array.Empty<IAsyncErrorObserver>();

    /// <inheritdoc />
    public int ObserverCount => _observers.Length;

    /// <summary>
    /// Gets the count of registered asynchronous observers.
    /// </summary>
    public int AsyncObserverCount => _asyncObservers.Length;

    /// <inheritdoc />
    public void RegisterObserver(IErrorObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        while (true)
        {
            var snapshot = _observers;
            if (Array.IndexOf(snapshot, observer) >= 0)
            {
                return;
            }

            var next = new IErrorObserver[snapshot.Length + 1];
            if (snapshot.Length > 0)
            {
                Array.Copy(snapshot, next, snapshot.Length);
            }

            next[^1] = observer;
            if (Interlocked.CompareExchange(ref _observers, next, snapshot) == snapshot)
            {
                return;
            }
        }
    }

    /// <summary>
    /// Registers an asynchronous observer.
    /// </summary>
    /// <param name="observer">The asynchronous observer to register.</param>
    public void RegisterAsyncObserver(IAsyncErrorObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        while (true)
        {
            var snapshot = _asyncObservers;
            if (Array.IndexOf(snapshot, observer) >= 0)
            {
                return;
            }

            var next = new IAsyncErrorObserver[snapshot.Length + 1];
            if (snapshot.Length > 0)
            {
                Array.Copy(snapshot, next, snapshot.Length);
            }

            next[^1] = observer;
            if (Interlocked.CompareExchange(ref _asyncObservers, next, snapshot) == snapshot)
            {
                return;
            }
        }
    }

    /// <inheritdoc />
    public void UnregisterObserver(IErrorObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        while (true)
        {
            var snapshot = _observers;
            if (snapshot.Length == 0)
            {
                return;
            }

            var index = Array.IndexOf(snapshot, observer);
            if (index < 0)
            {
                return;
            }

            if (snapshot.Length == 1)
            {
                if (Interlocked.CompareExchange(ref _observers, Array.Empty<IErrorObserver>(), snapshot) == snapshot)
                {
                    return;
                }

                continue;
            }

            var next = new IErrorObserver[snapshot.Length - 1];
            if (index > 0)
            {
                Array.Copy(snapshot, 0, next, 0, index);
            }

            if (index < snapshot.Length - 1)
            {
                Array.Copy(snapshot, index + 1, next, index, snapshot.Length - index - 1);
            }

            if (Interlocked.CompareExchange(ref _observers, next, snapshot) == snapshot)
            {
                return;
            }
        }
    }

    /// <summary>
    /// Unregisters an asynchronous observer.
    /// </summary>
    /// <param name="observer">The asynchronous observer to unregister.</param>
    public void UnregisterAsyncObserver(IAsyncErrorObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        while (true)
        {
            var snapshot = _asyncObservers;
            if (snapshot.Length == 0)
            {
                return;
            }

            var index = Array.IndexOf(snapshot, observer);
            if (index < 0)
            {
                return;
            }

            if (snapshot.Length == 1)
            {
                if (Interlocked.CompareExchange(ref _asyncObservers, Array.Empty<IAsyncErrorObserver>(), snapshot) == snapshot)
                {
                    return;
                }

                continue;
            }

            var next = new IAsyncErrorObserver[snapshot.Length - 1];
            if (index > 0)
            {
                Array.Copy(snapshot, 0, next, 0, index);
            }

            if (index < snapshot.Length - 1)
            {
                Array.Copy(snapshot, index + 1, next, index, snapshot.Length - index - 1);
            }

            if (Interlocked.CompareExchange(ref _asyncObservers, next, snapshot) == snapshot)
            {
                return;
            }
        }
    }

    /// <inheritdoc />
    public void Publish(IError errorInfo)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);

        var snapshot = _observers;
        for (var i = 0; i < snapshot.Length; i++)
        {
            snapshot[i].OnError(errorInfo);
        }
    }

    /// <summary>
    /// Publishes an error asynchronously to registered asynchronous observers.
    /// </summary>
    /// <param name="errorInfo">The error information to publish.</param>
    /// <param name="cancellationToken">Optional. A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    public async ValueTask PublishAsync(IError errorInfo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);

        var snapshot = _asyncObservers;
        for (var i = 0; i < snapshot.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await snapshot[i].OnErrorAsync(errorInfo, cancellationToken).ConfigureAwait(false);
        }
    }
}
