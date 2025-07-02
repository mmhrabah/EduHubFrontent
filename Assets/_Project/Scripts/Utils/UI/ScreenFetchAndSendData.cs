using System;
using System.Collections.Generic;
using Michsky.MUIP;
using Rabah.Utils.Network;
using UnityEngine;

namespace Rabah.Utils.UI
{
    /// <summary>
    /// This class is used to manage the screen that fetches and sends data to the database.
    /// </summary>
    public abstract class ScreenFetchAndSendData<R, U, T, Y, S> : Screen
                    where R : ResponseModel<U> // data fetched on the openning of the screen
                    where T : RequestModel // data sent to the database from on button clicked
                    where Y : ResponseModel<S>// data fetched in response to the sent data

    {
        [SerializeField]
        private ButtonManager sendButton;

        protected Action<UIElement> onInputHasInvalidData;
        protected Action<ResponseModel<U>> OnDataFetchedOnOpen;
        protected Action<string> onErrorReceivedFromDataFetchedOnOpen;
        protected Action<ResponseModel<S>> onResponseReceivedFromSendData;
        protected Action<string> onErrorReceivedFromSendData;

        private List<UIElement> uIElementsInputs = new();

        protected List<UIElement> UIElementsInputs { get => uIElementsInputs; private set => uIElementsInputs = value; }

        protected virtual bool MustParse { get; set; } = true;


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

        protected virtual void FetchData()
        {
            APIManager.Instance.Get<R>(ScreenSetupData.mainEndpoint,
                (response) =>
                {
                    OnDataFetchedOnOpen?.Invoke(response);
                    UIManager.Instance.HideLoading();
                },
                (error) =>
                {
                    onErrorReceivedFromDataFetchedOnOpen?.Invoke(error);
                    UIManager.Instance.HideLoading();
                },
                fixResponse: true,
                mustParse: MustParse
                );
        }


        protected virtual void OnSendButtonClicked()
        {
            if (IsScreenDataValid())
            {
                T data = ExtractDataFromInputs();
                APIManager.Instance.Post<Y>(ScreenSetupData.mainEndpoint, data,
                (response) =>
                {
                    onResponseReceivedFromSendData?.Invoke(response);
                    UIManager.Instance.HideLoading();
                },
                (error) =>
                {
                    onErrorReceivedFromSendData?.Invoke(error);
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