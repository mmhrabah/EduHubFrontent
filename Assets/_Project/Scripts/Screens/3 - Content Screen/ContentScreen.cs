using System.Collections.Generic;
using Rabah.CustomizedComponents;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
using UIManager = Rabah.Utils.UI.UIManager;
namespace Rabah.Screens
{
    public class ContentScreen : ScreenWithFetchDataOnOpen<ResponseModel<List<Content>>, List<Content>>
    {
        [SerializeField]
        private ContentViewerManager contentViewerManager;
        [SerializeField]
        private RectTransform addNewContentButtonPanel;

        public override void SetupLayout()
        {
            base.SetupLayout();
            UIManager.Instance.ResetWindowsRectData();
            UIManager.Instance.AddItemsToTopPanel(addNewContentButtonPanel, 4);
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(1);
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        protected override void OnDataFetched(ResponseModel<List<Content>> response)
        {
            contentViewerManager.ShowContentDetails(response.Data, (c) =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
            });
            foreach (var content in response.Data)
            {
                print($"Content ID: {content.Id}, Name: {content.Name}");
            }
        }

        protected override void OnErrorReceived(string error)
        {
            Debug.LogError($"Error fetching content: {error}");
            UIManager.Instance.ShowNotificationModal(
                title: "Error",
                descriptionText: "Internal server error, please try again later.",
                icon: null);
        }

        public override void OnClose()
        {
            base.OnClose();
            addNewContentButtonPanel.transform.parent = transform;
        }
    }
}