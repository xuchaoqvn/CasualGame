using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _2048
{
    /// <summary>
    /// 资源工具
    /// </summary>
    internal static class AssetUtility
    {
        /// <summary>
        /// 获取配置资源路径
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="fromBytes">是否是二进制文件</param>
        /// <returns>资源路径</returns>
        internal static string GetConfigAsset(string assetName, bool fromBytes = true)
        {
            return $"Assets/Games/2048/Configs/{assetName}.{(fromBytes ? "bytes" : "txt")}";
        }
    }
}