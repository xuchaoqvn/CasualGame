using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace _2048
{
    /// <summary>
    /// 等待用户输入动作流程
    /// </summary>
    public class Procedure2048WaitUserInputAction : ProcedureBase
    {
        #region Field
        /// <summary>
        /// 用户输入动作
        /// </summary>
        private bool m_UserInputAction;

        /// <summary>
        /// 移动方向
        /// </summary>
        private int m_MoveDir;
        #endregion

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            this.m_UserInputAction = false;
            this.m_MoveDir = default;
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!this.m_UserInputAction)
                return;

            procedureOwner.SetData<VarInt32>("MoveDir", this.m_MoveDir);
            this.ChangeState<Procedure2048TryMove>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            this.m_UserInputAction = false;
            this.m_MoveDir = default;
        }

        /// <summary>
        /// 当用户输入动作时
        /// </summary>
        private void OnUserInputAction()
        {

        }
    }
}