using System;

namespace Names
{
    internal static class HeatmapTask
    {
        private static string[] CreateLabels(int from, int count)
        {
            var labels = new string[count];
            for (int index = 0; index < count; index++)
                labels[index] = (from + index).ToString();
            return labels;
        }

        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] people)
        {
            var monthLabels = CreateLabels(1, 12);
            var dayLabels = CreateLabels(2, 30);

            var birthCount = new double[30, 12];

            foreach (var person in people)
            {
				if (person.BirthDate.Day > 1 && person.BirthDate.Day <= 31 && person.BirthDate.Month <= 12)
                	birthCount[person.BirthDate.Day - 2, person.BirthDate.Month - 1] += 1;
            }

            return new HeatmapData(
                "Интенсивность рождений по дням и месяцам",
                birthCount,
                dayLabels,
                monthLabels
            );
        }
    }
}
