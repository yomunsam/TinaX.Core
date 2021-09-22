namespace TinaX.Core.ConfigAssets.Pipelines
{
    /// <summary>
    /// 通用的 “加载配置资产” Handler
    /// </summary>
    public class GeneralLoadConfigAssetHandler : ILoadConfigAssetHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="next"></param>
        /// <returns>若返回false则中断pipeline流程</returns>
        public delegate bool LoadConfigAssetDelegate(ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next);

        public GeneralLoadConfigAssetHandler(string name, LoadConfigAssetDelegate func)
        {
            HandlerName = name;
            LoadConfigAssetFunc = func;
        }

        public string HandlerName { get; private set; }

        public LoadConfigAssetDelegate LoadConfigAssetFunc { get; private set; }

        public bool LoadConfigAsset(ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next)
        {
            return LoadConfigAssetFunc(ref payload, next);
        }
    }
}
