using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Michsky.MUIP;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using System;


namespace Rabah.Screens
{
    /// <summary>
    /// This class is used to manage the login screen.
    /// </summary>
    public class LoginScreen : ScreenSendDataToDatabase<LoginDataModelRequest, ResponseModel<LoginResponse>, LoginResponse>
    {
        [SerializeField]
        private InputFieldUIElement usernameInputField;
        [SerializeField]
        private InputFieldUIElement passwordInputField;
        [SerializeField]
        private ButtonManager regitserButton;
        [SerializeField]
        private Sprite warningIcon;


        protected override void Awake()
        {
            base.Awake();
            // Initialize the mock user database For testing purposes
            APIManager.Instance.MockUserDatabase.FillUserDictionary();
            /////////////////////////////////////////////////////////
            regitserButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenScreen(ScreenHandle.RegisterScreen);
            });
            onResponseReceived += (response) =>
            {
                // Handle successful login
                Debug.Log("Login successful");
                UIManager.Instance.OpenScreen(
                    handle: ScreenHandle.MainScreen,
                    data: new MainScreenData
                    {
                        User = APIManager.Instance.MockUserDatabase.GetUser(response.Data.Id).Data
                    });
            };
            onErrorReceived += (error) =>
                {
                    // Handle network error
                    Debug.LogError("Network error: " + error);
                    UIManager.Instance.ShowNotificationModal(
                        title: "Login Failed",
                        descriptionText: "Username or password is incorrect.",
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

        protected override LoginDataModelRequest ExtractDataFromInputs(List<UIElement> uIElementsInputs)
        {
            LoginDataModelRequest loginData = new LoginDataModelRequest();

            foreach (var element in uIElementsInputs)
            {
                if (element is InputFieldUIElement inputField)
                {
                    if (inputField == usernameInputField)
                    {
                        loginData.Username = inputField.InputField.text;
                    }
                    else if (inputField == passwordInputField)
                    {
                        loginData.Password = inputField.InputField.text;
                    }
                }
            }

            return loginData;
        }

        protected override void FillUIElementsInputs()
        {
            UIElementsInputs.Add(usernameInputField);
            UIElementsInputs.Add(passwordInputField);
        }

        protected override void OnSendButtonClicked()
        {
            // Fake login for testing purposes
            if (IsScreenDataValid())
            {
                StartCoroutine(FakeLogin());
            }
            /////////////////////////////////////
        }

        private System.Collections.IEnumerator FakeLogin()
        {
            UIManager.Instance.ShowLoading();
            yield return new WaitForSeconds(2.8f);
            LoginDataModelRequest data = ExtractDataFromInputs(UIElementsInputs);
            var loggedInUser = APIManager.Instance.MockUserDatabase.Login(data.Username, data.Password);
            if (loggedInUser.StatusCode == (int)HttpStatusCode.OK)
            {
                ResponseModel<LoginResponse> loginData = new()
                {
                    StatusCode = loggedInUser.StatusCode,
                    Data = new LoginResponse
                    {
                        Id = Guid.Parse(loggedInUser.Data.Id),
                        Username = loggedInUser.Data.Username,
                        Password = loggedInUser.Data.Password
                    }
                };
                onResponseReceived?.Invoke(loginData);
                UIManager.Instance.HideLoading();
            }
            else
            {
                onErrorReceived?.Invoke("Login failed");
                UIManager.Instance.HideLoading();
            }
        }
    }
}
