using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Extension.Runtime;

namespace _2048
{
    /// <summary>
    /// 2048检查游戏是否结束流程
    /// </summary>
    public class Procedure2048CheckGameover : ProcedureBase
    {
        #region Field
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        private bool m_GameOver;
        #endregion

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _2048Component _2048Component = GameEntry.GetGameFrameworkComponent<_2048Component>();
            this.m_GameOver = _2048Component.CheckGameOver();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (this.m_GameOver)
                this.ChangeState<Procedure2048Gameover>(procedureOwner);
            else
                this.ChangeState<Procedure2048WaitUserInputAction>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}