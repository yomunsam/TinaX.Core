/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */
using System.Collections.Generic;

namespace TinaX
{
    public interface ICommandLineArgs
    {
        IDictionary<string, string> GetAllKeyValueArgs();
        IList<string> GetAllSingleArgs();

        bool GetBool(string key);
        string GetValue(string key, string defaultValue = null);
        bool IsExistKey(string key);
        bool IsExistSingle(string name);
        bool TryGetValue(string key, out string value);
    }
}
