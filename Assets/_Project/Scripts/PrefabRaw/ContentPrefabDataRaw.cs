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
            UIManager.Instance.OpenScreen(ScreenHandle.EditContentScreen,
            data: new EditContentScreenData
            {
                Content = content
            });
            // UIManager.Instance.ShowComingSoonModal();
        }

        private void OnDeleteButtonClicked()
        {
            APIManager.Instance.Delete<string>($"content/{content.Id}",
                onSend: () =>
                {
                    UIManager.Instance.ShowLoading();
                },
                onSuccess: (response) =>
                {
                    Debug.Log("Content deleted successfully.");
                    UIManager.Instance.HideLoading();
                    onDeleteSuccess?.Invoke(content);
                },
                onFailure: (error) =>
                {
                    Debug.LogError($"Failed to delete content: {error}");
                    UIManager.Instance.HideLoading();
                    UIManager.Instance.ShowNotificationModal(
                        title: "Error",
                        descriptionText: error,
                        icon: null);
                },
                mustParse: false);
            // UIManager.Instance.ShowComingSoonModal();
        }

        public void SetContentItemDetails(Content content, Action<Content> onDeleteSuccess)
        {
            this.content = content;
            this.onDeleteSuccess = onDeleteSuccess;
            titleText.text = content.Name;
            typeText.text = ContentTypeMapping.GetContentTypeName(content.TypeId);
            versionText.text = content.Version;
        }
    }
}