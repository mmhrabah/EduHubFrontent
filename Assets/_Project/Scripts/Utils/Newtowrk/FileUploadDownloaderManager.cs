using System;
using System.Collections;
using System.IO;
using Rabah.GeneralDataModel;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Rabah.Utils.Network
{
    public class FileUploadDownloaderManager : BaseSingleton<FileUploadDownloaderManager>
    {
        [SerializeField]
        private NetworkData networkData;

        // Upload an image
        public IEnumerator UploadFile(string filePath, Action<string> onSuccess = null, Action<string> onError = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is empty or null.");
                yield break;
            }

            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError("File does not exist at path: " + filePath);
                yield break;
            }
            {
                // Create a UnityWebRequest for file upload
                byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                WWWForm form = new WWWForm();
                form.AddBinaryData("file", fileData, Path.GetFileName(filePath), GetMimeType(filePath));

                UnityWebRequest www = UnityWebRequest.Post(networkData.uploadUrl, form);
                www.SetRequestHeader("Authorization", "Bearer " + Session.Session.AccessToken);

                // Wait for the request to complete
                yield return www.SendWebRequest();

                // Check for errors
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Upload failed: " + www.error);
                    if (onError != null)
                    {
                        onError(www.error);
                    }
                }
                else
                {
                    FileUploadResponse response = JsonConvert.DeserializeObject<FileUploadResponse>(www.downloadHandler.text);
                    Debug.Log("Image uploaded successfully. URL: " + response.FileUrl);
                    onSuccess?.Invoke(response.FileUrl);
                }
            }
        }

        // Download an image
        public IEnumerator DownloadImage(string fileName)
        {
            string imageUrl = networkData.downloadUrl + fileName;

            UnityWebRequest www = UnityWebRequest.Get(imageUrl);

            // Wait for the request to complete
            www.SetRequestHeader("Authorization", "Bearer " + Session.Session.AccessToken);
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Download failed: " + www.error);
            }
            else
            {
                // On success, save the image to local storage or use it in your app
                byte[] imageData = www.downloadHandler.data;
                string filePath = Application.persistentDataPath + "/DownloadedImage.png";
                System.IO.File.WriteAllBytes(filePath, imageData);
                Debug.Log("Image downloaded successfully to: " + filePath);
            }
        }

        private string GetMimeType(string filePath)
        {
            string fileType = filePath.Substring(filePath.LastIndexOf('.') + 1);
            switch (fileType.ToLower())
            {
                case "epub": return "application/epub+zip";
                case "html": return "text/html";
                case "mp4": return "video/mp4";
                case "mp3": return "audio/mpeg";
                default: return "application/octet-stream";
            }
        }
    }
}