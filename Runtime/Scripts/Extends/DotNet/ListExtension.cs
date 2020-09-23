/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <me@yomunchan.moe> <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System.Collections.Generic;

namespace TinaX
{
    public static class ListExtension
    {
        /// <summary>
        /// if key not exist, add it to list and return 'true',  or else return 'false'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="addItem"></param>
        /// <returns></returns>
        public static bool AddIfNotExist<T>(this IList<T> list, T addItem)
        {
            if (list.Contains(addItem)) return false;
            list.Add(addItem);
            return true;
        }
    }
}
