// -----------------------------------------------------------------
// File:    State.cs
// Author:  mouguangyi
// Date:    2016.04.06
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 状态类。
    /// <seealso cref="StateMachine"/>
    /// </summary>
    public class State : C0
    {
        /// <summary>
        /// 进入状态。
        /// </summary>
        /// <param name="stateMachine">状态机。</param>
        public virtual void Enter(StateMachine stateMachine)
        { }

        /// <summary>
        /// 更新状态。
        /// </summary>
        /// <param name="stateMachine">状态机。</param>
        /// <param name="delta">帧间隔，单位为秒。</param>
        public virtual void Execute(StateMachine stateMachine, float delta)
        { }

        /// <summary>
        /// 退出状态。
        /// </summary>
        /// <param name="stateMachine">状态机。</param>
        public virtual void Exit(StateMachine stateMachine)
        { }
    }

    /// <summary>
    /// @details 状态集合。
    /// </summary>
    public sealed class StateSet<T> : C0
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            this.states = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="state"></param>
        public void AddState(T stateId, State state)
        {
            this.states[stateId] = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public State FindState(T stateId)
        {
            return this.states[stateId];
        }

        private Dictionary<T, State> states = new Dictionary<T, State>();
    }
}
