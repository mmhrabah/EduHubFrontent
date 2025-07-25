using System;
using System.Collections.Generic;
using Rabah.GeneralDataModel;
using Rabah.PrefabRaw;
using UnityEngine;

namespace Rabah.CustomizedComponents
{

    public class ContentViewerManager : MonoBehaviour
    {
        [SerializeField]
        private Transform userDetailsPrefabsParent;
        [SerializeField]
        private ContentPrefabDataRaw contentDetailsPrefab;

        public void ShowContentDetails(List<Content> contents, Action<Content> onDeleteSuccess)
        {
            // Clear existing content details
            foreach (Transform child in userDetailsPrefabsParent)
            {
                Destroy(child.gameObject);
            }

            // Instantiate new content details
            foreach (var content in contents)
            {
                var contentDetails = Instantiate(contentDetailsPrefab, userDetailsPrefabsParent);
                contentDetails.SetContentItemDetails(content, onDeleteSuccess);
            }
        }
    }
}