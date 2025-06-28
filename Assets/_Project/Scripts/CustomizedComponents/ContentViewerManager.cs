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
        private Transform contentDetailsPrefabsParent;
        [SerializeField]
        private ContentPrefabDataRaw contentDetailsPrefab;

        public void ShowContentDetails(List<Content> contents, Action<Content> onDeleteSuccess)
        {
            // Clear existing content details
            foreach (Transform child in contentDetailsPrefabsParent)
            {
                Destroy(child.gameObject);
            }

            // Instantiate new content details
            foreach (var content in contents)
            {
                var contentDetails = Instantiate(contentDetailsPrefab, contentDetailsPrefabsParent);
                contentDetails.SetContentItemDetails(content, onDeleteSuccess);
            }
        }
    }
}