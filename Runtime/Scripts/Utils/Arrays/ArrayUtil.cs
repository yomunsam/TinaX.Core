using System;
using System.Linq;

namespace TinaX.Core.Utils.Arrays
{
    public static class ArrayUtil
    {
        /// <summary>
        /// combine two array. | 拼合两个数组
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public static T[] Combine<T>(T[] arr1, T[] arr2) //代码参考CatLib: https://github.com/CatLib/Core/blob/2.0/src/CatLib.Core/Util/Arr.cs
        {
            arr1 = arr1 ?? Array.Empty<T>();
            if (arr2 == null || arr2.Length <= 0)
                return arr1;

            Array.Resize(ref arr1, arr1.Length + arr2.Length);
            Array.Copy(arr2, 0, arr1, arr1.Length - arr2.Length, arr2.Length);
            return arr1;
        }

        /// <summary>
        /// combine two array. | 拼合两个数组
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <returns></returns>
        public static void Combine<T>(ref T[] arr1, T[] arr2) //代码参考CatLib: https://github.com/CatLib/Core/blob/2.0/src/CatLib.Core/Util/Arr.cs
        {
            arr1 = arr1 ?? Array.Empty<T>();
            if (arr2 == null || arr2.Length <= 0)
                return;

            Array.Resize(ref arr1, arr1.Length + arr2.Length);
            Array.Copy(arr2, 0, arr1, arr1.Length - arr2.Length, arr2.Length);
        }

        public static void RemoveDuplicationElements<T>(ref T[] arr)
        {
            arr = arr.Distinct().ToArray();
        }
    }
}
