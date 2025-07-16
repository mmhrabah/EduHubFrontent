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
    public class AddClientScreen : ScreenSendDataToDatabase<AddClientRequest, ResponseModel<string>, string>
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

        protected override AddClientRequest ExtractDataFromInputs()
        {
            return new AddClientRequest()
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
