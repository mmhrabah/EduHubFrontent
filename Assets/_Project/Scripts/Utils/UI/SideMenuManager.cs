using DG.Tweening;
using UnityEngine;

namespace Rabah.Utils.UI
{
    public class SideMenuManager : BaseSingleton<SideMenuManager>
    {
        [SerializeField] private RectTransform sideMenu;

        private bool isMenuOpen = false;
        private Vector2 sideMenuOpenedPosition;
        public bool IsMenuOpen => isMenuOpen;

        protected override void Awake()
        {
            base.Awake();
            sideMenuOpenedPosition = sideMenu.anchoredPosition;
            sideMenu.anchoredPosition = new Vector2(-sideMenu.rect.width, sideMenu.anchoredPosition.y);
        }
        private void Start()
        {

        }

        private void Update()
        {

        }

        public void OpenSideMenu()
        {
            if (isMenuOpen) return;
            sideMenu.DOAnchorPos(sideMenuOpenedPosition, 0.5f).OnComplete(() =>
            {
                isMenuOpen = true;
            });
        }

        public void CloseSideMenu()
        {
            if (!isMenuOpen) return;
            sideMenu.DOAnchorPos(new Vector2(-sideMenu.rect.width, sideMenu.anchoredPosition.y), 0.5f).OnComplete(() =>
            {
                isMenuOpen = false;
            });
        }

        public void ToggleSideMenu()
        {
            if (isMenuOpen)
            {
                CloseSideMenu();
            }
            else
            {
                OpenSideMenu();
            }
        }
    }
}