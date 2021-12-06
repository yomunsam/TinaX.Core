using Cysharp.Threading.Tasks;

namespace TinaX.Core.Behaviours
{
    public interface IAwake : IAwakeBehaviour
    {
        void Awake();
    }

    public interface IAwakeAsync : IAwakeBehaviour
    {
        UniTask AwakeAsync();
    }

    public interface IAwakeBehaviour
    {
        int AwakeOrder { get; set; }
    }

}
