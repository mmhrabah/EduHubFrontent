using System;
using System.Linq;
using Michsky.MUIP;
using Rabah.UI.MainComponents;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
using UIManager = Rabah.Utils.UI.UIManager;



namespace Rabah.Screens
{
    public class EditClientScreen : ScreenSendDataToDatabase<EditClientRequest, ResponseModel<string>, string>
    {
        [SerializeField]
        private InputFieldUIElement nameInputField;
        [SerializeField]
        private InputFieldUIElement phoneUIElement;
        [SerializeField]
        private InputFieldUIElement startDateInputField;
        [SerializeField]
        private InputFieldUIElement endDateInputField;
        [SerializeField]
        private InputFieldUIElement macAdressesUIElement;
        [SerializeField]
        private ButtonManager cancelButton;

        EditClientScreenData editClientScreenData = new();
        protected override void Awake()
        {
            base.Awake();
            MustParse = false; // We don't need to parse the response in this case
        }
        public override void SetupLayout()
        {
            base.SetupLayout();
            var cancelButtonRectTransform = cancelButton.GetComponent<RectTransform>();
            UIManager.Instance.AddItemsToTopPanel(cancelButtonRectTransform, 4);
        }
        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            editClientScreenData = data as EditClientScreenData;
            if (editClientScreenData == null || editClientScreenData.Client == null)
            {
                Debug.LogError("EditClientScreenData or Client is null");
                return;
            }
            EndPoint = ScreenSetupData.mainEndpoint + editClientScreenData.Client.Id;
            HttpMethod = HttpMethod.PUT;
            nameInputField.InputField.text = editClientScreenData.Client.Name;
            phoneUIElement.InputField.text = editClientScreenData.Client.Phone;
            if (editClientScreenData.Client.SubscriptionStartDate.Year < 2000 || editClientScreenData.Client.SubscriptionEndDate.Year < 2000)
            {
                // Handle invalid dates
                startDateInputField.InputField.text = "N/A";
                endDateInputField.InputField.text = "N/A";
            }
            else
            {
                // Set valid dates
                startDateInputField.InputField.text = editClientScreenData.Client.SubscriptionStartDate.ToString("yyyy-MM-dd");
                endDateInputField.InputField.text = editClientScreenData.Client.SubscriptionEndDate.ToString("yyyy-MM-dd");
            }
            macAdressesUIElement.InputField.text = string.Join(",", editClientScreenData.Client.MacAddresses);
            UIManager.Instance.ResetWindowsRectData();
            cancelButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.UserSubscriptionsScreen);
            });

            onErrorReceived += (error) =>
            {
                UIManager.Instance.ShowNotificationModal(
                    title: "Error",
                    descriptionText: "Failed to add client: " + error,
                    icon: null);
                Debug.LogError($"Failed to add client: {error}");
            };
            onResponseReceived += (response) =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.UserSubscriptionsScreen);
            };
        }

        protected override EditClientRequest ExtractDataFromInputs()
        {
            return new EditClientRequest()
            {
                Name = nameInputField.GetElementDataClassType<string>(),
                Phone = phoneUIElement.GetElementDataClassType<string>(),
                SubscriptionStartDate = DateTime.Parse(startDateInputField.GetElementDataClassType<string>()),
                SubscriptionEndDate = DateTime.Parse(endDateInputField.GetElementDataClassType<string>()),
                MacAddresses = macAdressesUIElement.GetElementDataClassType<string>().Split(',').ToList()
            };
        }

        protected override void FillUIElementsInputs()
        {

        }

        public override void OnClose()
        {
            base.OnClose();
            var cancelButtonRectTransform = cancelButton.GetComponent<RectTransform>();
            cancelButtonRectTransform.transform.parent = transform;

        }
    }
}
