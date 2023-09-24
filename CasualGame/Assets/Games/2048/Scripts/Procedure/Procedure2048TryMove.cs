using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

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
            /*
            switch (moveDir)
            {
                case 0:
                    this.TryMoveUp();
                    break;
                case 1:
                    this.TryMoveRight();
                    break;
                case 2:
                    this.TryMoveDown();
                    break;
                case 3:
                    this.TryMoveLeft();
                    break;
                default:
                    break;
            }*/
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //this.ChangeState<Procedure2048WaitUserInputAction>(procedureOwner);
            //this.ChangeState<Procedure2048ItemTween>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 尝试向上移动单位
        /// </summary>
        private void TryMoveUp()
        {
            int startIndex;
            int deltaIndex;
            int moveDelta;

            /*
            int index = startIndex;
            while (index >= 0)
            {
                if (this.m_ChessPiecess.ContainsKey(index))
                    this.MoveChessPieces(this.m_ChessPiecess[index], -1, this.m_Chessboard.Size, moveDelta);

                index += deltaIndex;
            }
            */
        }
    }
}