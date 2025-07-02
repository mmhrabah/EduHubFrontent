using System;
using Michsky.MUIP;
using Rabah.UI.MainComponents;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
using UIManager = Rabah.Utils.UI.UIManager;



namespace Rabah.Screens
{
    public class AddContentScreen : ScreenSendDataToDatabase<AddContentRequest, ResponseModel<string>, string>
    {
        [SerializeField]
        private InputFieldUIElement titleInputField;
        [SerializeField]
        private ContentTypesDropDownUIElement contentTypesDropDown;
        [SerializeField]
        private CategoriesDropDownUIElement categoriesDropDown;
        [SerializeField]
        private InputFieldUIElement versionInputField;
        [SerializeField]
        private FilePickerUIElement filePickerUIElement;
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
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
            });

            onErrorReceived += (error) =>
            {
                UIManager.Instance.ShowNotificationModal(
                    title: "Error",
                    descriptionText: "Failed to add content: " + error,
                    icon: null);
                Debug.LogError($"Failed to add content: {error}");
            };
            onResponseReceived += (response) =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
            };
        }

        protected override AddContentRequest ExtractDataFromInputs()
        {
            return new AddContentRequest()
            {
                Name = titleInputField.GetElementDataClassType<string>(),
                Description = string.Empty,
                ImageUrl = string.Empty,
                Author = string.Empty,
                Publisher = "Nahdet Misr Publishing Group",
                TypeId = contentTypesDropDown.GetElementDataStructType<Guid>(),
                CategoryId = categoriesDropDown.GetElementDataStructType<Guid>(),
                Version = versionInputField.GetElementDataClassType<string>(),
                Link = filePickerUIElement.GetElementDataClassType<string>(),
            };
        }

        protected override void FillUIElementsInputs()
        {
            uIElementsInputs.Add(titleInputField);
            uIElementsInputs.Add(contentTypesDropDown);
            uIElementsInputs.Add(categoriesDropDown);
            uIElementsInputs.Add(versionInputField);
            uIElementsInputs.Add(filePickerUIElement);
        }

        public override void OnClose()
        {
            base.OnClose();
            var cancelButtonRectTransform = cancelButton.GetComponent<RectTransform>();
            cancelButtonRectTransform.transform.parent = transform;

        }
    }
}
