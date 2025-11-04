using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string targetName)
        {
            // Создаём подписи для оси X (1–31)
            var days = Enumerable.Range(1, 31)
                                 .Select(day => day.ToString())
                                 .ToArray();

            // Подсчёт количества рождений по дням месяца
            var counts = new double[31];
            foreach (var record in names)
            {
                var day = record.BirthDate.Day;
                if (record.Name == targetName && day != 1)
                    counts[day - 1]++;
            }

            var title = $"Рождаемость людей с именем {targetName} по дням месяца";
            return new HistogramData(title, days, counts);
        }
    }
}
