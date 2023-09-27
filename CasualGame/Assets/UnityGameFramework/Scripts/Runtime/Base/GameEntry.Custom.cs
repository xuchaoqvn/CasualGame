using System.Collections;
//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace UnityGameFramework.Extension.Runtime
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 自定义组件
        /// </summary>
        private static List<GameFrameworkComponent> CustomGameFrameworkComponents;

        private static void InitCustomComponents()
        {
            GameEntry.CustomGameFrameworkComponents = new List<GameFrameworkComponent>();
        }

        /// <summary>
        /// 注册Game framework组件
        /// </summary>
        /// <param name="gameFrameworkComponent">Game framework组件</param>
        public static void RegisterGameFrameworkComponent(GameFrameworkComponent gameFrameworkComponent)
        {
            if (GameEntry.CustomGameFrameworkComponents.Contains(gameFrameworkComponent))
            {
                Log.Error("123");
                return;
            }
            GameEntry.CustomGameFrameworkComponents.Add(gameFrameworkComponent);
        }

        /// <summary>
        /// 取消注册Game framework组件
        /// </summary>
        /// <param name="gameFrameworkComponent">Game framework组件</param>
        public static void UnRegisterGameFrameworkComponent<T>()
        {
            for (int i = 0; i < GameEntry.CustomGameFrameworkComponents.Count; i++)
            {
                GameFrameworkComponent gameFrameworkComponent = GameEntry.CustomGameFrameworkComponents[i];
                if (gameFrameworkComponent.GetType() == typeof(T))
                    GameEntry.CustomGameFrameworkComponents.RemoveAt(i);
            }
        }

        /// <summary>
        /// 获取Game framework组件
        /// </summary>
        /// <typeparam name="T">Game framework组件类型</typeparam>
        /// <returns>Game framework组件</returns>
        public static T GetGameFrameworkComponent<T>()
            where T : GameFrameworkComponent
        {
            for (int i = 0; i < GameEntry.CustomGameFrameworkComponents.Count; i++)
            {
                GameFrameworkComponent gameFrameworkComponent = GameEntry.CustomGameFrameworkComponents[i];
                if (gameFrameworkComponent.GetType() == typeof(T))
                    return (T)gameFrameworkComponent;
            }

            return null;
        }
    }
}
