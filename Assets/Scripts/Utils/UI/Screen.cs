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
        private string title;
        [SerializeField]
        private GameObject screenMainContent;
        [SerializeField]
        private RenderMode renderMode;

        public ScreenHandle Previous { get => previous; protected set => previous = value; }
        public ScreenHandle Next { get => next; protected set => next = value; }
        public string Title { get => title; protected set => title = value; }
        public RenderMode RenderMode { get => renderMode; private set => renderMode = value; }
        public GameObject ScreenMainContent { get => screenMainContent; protected set => screenMainContent = value; }

        public abstract void SetupLayout();
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