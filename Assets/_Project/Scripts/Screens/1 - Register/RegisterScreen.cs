using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using System;
using Rabah.GeneralDataModel;


namespace Rabah.Screens
{
    /// <summary>
    /// This class is used to manage the registeration screen.
    /// </summary>
    public class RegisterScreen : ScreenSendDataToDatabase<RegisterDataModelRequest, ResponseModel<RegisterResponse>, RegisterResponse>
    {
        [SerializeField]
        private InputFieldUIElement usernameInputField;
        [SerializeField]
        private InputFieldUIElement passwordInputField;
        [SerializeField]
        private InputFieldUIElement emailInputField;
        [SerializeField]
        private Sprite warningIcon;


        protected override void Awake()
        {
            base.Awake();
            onResponseReceived += (response) =>
            {
                // Handle successful registration
                Debug.Log("Registeration successful");
                UIManager.Instance.OpenScreen(
                    handle: ScreenHandle.MainScreen
                    , data: new MainScreenData
                    {
                        User =
                        new User
                        {
                            Id = response.Data.Id,
                            Username = response.Data.Username,
                            Email = response.Data.Email,
                            ProfilePictureUrl = response.Data.ProfilePictureUrl,
                            AccessToken = response.Data.AccessToken
                        }
                    });
            };
            onErrorReceived += (error) =>
                {
                    // Handle network error
                    Debug.LogError("Network error: " + error);
                    UIManager.Instance.ShowNotificationModal(
                        title: "Registration Failed",
                        descriptionText: error.ToString(),
                        icon: warningIcon);
                };
        }

        public override bool IsScreenDataValid()
        {
            return true;
        }

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
        }

        protected override RegisterDataModelRequest ExtractDataFromInputs()
        {
            RegisterDataModelRequest registrationData = new();

            foreach (var element in uIElementsInputs)
            {
                if (element is InputFieldUIElement inputField)
                {
                    if (inputField == usernameInputField)
                    {
                        registrationData.Username = inputField.InputField.text;
                    }
                    else if (inputField == passwordInputField)
                    {
                        registrationData.Password = inputField.InputField.text;
                    }
                    else if (inputField == emailInputField)
                    {
                        registrationData.Email = inputField.InputField.text;
                    }
                }
            }

            return registrationData;
        }

        protected override void FillUIElementsInputs()
        {
            UIElementsInputs.Add(usernameInputField);
            UIElementsInputs.Add(passwordInputField);
        }

        protected override void OnSendButtonClicked()
        {
            Register();
        }

        private void Register()
        {
            if (IsScreenDataValid())
            {
                UIManager.Instance.ShowLoading();
                RegisterDataModelRequest data = ExtractDataFromInputs();
                APIManager.Instance.Post<ResponseModel<RegisterResponse>>(
                    endpoint: ScreenSetupData.mainEndpoint,
                    data,
                    (response) =>
                    {
                        onResponseReceived?.Invoke(response);
                        UIManager.Instance.HideLoading();
                    },
                    (error) =>
                    {
                        onErrorReceived?.Invoke(error);
                        UIManager.Instance.HideLoading();
                    }
                );
            }
        }
    }
}
