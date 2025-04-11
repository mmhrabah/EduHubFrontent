using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class WeekPrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text weekStartDayText;
    [SerializeField]
    private List<TMP_Text> weekDaysText;

    public void SetWeek(string startDay, List<DateTime> weekDays)
    {
        weekStartDayText.text = startDay;
        for (int i = 0; i < weekDays.Count; i++)
        {
            var dayName = weekDays[i].ToString("ddd.", CultureInfo.InvariantCulture);
            var dayChar = dayName[0].ToString();
            weekDaysText[i].text = dayChar;
        }
    }
}
