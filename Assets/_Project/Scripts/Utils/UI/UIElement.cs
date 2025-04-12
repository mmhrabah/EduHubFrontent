using System;
using UnityEngine;

namespace Rabah.Utils.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public abstract void ResetElement();

        public abstract bool IsValid();

        public abstract bool IsValid(Action onCheck);

        public abstract T GetElementDataClassType<T>() where T : class;
        public abstract T GetElementDataStructType<T>() where T : struct;
    }
}