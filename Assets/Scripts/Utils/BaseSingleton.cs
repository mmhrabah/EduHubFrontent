using UnityEngine;

namespace Rabah.Utils
{
    // Generic Singleton Class
    public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        [SerializeField]
        private bool dontDestroyOnLoad = true;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        BaseSingleton<T> baseSingleton = singletonObject as BaseSingleton<T>;
                        _instance = singletonObject.AddComponent<T>();
                        if (baseSingleton.dontDestroyOnLoad)
                        {
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}