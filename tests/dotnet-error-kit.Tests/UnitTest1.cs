using System.Diagnostics.CodeAnalysis;
using DotNetErrorKit;
using DotNetErrorKit.Abstractions;
using DotNetErrorKit.Codes;
using DotNetErrorKit.Extensions;
using DotNetErrorKit.ProblemDetails;
using DotNetErrorKit.Results;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention." )]

namespace DotNetErrorKit.Tests;

public sealed class ErrorKitTests
{
    private static readonly ErrorCode SampleCode = new("TEST_001", "Test error");

    private sealed class SampleCategory : ErrorCategory
    {
        public static readonly ErrorCode CategoryCode = new("CAT_001", "Category error");
    }

    [Fact]
    public void ErrorCode_InvalidValue_ThrowsArgumentException()
    {
        var action = () => new ErrorCode(" ", "Description");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ErrorCode_InvalidDescription_ThrowsArgumentException()
    {
        var action = () => new ErrorCode("CODE_001", " ");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ErrorContext_InvalidKey_ThrowsArgumentException()
    {
        var action = () => new ErrorContext(" ", "value");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ErrorCodeRegistry_RegisterThenTryGet_ReturnsRegisteredCode()
    {
        var registry = new ErrorCodeRegistry();

        registry.Register(SampleCode);

        registry.TryGet(SampleCode.Value, out var resolved).Should().BeTrue();
        resolved.Should().BeSameAs(SampleCode);
    }

    [Fact]
    public void AppError_WithContext_AppendsContext()
    {
        var error = AppError.From(SampleCode)
            .WithContext("id", "123");

        error.Context.Should().ContainSingle();
        error.Context[0].Key.Should().Be("id");
        error.Context[0].Value.Should().Be("123");
    }

    [Fact]
    public void AppError_WithMetadata_AppendsMetadata()
    {
        var error = AppError.From(SampleCode)
            .WithMetadata("traceId", "abc");

        error.Metadata.Should().ContainKey("traceId");
        error.Metadata["traceId"].Should().Be("abc");
    }

    [Fact]
    public void AppError_WithMetadata_InvalidKey_ThrowsArgumentException()
    {
        var error = AppError.From(SampleCode);

        var action = () => error.WithMetadata(" ", "value");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AppError_WithMetadataDictionary_InvalidKey_ThrowsArgumentException()
    {
        var error = AppError.From(SampleCode);
        var metadata = new Dictionary<string, object?> { [""] = "value" };

        var action = () => error.WithMetadata(metadata);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Result_Failure_ThrowIfFailure_ThrowsErrorException()
    {
        var result = Result.Failure(AppError.From(SampleCode));

        var action = () => result.ThrowIfFailure();

        action.Should().Throw<ErrorException>()
            .Which.Error.Code.Should().Be(SampleCode);
    }

    [Fact]
    public void ResultOfT_ImplicitConversions_HandleSuccessAndFailure()
    {
        Result<string> success = "value";
        Result<string> failure = AppError.From(SampleCode);

        success.IsSuccess.Should().BeTrue();
        success.Value.Should().Be("value");
        failure.IsFailure.Should().BeTrue();
        failure.Error.Should().NotBeNull();
    }

    [Fact]
    public void ErrorProblemDetails_FromError_MapsExtensions()
    {
        var error = AppError.From(SampleCode)
            .WithContext("id", "123")
            .WithMetadata("traceId", "abc");

        var details = ErrorProblemDetails.FromError(error, status: 400);

        details.Type.Should().Be("about:blank");
        details.Status.Should().Be(400);
        details.Extensions.Should().ContainKey("code");
        details.Extensions["code"].Should().Be(SampleCode.Value);
        details.Extensions["severity"].Should().Be(error.Severity.ToString());
        details.Extensions["context"].Should().BeAssignableTo<KeyValuePair<string, string>[]>();
        details.Extensions["metadata"].Should().BeAssignableTo<IReadOnlyDictionary<string, object?>>();
    }

    [Fact]
    public void ErrorProblemDetailsJson_SerializeRoundTrip_ReturnsExpectedPayload()
    {
        var error = AppError.From(SampleCode).WithMetadata("traceId", "abc");
        var details = ErrorProblemDetails.FromError(error, status: 500);

        var json = ErrorProblemDetailsJson.Serialize(details);
        var deserialized = ErrorProblemDetailsJson.Deserialize(json);

        deserialized.Type.Should().Be(details.Type);
        deserialized.Status.Should().Be(details.Status);
        deserialized.Title.Should().Be(details.Title);
        deserialized.Detail.Should().Be(details.Detail);
    }

    [Fact]
    public void ErrorHub_Publish_NotifiesObservers()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var hub = provider.GetRequiredService<IErrorHub>();
        var observer = Substitute.For<IErrorObserver>();
        var error = AppError.From(SampleCode);

        hub.RegisterObserver(observer);
        hub.Publish(error);

        observer.Received(1).OnError(error);
        hub.ObserverCount.Should().Be(1);
    }

    [Fact]
    public void ErrorCodeRegistry_RegisterCategory_RegistersCategoryCodes()
    {
        var registry = new ErrorCodeRegistry();

        registry.RegisterCategory<SampleCategory>();

        registry.TryGet(SampleCategory.CategoryCode.Value, out var resolved).Should().BeTrue();
        resolved.Should().BeSameAs(SampleCategory.CategoryCode);
    }

    [Fact]
    public void ErrorReporter_Report_PublishesError()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var hub = provider.GetRequiredService<IErrorHub>();
        var reporter = provider.GetRequiredService<IErrorReporter>();
        var observer = Substitute.For<IErrorObserver>();
        var error = AppError.From(SampleCode);

        hub.RegisterObserver(observer);
        reporter.Report(error);

        observer.Received(1).OnError(error);
    }

    [Fact]
    public void ErrorExceptionBridge_FromException_UsesFallbackCode()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var bridge = provider.GetRequiredService<IErrorExceptionBridge>();
        var exception = new InvalidOperationException("Sensitive info");

        var error = bridge.FromException(exception);

        error.Code.Should().Be(SystemErrorCodes.UnhandledException);
        error.Context.Should().ContainSingle(context => context.Key == "exceptionType");
        error.Message.Should().Be(SystemErrorCodes.UnhandledException.Description);
    }

    [Fact]
    public async Task ErrorHub_PublishAsync_NotifiesAsyncObservers()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var hub = provider.GetRequiredService<IErrorHub>();
        var observer = Substitute.For<IAsyncErrorObserver>();
        var error = AppError.From(SampleCode);

        hub.RegisterAsyncObserver(observer);
        await hub.PublishAsync(error);

        await observer.Received(1).OnErrorAsync(error, Arg.Any<CancellationToken>());
        hub.AsyncObserverCount.Should().Be(1);
    }

    [Fact]
    public async Task ErrorReporter_ReportAsync_PublishesError()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var hub = provider.GetRequiredService<IErrorHub>();
        var reporter = provider.GetRequiredService<IErrorReporter>();
        var observer = Substitute.For<IAsyncErrorObserver>();
        var error = AppError.From(SampleCode);

        hub.RegisterAsyncObserver(observer);
        await reporter.ReportAsync(error);

        await observer.Received(1).OnErrorAsync(error, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ErrorRegistry_TryRegisterAsync_ReturnsTrue()
    {
        var registry = new ErrorCodeRegistry();

        var registered = await registry.TryRegisterAsync(SampleCode);
        var resolved = await registry.TryGetAsync(SampleCode.Value);

        registered.Should().BeTrue();
        resolved.Should().BeSameAs(SampleCode);
    }

    [Fact]
    public async Task ErrorFactory_CreateAsync_ReturnsError()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IAsyncErrorFactory>();

        var error = await factory.CreateAsync(SampleCode, message: "Failure");

        error.Message.Should().Be("Failure");
    }

    [Fact]
    public void AddErrorKit_RegistersCoreServices()
    {
        var services = new ServiceCollection();

        services.AddErrorKit();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IErrorRegistry>().Should().NotBeNull();
        provider.GetRequiredService<IAsyncErrorRegistry>().Should().NotBeNull();
        provider.GetRequiredService<IErrorHub>().Should().NotBeNull();
        provider.GetRequiredService<IErrorFactory>().Should().NotBeNull();
        provider.GetRequiredService<IAsyncErrorFactory>().Should().NotBeNull();
        provider.GetRequiredService<IErrorExceptionBridge>().Should().NotBeNull();
        provider.GetRequiredService<IErrorReporter>().Should().NotBeNull();
    }

    [Fact]
    public void ErrorProblemDetailsJson_Deserialize_InvalidJson_ThrowsArgumentException()
    {
        var action = () => ErrorProblemDetailsJson.Deserialize(" ");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ErrorProblemDetails_Constructor_InvalidType_ThrowsArgumentException()
    {
        var action = () => new ErrorProblemDetails(" ", "title", 400, "detail", null, new Dictionary<string, object?>());

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task ErrorRegistry_TryRegisterAsync_Canceled_ThrowsOperationCanceledException()
    {
        var registry = new ErrorCodeRegistry();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var action = async () => await registry.TryRegisterAsync(SampleCode, cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ErrorFactory_CreateAsync_Canceled_ThrowsOperationCanceledException()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IAsyncErrorFactory>();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var action = async () => await factory.CreateAsync(SampleCode, cancellationToken: cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ErrorHub_PublishAsync_Canceled_ThrowsOperationCanceledException()
    {
        var services = new ServiceCollection();
        services.AddErrorKit();
        using var provider = services.BuildServiceProvider();
        var hub = provider.GetRequiredService<IErrorHub>();
        var observer = Substitute.For<IAsyncErrorObserver>();
        var error = AppError.From(SampleCode);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        hub.RegisterAsyncObserver(observer);

        var action = async () => await hub.PublishAsync(error, cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }
}
