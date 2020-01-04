using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TinaX
{
    /// <summary>
    /// TinaX Service Provider 服务提供者
    /// </summary>
    public interface IXServiceProvider
    {
        Task OnInit();
        Task OnStart();
        Task OnClose();
        Task OnRestart();
    }

}
