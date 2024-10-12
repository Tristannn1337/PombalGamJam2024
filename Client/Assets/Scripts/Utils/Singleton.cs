namespace Pombal {
    using UnityEngine;

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        private static T _instance;
        private static bool _isShuttingDown = false;
        private static object _lock = new object();

        public static T Instance {
            get {
                if (_isShuttingDown) {
                    Debug.LogWarning($"[Singleton] Instance of {typeof(T)} already destroyed. Returning null.");
                    return null;
                }

                lock (_lock) {
                    if (_instance == null) {
                        _instance = (T)FindFirstObjectByType(typeof(T));

                        if (_instance == null) {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make the instance persistent across scene loads
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy() {
            _isShuttingDown = true;
        }

        protected virtual void OnApplicationQuit() {
            _isShuttingDown = true;
        }
    }
}
