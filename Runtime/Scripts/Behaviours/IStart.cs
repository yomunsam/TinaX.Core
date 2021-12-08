using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.Core.Behaviours
{
    public interface IStart : IStartBehaviour
    {
        void Start();
    }

    public interface IStartAsync : IStartBehaviour
    {
        UniTask StartAsync(CancellationToken cancellationToken = default);
    }

    public interface IStartBehaviour
    {
        int StartOrder { get; set; }
    }
}
