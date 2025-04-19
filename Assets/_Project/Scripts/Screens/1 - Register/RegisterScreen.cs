using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using System;


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
        private Sprite warningIcon;


        protected override void Awake()
        {
            base.Awake();
            // Initialize the mock user database For testing purposes
            APIManager.Instance.MockUserDatabase.FillUserDictionary();
            /////////////////////////////////////////////////////////
            onResponseReceived += (response) =>
            {
                // Handle successful registration
                Debug.Log("Registeration successful");
                UIManager.Instance.OpenScreen(
                    handle: ScreenHandle.MainScreen
                    , data: new MainScreenData
                    {
                        User = APIManager.Instance.MockUserDatabase.GetUser(response.Data.Id).Data
                    });
            };
            onErrorReceived += (error) =>
                {
                    // Handle network error
                    Debug.LogError("Network error: " + error);
                    UIManager.Instance.ShowNotificationModal(
                        title: "Registration Failed",
                        descriptionText: "Username is already exists.",
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

        protected override RegisterDataModelRequest ExtractDataFromInputs(List<UIElement> uIElementsInputs)
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
            // Fake registaration for testing purposes
            if (IsScreenDataValid())
            {
                StartCoroutine(FakeRegister());
            }
            /////////////////////////////////////
        }

        private System.Collections.IEnumerator FakeRegister()
        {
            UIManager.Instance.ShowLoading();
            yield return new WaitForSeconds(2.8f);
            RegisterDataModelRequest data = ExtractDataFromInputs(UIElementsInputs);
            User user = new User
            {
                Id = data.Id.ToString(),
                Username = data.Username,
                Password = data.Password
            };
            var registeredInUser = APIManager.Instance.MockUserDatabase.AddUser(user);
            if (registeredInUser.StatusCode == (int)HttpStatusCode.Created)
            {
                ResponseModel<RegisterResponse> registerationData = new()
                {
                    StatusCode = registeredInUser.StatusCode,
                    Data = new RegisterResponse
                    {
                        Id = Guid.Parse(registeredInUser.Data.Id),
                        Username = registeredInUser.Data.Username,
                        Password = registeredInUser.Data.Password
                    }
                };
                onResponseReceived?.Invoke(registerationData);
                UIManager.Instance.HideLoading();
            }
            else
            {
                onErrorReceived?.Invoke("Registration failed");
                UIManager.Instance.HideLoading();
            }
        }
    }
}
