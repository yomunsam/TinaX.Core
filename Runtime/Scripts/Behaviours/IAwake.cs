using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.Core.Behaviours
{
    public interface IAwake : IAwakeBehaviour
    {
        void Awake();
    }

    public interface IAwakeAsync : IAwakeBehaviour
    {
        UniTask AwakeAsync(CancellationToken cancellationToken = default);
    }

    public interface IAwakeBehaviour
    {
        int AwakeOrder { get; set; }
    }

}
