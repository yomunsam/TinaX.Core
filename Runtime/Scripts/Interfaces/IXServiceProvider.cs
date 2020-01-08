using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TinaX.Services
{
    /// <summary>
    /// TinaX Service Provider 服务提供者
    /// </summary>
    public interface IXServiceProvider
    {
        string ServiceName { get; }

        /// <summary>
        /// after "OnInit" and before "OnStart" 
        /// </summary>
        void OnServiceRegister();

        /// <summary>
        /// before "OnServiceRegister" and "OnStart"
        /// </summary>
        /// <returns>if return false , framework services start workflow will break. </returns>
        Task<bool> OnInit();

        /// <summary>
        /// after "OnInit" and "OnServiceRegister"
        /// </summary>
        /// <returns>if return false , framework services start workflow will break. </returns>
        Task<bool> OnStart();
        Task OnClose();
        Task OnRestart();
    }

}
