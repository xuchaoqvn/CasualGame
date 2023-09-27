using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Extension.Runtime;

namespace _2048
{
    /// <summary>
    /// 2048退出流程
    /// </summary>
    public class Procedure2048Leave : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _2048Component _2048Component = GameEntry.GetGameFrameworkComponent<_2048Component>();
            GameEntry.UnRegisterGameFrameworkComponent<_2048Component>();
            UnityEngine.Object.Destroy(_2048Component);
        }
    }
}