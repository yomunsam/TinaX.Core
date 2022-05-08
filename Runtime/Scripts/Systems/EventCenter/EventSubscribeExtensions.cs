using TinaX.Core.EventCenter;
using TinaX.Core.GameObjectDestroyedObserver;
using UnityEngine;

#nullable enable
namespace TinaX
{
    public static class EventSubscribeExtensions
    {
        /// <summary>
        /// Bind lifecycle to GameObject
        /// 绑定生命周期到GameObject
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static IEventSubscribe BindTo(this IEventSubscribe subscribe, in GameObject gameObject)
        {
            if (gameObject == null) //这里判断已销毁
            {
                subscribe.Unsubscribe();
                return subscribe;
            }
            gameObject.GetOrCreateDestroyedObserver().OnDestroyAction += (go) =>
            {
                subscribe.Unsubscribe();
            };
            return subscribe;
        }

        /// <summary>
        /// Bind the lifecycle to the GameObject the component is mounted on
        /// 绑定生命周期到组件所被挂载的GameObject
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static IEventSubscribe BindTo(this IEventSubscribe subscribe, in Component behaviour)
        {
            if(behaviour == null)
            {
                subscribe.Unsubscribe();
                return subscribe;
            }
            behaviour.gameObject.GetOrCreateDestroyedObserver().OnDestroyAction += go =>
            {
                subscribe.Unsubscribe();
            };
            return subscribe;
        }
    }
}
