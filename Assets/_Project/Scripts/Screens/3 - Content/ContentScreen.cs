using System.Collections.Generic;
using Rabah.CustomizedComponents;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
namespace Rabah.Screens
{
    public class ContentScreen : ScreenWithFetchDataOnOpen<ResponseModel<List<Content>>, List<Content>>
    {
        [SerializeField]
        private ContentViewerManager contentViewerManager;

        public override void SetupLayout()
        {
            base.SetupLayout();
            UIManager.Instance.ResetWindowsRectData();
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(1);
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        protected override void OnDataFetched(ResponseModel<List<Content>> response)
        {
            contentViewerManager.ShowContentDetails(response.Data, (content) =>
            {
                // Handle content deletion success
                APIManager.Instance.Delete<string>($"content/{content.Id}",
                    onSuccess: (deleteResponse) =>
                    {
                        Debug.Log("Content deleted successfully.");
                        contentViewerManager.ShowContentDetails(response.Data, null);
                    },
                    onFailure: (error) =>
                    {
                        Debug.LogError($"Failed to delete content: {error}");
                        UIManager.Instance.ShowNotificationModal(
                            title: "Error",
                            descriptionText: error,
                            icon: null);
                    });
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
    }
}