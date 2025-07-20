using System;
using Michsky.MUIP;
using Rabah.UI.MainComponents;
using Rabah.Utils.Network;
using Rabah.Utils.Session;
using Rabah.Utils.UI;
using UnityEngine;
using UIManager = Rabah.Utils.UI.UIManager;
namespace Rabah.Screens
{
    public class EditContentScreen : ScreenSendDataToDatabase<EditContentRequest, ResponseModel<string>, string>
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
        private EditContentScreenData editContentScreenData;

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

        public override bool IsScreenDataValid()
        {
            return true;
        }

        public override void OnClose()
        {
            base.OnClose();
            var cancelButtonRectTransform = cancelButton.GetComponent<RectTransform>();
            cancelButtonRectTransform.parent = transform;
        }

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            editContentScreenData = data as EditContentScreenData;
            if (editContentScreenData == null || editContentScreenData.Content == null)
            {
                Debug.LogError("EditContentScreenData or Content is null");
                return;
            }

            EndPoint = ScreenSetupData.mainEndpoint + editContentScreenData.Content.Id;
            HttpMethod = HttpMethod.PUT;

            titleInputField.InputField.text = editContentScreenData.Content.Name;
            int selectedIndex = Session.ContentTypes.FindIndex(ct => ct.Id == editContentScreenData.Content.TypeId);
            contentTypesDropDown.OnSelectItem(selectedIndex);
            int selectedCategoryIndex = Session.Categories.FindIndex(c => c.Id == editContentScreenData.Content.CategoryId);
            categoriesDropDown.OnSelectItem(selectedCategoryIndex);
            versionInputField.InputField.text = editContentScreenData.Content.Version;
            filePickerUIElement.SetSelectedFile(editContentScreenData.Content.Link);

            UIManager.Instance.ResetWindowsRectData();
            cancelButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
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
                UIManager.Instance.OpenScreen(ScreenHandle.ContentScreen);
            };
        }

        protected override EditContentRequest ExtractDataFromInputs()
        {
            return new EditContentRequest()
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
        }
    }
}