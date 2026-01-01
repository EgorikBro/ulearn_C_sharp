using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
    public static ChartData BuildChartDataForArrayCreation(
        IBenchmark benchmark, int repetitionsCount)
    {
        return BuildChartData(benchmark, repetitionsCount, new ArrayCreationTaskFactory(), "Create array");
    }

    public static ChartData BuildChartDataForMethodCall(
        IBenchmark benchmark, int repetitionsCount)
    {
        return BuildChartData(benchmark, repetitionsCount, new MethodCallTaskFactory(), "Call method with argument");
    }

    private static ChartData BuildChartData(IBenchmark benchmark, int repetitionsCount, ITaskFactory factory, string title)
    {
        var classesTimes = new List<ExperimentResult>();
        var structuresTimes = new List<ExperimentResult>();

        foreach (var size in Constants.FieldCounts)
        {
            var structTask = factory.CreateStructTask(size);
            var structTime = benchmark.MeasureDurationInMs(structTask, repetitionsCount);
            structuresTimes.Add(new ExperimentResult(size, structTime));

            var classTask = factory.CreateClassTask(size);
            var classTime = benchmark.MeasureDurationInMs(classTask, repetitionsCount);
            classesTimes.Add(new ExperimentResult(size, classTime));
        }

        return new ChartData
        {
            Title = title,
            ClassPoints = classesTimes,
            StructPoints = structuresTimes,
        };
    }

    private interface ITaskFactory
    {
        ITask CreateStructTask(int size);
        ITask CreateClassTask(int size);
    }

    private class ArrayCreationTaskFactory : ITaskFactory
    {
        public ITask CreateStructTask(int size) => new StructArrayCreationTask(size);
        public ITask CreateClassTask(int size) => new ClassArrayCreationTask(size);
    }

    private class MethodCallTaskFactory : ITaskFactory
    {
        public ITask CreateStructTask(int size) => new MethodCallWithStructArgumentTask(size);
        public ITask CreateClassTask(int size) => new MethodCallWithClassArgumentTask(size);
    }
}
