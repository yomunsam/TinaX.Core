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
using UnityEngine;

namespace TinaX.Services
{
    public interface ILocalizationService
    {
        IEnumerable<SystemLanguage> GetCurrentLanguages();
        string GetText(string key, string group, string defaultValue = null);
    }
}
