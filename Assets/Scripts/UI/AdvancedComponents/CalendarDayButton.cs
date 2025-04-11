using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rabah.UI.AdvancedComponents
{
    public class CalendarDayButton : Utils.UI.UIElement
    {
        [SerializeField]
        private Toggle toggle;
        [SerializeField]
        private TMP_Text dayText;
        [SerializeField]
        private CalendarPickerManager calendarPickerManager;

        private DateTime calendarDate;
        public override T GetElementDataClassType<T>()
        {
            throw new NotImplementedException();
        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(DateTime))
            {
                return (T)Convert.ChangeType(calendarDate, typeof(T));
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
            toggle.isOn = false;
        }

        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool arg0)
        {
            if (arg0)
            {
                calendarPickerManager.SelectedDate = calendarDate;
            }
        }

        public void SetDay(DateTime date, DateTime selectedDate, int month)
        {
            calendarDate = date;
            dayText.text = date.Day.ToString();
            toggle.isOn = date == selectedDate;
            toggle.interactable = date.Month == month;
        }
    }
}