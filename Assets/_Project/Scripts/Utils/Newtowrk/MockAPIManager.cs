using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Rabah.Utils;
using Rabah.Utils.Network;

namespace Assets._Project.Scripts.Utils.Newtowrk
{
    public class MockAPIManager : BaseSingleton<MockAPIManager>
    {
        [SerializeField]
        private NetworkData networkData;
        public event Action<UnityWebRequest> OnRequestSent;
        public event Action OnRequestSentWithoutRequest;
        private string baseUrl = "";

        public void Get<T>(ResponseModel<T> fakeResponse, Action<T> onSuccess, Action<string> onFailure, Action onSend = null, bool fixResponse = false)
        {
            onSend?.Invoke();
            StartCoroutine(
                SendRequest(
                    fakeResponse,
                    onSuccess,
                    onFailure,
                    fixResponse
                    )
                );
        }

        public void Post<T, Y>(ResponseModel<T> fakeResponse, object data, Action<Y> onSuccess, Action<string> onFailure, Action onSend = null)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            print(jsonData);
            onSend?.Invoke();
            StartCoroutine(
                SendRequest(
                    fakeResponse,
                    onSuccess,
                    onFailure
                    )
                );
        }

        public void Put<T>(ResponseModel<T> fakeResponse, object data, Action<string> onSuccess, Action<string> onFailure, Action onSend = null)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            print(jsonData);
            onSend?.Invoke();
            StartCoroutine(
                SendRequest(
                    fakeResponse,
                    onSuccess,
                    onFailure
                    )
                );
        }

        public void Delete<T>(ResponseModel<T> fakeResponse, Action<T> onSuccess, Action<string> onFailure, Action onSend = null)
        {
            string jsonData = JsonConvert.SerializeObject(fakeResponse);
            print(jsonData);
            onSend?.Invoke();
            StartCoroutine(
                SendRequest(
                    fakeResponse,
                    onSuccess,
                    onFailure
                    )
                );
        }


        private IEnumerator SendRequest<T, Y>(ResponseModel<T> fakeResponse, Action<Y> onSuccess, Action<string> onFailure, bool simulateSuccess = true)
        {
            OnRequestSentWithoutRequest?.Invoke();

            yield return new WaitForSeconds(0.9f); // Simulate a delay

            if (simulateSuccess)
            {
                var responseJson = JsonConvert.SerializeObject(fakeResponse);
                var response = JsonConvert.DeserializeObject<Y>(responseJson);
                onSuccess?.Invoke(response);
            }
            else
            {
                onFailure?.Invoke("Simulated failure");
            }
        }
    }
}