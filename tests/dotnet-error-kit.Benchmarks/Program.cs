using BenchmarkDotNet.Running;

namespace DotNetErrorKit.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ErrorKitBenchmarks>();
    }
}
