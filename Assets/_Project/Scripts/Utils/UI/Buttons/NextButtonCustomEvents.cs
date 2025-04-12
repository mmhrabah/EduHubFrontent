using Rabah.Utils;
using UnityEngine;

namespace Rabah.Scenes.Definitions.UI
{
    public class NextButtonCustomEvents : MonoBehaviour
    {
        public void OnNextInLastScreen()
        {
            SceneNavigation.Instance.LoadScene(AppScenesNames.ApplySubItemsOn3D);
        }

        public void OnNextInApplyItemsOnActivities3D()
        {
            SceneNavigation.Instance.LoadScene(AppScenesNames.ApplyItemsOnActivities3D);
        }

        public void OnNextInDefinitionDrawingAndJobOrders()
        {
            SceneNavigation.Instance.LoadScene(AppScenesNames.Definitions);
        }
    }
}
