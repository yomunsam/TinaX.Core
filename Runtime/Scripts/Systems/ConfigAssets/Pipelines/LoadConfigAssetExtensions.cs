using TinaX.Systems.Pipeline;

namespace TinaX.Core.ConfigAssets.Pipelines
{
    public static class LoadConfigAssetExtensions
    {

        /// <summary>
        /// 添加通用Handler
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="name"></param>
        /// <param name="handlerFunc"></param>
        /// <returns></returns>
        public static XPipeline<ILoadConfigAssetHandler> Use(this XPipeline<ILoadConfigAssetHandler> pipeline, string name, GeneralLoadConfigAssetHandler.LoadConfigAssetDelegate handlerFunc)
        {
            pipeline.AddLast(new GeneralLoadConfigAssetHandler(name, handlerFunc));
            return pipeline;
        }
    }
}
