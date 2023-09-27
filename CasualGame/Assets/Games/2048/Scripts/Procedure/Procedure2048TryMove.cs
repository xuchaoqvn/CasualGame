using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = UnityGameFramework.Extension.Runtime.GameEntry;

namespace _2048
{
    /// <summary>
    /// 尝试移动流程
    /// </summary>
    public class Procedure2048TryMove : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            int moveDir = procedureOwner.GetData<VarInt32>("MoveDir");
            _2048Component _2048Component = GameEntry.GetGameFrameworkComponent<_2048Component>();

            switch (moveDir)
            {
                case 0:
                    _2048Component.TryMoveUp();
                    break;
                case 1:
                    _2048Component.TryMoveRight();
                    break;
                case 2:
                    _2048Component.TryMoveDown();
                    break;
                case 3:
                    _2048Component.TryMoveLeft();
                    break;
                default:
                    break;
            }
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (DOTween.TotalActiveTweens() > 0)
                this.ChangeState<Procedure2048ItemTween>(procedureOwner);
            else
                this.ChangeState<Procedure2048WaitUserInputAction>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}