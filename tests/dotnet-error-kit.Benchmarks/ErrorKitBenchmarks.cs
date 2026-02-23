using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using DotNetErrorKit.Codes;
using DotNetErrorKit.ProblemDetails;
using DotNetErrorKit.Results;

namespace DotNetErrorKit.Benchmarks;

[MemoryDiagnoser]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "BenchmarkDotNet requires instance methods.")]
public sealed class ErrorKitBenchmarks
{
    private static readonly ErrorCode SampleCode = new("BENCH_001", "Benchmark error");
    private static readonly AppError SampleError = AppError.From(SampleCode).WithMetadata("traceId", "bench");

    [Benchmark]
    public Result CreateFailureResult() => Result.Failure(SampleError);

    [Benchmark]
    public ErrorProblemDetails CreateProblemDetails() => ErrorProblemDetails.FromError(SampleError);

    [Benchmark]
    public string SerializeProblemDetails()
    {
        var details = ErrorProblemDetails.FromError(SampleError);
        return ErrorProblemDetailsJson.Serialize(details);
    }
}
