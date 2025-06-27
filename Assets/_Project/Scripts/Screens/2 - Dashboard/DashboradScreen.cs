using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using TMPro;
using DG.Tweening;
using Michsky.MUIP;
using System.Linq;
using Rabah.Utils.Session;
using UnityEngine.UI;


namespace Rabah.Screens
{

    public class DashboradScreen : ScreenWithFetchDataOnOpen<ResponseModel<DashboardScreenResponse>, DashboardScreenResponse>
    {
        [SerializeField]
        private Sprite warningIcon;

        private void Awake()
        {

        }

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(0);
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        #region UI Animations
        #endregion

        #region UI Setup

        protected override void OnErrorReceived(string error)
        {
            string errorMessage = $"An error occurred while fetching data. Error: {error}";
            Debug.LogError(errorMessage);
            UIManager.Instance.ShowNotificationModal(
                title: "Error",
                descriptionText: error,
                icon: warningIcon,
                iconColor: Color.red);
        }

        protected override void OnDataFetched(ResponseModel<DashboardScreenResponse> response)
        {
        }

        #endregion
    }
}