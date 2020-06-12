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
        void OnServiceRegister(IXCore core);

        /// <summary>
        /// before "OnServiceRegister" and "OnStart"
        /// </summary>
        /// <returns>if return not null , framework services initialization workflow will break. </returns>
        Task<XException> OnInit(IXCore core);

        /// <summary>
        /// after "OnInit" and "OnServiceRegister"
        /// </summary>
        /// <returns>if return not null , framework services start workflow will break. </returns>
        Task<XException> OnStart(IXCore core);


        void OnQuit();
        Task OnRestart();
    }

}
