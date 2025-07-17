using System;
using Rabah.GeneralDataModel;
using Rabah.Screens;
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
            var nextSscreen = client.Status == UserStatus.Pending
                ? ScreenHandle.ApproveClientScreen
                : ScreenHandle.EditClientScreen;

            ScreenData nextScreenData = null;
            nextScreenData = nextSscreen == ScreenHandle.ApproveClientScreen
                ? new ApproveClientScreenData { Client = client }
                : new EditClientScreenData { Client = client };
            UIManager.Instance.OpenScreen(
                nextSscreen,
                data: nextScreenData);
            // UIManager.Instance.ShowComingSoonModal();
        }

        private void OnDeleteButtonClicked()
        {
            UIManager.Instance.ShowComingSoonModal();
        }

        public void SetUserItemDetails(Client client, Action<Client> onDeleteSuccess)
        {
            this.client = client;
            usernameText.text = client.Name;
            if (client.SubscriptionStartDate.Year < 2000 || client.SubscriptionEndDate.Year < 2000)
            {
                startDateText.text = "N/A";
                endDataText.text = "N/A";
            }
            else
            {
                startDateText.text = client.SubscriptionStartDate.ToString("yyyy-MM-dd");
                endDataText.text = client.SubscriptionEndDate.ToString("yyyy-MM-dd");
            }
            if (client.Status == UserStatus.Registered)
            {
                subscriptionStatusText.text = client.GetSubscriptionStatusText();
            }
            else
            {
                subscriptionStatusText.text = client.Status.ToString();
            }
            this.onDeleteSuccess = onDeleteSuccess;
        }
    }
}