using System;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

namespace Rabah.UI.AdvancedComponents
{
    public class CalendarPickerManager : Utils.UI.UIElement
    {
        [SerializeField]
        private List<CalendarDayButton> calendarDayButtons;
        [SerializeField]
        private TMP_Text monthText;
        [SerializeField]
        private ButtonManager previousButton;
        [SerializeField]
        private ButtonManager nextButton;
        [SerializeField]
        private ButtonManager doneButton;

        private DateTime selectedDate;
        private DateTime currentMonthDate;

        public Action<DateTime> OnSetDate;
        public Action<DateTime> OnSelectDate;

        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                print("Selected Date: " + selectedDate);
            }
        }




        private void Awake()
        {
            nextButton.onClick.AddListener(NextMonth);
            previousButton.onClick.AddListener(PreviousMonth);
            doneButton.onClick.AddListener(() =>
            {
                OnSelectDate?.Invoke(SelectedDate);
                gameObject.SetActive(false);
            });
        }


        public override T GetElementDataClassType<T>()
        {
            throw new NotImplementedException();
        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(DateTime))
            {
                return (T)Convert.ChangeType(SelectedDate, typeof(T));
            }
            return default;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Action onCheck)
        {
            throw new NotImplementedException();
        }

        public override void ResetElement()
        {
            SelectedDate = DateTime.Today;
        }

        public void OpenCalendar(DateTime start, DateTime end, DateTime selected)
        {
            SelectedDate = selected;
            currentMonthDate = selected;
            SetCalendarDataByYearRange(selected, start, end);
        }

        [ContextMenu("Set Calendar By Today Data")]
        public void SetCalendarDataByToday()
        {
            SelectedDate = DateTime.Today;
            currentMonthDate = DateTime.Today;
            SetCalendarDataByYear(DateTime.Today, 2025);
        }

        public void SetCalendarDataByYear(DateTime targetDate, int year)
        {
            TimeScheduleWeeksManager timeScheduleWeeksManager = new(year);
            List<TimeScheduleWeek> timeScheduleWeeks = timeScheduleWeeksManager.Weeks;
            int targetMonthNumber = targetDate.Month;
            var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            var weekIndex = timeScheduleWeeksManager.Weeks.FindIndex(x => x.Days.Contains(firstDayOfMonth));
            monthText.text = firstDayOfMonth.ToString("MMM yyyy");
            int dayIndex = 0;
            int finalDayOfWeek = dayIndex + 7;
            int maxWeekIndex = Math.Min(weekIndex + 6, timeScheduleWeeks.Count);
            for (int i = weekIndex; i < maxWeekIndex; i++)
            {
                var week = timeScheduleWeeks[i];
                for (; dayIndex < finalDayOfWeek; dayIndex++)
                {
                    var day = week.Days[dayIndex % 7];
                    calendarDayButtons[dayIndex].SetDay(day, SelectedDate, targetMonthNumber);
                }
                finalDayOfWeek += 7;
            }
            OnSetDate?.Invoke(targetDate);
        }

        public void SetCalendarDataByYearRange(DateTime targetDate, DateTime start, DateTime end)
        {
            TimeScheduleWeeksManager timeScheduleWeeksManager = new(start, end);
            List<TimeScheduleWeek> timeScheduleWeeks = timeScheduleWeeksManager.Weeks;
            int targetMonthNumber = targetDate.Month;
            var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            var weekIndex = timeScheduleWeeksManager.Weeks.FindIndex(x => x.Days.Contains(firstDayOfMonth));
            monthText.text = firstDayOfMonth.ToString("MMM yyyy");
            int dayIndex = 0;
            int finalDayOfWeek = dayIndex + 7;
            int maxWeekIndex = Math.Min(weekIndex + 6, timeScheduleWeeks.Count);
            for (int i = weekIndex; i < maxWeekIndex; i++)
            {
                var week = timeScheduleWeeks[i];
                for (; dayIndex < finalDayOfWeek; dayIndex++)
                {
                    var day = week.Days[dayIndex % 7];
                    calendarDayButtons[dayIndex].SetDay(day, SelectedDate, targetMonthNumber);
                }
                finalDayOfWeek += 7;
            }
            OnSetDate?.Invoke(targetDate);
        }

        public void NextMonth()
        {
            currentMonthDate = currentMonthDate.AddMonths(1);
            SetCalendarDataByYear(currentMonthDate, currentMonthDate.Year);
            OnSetDate += (date) =>
            {
                bool isFirstMonth = date.Month == 1;
                bool isLastMonth = date.Month == 12;
                previousButton.isInteractable = !isFirstMonth;
                nextButton.isInteractable = !isLastMonth;
            };
        }

        public void PreviousMonth()
        {
            currentMonthDate = currentMonthDate.AddMonths(-1);
            SetCalendarDataByYear(currentMonthDate, currentMonthDate.Year);
            OnSetDate += (date) =>
            {
                bool isFirstMonth = date.Month == 1;
                bool isLastMonth = date.Month == 12;
                previousButton.isInteractable = !isFirstMonth;
                nextButton.isInteractable = !isLastMonth;
            };
        }
    }
}