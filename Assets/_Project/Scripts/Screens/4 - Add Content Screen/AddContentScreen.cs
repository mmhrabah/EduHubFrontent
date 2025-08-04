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
        private ImageUIElement contentCoverImageUIElement;
        [SerializeField]
        private ButtonManager cancelButton;
        [SerializeField]
        private ButtonManager clearImageButton;
        [SerializeField]
        private ButtonManager selectCoverButton;

        private string coverURL = string.Empty;
        protected override void Awake()
        {
            base.Awake();
            MustParse = false;
            contentCoverImageUIElement.OnImageSelected += () =>
            {
                clearImageButton.gameObject.SetActive(true);
                selectCoverButton.gameObject.SetActive(false);
                UIManager.Instance.ShowLoading();
                var filePath = contentCoverImageUIElement.GetElementDataClassType<string>();
                StartCoroutine(FileUploadDownloaderManager.Instance.UploadFile(filePath,
                        (url) =>
                        {
                            Debug.Log("File uploaded successfully: " + url);
                            coverURL = url;
                            UIManager.Instance.HideLoading();
                        },
                        (error) =>
                        {
                            UIManager.Instance.HideLoading();
                            Debug.LogError("File upload failed: " + error);
                        }));
            };
            clearImageButton.onClick.AddListener(() =>
            {
                contentCoverImageUIElement.ResetElement();
                clearImageButton.gameObject.SetActive(false);
                selectCoverButton.gameObject.SetActive(true);
                coverURL = string.Empty;
            });
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
            var selectedContentType = Session.ContentTypes.Find(x => x.FileExtension == "ePub");
            var index = Session.ContentTypes.IndexOf(selectedContentType);
            contentTypesDropDown.OnSelectItem(index);
        }

        protected override AddContentRequest ExtractDataFromInputs()
        {
            return new AddContentRequest()
            {
                Name = titleInputField.GetElementDataClassType<string>(),
                Description = string.Empty,
                ImageUrl = coverURL,
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
