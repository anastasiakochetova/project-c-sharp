// Вставьте сюда финальное содержимое файла BenchmarkTask.cs

using System;
using System.Collections.Generic;
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
        for (int i = 0; i < repetitionCount; i++)
        {
            task.Run();
        }
        stopwatch.Stop();
        
        return stopwatch.ElapsedMilliseconds / (double)repetitionCount;
    }
}

public class StringBuilderTask : ITask
{
    public void Run()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 10000; i++)
        {
            sb.Append('a');
        }
        string result = sb.ToString();
    }
}

public class StringConstructorTask : ITask
{
    public void Run()
    {
        string result = new string('a', 10000);
    }
}

[TestFixture]
public class RealBenchmarkUsageSample
{
    [Test]
    public void StringConstructorFasterThanStringBuilder()
	{	
        var benchmark = new Benchmark();
        int repetitionsCount = 1000;
        
        var stringBuilderTask = new StringBuilderTask();
        var stringConstructorTask = new StringConstructorTask();
        
        var stringBuilderTime = benchmark.MeasureDurationInMs(stringBuilderTask, repetitionsCount);
        var stringConstructorTime = benchmark.MeasureDurationInMs(stringConstructorTask, repetitionsCount);
        
        Console.WriteLine($"StringBuilder: {stringBuilderTime} ms");
        Console.WriteLine($"String constructor: {stringConstructorTime} ms");
        
        Assert.That(stringConstructorTime, Is.LessThan(stringBuilderTime), 
            "String constructor should be faster than StringBuilder for repeating characters");
    }
}
