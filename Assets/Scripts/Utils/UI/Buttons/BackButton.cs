using Rabah.Utils.UI;
using UnityEngine;
using UnityEngine.Events;


namespace Rabah.Scenes.Definitions.UI
{
    public class BackButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent onClickInSreenWithNonePrevious;
        public void BackButtonOnClickEvent()
        {
            ScreenHandle previous = UIManager.Instance.CurrentScreen.Previous;
            if (previous == ScreenHandle.None)
            {
                onClickInSreenWithNonePrevious?.Invoke();
                return;
            }
            UIManager.Instance.OpenScreen(previous);
        }
    }
}