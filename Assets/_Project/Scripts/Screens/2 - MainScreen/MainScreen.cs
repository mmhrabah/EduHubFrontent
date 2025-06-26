using UnityEngine;
using Rabah.Utils.UI;
using System.Collections.Generic;
using Rabah.UI.MainComponents;
using UIManager = Rabah.Utils.UI.UIManager;
using Rabah.Utils.Network;
using System.Net;
using TMPro;
using DG.Tweening;
using Michsky.MUIP;
using System.Linq;
using Rabah.Utils.Session;
using System.Text;


namespace Rabah.Screens
{

    public class MainScreen : ScreenWithFetchDataOnOpen<ResponseModel<MainScreenResponse>, MainScreenResponse>
    {
        [SerializeField]
        private RectTransform welcomePanel;
        [SerializeField]
        private TMP_Text welcomeText;
        [SerializeField]
        private RectTransform statsPanel;
        [SerializeField]
        private ButtonManager manageYourProductsButton;
        [SerializeField]
        private ButtonManager openShowAllProductsButton;
        private CanvasGroup welcomePanelCanvasGroup;
        private CanvasGroup statsPanelCanvasGroup;
        private CanvasGroup manageYourProductsButtonCanvasGroup;

        private void Awake()
        {
            welcomePanelCanvasGroup = SetupCanvasGroup(welcomePanel);
            statsPanelCanvasGroup = SetupCanvasGroup(statsPanel);
            manageYourProductsButtonCanvasGroup = SetupCanvasGroup(manageYourProductsButton.GetComponent<RectTransform>());
            manageYourProductsButton.onClick.AddListener(() =>
            {
                //UIManager.Instance.OpenScreen(ScreenHandle.ManageProductsScreen);
            });

            openShowAllProductsButton.onClick.AddListener(() =>
            {
                //UIManager.Instance.OpenScreen(ScreenHandle.ShowAllProductsScreen);
            });
        }

        public override void OnOpen(ScreenData data)
        {

            welcomePanelCanvasGroup.DOFade(1, 0.5f)
                .OnPlay(() =>
                {
                    FetchData();
                    welcomeText.text = $"Welcome {Session.User.Username}!";
                })
                .OnComplete(() =>
                {
                    welcomePanelCanvasGroup.interactable = true;
                    welcomePanelCanvasGroup.blocksRaycasts = true;
                    statsPanelCanvasGroup.interactable = true;
                    statsPanelCanvasGroup.blocksRaycasts = true;
                    AnimateWelcomePanel();
                }
                );
        }
        public override bool IsScreenDataValid()
        {
            return true;
        }

        #region UI Animations
        private void AnimateWelcomePanel()
        {
            var finalMinAnchors = new Vector2(0, 0.85f);
            var finalMaxAnchors = new Vector2(0.5f, 1);
            welcomePanel.DOAnchorMin(finalMinAnchors, 1.5f)
            .SetDelay(2.5f)
            .SetEase(Ease.OutBack)
            .OnPlay(() =>
            {
                welcomePanel.DOAnchorMax(finalMaxAnchors, 1.5f)
                .SetEase(Ease.OutBack);
            })
            .OnComplete(() =>
            {
                statsPanelCanvasGroup.DOFade(1, 1.5f)
                .OnComplete(() =>
                {
                    manageYourProductsButtonCanvasGroup.DOFade(1, 1.5f)
                    .OnComplete(() =>
                    {
                        manageYourProductsButtonCanvasGroup.interactable = true;
                        manageYourProductsButtonCanvasGroup.blocksRaycasts = true;
                    });
                });
            });
        }
        #endregion

        #region UI Setup
        private CanvasGroup SetupCanvasGroup(RectTransform rectTransform)
        {
            var canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            return canvasGroup;
        }

        protected override void OnErrorReceived(string error)
        {
            Debug.LogError($"Failed to fetch data: {error}");
        }

        protected override void OnDataFetched(ResponseModel<MainScreenResponse> response)
        {
        }

        #endregion
    }
}