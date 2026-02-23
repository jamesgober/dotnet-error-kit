# API Reference

## Error codes

### Defining codes

```csharp
public sealed class UserErrors : ErrorCategory
{
    public static readonly ErrorCode NotFound = new("USER_001", "User not found");
    public static readonly ErrorCode InvalidEmail = new("USER_002", "Invalid email");
}
```

### Registering codes

```csharp
var registry = new ErrorCodeRegistry();
registry.RegisterCategory<UserErrors>();
```

### Async registry access

```csharp
var registry = new ErrorCodeRegistry();
await registry.TryRegisterAsync(UserErrors.NotFound, cancellationToken);
```

## Errors

### Creating errors

```csharp
var error = AppError.From(UserErrors.NotFound)
    .WithContext("id", userId);
```

### Error context

```csharp
var error = AppError.From(UserErrors.InvalidEmail)
    .WithContext(new ErrorContext("email", address));
```

### Metadata payload

```csharp
var error = AppError.From(UserErrors.InvalidEmail)
    .WithMetadata("traceId", traceId);
```

## Result pattern

### Success and failure

```csharp
Result<User> result = user;
Result failed = AppError.From(UserErrors.NotFound);
```

### Throw on failure

```csharp
result.ThrowIfFailure();
```

## Problem Details

### Mapping to RFC 7807

```csharp
var details = ErrorProblemDetails.FromError(error, status: 400);
```

### JSON serialization

```csharp
var json = ErrorProblemDetailsJson.Serialize(details);
var roundTrip = ErrorProblemDetailsJson.Deserialize(json);
```

## Error reporting

### Publishing errors to observers

```csharp
services.AddErrorKit();

var hub = provider.GetRequiredService<IErrorHub>();
hub.RegisterObserver(new ConsoleObserver());

var reporter = provider.GetRequiredService<IErrorReporter>();
reporter.Report(error);
```

### Publishing errors to async observers

```csharp
services.AddErrorKit();

var hub = provider.GetRequiredService<IErrorHub>();
hub.RegisterAsyncObserver(new TelemetryObserver());

var reporter = provider.GetRequiredService<IErrorReporter>();
await reporter.ReportAsync(error, cancellationToken);
```

## Exception bridge

### Converting errors to exceptions

```csharp
var bridge = provider.GetRequiredService<IErrorExceptionBridge>();
throw bridge.ToException(error);
```

### Converting exceptions to errors

```csharp
var bridge = provider.GetRequiredService<IErrorExceptionBridge>();
var error = bridge.FromException(exception);
```

## Async error factory

### Creating errors asynchronously

```csharp
var factory = provider.GetRequiredService<IAsyncErrorFactory>();
var error = await factory.CreateAsync(UserErrors.NotFound, cancellationToken: cancellationToken);
