// -----------------------------------------------------------------
// File:    StateMachine.cs
// Author:  mouguangyi
// Date:    2016.04.06
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 状态机。
    /// </summary>
    public class StateMachine : C0
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model">宿主对象。</param>
        public StateMachine(object model)
        {
            this.model = model;
        }

        /// <summary>
        /// 析构函数。
        /// </summary>
        public override void Dispose()
        {
            ChangeState(null);
            this.model = null;

            base.Dispose();
        }

        /// <summary>
        /// 切换状态。
        /// </summary>
        /// <param name="state">要切换的目标状态。</param>
        public void ChangeState(State state)
        {
            if (null != this.state) {
                this.state.Exit(this);
            }

            this.state = state;

            if (null != this.state) {
                this.state.Enter(this);
            }
        }

        /// <summary>
        /// 状态机刷新。
        /// </summary>
        /// <param name="delta">帧间隔，以秒为单位。</param>
        public void Pulse(float delta)
        {
            if (null != this.state) {
                this.state.Execute(this, delta);
            }
        }

        /// <summary>
        /// 宿主对象。
        /// </summary>
        public object Model
        {
            get {
                return this.model;
            }
        }

        /// <summary>
        /// 当前状态。
        /// </summary>
        public State State
        {
            get {
                return this.state;
            }
        }

        private object model = null;
        private State state = null;
    }
}
