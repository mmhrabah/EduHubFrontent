using TMPro;
using UnityEngine;

namespace Rabah.Screens
{

    public class DashboardContentItemDetails : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleText;
        [SerializeField]
        private TMP_Text typeText;
        [SerializeField]
        private TMP_Text versionText;


        public void SetContentItemDetails(string title, string type, string version)
        {
            titleText.text = title;
            typeText.text = type;
            versionText.text = version;
        }
    }
}
