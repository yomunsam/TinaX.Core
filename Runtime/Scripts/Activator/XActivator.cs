using System;
using System.Collections.Generic;

namespace TinaX.Core.Activator
{
    public class XActivator
    {
        private readonly List<ICreateInstance> m_CreateInstances = new List<ICreateInstance>();

        public object CreateInstance(Type type, params object[] args)
        {
            if(m_CreateInstances.Count > 0)
            {
                object instance = null;
                for(int i = 0; i < m_CreateInstances.Count; i++)
                {
                    if(m_CreateInstances[i].TryCreateInstance(type, out instance, args))
                    {
                        return instance;
                    }
                }
            }
            return System.Activator.CreateInstance(type, args);
        }

        public bool TryCreateInstance(Type type, out object instance, params object[] args)
        {
            if (m_CreateInstances.Count > 0)
            {
                for (int i = 0; i < m_CreateInstances.Count; i++)
                {
                    if (m_CreateInstances[i].TryCreateInstance(type, out instance, args))
                    {
                        return true;
                    }
                }
            }
            instance = null;
            return false;
        }


        public void RegisterCreator(ICreateInstance creator)
        {
            if(creator == null)
                throw new ArgumentNullException(nameof(creator));

            if(!m_CreateInstances.Contains(creator))
                m_CreateInstances.Add(creator);
        }

        public void RemoveCreator(ICreateInstance creator)
        {
            if(creator == null)
                throw new ArgumentNullException(nameof(creator));
            if(m_CreateInstances.Contains(creator))
                m_CreateInstances.Remove(creator);
        }
    }
}
