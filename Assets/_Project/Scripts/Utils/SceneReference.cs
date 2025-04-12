using UnityEngine;

namespace Rabah.Utils
{
    [System.Serializable]
    public class SceneReference
    {
        [SerializeField] private Object sceneAsset = null; // Reference to the scene asset
        [SerializeField] private string scenePath = string.Empty; // Store the path for runtime use

        public string ScenePath => scenePath;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sceneAsset != null)
            {
                // Ensure the selected object is a SceneAsset
                if (UnityEditor.AssetDatabase.GetAssetPath(sceneAsset).EndsWith(".unity"))
                {
                    scenePath = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
                }
                else
                {
                    sceneAsset = null;
                    scenePath = string.Empty;
                    Debug.LogWarning("Please assign a valid scene asset.");
                }
            }
            else
            {
                scenePath = string.Empty;
            }
        }
#endif
    }
}