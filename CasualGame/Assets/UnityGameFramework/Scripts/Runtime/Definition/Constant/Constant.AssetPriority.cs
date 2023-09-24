//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Extension.Runtime
{
    public static partial class Constant
    {
        /// <summary>
        /// 资源优先级。
        /// </summary>
        public static class AssetPriority
        {
            /// <summary>
            /// 配置
            /// </summary>
            public const int ConfigAsset = 100;

            /// <summary>
            /// 数据表
            /// </summary>
            public const int DataTableAsset = 100;

            /// <summary>
            /// 字体
            /// </summary>
            public const int FontAsset = 50;

            /// <summary>
            /// 音乐
            /// </summary>
            public const int MusicAsset = 20;

            /// <summary>
            /// 场景
            /// </summary>
            public const int SceneAsset = 0;

            /// <summary>
            /// 声音
            /// </summary>
            public const int SoundAsset = 30;

            /// <summary>
            /// UI面板预制体
            /// </summary>
            public const int UIFormAsset = 50;

            /// <summary>
            /// UI声音
            /// </summary>
            public const int UISoundAsset = 30;
        }
    }
}
