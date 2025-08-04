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

        protected TMP_Text MainText { get => mainText; set => mainText = value; }
        protected CustomDropdown Dropdown { get => dropdown; set => dropdown = value; }

        protected virtual void Awake()
        {
            Dropdown = GetComponent<CustomDropdown>();
            Dropdown.onValueChanged.AddListener(OnSelectItem);
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void ResetElement()
        {
            Dropdown.SetDropdownIndex(0);
        }

        public void AddDropdownItem(string newItem)
        {
            Dropdown.CreateNewItem(newItem);
            Dropdown.SetupDropdown();
        }

        public virtual void RemoveDrpoDownItem(string title, bool isNotify)
        {
            Dropdown.RemoveItem(title, isNotify);
        }

        public virtual void RemoveAllDrpoDownItems()
        {
            Dropdown.items.Clear();
            Dropdown.SetupDropdown();
        }

        public virtual void ResetSelectedText()
        {
            MainText.text = string.Empty;
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
