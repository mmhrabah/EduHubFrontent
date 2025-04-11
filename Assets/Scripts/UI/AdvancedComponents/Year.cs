using System.Collections.Generic;

namespace Rabah.UI.AdvancedComponents
{
    public class Year
    {
        public int YearNumber { get; }
        public List<Month> Months { get; private set; }

        public Year(int yearNumber)
        {
            YearNumber = yearNumber;
            Months = new List<Month>();

            for (int i = 1; i <= 12; i++)
            {
                Months.Add(new Month(yearNumber, i));
            }
        }

        public void Print()
        {
            UnityEngine.Debug.Log(YearNumber);
            foreach (var month in Months)
            {
                month.Print();
            }
        }
    }
}