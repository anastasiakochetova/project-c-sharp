using System.Collections.Generic;

namespace StructBenchmarking;

//Абстрактная фабрика для создания задач тестирования.
//Определяет интерфейс для создания связанных семейств объектов
public interface ITaskFactory
{
	//Создает задачу для тестирования с классом   
    //Создает задачу для тестирования со структурой  
    //Возвращает заголовок для графика эксперимента
    ITask CreateClassTask(int fieldCount);
    ITask CreateStructTask(int fieldCount);
    string Title { get; }
}

//Создает задачи, измеряющие время создания массивов классов и структур.

public class ArrayCreationTaskFactory : ITaskFactory
{
    public ITask CreateClassTask(int fieldCount) => new ClassArrayCreationTask(fieldCount);
    public ITask CreateStructTask(int fieldCount) => new StructArrayCreationTask(fieldCount);
    public string Title => "Create array";
}


//Создает задачи, измеряющие время вызова методов с параметрами-классами и параметрами-структурами.

public class MethodCallTaskFactory : ITaskFactory
{
    public ITask CreateClassTask(int fieldCount) => new MethodCallWithClassArgumentTask(fieldCount);
    public ITask CreateStructTask(int fieldCount) => new MethodCallWithStructArgumentTask(fieldCount);
    public string Title => "Call method with argument";
}


//Контейнер для хранения пар результатов эксперимента.
//Инкапсулирует результаты тестирования для классов и структур.

public class ChartDataPair
{
    public List<ExperimentResult> ClassResults { get; }
    public List<ExperimentResult> StructResults { get; }
    public string Title { get; }

    public ChartDataPair(
        List<ExperimentResult> classResults, 
        List<ExperimentResult> structResults,
        string title)
    {
        ClassResults = classResults;
        StructResults = structResults;
        Title = title;
    }
}


/// Реализует паттерн "Абстрактная фабрика" для устранения дублирования кода.

public class Experiments
{
    public static ChartData BuildChartDataForArrayCreation(
        IBenchmark benchmark, int repetitionsCount)
    {
        var factory = new ArrayCreationTaskFactory();
        var pair = RunExperiment(factory, benchmark, repetitionsCount);
        
        return new ChartData
        {
            Title = pair.Title,
            ClassPoints = pair.ClassResults,
            StructPoints = pair.StructResults,
        };
    }

    public static ChartData BuildChartDataForMethodCall(
        IBenchmark benchmark, int repetitionsCount)
    {
        var factory = new MethodCallTaskFactory();
        var pair = RunExperiment(factory, benchmark, repetitionsCount);
        
        return new ChartData
        {
            Title = pair.Title,
            ClassPoints = pair.ClassResults,
            StructPoints = pair.StructResults,
        };
    }

    //Выполняет эксперимент с использованием заданной фабрики.
    //Общий метод, который устраняет дублирование кода в двух экспериментах.

    private static ChartDataPair RunExperiment(
        ITaskFactory factory, IBenchmark benchmark, int repetitionsCount)
    {
        var classResults = new List<ExperimentResult>();
        var structResults = new List<ExperimentResult>();

        //Проходим по всем заданным размерам структур/классов
        foreach (var fieldCount in Constants.FieldCounts)
        {
            //Создаем задачи через фабрику
            var classTask = factory.CreateClassTask(fieldCount);
            var structTask = factory.CreateStructTask(fieldCount);
            var classTime = benchmark.MeasureDurationInMs(classTask, repetitionsCount);
            var structTime = benchmark.MeasureDurationInMs(structTask, repetitionsCount);
            classResults.Add(new ExperimentResult(fieldCount, classTime));
            structResults.Add(new ExperimentResult(fieldCount, structTime));
        }

        return new ChartDataPair(classResults, structResults, factory.Title);
    }
}
