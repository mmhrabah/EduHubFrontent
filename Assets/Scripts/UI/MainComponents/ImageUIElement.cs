using System;
using Rabah.Utils;
using Rabah.Utils.UI;
using UnityEngine;

namespace Rabah.UI.MainComponents
{
    public class ImageUIElement : UIElement
    {
        [SerializeField]
        private ImageController imageController;

        public override T GetElementDataClassType<T>()
        {
            if (typeof(T) == typeof(Byte[]))
            {
                return (T)(object)ImageConverter.TextureToByteArray(imageController.GetImage());
            }
            return default;
        }

        public override T GetElementDataStructType<T>()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            return true;
        }

        public override bool IsValid(Action onCheck)
        {
            return true;
        }

        public override void ResetElement()
        {
            imageController.ResetImage();
        }

        public void SetImage(byte[] image)
        {
            if (image == null || image.Length == 0)
            {
                imageController.ResetImage();
                return;
            }
            imageController.SetImage(image);
        }
    }
}