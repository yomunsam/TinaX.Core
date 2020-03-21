using System;
using System.Threading.Tasks;


namespace TinaX.Services
{
    public interface IAssetService : IBuiltInService
    {
        #region VFS IO

        T Load<T>(string assetPath) where T : UnityEngine.Object;

        UnityEngine.Object Load(string assetPath, Type type);

        Task<T> LoadAsync<T>(string assetPath) where T : UnityEngine.Object;

        void LoadAsync(string assetPath, Type type, Action<UnityEngine.Object, XException> callback);

        void Release(UnityEngine.Object asset);
        #endregion
    }
}
