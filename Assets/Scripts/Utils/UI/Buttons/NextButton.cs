using Michsky.MUIP;
using Rabah.Utils.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Rabah.Scenes.Definitions.UI
{
    public class NextButton : MonoBehaviour
    {
        [SerializeField] private NotificationManager notificationManager;
        [SerializeField] private UnityEvent onClickInSreenWithNoneNext;
        public void NextButtonOnClickEvent()
        {
            if (Utils.UI.UIManager.Instance.CurrentScreen.IsScreenDataValid())
            {
                ScreenHandle next = Utils.UI.UIManager.Instance.CurrentScreen.Next;
                if (next == ScreenHandle.None)
                {
                    onClickInSreenWithNoneNext?.Invoke();
                    return;
                }
                Utils.UI.UIManager.Instance.OpenScreen(next);
            }
            else
            {
                notificationManager.title = "Missing Data";
                notificationManager.description = "To go to next steps you must have at least one element in each Definition";
                notificationManager.UpdateUI();
                notificationManager.Open();
            }
        }
    }
}
