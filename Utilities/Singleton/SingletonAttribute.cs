using System;
using UnityEngine;

namespace Malgo.Singleton
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SingletonAttribute : Attribute
    {
        public enum SingletonDestroyStrategy
        {
            DestroyThis,   // Destroy the new duplicate
            DestroyOthers  // Destroy existing ones, keep this
        }

        // public enum SingletonDestroyLifecycle
        // {
        //     OnDisable = 1,
        //     OnDestroy = 2,
        // }

        /// <summary>
        /// Prefab name to auto-load from Resources (optional)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether to mark as DontDestroyOnLoad
        /// </summary>
        public bool IsDontDestroy { get; }

        /// <summary>
        /// Strategy when multiple instances exist
        /// </summary>
        public SingletonDestroyStrategy DestroyStrategy { get; }
        
        
        // /// <summary>
        // /// The lifecycle sequence that this game object will be destroyed
        // /// </summary>
        // public SingletonDestroyLifecycle DestroyLifecycle { get; }

        public SingletonAttribute(
            string name = null,
            bool isDontDestroy = false,
            SingletonDestroyStrategy destroyStrategy = SingletonDestroyStrategy.DestroyThis)
            // SingletonDestroyLifecycle destroyLifecycle = SingletonDestroyLifecycle.OnDisable)
        {
            Name = name;
            IsDontDestroy = isDontDestroy;
            DestroyStrategy = destroyStrategy;
            // DestroyLifecycle = destroyLifecycle;
        }
    }
}