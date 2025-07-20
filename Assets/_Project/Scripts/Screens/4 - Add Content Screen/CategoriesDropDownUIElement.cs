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
    public class CategoriesDropDownUIElement : DropDownUIElement
    {
        private Category selectedCategory;
        protected override void Awake()
        {
            base.Awake();
            SetDefinitionDropDownData(Session.Categories);
            if (Session.Categories.Count > 0)
            {
                OnSelectItem(0);
            }
        }
        public override void SetDefinitionDropDownData<T>(T data)
        {
            base.SetDefinitionDropDownData(data);
            List<string> categoryNames = Session.Categories.Select(c => c.Name).ToList();
            foreach (var item in categoryNames)
            {
                AddDropdownItem(item);
            }
        }
        public override void OnSelectItem(int index)
        {
            selectedCategory = Session.Categories[index];
            var selectedItemName = selectedCategory.Name;
            MainText.text = selectedItemName;
            Debug.Log($"Selected content type: {selectedItemName}");
        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)(object)selectedCategory.Id;
            }

            throw new InvalidCastException($"Cannot cast slected item value to {typeof(T)}");
        }
    }
}