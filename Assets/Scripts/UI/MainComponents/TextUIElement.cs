using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rabah.UI.MainComponents
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextUIElement : Utils.UI.UIElement
    {
        private const int charWidth = 18;
        [SerializeField]
        private LayoutElement layoutElement;

        [SerializeField]
        private UnityEvent _AfterFitText;

        [SerializeField]
        private float lineHeight = 50;

        private int maxCharactersNumberPerLine = 100;
        private TMP_Text text;

        protected virtual void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        public override bool IsValid()
        {
            return true;
        }

        public override void ResetElement()
        {
            text.text = string.Empty;
        }

        public override T GetElementDataClassType<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)text.text;
            }

            throw new System.InvalidCastException($"Cannot cast UITextElement value to {typeof(T)}");

        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)text.text;
            }

            throw new System.InvalidCastException($"Cannot cast UITextElement value to {typeof(T)}");
        }

        public virtual void FitText()
        {
            StartCoroutine(FitTextEnumerator());
        }

        private IEnumerator FitTextEnumerator()
        {
            yield return new WaitForSeconds(0.6f);
            if (layoutElement == null) { Debug.LogError("layoutElement is NULL!!"); }
            var layoutRect = layoutElement.GetComponent<RectTransform>();
            var layoutRectWidth = (int)layoutRect.sizeDelta.x;
            maxCharactersNumberPerLine = layoutRectWidth / charWidth;
            var added = GetAddedLayoutElementHeight();
            layoutElement.preferredHeight = added * lineHeight;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            Canvas.ForceUpdateCanvases();
            _AfterFitText?.Invoke();
        }

        private int GetAddedLayoutElementHeight() => (int)Math.Ceiling(text.text.Length / (float)maxCharactersNumberPerLine);

        public override bool IsValid(Action onCheck)
        {
            throw new NotImplementedException();
        }
    }
}