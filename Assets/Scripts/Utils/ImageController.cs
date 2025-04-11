using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Rabah.Utils
{
#if UNITY_STANDALONE || UNITY_EDITOR
    using SFB; // Standalone File Browser (for Windows/macOS)
#elif UNITY_ANDROID
using NativeGallery; // NativeGallery Plugin for Android
#endif

    public class ImageController : MonoBehaviour
    {
        [SerializeField]
        private Button pickImageButton; // Button to pick image from gallery
        [SerializeField]
        private Sprite defaultImage; // Default image to show when no image is selected
        [SerializeField]
        private Color defaultColor; // Default color to show when no image is selected
        public Image displayImage; // UI RawImage to show the selected image

        private void Awake()
        {
            pickImageButton.onClick.AddListener(PickImage);
        }

        public void PickImage()
        {
            PickImageAndroid();
            // #if UNITY_STANDALONE || UNITY_EDITOR
            //             // PickImageStandalone();
            //             PickImageAndroid();
            // #elif UNITY_ANDROID
            //         PickImageAndroid();
            // #endif
        }

        // Picking Image for Windows/macOS/Linux
        private void PickImageStandalone()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", "png,jpg,jpeg", false);
            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                LoadAndApplyImage(paths[0]);
            }
        }

        // Picking Image for Android
        private void PickImageAndroid()
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                if (!string.IsNullOrEmpty(path))
                {
                    LoadAndApplyImage(path);
                }
            }, "Select an image", "image/*");
        }
        // Load image as Texture2D and apply it to UI
        private void LoadAndApplyImage(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            if (displayImage != null)
            {
                displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

        public void ResetImage()
        {
            displayImage.sprite = defaultImage;
            displayImage.color = defaultColor;
        }

        public Texture2D GetImage()
        {
            var tex = displayImage.sprite.texture;
            return tex;
        }

        public void SetImage(byte[] image)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(image);

            if (displayImage != null)
            {
                displayImage.color = Color.white;
                displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}