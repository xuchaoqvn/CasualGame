using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Extension.Runtime;
using UnityGameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = UnityGameFramework.Extension.Runtime.GameEntry;
using GameFramework.Event;

namespace _2048
{
    /// <summary>
    /// 2048进入流程
    /// </summary>
    public class Procedure2048Enter : ProcedureBase
    {
        #region Field
        /// <summary>
        /// 加载配置
        /// </summary>
        private bool m_LoadConfig = false;
        #endregion

        #region Function
        /// <summary>
        /// 当进入流程时
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, this.OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, this.OnLoadConfigFailure);

            this.m_LoadConfig = false;

            string configAssetName = AssetUtility.GetConfigAsset("2048Config");
            GameEntry.Config.ReadData(configAssetName, this);
        }

        /// <summary>
        /// 当轮询流程时
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!this.m_LoadConfig)
                return;

            this.ChangeState<Procedure2048Idle>(procedureOwner);
        }

        /// <summary>
        /// 当离开流程时
        /// </summary>
        /// <param name="procedureOwner">流程持有者</param>
        /// <param name="isShutdown">是否是关闭状态机时触发</param>
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            this.m_LoadConfig = false;
        }

        /// <summary>
        /// 当配置加载成功时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = e as LoadConfigSuccessEventArgs;
            if (loadConfigSuccessEventArgs == null)
                return;

            if (loadConfigSuccessEventArgs.UserData != this)
                return;

            ConfigComponent config = GameEntry.Config;
            int width = config.GetInt("2048.ScreenWidth");
            int heigth = config.GetInt("2048.ScreenHeight");
            bool fullscreen = config.GetBool("2048.Fullscreen");
            float cameraSize = config.GetFloat("2048.CameraSize");

            Screen.SetResolution(width, heigth, fullscreen);
            UnityEngine.Camera.main.orthographicSize = cameraSize;

            this.m_LoadConfig = true;
        }

        /// <summary>
        /// 当配置加载失败时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs loadConfigFailureEventArgs = e as LoadConfigFailureEventArgs;
            if (loadConfigFailureEventArgs == null)
                return;

            if (loadConfigFailureEventArgs.UserData != this)
                return;

            this.m_LoadConfig = false;

            Log.Error($"Can not load config {loadConfigFailureEventArgs.ConfigAssetName} from '{loadConfigFailureEventArgs.ConfigAssetName}' with error message '{loadConfigFailureEventArgs.ErrorMessage}'.");
        }
        #endregion
    }
}