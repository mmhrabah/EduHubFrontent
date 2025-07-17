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
    public class ApproveClientScreen : ScreenSendDataToDatabase<ApproveClientRequest, ResponseModel<string>, string>
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

        private ApproveClientScreenData approveClientScreenData = new();
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
            approveClientScreenData = data as ApproveClientScreenData;
            if (approveClientScreenData == null || approveClientScreenData.Client == null)
            {
                Debug.LogError("ApproveClientScreenData or Client is null");
                return;
            }

            nameInputField.InputField.text = approveClientScreenData.Client.Name;
            phoneUIElement.InputField.text = approveClientScreenData.Client.Phone;
            if (approveClientScreenData.Client.SubscriptionStartDate.Year < 2000 || approveClientScreenData.Client.SubscriptionEndDate.Year < 2000)
            {
                // Handle invalid dates
                startDateInputField.InputField.text = "N/A";
                endDateInputField.InputField.text = "N/A";
            }
            else
            {
                // Set valid dates
                startDateInputField.InputField.text = approveClientScreenData.Client.SubscriptionStartDate.ToString("yyyy-MM-dd");
                endDateInputField.InputField.text = approveClientScreenData.Client.SubscriptionEndDate.ToString("yyyy-MM-dd");
            }
            macAdressesUIElement.InputField.text = string.Join(",", approveClientScreenData.Client.MacAddresses);
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

        protected override ApproveClientRequest ExtractDataFromInputs()
        {
            return new ApproveClientRequest()
            {
                Name = nameInputField.GetElementDataClassType<string>(),
                Phone = phoneUIElement.GetElementDataClassType<string>(),
                SubscriptionStartDate = DateTime.Parse(startDateInputField.GetElementDataClassType<string>()),
                SubscriptionEndDate = DateTime.Parse(startDateInputField.GetElementDataClassType<string>()),
                MacAddresses = macAdressesUIElement.GetElementDataClassType<string>().Split(',').ToList()
            };
        }

        protected override void FillUIElementsInputs()
        {
            uIElementsInputs.Add(nameInputField);
            uIElementsInputs.Add(phoneUIElement);
            uIElementsInputs.Add(startDateInputField);
            uIElementsInputs.Add(endDateInputField);
            uIElementsInputs.Add(macAdressesUIElement);
        }

        public override void OnClose()
        {
            base.OnClose();
            var cancelButtonRectTransform = cancelButton.GetComponent<RectTransform>();
            cancelButtonRectTransform.transform.parent = transform;

        }
    }
}
