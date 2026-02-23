namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Defines an observer that is notified when errors are published.
/// </summary>
public interface IErrorObserver
{
    /// <summary>
    /// Handles an error notification.
    /// </summary>
    /// <param name="errorInfo">The error being published.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorInfo"/> is null.</exception>
    void OnError(IError errorInfo);
}
