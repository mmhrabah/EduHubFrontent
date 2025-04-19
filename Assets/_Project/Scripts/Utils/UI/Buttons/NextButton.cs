using Michsky.MUIP;
using Rabah.Utils.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Rabah.Scenes.Definitions.UI
{
    public class NextButton : MonoBehaviour
    {
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
                Utils.UI.UIManager.Instance.ShowNotificationModal(
                    "Missing Data",
                    "To go to next steps you must have at least one element in each Definition",
                    null
                );
            }
        }
    }
}
