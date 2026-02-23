namespace DotNetErrorKit.Codes;

/// <summary>
/// Provides default system error codes used by the exception bridge.
/// </summary>
public sealed class SystemErrorCodes : ErrorCategory
{
    /// <summary>
    /// Represents an unhandled exception captured by the exception bridge.
    /// </summary>
    public static readonly ErrorCode UnhandledException = new(
        "SYS_001",
        "An unhandled exception occurred.");
}
