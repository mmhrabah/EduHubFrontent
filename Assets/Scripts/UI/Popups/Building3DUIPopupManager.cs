using Rabah.Utils;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Rabah.UI.Popups
{
    public class Building3DUIPopupManager : BaseSingleton<Building3DUIPopupManager>
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private float animationDuration = 0.8f;
        [SerializeField]
        private float popupHeight = 840;

        private Vector2 hiddenPosition = new();
        private Vector2 shownPosition = new(0, 0);
        private float popupWidth;

        public UnityEvent onPopupShow = new();
        public UnityEvent onPopupHide = new();

        protected override void Awake()
        {
            popupWidth = rectTransform.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
            hiddenPosition = new Vector2(0, -Screen.height);
            rectTransform.sizeDelta = new Vector2(Screen.width, popupHeight);
            rectTransform.DOAnchorPosY(hiddenPosition.y, 0);
            base.Awake();
        }

        [ContextMenu("Show Popup")]
        public void ShowPopup()
        {
            rectTransform.DOAnchorPosY(shownPosition.y, 0.5f);
            onPopupShow?.Invoke();
        }

        [ContextMenu("Hide Popup")]
        public void HidePopup()
        {
            rectTransform.DOAnchorPosY(hiddenPosition.y, 0.5f);
            onPopupHide?.Invoke();
        }
    }
}