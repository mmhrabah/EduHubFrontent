using System;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rabah.PrefabRaw
{
    public class ClientPrefabDataRaw : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text usernameText;
        [SerializeField]
        private TMP_Text startDateText;
        [SerializeField]
        private TMP_Text endDataText;
        [SerializeField]
        private TMP_Text subscriptionStatusText;
        [SerializeField]
        private Button editButton;
        [SerializeField]
        private Button deleteButton;


        private Client client;
        private Action<Client> onDeleteSuccess;
        private void Awake()
        {
            editButton.onClick.AddListener(OnEditButtonClicked);
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        }

        private void OnEditButtonClicked()
        {
            // UIManager.Instance.OpenScreen<EditContentScreen>(new EditContentScreenData
            // {
            //     user = user
            // });
            UIManager.Instance.ShowComingSoonModal();
        }

        private void OnDeleteButtonClicked()
        {
            // APIManager.Instance.Delete<string>($"content/{content.Id}",
            //     onSuccess: (response) =>
            //     {
            //         Debug.Log("Content deleted successfully.");
            //         onDeleteSuccess?.Invoke(content);
            //     },
            //     onFailure: (error) =>
            //     {
            //         Debug.LogError($"Failed to delete content: {error}");
            //         UIManager.Instance.ShowNotificationModal(
            //             title: "Error",
            //             descriptionText: error,
            //             icon: null);
            //     });
            UIManager.Instance.ShowComingSoonModal();
        }

        public void SetUserItemDetails(Client client, Action<Client> onDeleteSuccess)
        {
            this.client = client;
            usernameText.text = client.Name;
            startDateText.text = client.SubscriptionStartDate.ToString("yyyy-MM-dd");
            endDataText.text = client.SubscriptionEndDate.ToString("yyyy-MM-dd");
            subscriptionStatusText.text = client.GetSubscriptionStatusText();
            this.onDeleteSuccess = onDeleteSuccess;
        }
    }
}