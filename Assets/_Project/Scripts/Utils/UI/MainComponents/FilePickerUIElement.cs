using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AssetsSecuring;
using Crosstales.FB;
using Rabah.Utils.Network;
using Rabah.Utils.Session;
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
        [SerializeField]
        private ZipWithPassword zipWithPassword;


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
            fileNameText.text = selected ? singlefile : "No file selected";
            var guid = Guid.NewGuid();
            string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output");
            string zipPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "epubZipped.zip");
            string outputZipDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OutputUnZipped");
            filePath = singlefile;
            StartHash(guid, 100, outputDir, out var key, out var hmacKey);
            var passwordPlainText = hmacKey + key;
            var secretKey = "6g{^72Eaj+yJm15lAvmrDO.B;wzGSk3D";
            var keyOfZip = secretKey.Substring(0, 16);
            var hmacKeyOfZip = secretKey.Substring(16, 16);
            var passwordCipherText = Encrypt(passwordPlainText, keyOfZip, hmacKeyOfZip);
            CompressFile(outputDir, zipPath, passwordCipherText);
            var passwordAfterDecryption = Encrypt(passwordPlainText, keyOfZip, hmacKeyOfZip);
            DecompressFile(zipPath, outputZipDir, passwordAfterDecryption);
            JoinFiles(guid, outputZipDir, out key, out hmacKey);
            // UIManager.Instance.ShowLoading();
            // StartCoroutine(FileUploadDownloaderManager.Instance.UploadFile(singlefile,
            //         (url) =>
            //         {
            //             Debug.Log("File uploaded successfully: " + url);
            //             filePath = url;
            //             UIManager.Instance.HideLoading();
            //         },
            //         (error) =>
            //         {
            //             UIManager.Instance.HideLoading();
            //             Debug.LogError("File upload failed: " + error);
            //         }));
        }

        public void StartHash(Guid guid, int maxParts, string outputDir, out string key, out string hmacKey)
        {
            var guidString = guid.ToString("N");
            key = guidString.Substring(0, 16); // Ensure key is 16 characters long
            hmacKey = guidString.Substring(16, 16); // Ensure HMAC key is 16 characters long
            FileImportManager.ImportFile(
                filePath,
                maxParts,
                outputDir,
                key,
                hmacKey,
                guid
            );
        }
        public void CompressFile(string outputDir, string zipPath, string password)
        {
            zipWithPassword.CreateEncryptedZipFolder(outputDir, zipPath, password);
        }

        public void JoinFiles(Guid guid, string outputDir, out string key, out string hmacKey)
        {
            var guidString = guid.ToString("N");
            key = guidString.Substring(0, 16); // Ensure key is 16 characters long
            hmacKey = guidString.Substring(16, 16); // Ensure HMAC key is 16 characters long
            FileExportManager.ExportFile(
                    outputDir,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "JoinedFileSuccessfully.epub"),
                    key,
                    hmacKey,
                    guid
                );
        }

        public void DecompressFile(string zipPath, string outputDir, string password)
        {
            zipWithPassword.DecompressEncryptedZip(zipPath, outputDir, password);
        }

        public string Encrypt(string plainText, string Key, string IV)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new())
                using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new(cs))
                {
                    sw.Write(plainText);
                    sw.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
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