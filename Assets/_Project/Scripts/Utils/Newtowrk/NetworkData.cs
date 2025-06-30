using UnityEngine;

namespace Rabah.Utils.Network
{
    [CreateAssetMenu(fileName = "NetworkData", menuName = "ScriptableObjects/NetworkData", order = 1)]
    public class NetworkData : ScriptableObject
    {
        public string baseURL;
        public string uploadUrl;
        public string downloadUrl;
    }
}