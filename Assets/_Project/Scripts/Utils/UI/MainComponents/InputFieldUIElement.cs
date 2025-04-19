using System;
using TMPro;
using UnityEngine;

namespace Rabah.UI.MainComponents
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldUIElement : Utils.UI.UIElement
    {
        private TMP_InputField inputField;

        public TMP_InputField InputField { get => inputField; private set => inputField = value; }

        protected virtual void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public override bool IsValid()
        {
            if (InputField.contentType == TMP_InputField.ContentType.Standard)
            {
                return !string.IsNullOrEmpty(InputField.text);
            }
            else if (InputField.contentType == TMP_InputField.ContentType.DecimalNumber || InputField.contentType == TMP_InputField.ContentType.IntegerNumber)
            {
                return !string.IsNullOrEmpty(InputField.text) && float.Parse(InputField.text) > 0;
            }
            return true;
        }

        public override void ResetElement()
        {
            InputField.text = string.Empty;
        }

        public override T GetElementDataClassType<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)inputField.text;
            }

            throw new System.InvalidCastException($"Cannot cast UITextElement value to {typeof(T)}");

        }

        public override T GetElementDataStructType<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)inputField.text;
            }

            throw new System.InvalidCastException($"Cannot cast UITextElement value to {typeof(T)}");
        }

        public override bool IsValid(Action onCheck)
        {
            throw new NotImplementedException();
        }
    }
}
