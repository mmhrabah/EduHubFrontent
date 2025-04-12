using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rabah.UI.AdvancedComponents
{
    public class Month
    {
        public string Name { get; }
        public List<Week> Weeks { get; }

        public Month(int year, int month)
        {
            Name = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month);
            Weeks = new List<Week>();

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int weekNumber = 1;
            List<Day> currentWeekDays = new List<Day>();

            for (int day = 1; day <= lastDayOfMonth.Day; day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                Day dayObject = new Day(currentDate.DayOfWeek, day);
                currentWeekDays.Add(dayObject);

                // If Sunday or last day of month, finalize the week
                if (currentDate.DayOfWeek == DayOfWeek.Sunday || day == lastDayOfMonth.Day)
                {
                    int weekNumberOfYear = GetWeekNumberOfYear(currentDate);
                    Weeks.Add(new Week(month, weekNumber, weekNumberOfYear, new List<Day>(currentWeekDays)));
                    currentWeekDays.Clear();
                    weekNumber++;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine($"\n  {Name}");
            foreach (var week in Weeks)
            {
                week.Print();
            }
        }

        private int GetWeekNumberOfYear(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}