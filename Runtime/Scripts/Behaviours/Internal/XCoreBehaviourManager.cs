using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace TinaX.Core.Behaviours.Internal
{
    public class XCoreBehaviourManager : IBehaviourManager
    {
        private readonly List<IAwakeBehaviour> m_AwakeBehaviours = new List<IAwakeBehaviour>();
        private readonly List<IStartBehaviour> m_StartBehaviours = new List<IStartBehaviour>();

        public IAwakeBehaviour[] AwakeBehaviours => m_AwakeBehaviours.ToArray();
        public IStartBehaviour[] StartBehaviours => m_StartBehaviours.ToArray();

        public void RegisterObject(object obj)
        {
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));
            if(obj is IAwakeBehaviour)
            {
                var awake_obj = obj as IAwakeBehaviour;
                if (!m_AwakeBehaviours.Contains(awake_obj))
                    m_AwakeBehaviours.Add(awake_obj);
            }

            if (obj is IStartBehaviour)
            {
                var start_obj = obj as IStartBehaviour;
                if (!m_StartBehaviours.Contains(start_obj))
                    m_StartBehaviours.Add(start_obj);
            }
        }

        public void RegisterAwake(IAwakeBehaviour awakeBehaviour)
        {
            if (awakeBehaviour == null)
                throw new ArgumentNullException(nameof(awakeBehaviour));
            if (!m_AwakeBehaviours.Contains(awakeBehaviour))
                m_AwakeBehaviours.Add(awakeBehaviour);
        }

        public void Register(IAwakeBehaviour awakeBehaviour)
            => RegisterAwake(awakeBehaviour);


        public void RegisterStart(IStartBehaviour startBehaviour)
        {
            if (startBehaviour == null)
                throw new ArgumentNullException(nameof(startBehaviour));
            if (!m_StartBehaviours.Contains(startBehaviour))
                m_StartBehaviours.Add(startBehaviour);
        }

        public void Register(IStartBehaviour startBehaviour)
            => RegisterStart(startBehaviour);


        public async UniTask InvokeAwakeAsync()
        {
            var behaviour_group = m_AwakeBehaviours.GroupBy(b => b.AwakeOrder).OrderBy(b => b.Key);
            var group_Enumerator = behaviour_group.GetEnumerator();
            List<UniTask> tasks = new List<UniTask>();
            while (group_Enumerator.MoveNext())
            {
                //Debug.LogFormat("-- Awake排序：{0}", group_Enumerator.Current.Key);
                tasks.Clear();
                var behaviour_Enumerator = group_Enumerator.Current.GetEnumerator();
                while (behaviour_Enumerator.MoveNext())
                {
                    if(behaviour_Enumerator.Current is IAwakeAsync)
                    {
                        tasks.Add((behaviour_Enumerator.Current as IAwakeAsync).AwakeAsync());
                    }
                    else if(behaviour_Enumerator.Current is IAwake)
                    {
                        (behaviour_Enumerator.Current as IAwake).Awake();
                    }
                }
                if(tasks.Count > 0)
                {
                    await UniTask.WhenAll(tasks);
                }
            }

            tasks.Clear();
            m_AwakeBehaviours.Clear();
        }

        public async UniTask InvokeStartAsync()
        {
            var behaviour_group = m_StartBehaviours.GroupBy(b => b.StartOrder).OrderBy(b => b.Key);
            var group_Enumerator = behaviour_group.GetEnumerator();
            List<UniTask> tasks = new List<UniTask>();
            while (group_Enumerator.MoveNext())
            {
                //Debug.LogFormat("-- Start排序：{0}", group_Enumerator.Current.Key);
                tasks.Clear();
                var behaviour_Enumerator = group_Enumerator.Current.GetEnumerator();
                while (behaviour_Enumerator.MoveNext())
                {
                    if (behaviour_Enumerator.Current is IStartAsync)
                    {
                        tasks.Add((behaviour_Enumerator.Current as IStartAsync).StartAsync());
                    }
                    else if (behaviour_Enumerator.Current is IStart)
                    {
                        (behaviour_Enumerator.Current as IStart).Start();
                    }
                }
                if (tasks.Count > 0)
                {
                    await UniTask.WhenAll(tasks);
                }
            }

            tasks.Clear();
            m_StartBehaviours.Clear();
        }

    }
}
