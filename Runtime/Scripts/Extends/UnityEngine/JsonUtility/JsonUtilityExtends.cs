using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinaX
{
    public static class JsonUtilityExtends
    {
        public static string ToJson<TKey, TValue>(this Dictionary<TKey, TValue> dict)
            => JsonUtility.ToJson(new Serialization<TKey, TValue>(dict));

        public static string ToJson<T>(this List<T> list)
            => JsonUtility.ToJson(new Serialization<T>(list));
    }

}
