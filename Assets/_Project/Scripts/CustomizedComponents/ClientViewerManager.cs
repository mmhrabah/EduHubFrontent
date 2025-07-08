using System;
using System.Collections.Generic;
using Rabah.GeneralDataModel;
using Rabah.PrefabRaw;
using UnityEngine;

namespace Rabah.CustomizedComponents
{

    public class ClientViewerManager : MonoBehaviour
    {
        [SerializeField]
        private Transform userDetailsPrefabsParent;
        [SerializeField]
        private ClientPrefabDataRaw contentDetailsPrefab;

        public void ShowClientDetails(List<Client> clients, Action<Client> onDeleteSuccess)
        {
            // Clear existing content details
            foreach (Transform child in userDetailsPrefabsParent)
            {
                Destroy(child.gameObject);
            }

            // Instantiate new content details
            foreach (var client in clients)
            {
                var clientDetails = Instantiate(contentDetailsPrefab, userDetailsPrefabsParent);
                clientDetails.SetUserItemDetails(client, onDeleteSuccess);
            }
        }
    }
}