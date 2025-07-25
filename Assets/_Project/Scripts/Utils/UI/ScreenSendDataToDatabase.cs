using System;
using System.Collections.Generic;
using Michsky.MUIP;
using Rabah.Utils.Network;
using UnityEngine;

namespace Rabah.Utils.UI
{
    /// <summary>
    /// This class is used to manage the screen that sends data to the database.
    /// </summary>
    public abstract class ScreenSendDataToDatabase<T, Y, S> : Screen where T : RequestModel where Y : ResponseModel<S>
    {
        [Header("Send Data")]
        [SerializeField]
        private ButtonManager sendButton;

        [Header("UI Elements")]
        [SerializeField]
        protected List<UIElement> uIElementsInputs = new();

        protected Action<UIElement> onInputHasInvalidData;
        protected Action<ResponseModel<S>> onResponseReceived;
        protected Action<string> onErrorReceived;

        protected List<UIElement> UIElementsInputs { get => uIElementsInputs; private set => uIElementsInputs = value; }

        protected virtual bool MustParse { get; set; } = true;
        protected string EndPoint { get; set; }
        protected HttpMethod HttpMethod { get; set; } = HttpMethod.POST;
        protected virtual void Awake()
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            EndPoint = ScreenSetupData.mainEndpoint;
            FillUIElementsInputs();
        }

        protected virtual void OnSendButtonClicked()
        {

            if (IsScreenDataValid())
            {
                T data = ExtractDataFromInputs();
                if (HttpMethod == HttpMethod.POST)
                {
                    APIManager.Instance.Post<Y>(EndPoint, data,
                    (response) =>
                    {
                        onResponseReceived?.Invoke(response);
                        UIManager.Instance.HideLoading();
                    },
                    (error) =>
                    {
                        onErrorReceived?.Invoke(error);
                        UIManager.Instance.HideLoading();
                    },
                    onSend: () =>
                    {
                        // Handle send action if needed
                        UIManager.Instance.ShowLoading();
                    },
                    fixResponse: ScreenSetupData.fixedResponse,
                    mustParse: MustParse
                    );
                }
                else if (HttpMethod == HttpMethod.PUT)
                {
                    APIManager.Instance.Put<Y>(EndPoint, data,
                    (response) =>
                    {
                        onResponseReceived?.Invoke(response);
                        UIManager.Instance.HideLoading();
                    },
                    (error) =>
                    {
                        onErrorReceived?.Invoke(error);
                        UIManager.Instance.HideLoading();
                    },
                    onSend: () =>
                    {
                        // Handle send action if needed
                        UIManager.Instance.ShowLoading();
                    },
                    fixResponse: ScreenSetupData.fixedResponse,
                    mustParse: MustParse
                    );
                }
                else Debug.LogError("HttpMethod is not set or is invalid.");
            }
            else
            {
                // Handle invalid data case
                foreach (var element in UIElementsInputs)
                {
                    if (!element.IsValid())
                    {
                        onInputHasInvalidData?.Invoke(element);
                    }
                }
                Debug.LogError("Screen data is not valid.");
            }
        }

        protected abstract T ExtractDataFromInputs();
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