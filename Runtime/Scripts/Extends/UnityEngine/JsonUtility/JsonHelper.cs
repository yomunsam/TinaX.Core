using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinaX
{
    public static class JsonHelper  
    {
        public static Dictionary<TKey, TValue> FromJsonToDictionary<TKey, TValue>(string jsonContent)
            => JsonUtility.FromJson<Serialization<TKey, TValue>>(jsonContent).ToDictionary();

        public static string DictionaryToJson<TKey, TValue>(ref Dictionary<TKey, TValue> dict)
            => JsonUtility.ToJson(new Serialization<TKey, TValue>(dict));

        public static string ListToJson<T>(ref List<T> list)
            => JsonUtility.ToJson(new Serialization<T>(list));

        public static List<T> FromJsonToList<T>(string jsonContent)
            => JsonUtility.FromJson<Serialization<T>>(jsonContent).ToList();

    }
}
    