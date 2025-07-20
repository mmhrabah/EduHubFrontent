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
using Rabah.CustomizedComponents;

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
        private ContentViewerManager contentViewerManager;

        #endregion

        #region Overrides

        public override void SetupLayout()
        {
            base.SetupLayout();
            UIManager.Instance.ResetWindowsRectData();
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(0);
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
            Session.ContentTypes = response.Data.contentTypes;
            Session.Categories = response.Data.categories;
            totalContentItemsText.text = response.Data.totalContentItems.ToString();
            activeSubscriptionsText.text = response.Data.activeSubscriptions.ToString();
            contentAddedThisMonthText.text = response.Data.contentAddedThisMonth.ToString();
            contentViewerManager.ShowContentDetails(response.Data.recentlyAddedContent, (c) =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
            });
            foreach (var contentType in response.Data.contentTypes)
            {
                print($"Content Type: {contentType.Name}, ID: {contentType.Id}");
            }
            foreach (var category in response.Data.categories)
            {
                print($"Category: {category.Name}, ID: {category.Id}");
            }
        }

        #endregion
    }
}