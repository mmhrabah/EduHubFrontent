using UnityEngine;
using System;
using DG.Tweening;

namespace Rabah.Utils.UI
{
    public class ScreenButtonNavigator : MonoBehaviour
    {
        [SerializeField]
        private ScreenHandle goToScreenHandle;


        public void NavigateToScreen()
        {
            if (goToScreenHandle == ScreenHandle.None)
            {
                Debug.LogWarning("No screen specified to navigate to.");
                return;
            }
            // Assuming UIManager has a method to open screens
            UIManager.Instance.OpenScreen(goToScreenHandle);
        }
    }
}