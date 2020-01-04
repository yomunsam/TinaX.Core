using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace TinaX
{
    public class XCore : IXCore
    {
        

        public Task RunAsync()
        {




            Debug.Log("TinaX Framework.");
            return Task.CompletedTask;
        }
    }
}

