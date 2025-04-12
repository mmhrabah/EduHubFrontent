using UnityEngine;
using System;
using Rabah.Utils;
using System.Collections.Generic;

namespace Rabah.UI.AdvancedComponents
{
    public class TimeScheduleManager : BaseSingleton<TimeScheduleManager>
    {
        [SerializeField]
        private List<WeekPrefab> weekPrefabs;

        private readonly Month[] months = new Month[12];

        public Month[] Months => months;

        protected override void Awake()
        {
            base.Awake();
        }


        public Month GetMonth(int monthNumber)
        {
            return Months[monthNumber];
        }

        public Month GetMonth(string monthName)
        {
            return Array.Find(Months, month => month.Name == monthName);
        }

        public int GetMonthNumber(string monthName)
        {
            return Array.FindIndex(Months, month => month.Name == monthName);
        }

        public void ViewWeeks(int startWeekIndex)
        {
            TimeScheduleWeeksManager timeScheduleWeeksManager = new TimeScheduleWeeksManager(2025);
            for (int i = 0; i < weekPrefabs.Count; i++)
            {
                int weekIndex = Mathf.Min(i + startWeekIndex, timeScheduleWeeksManager.Weeks.Count - 1);
                var week = timeScheduleWeeksManager.Weeks[weekIndex];
                var startDay = week.Start.ToString("dd MMM");
                weekPrefabs[i].SetWeek(startDay, week.Days);
            }
        }
    }
}