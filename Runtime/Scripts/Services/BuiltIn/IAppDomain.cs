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
using System;
using TinaX.Services.Builtin.Base;

namespace TinaX.Services
{
    public interface IAppDomain : IBuiltinServiceBase
    {
        bool TryCreateInstance(Type type, out object result, object[] args);
        bool TryCreateInstanceAndInject(Type type, out object result, object[] args);
    }
}
