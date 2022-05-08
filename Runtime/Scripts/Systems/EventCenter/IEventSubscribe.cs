#nullable enable
namespace TinaX.Core.EventCenter
{
    /// <summary>
    /// Event subscribe interface
    /// 事件订阅接口
    /// </summary>
    public interface IEventSubscribe
    {
        string EventName { get; }

        void Unsubscribe();
    }

    
}
