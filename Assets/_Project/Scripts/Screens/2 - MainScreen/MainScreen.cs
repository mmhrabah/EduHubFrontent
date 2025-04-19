using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using TMPro;


namespace Rabah.Screens
{

    public class MainScreen : ScreenWithFetchDataOnOpen
    {
        [SerializeField]
        private TMP_Text welcomeText;

        private MainScreenData mainScreenData = new();

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            if (data is MainScreenData mainScreenData)
            {
                this.mainScreenData = mainScreenData;
                welcomeText.text = $"Welcome {mainScreenData.User.Username}";
            }
            else
            {
                Debug.LogError("Invalid data passed to MainScreen");
            }
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        protected override void FetchData()
        {

        }
    }
}