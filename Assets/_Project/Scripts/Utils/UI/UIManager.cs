using Rabah.Scenes.Definitions.UI;
using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Rabah.Utils.UI
{
    public class UIManager : BaseSingleton<UIManager>
    {
        [SerializeField]
        [Tooltip("All windows will be instantiated under this this parent")]
        private RectTransform windowsParent;
        [Space(5)]
        [SerializeField]
        [Tooltip("All windows has main data [handle - prefab - next - previous]")]
        private List<ScreenPrefab> screenPrefabs;

        [Space(5)]
        [Header("Buttons")]
        [SerializeField]
        private BackButton backButton;
        [SerializeField]
        private ButtonManager resetButton;
        [SerializeField]
        private NextButton nextButton;

        [Space(5)]
        [Header("Screen Shared Elements")]
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject loading;
        [SerializeField]
        private TMP_Text screenTitleTMP_Text;

        [Space(5)]
        [Header("Screen Backgrounds")]
        [SerializeField]
        private Image mainBackground;
        [SerializeField]
        private Image changeableBackground;
        [SerializeField]
        private List<Sprite> changeableBackgroundSprites;

        [Space(5)]
        [Header("Screen Layout")]
        [SerializeField]
        private GameObject topPanel;
        [SerializeField]
        private GameObject bottomPanel;
        [SerializeField]
        private GameObject leftPanel;

        [Space(5)]
        [Header("Notifications")]
        [SerializeField]
        private ModalWindowManager notificationModalViewManager;

        [Space(5)]
        [Header("Mishky UI Manager")]
        [SerializeField]
        private Michsky.MUIP.UIManager michskyUIManager;

        private Dictionary<ScreenHandle, Screen> prefabDictionary = new();
        private Dictionary<ScreenHandle, Screen> allScreens = new();
        private Screen currentScreen;
        private readonly Vector2 windowsRectAnchorMin = new(0, 0);
        private readonly Vector2 windowsRectAnchorMax = new(1, 1);
        private readonly Vector2 windowsRectOffsetMin = new(0, 125);
        private readonly Vector2 windowsRectOffsetMax = new(0, -150);
        private int changeableBGIndex = 0;
        public Screen CurrentScreen { get => currentScreen; set => currentScreen = value; }

        protected override void Awake()
        {
            base.Awake();

            foreach (var screenPrefab in screenPrefabs)
            {
                if (!prefabDictionary.ContainsKey(screenPrefab.handle))
                {
                    prefabDictionary.Add(screenPrefab.handle, screenPrefab.prefab);
                    allScreens.Add(screenPrefab.handle, screenPrefab.prefab);
                }
            }
            if (screenPrefabs.Count > 0)
            {
                var firstScreen = screenPrefabs[0];
                OpenScreen(firstScreen.handle);
            }
        }

        public void ShowLoading()
        {
            loading.SetActive(true);
        }

        public void HideLoading()
        {
            loading.SetActive(false);
        }

        public void OpenScreen(ScreenHandle handle, bool closePreviousScreen = true, ScreenData data = null, bool useFadeAnimation = true)
        {
            ++changeableBGIndex;
            if (changeableBackgroundSprites.Count > 0)
            {
                changeableBGIndex %= changeableBackgroundSprites.Count;
                changeableBackground.sprite = changeableBackgroundSprites[changeableBGIndex];
            }
            if (prefabDictionary.TryGetValue(handle, out var prefab))
            {
                if (closePreviousScreen)
                {
                    CloseScreen(useFadeAnimation);
                }

                var screenInstance = Instantiate(prefab, windowsParent.transform);
                CurrentScreen = screenInstance;
                CurrentScreen.SetupLayout();
                CurrentScreen.ControlScreenMainContent(false);
                canvas.renderMode = screenInstance.RenderMode;
                screenTitleTMP_Text.text = CurrentScreen.ScreenSetupData.title;
                if (useFadeAnimation)
                {
                    CurrentScreen.FadeInScreenContent(() =>
                    {
                        screenInstance.OnOpen(data);
                    });
                }
                else
                {
                    screenInstance.OnOpen(data);
                }
            }
            else
            {
                Debug.LogError($"No prefab found for screen handle: {handle}");
            }
        }

        public void CloseScreen(bool useFadeAnimation)
        {
            if (currentScreen == null)
            {
                Debug.LogWarning("No active screen found to close");
                return;
            }
            if (useFadeAnimation)
            {
                Screen screenWillClose = currentScreen;
                screenWillClose.FadeOutScreenContent(() =>
                {
                    screenWillClose.OnClose();
                    Destroy(screenWillClose.gameObject);
                });
            }
            else
            {
                currentScreen.OnClose();
                Destroy(currentScreen.gameObject);
            }
        }

        public void CloseScreen(Screen screen)
        {
            screen.OnClose();
            Destroy(screen.gameObject);
        }

        public void ResetScreen(Screen screen)
        {
            screen.OnReset();
        }

        public void ResetCurrentScreen()
        {
            currentScreen.OnReset();
        }

        public Screen GetScreen(ScreenHandle handle)
        {
            if (allScreens.TryGetValue(handle, out var screen))
            {
                return screen;
            }

            Debug.LogWarning($"No active screen found for handle: {handle}");
            return null;
        }

        public void ControlNextButton(bool isActive)
        {
            nextButton.gameObject.SetActive(isActive);
        }

        public void ControlBackButton(bool isActive)
        {
            backButton.gameObject.SetActive(isActive);
        }

        public void ControlBottomPanel(bool isActive)
        {
            bottomPanel.SetActive(isActive);
        }

        public void ControlLeftPanel(bool isActive)
        {
            leftPanel.SetActive(isActive);
        }

        public void ControlCamera(bool isActive)
        {
            mainCamera.gameObject.SetActive(isActive);
        }

        public void AddItemsToBottomPanel(RectTransform item, int siblingIndex)
        {
            var itemName = item.name;
            item.name = $"{itemName} - {CurrentScreen.name}";
            item.SetParent(bottomPanel.transform);
            item.SetSiblingIndex(siblingIndex);
        }

        public void ControlBackground(bool isActive)
        {
            mainBackground.gameObject.SetActive(isActive);
        }


        public void ShowComingSoonModal()
        {
            ShowNotificationModal(
                title: "Coming Soon",
                descriptionText: "This feature is not available yet.",
                icon: null,
                iconColor: Color.black);
        }

        public void ShowNotificationModal(string title, string descriptionText, Sprite icon, Color? iconColor = null, bool isConfirmButton = false, Action okConfirmAction = null, bool isCancelButton = false)
        {

            if (isCancelButton)
            {
                notificationModalViewManager.showCancelButton = isCancelButton;
                notificationModalViewManager.closeOnCancel = true;
            }
            else
            {
                notificationModalViewManager.showCancelButton = false;
            }
            if (isConfirmButton)
            {
                notificationModalViewManager.showConfirmButton = isConfirmButton;
                notificationModalViewManager.closeOnConfirm = true;
                notificationModalViewManager.closeOnCancel = true;
                notificationModalViewManager.confirmButton.onClick.RemoveAllListeners();
                notificationModalViewManager.confirmButton.onClick.AddListener(() =>
                {
                    okConfirmAction?.Invoke();
                });
            }
            else
            {
                notificationModalViewManager.cancelButton.onClick.RemoveAllListeners();
                notificationModalViewManager.closeOnCancel = true;
            }
            notificationModalViewManager.titleText = title;
            notificationModalViewManager.descriptionText = descriptionText;

            if (icon == null)
            {
                notificationModalViewManager.windowIcon.gameObject.SetActive(false);
            }
            else
            {
                notificationModalViewManager.windowIcon.gameObject.SetActive(true);
                notificationModalViewManager.icon = icon;
                michskyUIManager.modalWindowIconColor = iconColor ?? Color.black;
            }
            notificationModalViewManager.UpdateUI();
            notificationModalViewManager.OpenWindow();
        }

        public void RefreshCanvas(RectTransform rectTransform)
        {
            StartCoroutine(RefreshCanvasCoroutine(rectTransform));
        }

        private IEnumerator RefreshCanvasCoroutine(RectTransform rectTransform)
        {
            yield return new WaitForEndOfFrame();
            var pos = rectTransform.sizeDelta;
            rectTransform.sizeDelta = new() { x = pos.x + 0.05f, y = pos.y + 0.05f };
            yield return new WaitForEndOfFrame();
            rectTransform.sizeDelta = pos;
        }

        public void ControlTopPanel(bool isActive)
        {
            topPanel.SetActive(isActive);
        }

        public void SetWindowsRectData(Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            windowsParent.anchorMin = anchorMin;
            windowsParent.anchorMax = anchorMax;
            windowsParent.offsetMin = offsetMin;
            windowsParent.offsetMax = offsetMax;
        }

        public void ResetWindowsRectData()
        {
            SetWindowsRectData(windowsRectAnchorMin, windowsRectAnchorMax, windowsRectOffsetMin, windowsRectOffsetMax);
        }

        public void ControlResetButton(bool isActive)
        {
            resetButton.gameObject.SetActive(isActive);
        }
    }
}
