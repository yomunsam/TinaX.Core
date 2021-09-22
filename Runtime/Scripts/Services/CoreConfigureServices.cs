using TinaX.Container;
using TinaX.Services.ConfigAssets;

namespace TinaX.Core.Serivces
{
    public static class CoreConfigureServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="core"></param>
        public static void ConfigureServices(IServiceContainer services, IXCore core)
        {
            services.SingletonIf<IConfigAssetService, ConfigAssetService>(out _);
        }
    }
}
