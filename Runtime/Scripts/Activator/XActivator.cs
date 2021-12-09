using System;
using System.Collections.Generic;

namespace TinaX.Core.Activator
{
    public class XActivator
    {
        private readonly List<ICreateInstance> createInstances = new List<ICreateInstance>();

        public object CreateInstance(Type type, params object[] args)
        {
            if(createInstances.Count > 0)
            {
                object instance = null;
                for(int i = 0; i < createInstances.Count; i++)
                {
                    if(createInstances[i].TryCreateInstance(type, out instance, args))
                    {
                        return instance;
                    }
                }
            }
            return System.Activator.CreateInstance(type, args);
        }

        public bool TryCreateInstance(Type type, out object instance, params object[] args)
        {
            if (createInstances.Count > 0)
            {
                for (int i = 0; i < createInstances.Count; i++)
                {
                    if (createInstances[i].TryCreateInstance(type, out instance, args))
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

            if(!createInstances.Contains(creator))
                createInstances.Add(creator);
        }

        public void RemoveCreator(ICreateInstance creator)
        {
            if(creator == null)
                throw new ArgumentNullException(nameof(creator));
            if(createInstances.Contains(creator))
                createInstances.Remove(creator);
        }
    }
}
