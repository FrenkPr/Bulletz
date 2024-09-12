using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    enum StateType
    {
        Patrol,
        Chase,
        Attack
    }

    abstract class State
    {
        protected StateMachine fsm;

        public State(StateMachine fsm)
        {
            this.fsm = fsm;
        }
        
        public virtual void OnEnter(){}

        public virtual void OnExit(){}

        public abstract void Update();

        public void SetStateMachine(StateMachine state)
        {
            fsm = state;
        }
    }
}
