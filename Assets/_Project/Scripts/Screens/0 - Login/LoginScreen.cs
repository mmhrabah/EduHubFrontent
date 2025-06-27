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
using DG.Tweening;
using UnityEngine.UI;


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
        [SerializeField]
        private CanvasGroup logosCanvasGroup;
        [SerializeField]
        private CanvasGroup loginContentCanvasGroup;
        [SerializeField]
        private Image companyLogoImage;
        [SerializeField]
        private Image appLogoImage;
        [SerializeField]
        private float animationDuration = 1.5f;
        [SerializeField]
        private Ease animationEase = Ease.OutBounce;


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
                        descriptionText: error,
                        icon: warningIcon,
                        iconColor: Color.red,
                        isCancelButton: false);
                };
        }

        public override bool IsScreenDataValid()
        {
            return true;
        }

        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            PlayLoginAnimation();
        }


        [ContextMenu("Play Login Animation")]
        private void PlayLoginAnimation()
        {
            logosCanvasGroup.DOFade(1f, animationDuration)
                .OnComplete(() =>
                {
                    appLogoImage.transform.DOScale(1f, animationDuration)
                        .SetEase(animationEase)
                        .OnComplete(() =>
                        {
                            companyLogoImage.transform.DOScale(1f, animationDuration)
                                .SetEase(animationEase)
                                .OnComplete(() =>
                                {
                                    appLogoImage.DOFade(0f, animationDuration)
                                    .OnPlay(() =>
                                    {
                                        companyLogoImage.DOFade(0f, animationDuration)
                                            .OnComplete(() =>
                                            {
                                                logosCanvasGroup.interactable = false;
                                                logosCanvasGroup.blocksRaycasts = false;
                                                loginContentCanvasGroup.DOFade(1f, animationDuration)
                                                    .OnComplete(() =>
                                                    {
                                                        loginContentCanvasGroup.interactable = true;
                                                        loginContentCanvasGroup.blocksRaycasts = true;
                                                    });
                                            });
                                    });
                                });
                        });
                });
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
            UIElementsInputs.Add(usernameInputField);
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

        public override void SetupLayout()
        {
            base.SetupLayout();
            ResetSplashElementsAnimations();
        }

        [ContextMenu("Reset Splash Elements Animations")]
        private void ResetSplashElementsAnimations()
        {
            logosCanvasGroup.alpha = 0f;
            loginContentCanvasGroup.alpha = 0f;
            logosCanvasGroup.interactable = true;
            logosCanvasGroup.blocksRaycasts = true;
            loginContentCanvasGroup.interactable = false;
            loginContentCanvasGroup.blocksRaycasts = false;
            companyLogoImage.transform.DOScale(0.0f, 0.0f);
            appLogoImage.transform.DOScale(0.0f, 0.0f);
        }
    }
}
