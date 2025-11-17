using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Malgo.Utilities.DataStructure
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [Serializable]
        private struct KeyValue
        {
            public TKey key;
            public TValue value;
        }

        [SerializeField, TableList]
        private List<KeyValue> list = new List<KeyValue>();

        private Dictionary<TKey, TValue> dict;

        /// <summary>
        /// Lazily converts list to dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            if (dict == null)
            {
                dict = new Dictionary<TKey, TValue>();
                foreach (var kv in list)
                {
                    if (!dict.ContainsKey(kv.key))
                        dict.Add(kv.key, kv.value);
                    else
                        Debug.LogWarning($"Duplicate key {kv.key} found in SerializableDictionary");
                }
            }
            return dict;
        }

        /// <summary>
        /// Optional: expose indexer-like access.
        /// </summary>
        public TValue this[TKey key] => ToDictionary()[key];
    }

    // How to use
    // [Serializable]
    // public class WeaponMovesetDictionary : SerializableDictionary<WeaponType, WeaponMovesetSO>
    // {
    // }   
}
