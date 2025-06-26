using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Michsky.MUIP;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using Rabah.Utils.Session;
using Rabah.GeneralDataModel;


namespace Rabah.Screens
{
    /// <summary>
    /// This class is used to manage the login screen.
    /// </summary>
    public class LoginScreen : ScreenSendDataToDatabase<LoginDataModelRequest, ResponseModel<LoginResponse>, LoginResponse>
    {
        [SerializeField]
        private InputFieldUIElement emailInputField;
        [SerializeField]
        private InputFieldUIElement passwordInputField;
        [SerializeField]
        private ButtonManager regitserButton;
        [SerializeField]
        private Sprite warningIcon;


        protected override void Awake()
        {
            base.Awake();
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
                        User = new()
                        {
                            Id = response.Data.Id,
                            Username = response.Data.Username,
                        }
                    });

                Session.User = new()
                {
                    Id = response.Data.Id,
                    Username = response.Data.Username,
                    Email = response.Data.Email,
                    PhoneNumber = response.Data.PhoneNumber,
                    DateOfBirth = response.Data.DateOfBirth,
                    ProfilePictureUrl = response.Data.ProfilePictureUrl,
                    AccessToken = response.Data.AccessToken
                };
                Session.AccessToken = Session.User.AccessToken;
                // Session.RefreshToken = response.Data.data.refreshToken;
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

        protected override LoginDataModelRequest ExtractDataFromInputs()
        {
            LoginDataModelRequest loginData = new();
            string password = passwordInputField.GetElementDataClassType<string>();
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            string base64Password = System.Convert.ToBase64String(passwordBytes);
            loginData.Password = base64Password;
            return loginData;
        }

        protected override void FillUIElementsInputs()
        {
            UIElementsInputs.Add(emailInputField);
            UIElementsInputs.Add(passwordInputField);
        }

        protected override void OnSendButtonClicked()
        {
            if (IsScreenDataValid())
            {
                Login();
            }
        }

        private void Login()
        {
            UIManager.Instance.ShowLoading();
            LoginDataModelRequest data = ExtractDataFromInputs();
            APIManager.Instance.Post<ResponseModel<LoginResponse>>(
                endpoint: ScreenSetupData.mainEndpoint,
                data,
                (response) =>
                {
                    ResponseModel<LoginResponse> loginData = new()
                    {
                        StatusCode = response.StatusCode,
                        Data = response.Data
                    };
                },
                (error) =>
                {
                    onErrorReceived?.Invoke(error);
                    UIManager.Instance.HideLoading();
                },
                fixResponse: true
            );
        }
    }
}
