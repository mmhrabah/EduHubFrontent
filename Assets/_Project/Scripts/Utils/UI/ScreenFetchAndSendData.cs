using System.Collections.Generic;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.Events;

namespace Rabah.Utils.UI
{
    /// <summary>
    /// This class is used to manage the screen that fetches and sends data to the database.
    /// </summary>
    public abstract class ScreenFetchAndSendData : Screen
    {
        [SerializeField]
        private ButtonManager sendButton;

        protected UnityEvent<UIElement> onInputHasInvalidData = new();

        private List<UIElement> uIElementsInputs = new();

        protected List<UIElement> UIElementsInputs { get => uIElementsInputs; private set => uIElementsInputs = value; }

        private void Awake()
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }


        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            FillUIElementsInputs();
            FetchData();
        }

        protected abstract void FetchData();


        protected virtual void OnSendButtonClicked()
        {
            if (IsScreenDataValid())
            {
                ExtractDataFromInputs(UIElementsInputs);
            }
            else
            {
                // Handle invalid data case
                Debug.LogError("Screen data is not valid.");
            }
        }
        protected abstract void ExtractDataFromInputs(List<UIElement> uIElementsInputs);
        protected abstract void FillUIElementsInputs();

        public override bool IsScreenDataValid()
        {
            // Implement your validation logic here
            foreach (var element in UIElementsInputs)
            {
                if (!element.IsValid())
                {
                    Debug.LogError($"Element {element.name} is not valid.");
                    onInputHasInvalidData?.Invoke(element);
                    return false;
                }
            }
            return true;
        }
    }
}