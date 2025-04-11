using System;
using System.Collections.Generic;

namespace Rabah.UI.AdvancedComponents
{
    public class TimeScheduleWeeksManager
    {
        public List<TimeScheduleWeek> Weeks { get; }

        public TimeScheduleWeeksManager(int year)
        {
            Weeks = TimeScheduleWeek.GetWeeksOfYear(year);
        }

        public TimeScheduleWeeksManager(DateTime start, DateTime end)
        {
            Weeks = TimeScheduleWeek.GetWeeksOfYearRange(start, end);
        }
    }

    public class TimeScheduleWeek
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public List<DateTime> Days { get; }
        public TimeScheduleWeek(DateTime start)
        {
            Start = start;
            End = start.AddDays(6); // Friday is the last day of the week
            Days = new List<DateTime>();

            for (DateTime day = Start; day <= End; day = day.AddDays(1))
            {
                Days.Add(day);
            }
        }

        public override string ToString()
        {
            return $"{Start:dd MMMM yyyy} - {End:dd MMMM yyyy}";
        }

        public static List<TimeScheduleWeek> GetWeeksOfYear(int year)
        {
            List<TimeScheduleWeek> weeks = new List<TimeScheduleWeek>();

            // Find the first Saturday before or on Jan 1st
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime firstWeekStart = GetFirstDayOfWeek(firstDay);

            // Start iterating weeks from first found Saturday
            DateTime weekStart = firstWeekStart;
            while (weekStart.Year < year + 1) // Continue until the next year's first week
            {
                weeks.Add(new TimeScheduleWeek(weekStart));
                weekStart = weekStart.AddDays(7); // Move to the next week's Saturday
            }

            return weeks;
        }

        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            // Find the nearest past or same Saturday
            int daysToSubtract = (int)date.DayOfWeek - (int)DayOfWeek.Saturday;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }
            return date.AddDays(-daysToSubtract);
        }

        public static List<TimeScheduleWeek> GetWeeksOfYearRange(DateTime start, DateTime end)
        {
            List<TimeScheduleWeek> weeks = new List<TimeScheduleWeek>();

            DateTime weekStart = GetFirstDayOfWeek(start);
            while (weekStart <= end)
            {
                weeks.Add(new TimeScheduleWeek(weekStart));
                weekStart = weekStart.AddDays(7);
            }

            return weeks;
        }
    }
}