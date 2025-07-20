using System;
using Crosstales.FB;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using TMPro;
using UnityEngine;

namespace Rabah.UI.MainComponents
{

    public class FilePickerUIElement : UIElement
    {
        [SerializeField]
        private string Extension = "*";
        [SerializeField]
        private TMP_Text fileNameText;
        private string filePath = "";


        private void OnEnable()
        {
            FileBrowser.Instance.OnOpenFilesComplete += onOpenFilesComplete;
        }

        private void OnDisable()
        {
            if (FileBrowser.Instance != null)
                FileBrowser.Instance.OnOpenFilesComplete -= onOpenFilesComplete;
        }

        public void OpenFile()
        {
            FileBrowser.Instance.OpenSingleFileAsync(Extension);
        }
        private void onOpenFilesComplete(bool selected, string singlefile, string[] files)
        {
            fileNameText.text = selected ? singlefile : "No file selected";
            UIManager.Instance.ShowLoading();
            StartCoroutine(FileUploadDownloaderManager.Instance.UploadFile(singlefile,
                    (url) =>
                    {
                        Debug.Log("File uploaded successfully: " + url);
                        filePath = url;
                        UIManager.Instance.HideLoading();
                    },
                    (error) =>
                    {
                        UIManager.Instance.HideLoading();
                        Debug.LogError("File upload failed: " + error);
                    }));
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
                if (FileBrowser.Instance != null)
                    FileBrowser.Instance.OnOpenFilesComplete -= onOpenFilesComplete;
            }
        }

        public void SetSelectedFile(string filePath)
        {
            this.filePath = filePath;
            fileNameText.text = System.IO.Path.GetFileName(filePath);
        }
    }
}