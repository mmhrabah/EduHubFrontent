using System;
using System.Collections.Generic;
using System.Linq;
using Rabah.GeneralDataModel;
using Rabah.UI.MainComponents;
using Rabah.Utils.Session;
using TMPro;
using UnityEngine;

namespace Rabah.Screens
{
    public class ContentTypesDropDownUIElement : DropDownUIElement
    {
        private ContentType selectedContentType;
        protected override void Awake()
        {
            base.Awake();
            SetDefinitionDropDownData(Session.ContentTypes);
        }
        public override void SetDefinitionDropDownData<T>(T data)
        {
            base.SetDefinitionDropDownData(data);
            List<string> contentTypeNames = Session.ContentTypes.Select(c => c.Name).ToList();
            foreach (var item in contentTypeNames)
            {
                AddDropdownItem(item);
            }
            Dropdown.Interactable(false);
        }
        public override void OnSelectItem(int index)
        {
            selectedContentType = Session.ContentTypes[index];
            var selectedItemName = selectedContentType.Name;
            MainText.text = selectedItemName;
            Debug.Log($"Selected content type: {selectedItemName}");
        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)(object)selectedContentType.Id;
            }

            throw new InvalidCastException($"Cannot cast slected item value to {typeof(T)}");
        }
    }
}