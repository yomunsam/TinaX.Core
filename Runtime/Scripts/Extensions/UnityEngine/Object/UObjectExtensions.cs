using System;
using UObject = UnityEngine.Object;

namespace TinaX
{
    public static class UObjectExtensions
    {
        public static void DontDestroyOnLoad(this UObject selfObj)
            => UObject.DontDestroyOnLoad(selfObj);

        public static void Destroy(this UObject selfObj)
            => UObject.Destroy(selfObj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selfObj"></param>
        /// <param name="delay">The optional amount of time to delay before destroying the object.</param>
        public static void Destroy(this UObject selfObj, float delay)
            => UObject.Destroy(selfObj, delay);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selfObj"></param>
        /// <param name="delay">The optional amount of time to delay before destroying the object.</param>
        public static void Destroy(this UObject selfObj, TimeSpan delay)
            => UObject.Destroy(selfObj, (float)delay.TotalSeconds);
    }
}
