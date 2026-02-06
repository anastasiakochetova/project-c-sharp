using System;

namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        const int totalDaysInMonth = 31;
        var birthsCountByDay = new double[totalDaysInMonth];
        
        CountBirthsByDay(names, name, birthsCountByDay);
        
        var dayLabels = CreateDayLabels(totalDaysInMonth);
        string chartTitle = CreateChartTitle(name);
        
        return new HistogramData(
            title: chartTitle,
            xLabels: dayLabels,
            yValues: birthsCountByDay);
    }
    
    private static void CountBirthsByDay(NameData[] names, string targetName, double[] birthsCount)
    {
        foreach (var person in names)
        {
            if (!IsPersonWithTargetName(person, targetName))
                continue;
                
            int birthDay = person.BirthDate.Day;
            
            if (ShouldSkipBirthDay(birthDay))
                continue;
            
            IncrementBirthDayCount(birthsCount, birthDay);
        }
    }
    
    private static bool IsPersonWithTargetName(NameData person, string targetName)
    {
        return person.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase);
    }
    
    private static bool ShouldSkipBirthDay(int birthDay)
    {
        return birthDay == 1;
    }
    
    private static void IncrementBirthDayCount(double[] birthsCount, int birthDay)
    {
        int arrayIndex = birthDay - 1;
        birthsCount[arrayIndex] += 1.0;
    }
    
    private static string[] CreateDayLabels(int daysCount)
    {
        var labels = new string[daysCount];
        
        for (int dayNumber = 0; dayNumber < daysCount; dayNumber++)
        {
            labels[dayNumber] = FormatDayNumber(dayNumber + 1);
        }
        
        return labels;
    }
    
    private static string FormatDayNumber(int day)
    {
        return day.ToString("D");
    }
    
    private static string CreateChartTitle(string name)
    {
        return $"Распределение рождений по дням месяца для имени: {name}";
    }
}
