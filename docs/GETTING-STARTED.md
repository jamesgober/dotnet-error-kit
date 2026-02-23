# Getting Started

This guide walks through common setup steps and the core workflows for `dotnet-error-kit`.

## Install

```bash
dotnet add package JG.ErrorKit
```

## Define error codes

```csharp
public sealed class UserErrors : ErrorCategory
{
    public static readonly ErrorCode NotFound = new("USER_001", "User not found");
    public static readonly ErrorCode InvalidEmail = new("USER_002", "Invalid email format");
}
```

## Register services

```csharp
var services = new ServiceCollection();
services.AddErrorKit(options =>
{
    options.Registry.RegisterCategory<UserErrors>();
});
```

## Return typed results

```csharp
public Result<User> GetUser(string id)
{
    var user = _repo.Find(id);
    if (user is null)
    {
        return AppError.From(UserErrors.NotFound)
            .WithContext("id", id)
            .WithMetadata("traceId", _traceAccessor.TraceId);
    }

    return user;
}
```

## Publish errors to observers

```csharp
public sealed class ConsoleObserver : IErrorObserver
{
    public void OnError(IError errorInfo)
    {
        Console.WriteLine($"{errorInfo.Code.Value}: {errorInfo.Message}");
    }
}

var hub = provider.GetRequiredService<IErrorHub>();
hub.RegisterObserver(new ConsoleObserver());

var reporter = provider.GetRequiredService<IErrorReporter>();
reporter.Report(AppError.From(UserErrors.NotFound));
```

## Async observers

```csharp
public sealed class TelemetryObserver : IAsyncErrorObserver
{
    public ValueTask OnErrorAsync(IError errorInfo, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }
}

hub.RegisterAsyncObserver(new TelemetryObserver());
await reporter.ReportAsync(AppError.From(UserErrors.InvalidEmail), cancellationToken);
```

## Problem Details JSON

```csharp
var error = AppError.From(UserErrors.NotFound);
var details = ErrorProblemDetails.FromError(error, status: 404);
var json = ErrorProblemDetailsJson.Serialize(details);
```

## Exception bridge

```csharp
var bridge = provider.GetRequiredService<IErrorExceptionBridge>();
throw bridge.ToException(AppError.From(UserErrors.NotFound));
```
