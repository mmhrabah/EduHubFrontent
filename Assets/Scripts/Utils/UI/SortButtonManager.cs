using DG.Tweening;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Rabah.Utils.UI
{
    public class SortButtonManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite ascendingSprite;
        [SerializeField]
        private Sprite descendingSprite;
        [SerializeField]
        private ButtonManager sortButton;
        [SerializeField]
        private CanvasGroup sortButtonHintPanel;
        [SerializeField]
        private TMP_Text sortingButtonHintText;


        private UnityEvent onSortAscending = new();
        private UnityEvent onSortDescending = new();
        private bool isAscending = true;

        public UnityEvent OnSortAscending { get => onSortAscending; set => onSortAscending = value; }
        public UnityEvent OnSortDescending { get => onSortDescending; set => onSortDescending = value; }

        private void Awake()
        {
            sortButtonHintPanel.alpha = 0;
            sortButtonHintPanel.interactable = false;
            sortButtonHintPanel.blocksRaycasts = false;
            sortButtonHintPanel.gameObject.SetActive(false);
            sortButton.onClick.AddListener(OnSortButtonClick);
            sortButton.onHover.AddListener(() =>
            {
                if (isAscending)
                {

                    sortButtonHintPanel.DOFade(1, 0.5f).OnPlay(() =>
                    {
                        sortButtonHintPanel.gameObject.SetActive(true);
                        sortingButtonHintText.text = "Sort Desc.";
                    }).OnComplete(() =>
                    {
                        sortButtonHintPanel.DOFade(0, 0.5f).OnComplete(() =>
                        {
                            sortButtonHintPanel.gameObject.SetActive(false);
                        });
                    });
                }
                else
                {
                    sortButtonHintPanel.DOFade(1, 0.5f).OnPlay(() =>
                    {
                        sortButtonHintPanel.gameObject.SetActive(true);
                        sortingButtonHintText.text = "Sort Asc.";
                    }).OnComplete(() =>
                    {
                        sortButtonHintPanel.DOFade(0, 0.5f).OnComplete(() =>
                        {
                            sortButtonHintPanel.gameObject.SetActive(false);
                        });
                    });
                }
            });
        }

        private void OnSortButtonClick()
        {
            isAscending = !isAscending;

            if (isAscending)
            {
                sortButton.SetIcon(descendingSprite);
                OnSortAscending?.Invoke();
            }
            else
            {
                sortButton.SetIcon(ascendingSprite);
                OnSortDescending?.Invoke();
            }
        }
    }
}