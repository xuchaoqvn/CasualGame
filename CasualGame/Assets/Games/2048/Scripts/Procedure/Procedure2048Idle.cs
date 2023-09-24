using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityGameFramework.Extension.Runtime;
using GameEntry = UnityGameFramework.Extension.Runtime.GameEntry;
using GameFramework.Event;
using System;

namespace _2048
{
    /// <summary>
    /// 默认待机流程
    /// </summary>
    public class Procedure2048Idle : ProcedureBase
    {
        #region Field

        #endregion

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, this.OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, this.OnOpenUIFormFailure);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //this.ChangeState<Procedure2048CreatMap>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, this.OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, this.OnOpenUIFormFailure);
        }

        /// <summary>
        /// 当2048默认待机UI面板打开成功时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当2048默认待机UI面板打开失败时
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}