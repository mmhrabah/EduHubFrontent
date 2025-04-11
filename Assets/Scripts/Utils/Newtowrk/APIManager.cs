using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace Rabah.Utils.Network
{
    public class APIManager : BaseSingleton<APIManager>
    {
        [SerializeField]
        private NetworkData networkData;
        public event Action<UnityWebRequest> OnRequestSent;
        private string baseUrl = "";

        public void Get<T>(string endpoint, Action<T> onSuccess, Action<string> onFailure, Action onSend = null, bool fixResponse = false)
        {
            baseUrl = networkData.baseURL;
            UnityWebRequest request = UnityWebRequest.Get(baseUrl + endpoint);
            onSend?.Invoke();
            StartCoroutine(
                SendRequest(
                    request,
                    (response) =>
                    {
                        T data = JsonConvert.DeserializeObject<T>(response);
                        onSuccess?.Invoke(data);
                    },
                    onFailure,
                    fixResponse
                    )
                );
        }

        public void Post<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onFailure, Action onSend = null)
        {
            baseUrl = networkData.baseURL;
            string jsonData = JsonConvert.SerializeObject(data);
            print(jsonData);
            UnityWebRequest request = UnityWebRequest.Post(baseUrl + endpoint, jsonData, "Content-Type");
            onSend?.Invoke();
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(
                SendRequest(
                    request,
                    (response) =>
                    {
                        T data = JsonConvert.DeserializeObject<T>(response);
                        onSuccess?.Invoke(data);
                    },
                    onFailure
                    )
                );
        }

        public void Put<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onFailure, Action onSend = null, bool fixResponse = false)
        {
            baseUrl = networkData.baseURL;
            string jsonData = JsonConvert.SerializeObject(data);
            UnityWebRequest request = UnityWebRequest.Put(baseUrl + endpoint, jsonData);
            onSend?.Invoke();
            request.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(
                SendRequest(
                request,
                (response) =>
                {
                    T data = JsonConvert.DeserializeObject<T>(response);
                    onSuccess?.Invoke(data);
                },
                onFailure,
                fixResponse
                )
            );
        }

        public void Delete<T>(string endpoint, Action<T> onSuccess, Action<string> onFailure, Action onSend = null)
        {
            baseUrl = networkData.baseURL;
            UnityWebRequest request = UnityWebRequest.Delete(baseUrl + endpoint);
            onSend?.Invoke();
            StartCoroutine(SendRequest(request, (response) =>
            {
                T data = JsonConvert.DeserializeObject<T>(response);
                onSuccess?.Invoke(data);
            },
            onFailure
                )
            );
        }

        private IEnumerator SendRequest(UnityWebRequest request, Action<string> onSuccess, Action<string> onFailure, bool fixResponse = false)
        {
            OnRequestSent?.Invoke(request);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (fixResponse)
                {
                    string responseText = request.downloadHandler.text;
                    string fixedResponse = "{\r\n    \"data\":" + responseText + "\r\n}";
                    onSuccess?.Invoke(fixedResponse);
                }
                else
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
            }
            else
            {
                UnityEngine.Debug.LogError(request.error + " " + request.downloadHandler.text);
                onFailure?.Invoke(request.error + " " + request.downloadHandler.text);
            }
        }
    }
}