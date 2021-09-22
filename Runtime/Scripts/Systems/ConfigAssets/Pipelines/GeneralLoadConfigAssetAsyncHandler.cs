using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.Core.ConfigAssets.Pipelines
{
    public class GeneralLoadConfigAssetAsyncHandler : ILoadConfigAssetAsyncHandler
    {
        public delegate UniTask<bool> LoadConfigAssetAsyncDelegate(LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken = default);

        public GeneralLoadConfigAssetAsyncHandler(string name, LoadConfigAssetAsyncDelegate func)
        {
            HandlerName = name;
            LoadConfigAssetAsyncFunc = func;
        }

        public string HandlerName { get; private set; }

        public LoadConfigAssetAsyncDelegate LoadConfigAssetAsyncFunc { get; private set; }

        public UniTask<bool> LoadConfigAssetAsync(LoadConfigAssetPayload payload, ILoadConfigAssetAsyncHandler next, CancellationToken cancellationToken = default)
        {
            return LoadConfigAssetAsyncFunc(payload, next, cancellationToken);
        }
    }
}
