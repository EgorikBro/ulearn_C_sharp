using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking;

public class Benchmark : IBenchmark
{
    public double MeasureDurationInMs(ITask task, int repetitionCount)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        task.Run();

        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < repetitionCount; i++)
        {
            task.Run();
        }
        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMilliseconds / repetitionCount;
    }
}

[TestFixture]
public class RealBenchmarkUsageSample
{
    [Test]
    public void StringConstructorFasterThanStringBuilder()
    {
        var benchmark = new Benchmark();
        var sbTask = new StringBuilderTask();
        var strTask = new StringConstructorTask();

        var repetitionCount = 10000;

        var sbDuration = benchmark.MeasureDurationInMs(sbTask, repetitionCount);
        var strDuration = benchmark.MeasureDurationInMs(strTask, repetitionCount);

        Assert.That(strDuration, Is.LessThan(sbDuration));
    }
}

public class StringBuilderTask : ITask
{
    public void Run()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 10000; i++)
            sb.Append('a');
        sb.ToString();
    }
}

public class StringConstructorTask : ITask
{
    public void Run()
    {
        new string('a', 10000);
    }
}
