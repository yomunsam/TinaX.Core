using UnityEngine;

namespace TinaX.Services.ConfigAssets
{
    public interface IConfigAssetService
    {
        T GetConfig<T>(string loadPath) where T : ScriptableObject;
    }
}
