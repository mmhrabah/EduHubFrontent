using System;
using DG.Tweening;
using UnityEngine;

namespace Rabah.Utils.UI
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField]
        private ScreenHandle previous;
        [SerializeField]
        private ScreenHandle next;
        [SerializeField]
        private GameObject screenMainContent;
        [SerializeField]
        private RenderMode renderMode;
        [SerializeField]
        private ScreenSetupDataSO screenSetupData;

        public ScreenHandle Previous { get => previous; protected set => previous = value; }
        public ScreenHandle Next { get => next; protected set => next = value; }
        public RenderMode RenderMode { get => renderMode; private set => renderMode = value; }
        public GameObject ScreenMainContent { get => screenMainContent; protected set => screenMainContent = value; }
        public ScreenSetupDataSO ScreenSetupData { get => screenSetupData; private set => screenSetupData = value; }

        public virtual void SetupLayout()
        {
            if (ScreenSetupData == null) { Debug.LogError("ScreenSetupData is NOT assigned!!"); return; }
            renderMode = ScreenSetupData.renderMode;
            ScreenMainContent.SetActive(false);
            // if (ScreenSetupData.isFullScreen)
            // {
            //     ScreenMainContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenSetupData.screenWidth, ScreenSetupData.screenHeight);
            // }
            // else
            // {
            //     ScreenMainContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenSetupData.screenWidth * ScreenSetupData.screenScale, ScreenSetupData.screenHeight * ScreenSetupData.screenScale);
            // }
            ScreenMainContent.GetComponent<RectTransform>().localScale = new Vector3(ScreenSetupData.screenScale, ScreenSetupData.screenScale, 1);
            UIManager.Instance.ControlBackButton(ScreenSetupData.hasBackButton);
            UIManager.Instance.ControlResetButton(ScreenSetupData.hasResetButton);
            UIManager.Instance.ControlNextButton(ScreenSetupData.hasNextButton);
            UIManager.Instance.ControlLeftPanel(ScreenSetupData.hasLeftPanel);
            UIManager.Instance.ControlBottomPanel(ScreenSetupData.hasBottomPanel);
            UIManager.Instance.ControlTopPanel(ScreenSetupData.hasTopPanel);
        }
        public virtual void OnOpen(ScreenData data) { }
        public virtual void OnClose() { }
        public virtual void OnReset()
        {
            UIElement[] UIElementList = GetComponentsInChildren<UIElement>();
            foreach (var element in UIElementList)
            {
                element.ResetElement();
            }
        }
        public abstract bool IsScreenDataValid();

        public void ControlScreenMainContent(bool isActive)
        {
            if (ScreenMainContent == null) { Debug.LogError("screenMainContent is NOT assinged!!"); return; }
            ScreenMainContent.SetActive(isActive);
        }

        public void FadeInScreenContent(Action onComplete = null)
        {
            if (!ScreenMainContent.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup = ScreenMainContent.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.5f)
                .OnPlay(() =>
                {
                    ControlScreenMainContent(true);
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }

        public void FadeOutScreenContent(Action onComplete = null)
        {
            ScreenMainContent.TryGetComponent<CanvasGroup>(out var canvasGroup);
            if (canvasGroup == null)
            {
                ScreenMainContent.AddComponent<CanvasGroup>();
            }
            canvasGroup.DOFade(0, 0.5f)
                .OnPlay(() =>
                {
                    ControlScreenMainContent(true);
                    canvasGroup.alpha = 1;
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    ControlScreenMainContent(false);
                });
        }
    }
}