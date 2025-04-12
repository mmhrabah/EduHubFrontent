using Rabah.Utils;
using UnityEngine;

namespace Rabah.Scenes.Definitions.UI
{
    public class BackButtonCustomEvents : MonoBehaviour
    {
        public void OnBackInFirstScreen()
        {
            SceneNavigation.Instance.LoadScene(AppScenesNames.Definitions);
        }
    }
}