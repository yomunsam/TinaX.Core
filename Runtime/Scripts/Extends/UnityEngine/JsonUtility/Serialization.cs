using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinaX
{
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;

        [SerializeField]
        List<TValue> values;

        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary() => this.target;

        public Serialization(Dictionary<TKey,TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; i++) 
            {
                target.Add(keys[i], values[i]);
            }
        }

    }

    [Serializable]
    public class Serialization<T>
    {
        [SerializeField]
#pragma warning disable CA2235 // Mark all non-serializable fields
        List<T> target;
#pragma warning restore CA2235 // Mark all non-serializable fields

        public List<T> ToList() => this.target;

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
}
