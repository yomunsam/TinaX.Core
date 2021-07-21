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
using System.Threading;
using System.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.Services.Builtin.Base;

namespace TinaX.Services
{
    public interface IAssetService : IBuiltinServiceBase
    {
        #region VFS IO

        T Load<T>(string assetPath) where T : UnityEngine.Object;

        UnityEngine.Object Load(string assetPath, Type type);

        Task<T> LoadAsync<T>(string assetPath, CancellationToken cancellationToken = default) where T : UnityEngine.Object;

        void LoadAsync(string assetPath, Type type, Action<UnityEngine.Object, XException> callback);

        void Release(UnityEngine.Object asset);
        #endregion
    }
}
