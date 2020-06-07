using System;
using System.Collections.Generic;

namespace TinaX.Utils
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DisposableGroup : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        public HashSet<IDisposable> RegisteredDisposables { get; private set; } = new HashSet<IDisposable>();

        

        public DisposableGroup Register(IDisposable obj, bool InvokeOnceDisposeIfExist = false)
        {
            lock (this)
            {
                if (RegisteredDisposables.Contains(obj))
                {
                    if (InvokeOnceDisposeIfExist)
                        obj.Dispose();
                }
                else
                    RegisteredDisposables.Add(obj);
            }

            return this;
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            if(RegisteredDisposables .Count > 0)
            {
                foreach(var item in RegisteredDisposables)
                    item.Dispose();
            }
        }
    }
}
