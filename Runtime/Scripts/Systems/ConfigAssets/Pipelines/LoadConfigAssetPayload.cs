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
using System.Collections.Generic;
using TinaX.Services;
using UObject = UnityEngine.Object;

namespace TinaX.Core.ConfigAssets.Pipelines
{
    public class LoadConfigAssetPayload
    {
        /// <summary>
        /// 原始的加载路径
        /// </summary>
        public string LoadPath { get; set; }

        public Type AssetType { get; set; }

        /// <summary>
        /// 从内置的资产服务接口加载配置资产
        /// </summary>
        public bool LoadFormAssetService { get; set; }

        /// <summary>
        /// 约定 如果使用内置的资产服务接口加载配置资产，则在准备流程中对此处赋值
        /// </summary>
        public IAssetService AssetService { get; set; }

        /// <summary>
        /// 使用内置资产服务接口加载配置资产时的加载路径
        /// </summary>
        public string AssetServiceLoadPath { get; set; }

        public bool LoadFromResources { get; set; }

        public string ResourcesLoadPath { get; set; }


        public Dictionary<string, object> Items { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 加载结果
        /// </summary>
        public UObject LoadedAsset { get; set; }
    }
}
