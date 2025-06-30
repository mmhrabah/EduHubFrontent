using System;
using Rabah.UI.MainComponents;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;


namespace Rabah.Screens
{
    public class AddContentScreen : ScreenSendDataToDatabase<AddContentRequest, ResponseModel<RequestModel>, RequestModel>
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


        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            UIManager.Instance.ResetWindowsRectData();
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

        }
    }
}
