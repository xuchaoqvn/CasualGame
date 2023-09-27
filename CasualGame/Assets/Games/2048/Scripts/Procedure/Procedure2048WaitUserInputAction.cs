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

            this.Keyboard();

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

        /// <summary>
        /// 键盘
        /// </summary>
        private void Keyboard()
        {
            UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard.wKey.wasPressedThisFrame)
            {
                this.m_MoveDir = 0;
                this.m_UserInputAction = true;
            }
            else if (keyboard.dKey.wasPressedThisFrame)
            {
                this.m_MoveDir = 1;
                this.m_UserInputAction = true;
            }
            else if (keyboard.sKey.wasPressedThisFrame)
            {
                this.m_MoveDir = 2;
                this.m_UserInputAction = true;
            }
            else if (keyboard.aKey.wasPressedThisFrame)
            {
                this.m_MoveDir = 3;
                this.m_UserInputAction = true;
            }
        }

        private Vector2 m_LastPosition;

        private void Mouse()
        {
            UnityEngine.InputSystem.Mouse mouse = UnityEngine.InputSystem.Mouse.current;

            if (mouse.leftButton.wasPressedThisFrame)
                this.m_LastPosition = mouse.position.value;
            else if (mouse.leftButton.isPressed)
            {
                //左
                if (mouse.position.value.x - this.m_LastPosition.x < 0)
                {

                }
                //右
                else if (mouse.position.value.x - this.m_LastPosition.x > 0)
                {

                }
                //上
                else if (mouse.position.value.y - this.m_LastPosition.y > 0)
                {

                }
                //下
                else if (mouse.position.value.y - this.m_LastPosition.y < 0)
                {

                }
                this.m_LastPosition = mouse.position.value;
            }
            else if (mouse.leftButton.wasReleasedThisFrame)
            {

            }
        }
    }
}