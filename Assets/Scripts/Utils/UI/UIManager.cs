using Rabah.Scenes.Definitions.UI;
using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rabah.Utils.UI
{
    public class UIManager : BaseSingleton<UIManager>
    {


        [SerializeField]
        [Tooltip("All windows will be instantiated under this this parent")]
        private Transform windowsParent;
        [Space(5)]
        [SerializeField]
        [Tooltip("All windows has main data [handle - prefab - next - previous]")]
        private List<ScreenPrefab> screenPrefabs;
        [SerializeField]
        private GameObject loading;
        [SerializeField]
        private TMP_Text screenTitleTMP_Text;
        [SerializeField]
        private NextButton nextButton;
        [SerializeField]
        private BackButton backButton;
        [SerializeField]
        private GameObject bottomPanel;
        [SerializeField]
        private GameObject topPanel;
        [SerializeField]
        private GameObject resetButton;
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Image background;
        [SerializeField]
        private ModalWindowManager notificationModalViewManager;
        [SerializeField]
        private ModalWindowManager comingSoonModalViewManager;
        [SerializeField]
        private RectTransform windowsRectTransform;
        [SerializeField]
        private RawImage buildingVideoRawImage;
        [SerializeField]
        private Image buildingBGImage;
        [SerializeField]
        private List<Sprite> buildingBGSprites;

        private Dictionary<ScreenHandle, Screen> prefabDictionary = new();
        private Dictionary<ScreenHandle, Screen> allScreens = new();
        private Screen currentScreen;
        private readonly Vector2 windowsRectAnchorMin = new(0, 0);
        private readonly Vector2 windowsRectAnchorMax = new(1, 1);
        private readonly Vector2 windowsRectOffsetMin = new(0, 125);
        private readonly Vector2 windowsRectOffsetMax = new(0, -150);
        private int currentBuildingBGIndex = 0;
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
            ++currentBuildingBGIndex;
            currentBuildingBGIndex %= buildingBGSprites.Count;
            buildingBGImage.sprite = buildingBGSprites[currentBuildingBGIndex];
            if (prefabDictionary.TryGetValue(handle, out var prefab))
            {
                if (closePreviousScreen)
                {
                    CloseScreen(useFadeAnimation);
                }

                var screenInstance = Instantiate(prefab, windowsParent);
                CurrentScreen = screenInstance;
                CurrentScreen.SetupLayout();
                CurrentScreen.ControlScreenMainContent(false);
                canvas.renderMode = screenInstance.RenderMode;
                screenTitleTMP_Text.text = CurrentScreen.Title;
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
            background.gameObject.SetActive(isActive);
        }


        public void ShowComingSoonModal()
        {
            comingSoonModalViewManager.OpenWindow();
        }

        public void ShowNotificationModal(string title, string descriptionText)
        {
            notificationModalViewManager.titleText = title;
            notificationModalViewManager.descriptionText = descriptionText;
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
            windowsRectTransform.anchorMin = anchorMin;
            windowsRectTransform.anchorMax = anchorMax;
            windowsRectTransform.offsetMin = offsetMin;
            windowsRectTransform.offsetMax = offsetMax;
        }

        public void ResetWindowsRectData()
        {
            SetWindowsRectData(windowsRectAnchorMin, windowsRectAnchorMax, windowsRectOffsetMin, windowsRectOffsetMax);
        }

        public void ControlBuildingVideo(bool isActive)
        {
            buildingVideoRawImage.gameObject.SetActive(isActive);
        }

        public void ControlResetButton(bool isActive)
        {
            resetButton.SetActive(isActive);
        }
    }
}
