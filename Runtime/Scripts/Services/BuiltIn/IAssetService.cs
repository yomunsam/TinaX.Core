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
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.Services.Builtin.Base;
using UObject = UnityEngine.Object;

namespace TinaX.Services
{
    public interface IAssetService : IBuiltinServiceBase
    {
        #region VFS IO

        T Load<T>(string assetPath) where T : UObject;

        UObject Load(string assetPath, Type type);

        UniTask<T> LoadAsync<T>(string assetPath, CancellationToken cancellationToken = default) where T : UObject;

        UniTask<UObject> LoadAsync(string assetPath, Type type, CancellationToken cancellationToken = default);

        void LoadAsync(string assetPath, Type type, Action<UObject, XException> callback);

        void Release(UObject asset);
        #endregion
    }
}
