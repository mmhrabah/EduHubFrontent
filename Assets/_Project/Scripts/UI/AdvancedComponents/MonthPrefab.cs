using TMPro;
using UnityEngine;

namespace Rabah.UI.AdvancedComponents
{
    public class MonthPrefab : MonoBehaviour
    {
        private string monthName;
        private int monthNumber;
        [SerializeField] private TMP_Text monthText;

        public int MonthNumber { get => monthNumber; private set => monthNumber = value; }
        public string MonthName { get => monthName; private set => monthName = value; }

        public void SetPrefabData(string monthName, int monthNumber)
        {
            this.MonthName = monthName;
            this.MonthNumber = monthNumber;
            monthText.text = monthName;
        }
    }
}