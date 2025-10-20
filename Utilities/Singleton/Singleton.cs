using System;
using UnityEngine;

namespace Malgo.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        protected virtual void Awake()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(SingletonAttribute)) as SingletonAttribute;
            
            // Check if instance already exists
            if (_instance != null && _instance != this)
            {
                // Another instance exists
                if (attribute != null && attribute.DestroyStrategy == SingletonAttribute.SingletonDestroyStrategy.DestroyOthers)
                {
                    // Destroy the old instance, keep this one
                    Destroy(_instance.gameObject);
                    _instance = this as T;
                    
                    if (attribute.IsDontDestroy)
                        DontDestroyOnLoad(gameObject);
                    
                    Init();
                }
                else
                {
                    // Default: destroy this, keep existing
                    Destroy(gameObject);
                }
                return;
            }

            // First instance
            _instance = this as T;
            
            if (attribute != null && attribute.IsDontDestroy)
                DontDestroyOnLoad(gameObject);
            
            Init();
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public abstract void Init();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Try to find existing instance in scene
                    _instance = FindAnyObjectByType<T>();
                    
                    if (_instance == null)
                    {
                        // Try to load from resources
                        var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(SingletonAttribute)) as SingletonAttribute;
                        
                        if (attribute == null)
                        {
                            return null;
                        }

                        if (string.IsNullOrEmpty(attribute.Name))
                        {
                            Debug.LogError($"Cannot find prefab name for {typeof(T)}");
                            return null;
                        }

                        GameObject prefab = Resources.Load<GameObject>(attribute.Name);
                        if (prefab == null)
                        {
                            Debug.LogError($"Cannot find prefab '{attribute.Name}' for {typeof(T)}! Place it in Resources folder.");
                            return null;
                        }

                        GameObject go = Instantiate(prefab);
                        go.name = typeof(T).Name;
                        _instance = go.GetComponent<T>();
                        
                        if (_instance == null)
                        {
                            Debug.LogError($"Prefab '{attribute.Name}' doesn't have component {typeof(T)}!");
                            Destroy(go);
                            return null;
                        }
                    }
                }
                
                return _instance;
            }
        }
    }
}