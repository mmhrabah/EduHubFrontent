using System;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rabah.PrefabRaw
{
    public class ContentPrefabDataRaw : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleText;
        [SerializeField]
        private TMP_Text typeText;
        [SerializeField]
        private TMP_Text versionText;
        [SerializeField]
        private Button editButton;
        [SerializeField]
        private Button deleteButton;


        private Content content;
        private Action<Content> onDeleteSuccess;
        private void Awake()
        {
            editButton.onClick.AddListener(OnEditButtonClicked);
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        }

        private void OnEditButtonClicked()
        {
            // UIManager.Instance.OpenScreen<EditContentScreen>(new EditContentScreenData
            // {
            //     content = content
            // });
        }

        private void OnDeleteButtonClicked()
        {
            APIManager.Instance.Delete<string>($"content/{content.Id}",
                onSuccess: (response) =>
                {
                    Debug.Log("Content deleted successfully.");
                    onDeleteSuccess?.Invoke(content);
                },
                onFailure: (error) =>
                {
                    Debug.LogError($"Failed to delete content: {error}");
                    UIManager.Instance.ShowNotificationModal(
                        title: "Error",
                        descriptionText: error,
                        icon: null);
                });
        }

        public void SetContentItemDetails(Content content, Action<Content> onDeleteSuccess)
        {
            this.content = content;
            this.onDeleteSuccess = onDeleteSuccess;
            titleText.text = content.Name;
            typeText.text = content.Type.ToString();
            versionText.text = content.Version;
        }
    }
}