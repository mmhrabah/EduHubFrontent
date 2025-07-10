using System.Collections.Generic;
using Rabah.CustomizedComponents;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;

namespace Rabah.Screens
{
    public class ClientsScreen : ScreenWithFetchDataOnOpen<ResponseModel<List<Client>>, List<Client>>
    {
        [SerializeField]
        private ClientViewerManager clientViewerManager;
        [SerializeField]
        private RectTransform addNewClientButtonPanel;

        public override void SetupLayout()
        {
            base.SetupLayout();
            UIManager.Instance.ResetWindowsRectData();
            UIManager.Instance.AddItemsToTopPanel(addNewClientButtonPanel, 4);
            UIManager.Instance.LeftPanelButtonsManager.SelectButton(3);
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        protected override void OnDataFetched(ResponseModel<List<Client>> response)
        {
            clientViewerManager.ShowClientDetails(response.Data, (client) =>
            {
                // Handle client deletion success
                APIManager.Instance.Delete<string>($"client/{client.Id}",
                    onSuccess: (deleteResponse) =>
                    {
                        Debug.Log("client deleted successfully.");
                        clientViewerManager.ShowClientDetails(response.Data, null);
                    },
                    onFailure: (error) =>
                    {
                        Debug.LogError($"Failed to delete client: {error}");
                        UIManager.Instance.ShowNotificationModal(
                            title: "Error",
                            descriptionText: error,
                            icon: null);
                    });
            });
            foreach (var user in response.Data)
            {
                print($"User ID: {user.Id}, Name: {user.Name}");
            }
        }

        protected override void OnErrorReceived(string error)
        {
            Debug.LogError($"Error fetching user: {error}");
            UIManager.Instance.ShowNotificationModal(
                title: "Error",
                descriptionText: "Internal server error, please try again later.",
                icon: null);
        }

        public override void OnClose()
        {
            base.OnClose();
            addNewClientButtonPanel.transform.parent = transform;
        }
    }
}
