using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = UnityGameFramework.Extension.Runtime.GameEntry;

namespace _2048
{
    /// <summary>
    /// 2048生成地图流程
    /// </summary>
    public class Procedure2048CreatMap : ProcedureBase
    {
        #region Field
        /// <summary>
        /// 宽
        /// </summary>
        private int m_Width;

        /// <summary>
        /// 高
        /// </summary>
        private int m_Height;
        #endregion

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            this.m_Width = procedureOwner.GetData<VarInt32>("Width");
            this.m_Height = procedureOwner.GetData<VarInt32>("Height");
            procedureOwner.SetData<VarInt32>("Width", this.m_Width);
            procedureOwner.SetData<VarInt32>("Height", this.m_Height);

            this.CreatMap();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //this.ChangeState<Procedure2048SpawnItem>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 生成地图
        /// </summary>
        private void CreatMap()
        {

        }
    }
}