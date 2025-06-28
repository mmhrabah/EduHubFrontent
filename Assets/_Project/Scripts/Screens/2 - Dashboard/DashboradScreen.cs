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
using Rabah.PrefabRaw;

namespace Rabah.Screens
{
    public class DashboradScreen : ScreenWithFetchDataOnOpen<ResponseModel<DashboardScreenResponse>, DashboardScreenResponse>
    {
        #region Private Fields

        [SerializeField]
        private Sprite warningIcon;
        [SerializeField]
        private TMP_Text totalContentItemsText;
        [SerializeField]
        private TMP_Text activeSubscriptionsText;
        [SerializeField]
        private TMP_Text contentAddedThisMonthText;
        [SerializeField]
        private List<ContentPrefabDataRaw> contentItemsDetails;

        #endregion

        #region Overrides

        public override void SetupLayout()
        {
            base.SetupLayout();
            UIManager.Instance.ResetWindowsRectData();
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(0);
            ResetRecentlyContentItems();
        }

        public override bool IsScreenDataValid()
        {
            return true;
        }

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
            ResetRecentlyContentItems();
            totalContentItemsText.text = response.Data.totalContentItems.ToString();
            activeSubscriptionsText.text = response.Data.activeSubscriptions.ToString();
            contentAddedThisMonthText.text = response.Data.contentAddedThisMonth.ToString();
            for (int i = 0; i < contentItemsDetails.Count; i++)
            {
                if (i < response.Data.recentlyAddedContent.Count)
                {
                    var contentItem = response.Data.recentlyAddedContent[i];
                    contentItemsDetails[i].SetContentItemDetails(contentItem, (c) => { FetchData(); });
                    contentItemsDetails[i].gameObject.SetActive(true);
                }
                else
                {
                    contentItemsDetails[i].gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Private Methods

        private void ResetRecentlyContentItems()
        {
            foreach (var contentItemDetails in contentItemsDetails)
            {
                contentItemDetails.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}