using System;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

namespace Rabah.UI.MainComponents
{
    [RequireComponent(typeof(CustomDropdown))]
    public class DropDownUIElement : Utils.UI.UIElement
    {
        [SerializeField]
        private TMP_Text mainText;
        private CustomDropdown dropdown;
        protected virtual void Awake()
        {
            dropdown = GetComponent<CustomDropdown>();
            dropdown.onValueChanged.AddListener(OnSelectItem);
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void ResetElement()
        {
            dropdown.SetDropdownIndex(0);
        }

        public void AddDropdownItem(string newItem)
        {
            dropdown.CreateNewItem(newItem);
            dropdown.SetupDropdown();
        }

        public virtual void RemoveDrpoDownItem(string title, bool isNotify)
        {
            dropdown.RemoveItem(title, isNotify);
        }

        public virtual void RemoveAllDrpoDownItems()
        {
            dropdown.items.Clear();
            dropdown.SetupDropdown();
        }

        public virtual void ResetSelectedText()
        {
            mainText.text = string.Empty;
        }

        public override T GetElementDataClassType<T>()
        {
            return null;
        }

        public virtual void OnSelectItem(int index)
        {

        }

        public override T GetElementDataStructType<T>()
        {
            return default;
        }

        public virtual void SetDefinitionDropDownData<T>(T data)
        {
            RemoveAllDrpoDownItems();
        }

        public override bool IsValid(Action onCheck)
        {
            throw new NotImplementedException();
        }
    }
}
