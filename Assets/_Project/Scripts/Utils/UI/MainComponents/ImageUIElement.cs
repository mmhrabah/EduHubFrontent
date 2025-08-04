using System;
using Crosstales.FB;
using Michsky.MUIP;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using UIManager = Rabah.Utils.UI.UIManager;

namespace Rabah.UI.MainComponents
{
    public class ImageUIElement : UIElement
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private ButtonManager selectImageButton;
        private string Extension = "png,jpg,jpeg";
        private string filePath = "";
        private Action onImageSelected;

        public Action OnImageSelected { get => onImageSelected; set => onImageSelected = value; }

        private void Awake()
        {
            selectImageButton.onClick.AddListener(OpenFile);
        }

        private void OnDisable()
        {
            if (FileBrowser.Instance != null)
                FileBrowser.Instance.OnOpenFilesComplete -= onOpenFilesComplete;
        }

        public void OpenFile()
        {
            if (FileBrowser.Instance != null)
                FileBrowser.Instance.OnOpenFilesComplete += onOpenFilesComplete;
            FileBrowser.Instance.OpenSingleFileAsync(Extension);
        }

        private void onOpenFilesComplete(bool selected, string singlefile, string[] files)
        {
            if (FileBrowser.Instance != null)
                FileBrowser.Instance.OnOpenFilesComplete -= onOpenFilesComplete;
            if (selected && !string.IsNullOrEmpty(singlefile))
            {
                filePath = singlefile;
                SetImage(System.IO.File.ReadAllBytes(filePath));
                OnImageSelected?.Invoke();
            }
            else
            {
                Debug.LogWarning("No file selected or file path is empty.");
            }
        }

        public override T GetElementDataClassType<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)filePath;
            }
            return default(T);
        }

        public override T GetElementDataStructType<T>()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(filePath);
        }

        public override bool IsValid(Action onCheck)
        {
            onCheck?.Invoke();
            return IsValid();
        }

        public override void ResetElement()
        {
            filePath = string.Empty;
            if (FileBrowser.Instance != null)
            {
                FileBrowser.Instance.OnOpenFilesComplete -= onOpenFilesComplete;
            }
            image.sprite = null;
        }

        public void SetImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                image.sprite = null;
                return;
            }
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                texture.Apply();
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
            }
            else
            {
                Debug.LogError("Failed to load image from byte array.");
            }
        }
    }
}