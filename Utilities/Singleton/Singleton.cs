using System;
using UnityEngine;

namespace Malgo.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public void Awake()
        {
            var objects = FindObjectsByType<T>(FindObjectsSortMode.None);
            bool continueAwake = true;

            if (objects.Length > 1)
            {
                Destroy(this.gameObject);
                continueAwake = false;
            }

            GetInstance();
            if (continueAwake)
            {
                Init();
            }
        }

        public abstract void Init();

        public static T Instance
        {
            get { return _instance; }

            private set { _instance = value; }
        }

        private static void GetInstance()
        {
            if (_instance != null)
            {
                return;
            }

            var type = typeof(T);
            var attribute = Attribute.GetCustomAttribute(type, typeof(SingletonAttribute)) as SingletonAttribute;

            var objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (objects.Length > 0)
            {
                _instance = objects[0];
                if (objects.Length > 1)
                {
                    Debug.LogWarning("There is more than one instance of Singleton of type \"" + type +
                                     "\". Keeping the first. Destroying the others.");
                    for (var i = 1; i < objects.Length; i++) DestroyImmediate(objects[i].gameObject);
                }

                if (attribute != null && attribute.IsDontDestroy)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return;
            }

            if (attribute == null)
            {
                Debug.LogError(type + "class does not have SingletonAttribute ! Please add SingletonAttribute for " +
                               type);
                Instance = null;
                return;
            }

            if (string.IsNullOrEmpty(attribute.Name))
            {
                Debug.LogError("Cannot find prefab of " + type);
                Instance = null;
                return;
            }

            GameObject prefab = Resources.Load(attribute.Name) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("Cannot find prefab of " + type + "! Put prefab of" + type + " into Resources folder");
                Instance = null;
                return;
            }

            GameObject gameObject = Instantiate(prefab);
            _instance = gameObject.GetComponent<T>();
            gameObject.name = type.ToString();
        }
    }
}