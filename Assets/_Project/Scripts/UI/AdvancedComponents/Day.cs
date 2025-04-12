using System;

namespace Rabah.UI.AdvancedComponents
{
    public class Day
    {
        public string DayName { get; }
        public int DayNumber { get; }

        public Day(DayOfWeek dayOfWeek, int dayNumber)
        {
            DayName = dayOfWeek.ToString();
            DayNumber = dayNumber;
        }

        public void Print()
        {
            UnityEngine.Debug.Log($"     {DayName} {DayNumber}");
        }
    }
}