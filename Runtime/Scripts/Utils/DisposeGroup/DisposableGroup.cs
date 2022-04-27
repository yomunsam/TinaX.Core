using System;
using System.Collections.Generic;

namespace TinaX.Core.Utils
{
    public class DisposableGroup : IDisposable
    {
        public HashSet<IDisposable> RegisteredDisposables { get; private set; } = new HashSet<IDisposable>();

        

        public DisposableGroup Register(IDisposable obj, bool InvokeDisposeOnceIfExist = false)
        {
            lock (this)
            {
                if (RegisteredDisposables.Contains(obj))
                {
                    if (InvokeDisposeOnceIfExist)
                        obj.Dispose();
                }
                else
                    RegisteredDisposables.Add(obj);
            }

            return this;
        }

        public void Dispose()
        {
            if(RegisteredDisposables .Count > 0)
            {
                foreach(var item in RegisteredDisposables)
                    item.Dispose();
            }

            RegisteredDisposables.Clear();
        }
    }
}
