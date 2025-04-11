using System.Collections.Generic;

namespace Rabah.UI.AdvancedComponents
{
    public class Week
    {
        public int month;
        public int WeekNumber { get; }
        public int WeekNumberOfYear { get; } // New attribute
        public List<Day> Days { get; }

        public Week(int month, int weekNumber, int weekNumberOfYear, List<Day> days)
        {
            this.month = month;
            WeekNumber = weekNumber;
            WeekNumberOfYear = weekNumberOfYear;
            Days = days;
        }

        public void Print()
        {
            UnityEngine.Debug.Log($"   Week {WeekNumber} (Week {WeekNumberOfYear} of the Year)");
            foreach (var day in Days)
            {
                day.Print();
            }
        }
    }
}