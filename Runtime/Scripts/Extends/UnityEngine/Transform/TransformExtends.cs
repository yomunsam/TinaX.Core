using UnityEngine;

namespace TinaX
{
    public static class TransformExtends
    {
        /// <summary>
        /// 获取Component，如果不存在就新建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponentOrAdd<T>(this Transform obj) where T : Component
        {
            var t = obj.GetComponent<T>();

            if (t == null)
                t = obj.gameObject.AddComponent<T>();

            return t;
        }

        /// <summary>
        /// 获取Component，如果不存在就新建
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">Component type</param>
        /// <returns></returns>
        public static Component GetComponentOrAdd(this Transform obj, System.Type type)
        {
            var c = obj.GetComponent(type);
            if (c == null)
                c = obj.gameObject.AddComponent(type);
            return c;
        }
    }
}
