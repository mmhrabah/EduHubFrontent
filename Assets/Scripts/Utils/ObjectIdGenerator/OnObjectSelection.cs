using UnityEngine;

namespace Rabah.Utils.ObjectIdGenerator
{
    [RequireComponent(typeof(GenerateGuid))]
    public class OnObjectSelection : MonoBehaviour
    {
        private GenerateGuid generateGuid;

        private void Awake()
        {
            generateGuid = GetComponent<GenerateGuid>();
        }

        private void OnMouseDown()
        {
            print($"Name : {name}   ,  guid : {generateGuid.Guid}");
        }
    }
}