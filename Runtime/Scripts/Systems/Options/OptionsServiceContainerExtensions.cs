/*
 * This file is part of the "TinaX Framework".
 * https://github.com/yomunsam/TinaX
 *
 * (c) Nekonya Studio <yomunsam@nekonya.io>
 * https://nekonya.io
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using TinaX.Container;
using TinaX.Options.Internal;
using UnityEngine;

namespace TinaX.Options
{
    public static class OptionsServiceContainerExtensions
    {
        public static IServiceContainer AddOptions(this IServiceContainer services)
        {
            if(services.SingletonIf<IOptionsManager, OptionsManager>(out var bd))
            {
                bd.SetAlias<OptionsManager>();
            }
            return services;
        }


        /// <summary>
        /// Configure and inject it into the service container
        /// 添加配置，并注入到服务容器
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceContainer Configure<TOptions>(this IServiceContainer services, Action<TOptions> configureOptions) where TOptions : class
        {
            var options = services.DoConfigure(typeof(TOptions), (opt)=>
            {
                configureOptions.Invoke((TOptions)opt);
            }, null);
            var generic_options = new Options<TOptions>(options);
            services.Instance<IOptions<TOptions>>(generic_options);
            return services;
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceContainer Configure(this IServiceContainer services, Type type, Action<object> configureOptions)
        {
            services.DoConfigure(type, configureOptions, null);
            return services;
        }


        private static Internal.Options DoConfigure(this IServiceContainer services, Type type, Action<object> configureOptions, string typeName = null)
        {
            var options = new TinaX.Options.Internal.Options(configureOptions, services, type);
            if (services.TryGet<OptionsManager>(out var optionsMgr))
            {
                optionsMgr.Set(typeName ?? type.FullName, Internal.Options.DefaultName, options);
            }
            else
            {
                Debug.LogError("[TinaX]Options not enable. Please invoke \"IServiceContainer.AddOptions\".");
            }

            return options;
        }


    }
}
