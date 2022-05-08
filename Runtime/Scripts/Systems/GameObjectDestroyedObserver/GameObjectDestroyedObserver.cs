#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;

namespace TinaX.Core.GameObjectDestroyedObserver
{

    [DisallowMultipleComponent]
    public class GameObjectDestroyedObserver : MonoBehaviour
    {
        public UnityEvent<GameObject>? OnDestroyUEvent;

        public Action<GameObject>? OnDestroyAction;


        private void OnDestroy()
        {
            OnDestroyAction?.Invoke(this.gameObject);
            OnDestroyUEvent?.Invoke(this.gameObject);
        }

    }

    public static class GameObjectDestroyedObserverExtensions
    {
        public static GameObjectDestroyedObserver GetOrCreateDestroyedObserver(this GameObject go)
            => go.GetComponentOrAdd<GameObjectDestroyedObserver>();
    }
}
