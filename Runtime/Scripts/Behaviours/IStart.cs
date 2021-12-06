using Cysharp.Threading.Tasks;

namespace TinaX.Core.Behaviours
{
    public interface IStart : IStartBehaviour
    {
        void Start();
    }

    public interface IStartAsync : IStartBehaviour
    {
        UniTask StartAsync();
    }

    public interface IStartBehaviour
    {
        int StartOrder { get; set; }
    }
}
