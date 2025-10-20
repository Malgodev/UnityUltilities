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

        public SingletonAttribute(
            string name = null,
            bool isDontDestroy = false,
            SingletonDestroyStrategy destroyStrategy = SingletonDestroyStrategy.DestroyThis)
        {
            Name = name;
            IsDontDestroy = isDontDestroy;
            DestroyStrategy = destroyStrategy;
        }
    }
}