namespace TinaX.Core.ConfigAssets.Pipelines
{
    /// <summary>
    /// 加载配置资产处理器
    /// </summary>
    public interface ILoadConfigAssetHandler
    {
        string HandlerName { get; }

        /// <summary>
        /// 加载配置资产
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="next"></param>
        /// <returns>若返回false则中断pipeline流程</returns>
        bool LoadConfigAsset(ref LoadConfigAssetPayload payload, ILoadConfigAssetHandler next);
    }
}
