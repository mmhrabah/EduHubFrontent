using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Rabah.Utils.ObjectIdGenerator
{
    [ExecuteAlways]
    public class GenerateGuid : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private string guid;

        public string Guid => guid; // Public getter to expose the GUID without allowing editing

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(guid))
            {
                GenerateNewGuid();
            }
        }

        private void Reset()
        {
            GenerateNewGuid();
        }

        private void GenerateNewGuid()
        {
            guid = System.Guid.NewGuid().ToString();
            Debug.Log($"Generated GUID: {guid} for {gameObject.name}");
            EditorUtility.SetDirty(this); // Mark the object as dirty to save the change
        }
#endif
    }
}